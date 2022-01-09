using Microsoft.Extensions.DependencyInjection;
using PrismMasonManagement.Application.Authorization.Services;
using PrismMasonManagement.Application.Contracts.Authorization.Interfaces;
using PrismMasonManagement.Application.Contracts.PrismMasonManagementDTOs;
using PrismMasonManagement.Application.PrismMasonManagementAppServices;
using PrismMasonManagement.Infrastructure.PrismMasonManagementDomainService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace PrismMasonManagement.Api
{
    public static class PrismMasonManagementDIHandler
    {
        public static void AddPrismMasonManagementDependencies(this IServiceCollection services)
        {
            services.AddTransient<IUserRoleAppService, UserRoleAppService>();
            services.AddTransient<ICurrentUser, CurrentUser>();
            services.AddTransient<IItemAppService, ItemAppService>();
        }

        public static void AddAllTypes<T>(this IServiceCollection services
            , Assembly[] assemblies
            , bool additionalRegisterTypesByThemself = false
            , ServiceLifetime lifetime = ServiceLifetime.Transient
        )
        {
            var typesFromAssemblies = assemblies.SelectMany(a =>
                a.DefinedTypes.Where(x => x.GetInterfaces().Any(i => i == typeof(T))));
            foreach (var type in typesFromAssemblies)
            {
                services.Add(new ServiceDescriptor(typeof(T), type, lifetime));
                if (additionalRegisterTypesByThemself)
                    services.Add(new ServiceDescriptor(type, type, lifetime));
            }
        }
    }
}
