using Fin.Core.Infrastructure.Db;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Fin.Core.Infrastructure;
using Fin.Core.Handlers;
using Fin.Core.Behaviours;
using System.Text.Json;
using FluentValidation.AspNetCore;
using FluentValidation;
using Fin.Api.Model;
using Fin.Api.Validation;
using Fin.Core.Domain;

namespace Fin.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<FinContext>(opt => opt.UseInMemoryDatabase("FinDB"));
            services.AddDomainInjections();
            services.AddDBInjections();
            services.AddBehaviours();
            services.AddHandlersAndValidators();
            services.AddControllers()
                .AddJsonOptions(option => option.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase)
                .AddFluentValidation();

            services.AddTransient<IValidator<CreateTransaction>, CreateTransactionValidator>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            using (var scope = app.ApplicationServices.CreateScope())
            using (var context = scope.ServiceProvider.GetService<FinContext>())
                context.Database.EnsureCreated();

        }
    }
}
