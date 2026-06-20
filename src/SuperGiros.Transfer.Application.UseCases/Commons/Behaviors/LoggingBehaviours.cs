using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace SuperGiros.Transfer.Application.UseCases.Commons.Behaviors
{
    internal class LoggingBehaviours<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<LoggingBehaviours<TRequest, TResponse>> _logger;

        public LoggingBehaviours(ILogger<LoggingBehaviours<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var correlationId = Guid.NewGuid();

            _logger.LogInformation("SuperGiros Request: {correlationId} {name} {@request}", correlationId, typeof(TRequest).Name, JsonSerializer.Serialize(request));

            var response = await next();

            _logger.LogInformation("SuperGiros Response: {correlationId} {name} {@response}", correlationId, typeof(TResponse).Name, JsonSerializer.Serialize(response));

            return response;
        }
    }
}
