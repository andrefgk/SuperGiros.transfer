# 📋 Guía de Pruebas — SuperGiros Transfer System
## Cómo verificar cada objetivo específico

---

## ✅ OBJETIVO 1 — Diagrama de Arquitectura

**Qué mostrar:** El diagrama de capas del sistema.

```
[ Postman / Swagger ]
        ↓
[ Services.gRPC ]  ← AuthInterceptor + GlobalExceptionHandler
        ↓
[ Application.UseCases ]  ← CQRS (Commands + Queries via MediatR)
        ↓
[ Application.Interfaces ]  ← Contratos (IApplicationDbContext)
        ↓
[ Persistence ]  ← EF Core + SQL Server
        ↓
[ Domain ]  ← Entidades + Events + Enums
        ↓
[ Messaging ]  ← MassTransit In-Memory (Publish + Consume)
```

**Cómo demostrarlo:** Mostrar esta estructura de carpetas en Visual Studio o el explorador de archivos.

---

## ✅ OBJETIVO 2 — API REST + gRPC documentada con Swagger

### Paso a paso:

1. Ejecuta el proyecto con `dotnet run` o `F5` en Visual Studio.
2. Abre el navegador en: `http://localhost:5220/swagger`
3. Verás todos los endpoints documentados.

### Prueba REST en Postman:
- **Método:** POST
- **URL:** `http://localhost:5220/api/Auth/login`
- **Body (JSON):**
```json
{
  "username": "admin",
  "password": "Admin123!"
}
```
- **Resultado esperado:** `200 OK` con un token JWT.

### Prueba gRPC en Postman:
1. En Postman → New → gRPC Request
2. URL: `localhost:5221`
3. Importa el `.proto` desde `src/SuperGiros.Transfer.Services.gRPC/Protos/`
4. Selecciona el método `Login` del servicio `UserServices`
5. Body:
```json
{ "username": "admin", "password": "Admin123!" }
```

---

## ✅ OBJETIVO 3 — Patrón CQRS

**Qué mostrar:** Un Command y una Query funcionando.

### Prueba COMMAND (Crear transacción) — gRPC Postman:
1. Primero obtén token de Admin (ver Objetivo 2).
2. En Postman gRPC → servicio `Transactions` → método `CreateTransaction`
3. Metadata: `authorization: Bearer <tu_token_admin>`
4. Body:
```json
{
  "accountId": 1,
  "tipoMovimiento": 0,
  "monto": 500.0,
  "moneda": "PEN",
  "descripcion": "Giro de prueba CQRS",
  "sede": "Lima Centro",
  "fechaRealizacion": "2026-06-16T10:00:00Z"
}
```
5. **Resultado esperado:** `true` (la transacción se creó).

### Prueba QUERY (Listar transacciones):
1. En Postman gRPC → método `GetAllTransaction`
2. Metadata: `authorization: Bearer <tu_token_usuario_o_admin>`
3. Body: `{}`
4. **Resultado esperado:** Lista de transacciones.

### En código: mira `Features/Transaction/Commands/CreateTransaction/CreateTransactionHandler.cs`
y `Features/Transaction/Querys/GetAllTransaction/GetAllTransactionHandler.cs`.

---

## ✅ OBJETIVO 4 — SQL Integrado

**Qué mostrar:** La base de datos creada y los datos seed.

### Paso a paso:
1. Ejecuta las migraciones (solo la primera vez):
```bash
dotnet ef database update --project src/SuperGiros.Transfer.Persistence --startup-project src/SuperGiros.Transfer.Services.gRPC
```
2. Abre SQL Server Management Studio (SSMS).
3. Conecta a `DESKTOP-DJ9N983\SQLEXPRESS` con usuario `sa`.
4. Verás la base de datos `transfer2` con las tablas: `Users`, `Customers`, `Offices`, `Transactions`.
5. Ejecuta: `SELECT * FROM Users` → verás los usuarios seed.

### Prueba desde Postman:
1. Llama a `GetAllCustomer` o `GetAllOffice` en gRPC.
2. Si retorna datos → SQL está integrado y funcionando.

---

