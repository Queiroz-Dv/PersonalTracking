﻿using Atron.Application.Interfaces;
using Atron.Application.Mapping;
using Atron.Application.Services;
using Atron.Domain.Entities;
using Atron.Domain.Interfaces;
using Atron.Domain.Validations;
using Atron.Infrastructure.Context;
using Atron.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Notification.Interfaces;
using Notification.Interfaces.DTO;
using Notification.Models;

namespace Atron.Infra.IoC
{
    public static class DependencyInjectionContainerAPI
    {
        public static IServiceCollection AddInfrastructureAPI(this IServiceCollection services, IConfiguration configuration)
        {
            // O método AddScoped indica que os serviços são criados uma vez por requisição HTTP
            // O método Singleton indica que o serviço é criado uma vez para todas as requisições
            // O método Transiente indica que sempre será criado um novo serviço cada vez que for necessário

            // Como padrão vou manter o AddScoped pois atende melhor a aplicação com um todo 
            services.AddDbContext<AtronDbContext>(options =>
            // Define o provedor e a string de conexão
            options.UseSqlServer(configuration.GetConnectionString("AtronConnection"),
            // Define o asembly de onde as migrações devem ser mantidas 
            m => m.MigrationsAssembly(typeof(AtronDbContext).Assembly.FullName)));

            // Registra os repositories e services
            services.AddScoped<IDepartamentoRepository, DepartamentoRepository>();
            services.AddScoped<IDepartamentoService, DepartamentoService>();

            services.AddScoped<ICargoRepository, CargoRepository>();
            services.AddScoped<ICargoService, CargoService>();

            services.AddScoped<IUsuarioService, UsuarioService>();
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();

            services.AddScoped<ITarefaRepository, TarefaRepository>();
            services.AddScoped<ITarefaService, TarefaService>();

            services.AddScoped<ITarefaEstadoRepository, TarefaEstadoRepository>();

            services.AddScoped<ISalarioRepository, SalarioRepository>();
            services.AddScoped<ISalarioService, SalarioService>();
            services.AddScoped<IMesRepository, MesRepository>();

            // Serviços utilitários 
            services.AddAutoMapper(typeof(DomainToDtoMappingProfile));

            // Serviços de Notificação e Validação
            services.AddScoped<INotificationService, DepartamentoValidation>();
            services.AddScoped<INotificationService, CargoValidation>();
            services.AddScoped<INotificationService, UsuarioValidation>();
            services.AddScoped<INotificationService, TarefaValidation>();
            services.AddScoped<INotificationService, SalarioValidation>();

            services.AddScoped<NotificationModel<Departamento>, DepartamentoValidation>();
            services.AddScoped<NotificationModel<Cargo>, CargoValidation>();
            services.AddScoped<NotificationModel<Usuario>, UsuarioValidation>();    
            services.AddScoped<NotificationModel<Tarefa>, TarefaValidation>();
            services.AddScoped<NotificationModel<Salario>, SalarioValidation>();

            return services;
        }
    }
}