using BlazorBattles.Server.Config;
using BlazorBattles.Server.Services;
using BlazorBattles.Server.Services.Contracts;
using DataAccess.Data;
using DataAccess.Data.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BlazorBattles.Server.Extensions
{
    public static class IdentityServiceExtensions
    {
        public static IServiceCollection AddMyIdentityServices(this IServiceCollection services, IConfiguration config)
        {

            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<DataContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options => {
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 5;
                // options.Password.RequireDigit = true;
                options.Lockout.MaxFailedAccessAttempts = 3;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(2);
                // options.SignIn.RequireConfirmedEmail = true;
                // options.SignIn.RequireConfirmedAccount = true;
            });

            services.Configure<APISettings>(config.GetSection("APISettings"));

            var apiSettings = config.GetSection("APISettings").Get<APISettings>();
            var key = Encoding.UTF8.GetBytes(apiSettings.SecretKey);

            services.AddScoped<ITokenService, TokenService>();

            //services.AddAuthentication(opt =>
            //{
            //    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //    opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            //})
            //    .AddJwtBearer(x =>
            //    {
            //        x.RequireHttpsMetadata = false;
            //        x.SaveToken = true;
            //        x.TokenValidationParameters = new TokenValidationParameters()
            //        {
            //            ValidateIssuerSigningKey = true,
            //            IssuerSigningKey = new SymmetricSecurityKey(key),
            //            ValidateAudience = true,
            //            ValidateIssuer = true,
            //            ValidAudience = apiSettings.ValidAudience,
            //            ValidIssuer = apiSettings.ValidIssuer,
            //            ClockSkew = TimeSpan.Zero
            //        };
            //    });

            return services;
        }

    }
}
