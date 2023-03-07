﻿using CopetSystem.Application.Interfaces;
using CopetSystem.Application.Mappings;
using CopetSystem.Application.Services;
using CopetSystem.Domain.Interfaces;
using CopetSystem.Infra.Data.Context;
using CopetSystem.Infra.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace CopetSystem.Infra.IoC;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,
            IConfiguration configuration)
    {
        #region Acesso Appsettings
        // Adicione o caminho base para o arquivo appsettings.json
        var basePath = Path.GetDirectoryName(typeof(DependencyInjection).Assembly.Location);
        var configurationBuilder = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

        // Use a configuração criada acima para ler as configurações do appsettings.json
        configuration = configurationBuilder.Build();
        #endregion

        #region Inicialização do banco de dados
        services.AddDbContext<ApplicationDbContext>(
            o => o.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
            b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
        #endregion

        #region Serviços de Log 
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();
        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.ClearProviders();
            loggingBuilder.AddSerilog(Log.Logger, dispose: true);
        });
        #endregion

        #region Serviços de Negócios
        services.AddScoped<IAreaService, AreaService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IMainAreaService, MainAreaService>();
        services.AddScoped<ISubAreaService, SubAreaService>();
        services.AddScoped<IUserService, UserService>();
        #endregion

        #region Repositórios
        services.AddScoped<IAreaRepository, AreaRepository>();
        services.AddScoped<IMainAreaRepository, MainAreaRepository>();
        services.AddScoped<ISubAreaRepository, SubAreaRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        #endregion

        #region DTO Mappers
        services.AddAutoMapper(typeof(AreaMappings));
        services.AddAutoMapper(typeof(AuthMappings));
        services.AddAutoMapper(typeof(MainAreaMappings));
        services.AddAutoMapper(typeof(SubAreaMappings));
        services.AddAutoMapper(typeof(UserMappings));
        #endregion

        return services;
    }
}

