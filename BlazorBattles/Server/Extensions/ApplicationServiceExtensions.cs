using DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Repositories;
using Repositories.Contracts;

namespace BlazorBattles.Server.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddMyAppServices(this IServiceCollection services, IConfiguration config)
        {


            services.AddDbContext<DataContext>(opt =>
            {
                opt.UseSqlServer(config.GetConnectionString("DefaultConnection"));
            });

            services.AddScoped<IUnitRepository, UnitRepository>();

            //var appSettingsSection = config.GetSection("APISettings");
            //services.Configure<APISettings>(appSettingsSection);

            //services.AddAutoMapper(typeof(MappingProfile));

            return services;
        }
    }
}

