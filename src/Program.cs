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
using SPX_WEBAPI.Domain.Models;

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

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<InMemoryContext>(options => options.UseInMemoryDatabase("Spx"));

            builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            builder.Services.AddScoped(typeof(IUsersRepository), typeof(UsersRepository));
            builder.Services.AddScoped(typeof(ILogRepository), typeof(LogTxtRepository));
            builder.Services.AddTransient<InMemoryDataGenerator>();
            builder.Services.AddSingleton<Token>();
            builder.Services.AddSingleton<TokenService>();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            });


            // Add JSonPatch to use HttpPatch method
            builder.Services.AddControllers().AddNewtonsoftJson();    

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            
            app.UseHttpsRedirection();

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