using Grpc.Core;
using Grpc.Core.Interceptors;
using System.Security.Claims;

namespace SuperGiros.Transfer.Services.gRPC.Commons.Auth
{
    public class AuthInterceptor : Interceptor
    {
        private readonly ITokenService _tokenService;

        // Diccionario con las rutas gRPC EXACTAS y el rol requerido
        private readonly Dictionary<string, string> _methodRoles = new()
        {
            // --- USER ---
            { "/user.UserServices/Login",          "" },       // público
            { "/user.UserServices/CreateUser",     "Admin" },
            { "/user.UserServices/UpdateUser",     "Admin" },
            { "/user.UserServices/ChangeUserState","Admin" },
            { "/user.UserServices/GetUsers",       "Admin" },

            // --- CUSTOMER ---
            { "/Customers/GetAllCustomer", "Usuario" },
            { "/Customers/GetCustomer",    "Usuario" },
            { "/Customers/CreateCustomer", "Admin"   },
            { "/Customers/UpdateCustomer", "Admin"   },
            { "/Customers/CancelCustomer", "Admin"   },

            // --- OFFICE ---
            { "/Offices/GetAllOffice", "Usuario" },
            { "/Offices/GetOffice",    "Usuario" },
            { "/Offices/CreateOffice", "Admin"   },
            { "/Offices/UpdateOffice", "Admin"   },
            { "/Offices/CancelOffice", "Admin"   },

            // --- TRANSACTION ---
            { "/Transactions/GetAllTransaction", "Usuario" },
            { "/Transactions/GetTransaction",    "Usuario" },
            { "/Transactions/CreateTransaction", "Admin"   },
            { "/Transactions/UpdateTransaction", "Admin"   },
            { "/Transactions/CancelTransaction", "Admin"   },
        };

        public AuthInterceptor(ITokenService tokenService) => _tokenService = tokenService;

        public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
            TRequest request, ServerCallContext context, UnaryServerMethod<TRequest, TResponse> continuation)
        {
            var methodName = context.Method;

            // Si la ruta no está en el diccionario o no requiere rol, deja pasar
            if (!_methodRoles.TryGetValue(methodName, out var requiredRole) || string.IsNullOrEmpty(requiredRole))
                return await continuation(request, context);

            // Obtener el header Authorization
            var authHeader = context.RequestHeaders
                .FirstOrDefault(h => h.Key.Equals("authorization", StringComparison.OrdinalIgnoreCase))?.Value;

            // ✅ Sin token → 401 con mensaje claro
            if (string.IsNullOrEmpty(authHeader))
                throw new RpcException(new Status(StatusCode.Unauthenticated,
                    "❌ Token JWT requerido. Envía el header 'authorization: Bearer <token>'. " +
                    "Genera tu token en POST /api/Auth/login"));

            var token = authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase)
                ? authHeader[7..].Trim()
                : authHeader.Trim();

            // ✅ Token inválido o expirado → 401 con mensaje claro
            if (!_tokenService.ValidateToken(token))
                throw new RpcException(new Status(StatusCode.Unauthenticated,
                    "❌ Token inválido o expirado. Genera uno nuevo en POST /api/Auth/login " +
                    "con tus credenciales de usuario."));

            var principal = _tokenService.GetPrincipalFromToken(token);
            if (principal == null)
                throw new RpcException(new Status(StatusCode.Unauthenticated,
                    "❌ No se pudo leer la identidad del token. El token puede estar corrupto."));

            var userRole = principal.FindFirst(ClaimTypes.Role)?.Value ?? "";
            var username = principal.FindFirst(ClaimTypes.Name)?.Value ?? "desconocido";

            if (string.IsNullOrEmpty(userRole))
                throw new RpcException(new Status(StatusCode.Unauthenticated,
                    "❌ El token no contiene información de rol. Genera un nuevo token."));

            // ✅ CORRECCIÓN: Mensaje claro cuando el rol no tiene permisos (403 PermissionDenied)
            // Lógica jerárquica: Admin puede todo; Usuario solo puede operaciones de lectura
            if (requiredRole == "Admin" && !userRole.Equals("Admin", StringComparison.OrdinalIgnoreCase))
                throw new RpcException(new Status(StatusCode.PermissionDenied,
                    $"❌ Acceso denegado para '{username}'. " +
                    $"Esta operación ({methodName.Split('/').Last()}) es exclusiva para Administradores. " +
                    $"Tu rol actual es: '{userRole}'. " +
                    $"Contacta a un Admin para realizar esta acción."));

            return await continuation(request, context);
        }
    }
}
