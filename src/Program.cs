using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using SPX_WEBAPI.AuthorizationAndAuthentication;
using SPX_WEBAPI.AuthorizationAndAuthentication.Interfaces;
using SPX_WEBAPI.Filters;
using SPX_WEBAPI.Infra.Data;
using SPX_WEBAPI.Infra.Interfaces;
using SPX_WEBAPI.Infra.Repository;
using SPX_WEBAPI.Loggers;
using SPX_WEBAPI.Validators;
using System;
using System.Text;

namespace SPX_WEBAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region "Serilog Config"
            builder.Host.UseSerilog((context, configuration) =>
            {
                configuration
                    .WriteTo.Console()
                    .WriteTo.MSSqlServer(
                        context.Configuration["ConnectionStrings:DefaultConnection"],
                            sinkOptions: new MSSqlServerSinkOptions()
                            {
                                AutoCreateSqlTable = true,
                                TableName = "LogAPI"
                            });
            });
            #endregion

            #region "CORS Config"
            builder.Services.AddCors(cors => cors.AddPolicy("AllowLocalhost", policy => policy
                .WithOrigins("http://localhost:5000")
                .AllowAnyMethod()));
            #endregion

            builder.Services.AddControllers();

            #region "Fluent Validation injection"
            // Old way
            //builder.Services.AddControllers()
            //    .AddFluentValidation(config => config.RegisterValidatorsFromAssemblyContaining<UsersValidator>());

            // new way
            builder.Services
                .AddFluentValidationAutoValidation()
                .AddFluentValidationClientsideAdapters()
                .AddValidatorsFromAssemblyContaining<UsersValidator>()
                .AddValidatorsFromAssemblyContaining<SpxValidator>()
                .AddValidatorsFromAssemblyContaining<SpxDateIntervalValidator>();
            #endregion

            builder.Services.AddControllers(options =>
            {
                options.Filters.Add(typeof(ActionFilterSpxLogger));
                options.Filters.Add(typeof(CustomGlobalExceptionFilter));
            });

            // Add JSonPatch to use HttpPatch method
            builder.Services.AddControllers().AddNewtonsoftJson();

            builder.Services.AddEndpointsApiExplorer();

            #region "Swagger Config"
            builder.Services.AddSwaggerGen(option =>
            {
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Insert a token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });
            #endregion

            #region "Creating database"
            builder.Services.AddSqlServer<ApplicationDbContext>(builder.Configuration.GetConnectionString("DefaultConnection"));
            #endregion

            #region "Dependency injection"
            builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            builder.Services.AddScoped(typeof(IUsersRepository), typeof(UsersRepository));
            builder.Services.AddScoped(typeof(ILogRepository), typeof(LogTxtRepository));
            builder.Services.AddTransient<DataGenerator>();
            builder.Services.AddTransient<CustomSpxLogs<ActionFilterSpxLogger>>();
            builder.Services.AddSingleton<ITokenService, TokenService>();
            builder.Services.AddSingleton<IToken, Token>();
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

            //if (app.Environment.IsDevelopment())
            //{
            //    app.UseSwagger();
            //    app.UseSwaggerUI();
            //}

            app.UseSwagger();
            app.UseSwaggerUI();

            DatabaseManagementService.ExecuteMigration(app);

            //app.UseHttpsRedirection();

            app.UseCors("AllowLocalhost");

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            
            #region Generate Database
            var scopedFactory = app.Services.GetService<IServiceScopeFactory>();
            using (var scope = scopedFactory.CreateScope())
            {
                var service = scope.ServiceProvider.GetService<DataGenerator>();
                service.Generate();
            }
            #endregion

            app.Run();
        }
    }
}