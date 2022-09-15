using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using SPX_WEBAPI.Filters;
using SPX_WEBAPI.Infra.Data;
using SPX_WEBAPI.Infra.Interfaces;
using SPX_WEBAPI.Infra.Repository;
using System.Text;
using System;
using SPX_WEBAPI.AuthorizationAndAuthentication;
using Microsoft.AspNetCore.Authorization;

namespace SPX_WEBAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddControllers(options => {
                options.Filters.Add(typeof(ActionFilterSpxLogger));
            });

            // Add JSonPatch to use HttpPatch method
            builder.Services.AddControllers().AddNewtonsoftJson();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            #region "Creating database"
            builder.Services.AddDbContext<InMemoryContext>(options => options.UseInMemoryDatabase("Spx"));
            #endregion

            #region "Dependency injection"
            builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            builder.Services.AddScoped(typeof(IUsersRepository), typeof(UsersRepository));
            builder.Services.AddScoped(typeof(ILogRepository), typeof(LogTxtRepository));
            builder.Services.AddTransient<InMemoryDataGenerator>();
            builder.Services.AddSingleton<Token>();
            builder.Services.AddSingleton<TokenService>();
            #endregion

            #region "Authentication and Authorization"
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ClockSkew = TimeSpan.Zero,
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidIssuer = builder.Configuration["Token:Issuer"],
                    ValidAudience = builder.Configuration["Token:Audience"],
                    IssuerSigningKey = 
                        new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["Token:Secret"]))
                };
            });

            // Add Authorization by default for all endpoints without defining an attribute on each controller
            builder.Services.AddAuthorization(options =>
            {
                options.FallbackPolicy = new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser()
                    .Build();
            });
            #endregion

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            
            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            #region Generate In Memory Database
            var scopedFactory = app.Services.GetService<IServiceScopeFactory>();
            using (var scope = scopedFactory.CreateScope())
            {
                var service = scope.ServiceProvider.GetService<InMemoryDataGenerator>();
                service.Generate();
            }
            #endregion

            app.Run();
        }
    }
}