## ✅ OBJETIVO 5 — Seguridad con Roles y Claims

### Prueba A: Sin token (debe dar error claro)
1. En Postman gRPC → `GetAllCustomer` → **sin** agregar metadata de authorization.
2. **Resultado esperado:**
```
Status: UNAUTHENTICATED
Message: ❌ Token JWT requerido. Envía el header 'authorization: Bearer <token>'...
```

### Prueba B: Token inválido (debe dar error claro)
1. En Postman gRPC → `GetAllCustomer`
2. Metadata: `authorization: Bearer token_falso_123`
3. **Resultado esperado:**
```
Status: UNAUTHENTICATED
Message: ❌ Token inválido o expirado. Genera uno nuevo en POST /api/Auth/login...
```

### Prueba C: Token de Usuario intentando operación de Admin
1. Obtén token con rol `Usuario` (login con usuario normal).
2. En Postman gRPC → `CreateTransaction` o `CreateCustomer`
3. Metadata: `authorization: Bearer <token_usuario>`
4. **Resultado esperado:**
```
Status: PERMISSION_DENIED
Message: ❌ Acceso denegado para 'juan'. Esta operación (CreateTransaction) es exclusiva para Administradores. Tu rol actual es: 'Usuario'...
```

### Prueba D: Token de Admin con acceso total
1. Obtén token con rol `Admin`.
2. Llama a cualquier endpoint.
3. **Resultado esperado:** Respuesta correcta (200 OK o datos).

### Para REST (Swagger o Postman HTTP):
- Sin token en endpoint protegido:
```json
{
  "error": "No autorizado",
  "mensaje": "Token JWT requerido. Genera uno en POST /api/Auth/login",
  "status": 401,
  "hint": "Envía el header: Authorization: Bearer <tu_token>"
}
```

---

## ✅ OBJETIVO 6 — Event-Driven (Mensajería)

**Qué mostrar:** Que al crear una transacción se publican y consumen eventos.

### Paso a paso:
1. Asegúrate de tener la consola de Visual Studio visible (Output → Debug).
2. Llama a `CreateTransaction` con token Admin (ver Objetivo 3).
3. Mira los logs en la consola. Debes ver algo como:
```
[EVENT RECIBIDO] TransactionCreated | Id:5 | Cuenta:1 | Giro | S/ 500 PEN | Sede:Lima Centro
```
4. Esto confirma que el evento fue **publicado** por `CreateTransactionHandler` y **consumido** por `TransactionCreatedConsumer`.

### También verifica al cancelar:
1. Llama a `CancelTransaction` con el ID de una transacción existente.
2. En los logs verás:
```
[EVENT RECIBIDO] TransactionCanceled | Id:5 | ...
```

---

## ✅ OBJETIVO 7 — Pruebas Unitarias xUnit + Moq

### Paso a paso:
1. En Visual Studio → Test → Run All Tests (Ctrl+R, A).
2. O desde terminal:
```bash
dotnet test tests/SuperGiros.Transfer.Tests/SuperGiros.Transfer.Tests.csproj -v normal
```
3. **Resultado esperado:** 5 tests en verde ✅:
   - `Handle_TransaccionValida_ReturnsTrueYPublicaEvento`
   - `Handle_SaveFalla_ReturnsFalseYNoPublicaEvento`
   - `Handle_LlamaSaveChangeAsyncExactamenteUnaVez`
   - `Handle_TransaccionExistente_CambiaEstadoYPublicaEvento`
   - `Handle_TransaccionNoExiste_LanzaException`

### Qué muestran los tests:
- Usan **Moq** para simular la base de datos (no necesitan SQL real).
- Usan **xUnit** como framework de assertions.
- Verifican comportamiento del handler sin dependencias externas.

---

## ✅ OBJETIVO 8 — Dockerfile

### Paso a paso:
```bash
# Desde la raíz del proyecto
docker build -t supergiros-api:dev -f src/SuperGiros.Transfer.Services.gRPC/Dockerfile .
```
- **Resultado esperado:** Imagen construida sin errores (proceso en 3 etapas: build → publish → runtime).
- Verifica: `docker images | grep supergiros-api`

---

