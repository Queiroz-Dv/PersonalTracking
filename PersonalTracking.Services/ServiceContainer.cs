﻿using BLL.Interfaces;
using BLL.Services;
using DAL;
using DAL.DAO;
using DAL.Interfaces;
using DAL.Repositories;
using HLP.Entity;
using HLP.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Web.Http;
using Unity;
using Unity.WebApi;

namespace PersonalTracking.Services
{
    public class ServiceContainer
    {
        public static IServiceCollection AddDesktopServices()
        {
            var services = new ServiceCollection();

            // Registre os serviços desktop aqui
            services.AddScoped<IEmployeeRepository<EMPLOYEE>, EmployeeRepository>();
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<IEntityMessages, InformationMessage>();
            services.AddScoped<IDepartmentService, DepartmentService>();
            services.AddScoped<IPositionRepository<POSITION>, PositionRepository>();
            services.AddScoped<Context>();

            return services;
        }

        public static void AddWebServices()
        {
            var container = new UnityContainer();
            // Registra os serviços web aqui
            container.RegisterType<IDepartmentService, DepartmentService>();
            container.RegisterType<IDepartmentRepository, DepartmentRepository>();
            container.RegisterType<IEntityMessages, InformationMessage>();

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}
