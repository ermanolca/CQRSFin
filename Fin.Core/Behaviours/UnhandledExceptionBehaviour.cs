using Fin.Core.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Fin.Core.Behaviours
{
    public class UnhandledExceptionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
            where TResponse : FinResponse, new()
    {
        private readonly ILogger<UnhandledExceptionBehavior<TRequest, TResponse>> logger;

        public UnhandledExceptionBehavior(ILogger<UnhandledExceptionBehavior<TRequest, TResponse>> logger)
        {
            this.logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            try
            {
                return await next();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "{Request} - Unhandled exception caught.", typeof(TRequest).Name);
                var response = new TResponse();
                response.ServerError();
                return response;
            }
        }
    }
}