## ✅ OBJETIVO 9 — Docker Compose (API + DB)

### Paso a paso:
```bash
# Desde la raíz del proyecto
docker compose up --build
```

Espera que aparezca:
```
supergiros-api    | ✅ SuperGiros Transfer Service activo
supergiros-api    | Now listening on: http://0.0.0.0:5220
```

Verifica:
- Swagger: `http://localhost:5220/swagger`
- Login: `POST http://localhost:5220/api/Auth/login`

Para detener:
```bash
docker compose down
```

---

## ✅ OBJETIVO 10 — Kubernetes (Cluster local con Minikube)

### Requisito previo: tener Minikube instalado.

```bash
# 1. Iniciar Minikube
minikube start

# 2. Cargar imagen local en Minikube (importante: usa la imagen local)
minikube image load supergiros-api:dev

# 3. Aplicar todos los manifiestos K8s
kubectl apply -f k8s/configmap.yaml
kubectl apply -f k8s/secret.yaml
kubectl apply -f k8s/sqlserver-deployment.yaml
kubectl apply -f k8s/api-deployment.yaml

# 4. Verificar que los pods estén corriendo
kubectl get pods

# Resultado esperado:
# NAME                              READY   STATUS    RESTARTS
# supergiros-api-xxx                1/1     Running   0
# supergiros-sqlserver-xxx          1/1     Running   0

# 5. Acceder al servicio
minikube service supergiros-api-svc --url
# Te da la URL de acceso (ej: http://192.168.49.2:30220)
```

---

## ✅ OBJETIVO 11 — Secrets seguros en K8s

**Qué mostrar:** Que los valores sensibles están en el Secret, no en el código.

```bash
# Ver que el secret existe
kubectl get secret supergiros-secrets

# Ver el contenido (en base64)
kubectl describe secret supergiros-secrets

# Decodificar para verificar (solo para demostración)
kubectl get secret supergiros-secrets -o jsonpath="{.data.jwt-secret}" | base64 --decode
```

**Resultado esperado:** El JWT secret, connection string y SA password están en el Secret de K8s, no hardcodeados en el código.

---

## ✅ OBJETIVO 12 — Pipeline CI/CD

**Qué mostrar:** El archivo `.github/workflows/ci-cd.yml` con sus jobs.

### Cómo activarlo:
1. Sube el proyecto a GitHub en un repositorio.
2. Haz un push a `main` o `master`.
3. Ve a GitHub → pestaña "Actions".
4. Verás el pipeline ejecutándose con 2 jobs:
   - **Build & Test** → compila + ejecuta los 5 tests unitarios
   - **Docker Build** → construye la imagen Docker

### Para mostrar localmente sin GitHub:
```bash
# Simula el job 1: build
dotnet build src/SuperGiros.Transfer.Services.gRPC/SuperGiros.Transfer.Services.gRPC.csproj -c Release

# Simula el job 1: tests
dotnet test tests/SuperGiros.Transfer.Tests/SuperGiros.Transfer.Tests.csproj -c Release -v normal

# Simula el job 2: docker build
docker build -t supergiros-api:ci-test -f src/SuperGiros.Transfer.Services.gRPC/Dockerfile .
```

---

## 🎯 Resumen de comandos de verificación rápida

| Objetivo | Comando / Acción |
|----------|-----------------|
| 1. Arquitectura | Mostrar carpetas en Visual Studio |
| 2. Swagger/gRPC | `http://localhost:5220/swagger` |
| 3. CQRS | Llamar CreateTransaction + GetAllTransaction |
| 4. SQL | `SELECT * FROM Users` en SSMS |
| 5. Seguridad | Postman sin token → ver mensaje de error |
| 6. Event-Driven | Ver logs al crear transacción |
| 7. Tests | `dotnet test` → 5 tests verdes |
| 8. Dockerfile | `docker build ...` → sin errores |
| 9. Docker Compose | `docker compose up` → swagger accesible |
| 10. Kubernetes | `kubectl get pods` → Running |
| 11. Secrets K8s | `kubectl describe secret supergiros-secrets` |
| 12. CI/CD | Push a GitHub → ver Actions |
