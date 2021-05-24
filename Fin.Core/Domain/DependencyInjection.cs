using Microsoft.Extensions.DependencyInjection;

namespace Fin.Core.Domain
{
    public static class DependencyInjection 
    {
        public static void AddDomainInjections(this IServiceCollection services)
        {
            // Add the database
            services.AddTransient<ICommissionCalculator, CommissionCalculator>();
        }
    }
}
