using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.Extensions.Logging;

namespace SuperGiros.Transfer.Services.gRPC.Commons.GlobalExceptions
{
    public class GlobalExceptionHandler : Interceptor
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
            TRequest request, ServerCallContext context, UnaryServerMethod<TRequest, TResponse> continuation)
        {
            try
            {
                return await continuation(request, context);
            }
            catch (RpcException)
            {
                // Las RpcException (como las de AuthInterceptor) se propagan directamente
                // con su mensaje original sin modificar
                throw;
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning("Recurso no encontrado: {Message}", ex.Message);
                throw new RpcException(new Status(StatusCode.NotFound,
                    $"❌ Recurso no encontrado: {ex.Message}"));
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning("Acceso denegado: {Message}", ex.Message);
                throw new RpcException(new Status(StatusCode.PermissionDenied,
                    $"❌ Acceso denegado: {ex.Message}"));
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Argumento inválido: {Message}", ex.Message);
                throw new RpcException(new Status(StatusCode.InvalidArgument,
                    $"❌ Datos inválidos: {ex.Message}"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error interno en {Method}", context.Method);
                throw new RpcException(new Status(StatusCode.Internal,
                    $"❌ Error interno del servidor. Contacta al administrador."));
            }
        }
    }
}
