using Client_Management_Database.Mappings;
using Client_Management_Database.Models;
using Client_Management_Database.Repositories;
using Client_Management_Database.Repositories.Interfaces;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Client_Management_Database
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDataInfrastructure(this IServiceCollection services, string connectionString)
        {

            SqlMapperConfigurations.Map();

            //Contexts
            services.AddDbContext<ClientManagementDemoContext>(options =>
                options.UseSqlServer(connectionString, sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorNumbersToAdd: null);
                }));

            services.AddScoped<IClientManagementDemoContextProcedures, ClientManagementDemoContextProcedures>();
            
            //Auto mapper
            services.AddAutoMapper(cfg => { }, typeof(ClientMappingProfile).Assembly);

            //Repos
            services.AddScoped<IClientRepo, ClientRepo>();
            
            return services;

        }
    }
}
