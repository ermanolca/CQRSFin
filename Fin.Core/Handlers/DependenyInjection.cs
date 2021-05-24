using MediatR;
using Microsoft.Extensions.DependencyInjection;
using static Fin.Core.Handlers.Command.CreateAdjustmentTransaction;
using static Fin.Core.Handlers.Command.CreatePaymentTransaction;
using static Fin.Core.Handlers.Query.GetAccountBalance;

namespace Fin.Core.Handlers
{
    public static class DependencyInjection
    {
        public static void AddHandlersAndValidators(this IServiceCollection services)
        {
            // Add MediatR - This adds all of the command and query handlers
            services.AddMediatR(typeof(DependencyInjection).Assembly);

            services.AddTransient<Models.IValidationHandler<GetAccountBalanceRequest>, GetAccountBalanceValidationHandler>();
            services.AddTransient<Models.IValidationHandler<CreatePaymentTransactionRequest>, CreatePaymentTransactionRequestValidationHandler>();
            services.AddTransient<Models.IValidationHandler<CreateAdjustmentTransactionRequest>, CreateAdjustmentTransactionRequestValidationHandler>();
        }
    }
}