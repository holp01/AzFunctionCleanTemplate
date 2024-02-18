using AzFunctionCleanTemplate.Application;
using AzFunctionCleanTemplate.Application.Interfaces;
using AzFunctionCleanTemplate.Infrastructure;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(AzFunctionCleanTemplate.Function.Startup))]

namespace AzFunctionCleanTemplate.Function
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            // Register the Dataverse context

            var configuration = builder.Services.BuildServiceProvider().GetService<IConfiguration>();
            string clientId = configuration["DataverseClientId"];
            string clientSecret = configuration["DataverseSecret"];
            string environment = configuration["DataverseEnvironment"];
            string tenantId = configuration["DataverseTenantId"];
            var connectionString = @$"Url=https://{environment}.crm4.dynamics.com;AuthType=ClientSecret;ClientId={clientId};ClientSecret={clientSecret};Authority=https://login.microsoftonline.com/{tenantId};RequireNewInstance=true";
            builder.Services.AddSingleton<Lazy<DataverseContext>>(provider => new Lazy<DataverseContext>(() =>
                new DataverseContext(connectionString)));

            // Register the generic repository for various entities
            builder.Services.AddTransient(typeof(IRepository<>), typeof(Repository<>));

            // Assuming 'ApplicationServiceReference' is a known type in the Application project
            var applicationAssembly = Assembly.GetAssembly(typeof(ApplicationServiceReference));
            var infrastuctureAssembly = Assembly.GetAssembly(typeof(InfrastructureServiceReference));

            // Register all application services that implement IApplicationBase
            var appServices = applicationAssembly.GetTypes()
                                                 .Where(t => typeof(IApplicationBase).IsAssignableFrom(t) && t.IsInterface);

            foreach (var serviceType in appServices)
            {
                var implementationType = infrastuctureAssembly.GetTypes()
                                                            .FirstOrDefault(t => serviceType.IsAssignableFrom(t) && t.IsClass);
                if (implementationType != null)
                {
                    builder.Services.AddScoped(serviceType, implementationType);
                }
            }

            builder.Services.AddAutoMapper(typeof(AutoMapperProfile));
        }
    }
}
