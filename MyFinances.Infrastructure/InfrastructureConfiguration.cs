using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MyFinances.Application.Abstractions.Interfaces;
using MyFinances.Application.Abstractions.Services;
using MyFinances.Domain.Repositories;
using MyFinances.Infrastructure.Authentication;
using MyFinances.Infrastructure.Notification;
using MyFinances.Infrastructure.Persistence.Context;
using MyFinances.Infrastructure.Persistence.Interceptors;
using MyFinances.Infrastructure.Persistence.Repositories;

namespace MyFinances.Infrastructure;

public static class InfrastructureConfiguration
{
    public static IServiceCollection ConfigureInfrastructure(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddAuthentication(configuration);
        services.AddPersistence(configuration);
        services.AddRepositories();
        services.AddContext();
        services.AddNotifications(configuration);

        return services;
    }

    private static void AddAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtOptions = new JwtOptions();
        configuration.Bind(nameof(jwtOptions), jwtOptions);
        services.AddSingleton(jwtOptions);

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidateAudience = true,
                    ValidAudience = jwtOptions.Audience,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey)),
                    ValidateLifetime = true
                };
            });
        services.AddAuthorization();

        services.AddScoped<IAuthenticationService, AuthenticationService>();
    }

    private static void AddPersistence(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<SoftDeleteInterceptor>();
        services.AddScoped<UpdateInterceptor>();
        services.AddScoped<AuditInterceptor>();

        var connectionString = configuration.GetConnectionString("MyFinancesCs");
        services.AddDbContext<MyFinancesDbContext>((sp, options) =>
        {
            options.UseSqlServer(connectionString);
            options.EnableSensitiveDataLogging();
            options.AddInterceptors(
                sp.GetRequiredService<SoftDeleteInterceptor>(),
                sp.GetRequiredService<UpdateInterceptor>(),
                sp.GetRequiredService<AuditInterceptor>());
        });
    }

    private static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IFamilyRepository, FamilyRepository>();
    }

    private static void AddContext(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<IUserContext, UserContext.UserContext>();
    }

    private static void AddNotifications(this IServiceCollection services, IConfiguration configuration)
    {
        var emailServiceOptions = new EmailServiceOptions();
        configuration.Bind(nameof(emailServiceOptions), emailServiceOptions);
        services.AddSingleton(emailServiceOptions);

        services.AddScoped<IEmailService, EmailService>();
    }
}