using Fin.Core.Infrastructure.Db;
using Fin.Core.Infrastructure.Db.Repository;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fin.Core.Infrastructure
{
    public static class DependencyInjection 
    {
        public static void AddDBInjections(this IServiceCollection services)
        {
            // Add the database
            services.AddTransient<IAccountRepository, AccountRepository>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
        }
    }
}
