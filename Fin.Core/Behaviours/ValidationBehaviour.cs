using Fin.Core.Models;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Fin.Core.Behaviours
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
            where TResponse : FinResponse, new()
    {
        private readonly ILogger<ValidationBehavior<TRequest, TResponse>> logger;
        private readonly IServiceProvider serviceProvider;

        public ValidationBehavior(ILogger<ValidationBehavior<TRequest, TResponse>> logger, IServiceProvider serviceProvider)
        {
            this.logger = logger;
            this.serviceProvider = serviceProvider;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            IValidationHandler<TRequest> validator = serviceProvider.GetService<IValidationHandler<TRequest>>();
            if (validator != null)
            {
                var requestName = typeof(TRequest).Name;
                logger.LogInformation("Running validation for {Request}", requestName);
                var result = await validator.Validate(request);
                if (result.IsSuccessfull == false)
                {
                    logger.LogWarning("Validation failed for {Request}. Reason: {Reason}", requestName, result.ErrorMessage);
                    var response = new TResponse();
                    response.BadRequest(result);
                    return response;
                }
                logger.LogInformation("Validation successful for {Request}", requestName);
            }

            // Go to the next behaviour in the pipeline
            return await next();
        }
    }
}
