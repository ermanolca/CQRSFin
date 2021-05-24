using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Fin.Core.Behaviours
{

    public static class DependencyInjection
    {
        public static void AddBehaviours(this IServiceCollection services)
        {
             /**
              * Follows the order of registration while executing
              */
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        }
    }

}
