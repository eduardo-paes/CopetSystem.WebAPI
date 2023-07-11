﻿using AspNetCoreRateLimit;
using Domain.Interfaces.Repositories;
using Infrastructure.IoC.Utils;
using Infrastructure.Persistence.Context;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Services;
using Infrastructure.Services.Email.Configs;
using Infrastructure.Services.Email.Factories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Infrastructure.IoC;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        // Define valores das propriedades de configuração
        IConfiguration configuration = SettingsConfiguration.GetConfiguration();
        services.AddSingleton(configuration);

        // Carrega informações de ambiente (.env)
        var dotEnvSecrets = new DotEnvSecrets();
        services.AddScoped<IDotEnvSecrets, DotEnvSecrets>();

        #region Inicialização do banco de dados
        services.AddDbContext<ApplicationDbContext>(
            o => o.UseNpgsql(dotEnvSecrets.GetDatabaseConnectionString(),
            b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
        #endregion

        #region Serviço de Log 
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();
        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.ClearProviders();
            loggingBuilder.AddSerilog(Log.Logger, dispose: true);
        });
        #endregion

        #region Serviço de E-mail

        var smtpConfig = new SmtpConfiguration();
        configuration.GetSection("SmtpConfiguration").Bind(smtpConfig);
        smtpConfig.Password = dotEnvSecrets.GetSmtpUserPassword();
        smtpConfig.Username = dotEnvSecrets.GetSmtpUserName();
        services.AddSingleton<IEmailServiceFactory, EmailServiceFactory>();
        services.AddSingleton(sp =>
        {
            var factory = sp.GetRequiredService<IEmailServiceFactory>();
            return factory.Create(smtpConfig);
        });
        #endregion

        #region Repositórios
        services.AddScoped<IAreaRepository, AreaRepository>();
        services.AddScoped<ICampusRepository, CampusRepository>();
        services.AddScoped<ICourseRepository, CourseRepository>();
        services.AddScoped<IMainAreaRepository, MainAreaRepository>();
        services.AddScoped<INoticeRepository, NoticeRepository>();
        services.AddScoped<IProfessorRepository, ProfessorRepository>();
        services.AddScoped<IProgramTypeRepository, ProgramTypeRepository>();
        services.AddScoped<IProjectRepository, ProjectRepository>();
        services.AddScoped<ITypeAssistanceRepository, TypeAssistanceRepository>();
        services.AddScoped<IStudentRepository, StudentRepository>();
        services.AddScoped<ISubAreaRepository, SubAreaRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        #endregion

        #region Rate Limit
        services.AddMemoryCache();
        services.AddInMemoryRateLimiting();
        services.Configure<ClientRateLimitOptions>(configuration.GetSection("IpRateLimiting"));
        services.AddSingleton<IClientPolicyStore, MemoryCacheClientPolicyStore>();
        services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
        services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
        services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
        #endregion

        return services;
    }
}