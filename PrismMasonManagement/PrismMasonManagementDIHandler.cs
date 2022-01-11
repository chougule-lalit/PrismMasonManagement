using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using PrismMasonManagement.Api.Errors;
using PrismMasonManagement.Application.Administration.Services;
using PrismMasonManagement.Application.Contracts.Administration.Interfaces;
using PrismMasonManagement.Application.Contracts.PrismMasonManagementDTOs;
using PrismMasonManagement.Application.PrismMasonManagementAppServices;
using PrismMasonManagement.Core.PrismMasonManagementCoreBase.Interface;
using PrismMasonManagement.Infrastructure.PrismMasonManagementDomainService;
using PrismMasonManagement.Infrastructure.PrismMasonManagementInfrastructureBase;
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
            services.AddScoped<ICurrentUser, CurrentUser>();
            services.AddScoped(typeof(IRepository<>), (typeof(Repository<>)));
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddTransient<IUserRoleAppService, UserRoleAppService>();
            services.AddTransient<IUserAppService, UserAppService>();
            services.AddTransient<IItemAppService, ItemAppService>();


            //Configuring Global Exception handler options for ValidationErrors
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var errors = actionContext.ModelState
                        .Where(x => x.Value.Errors.Count > 0)
                        .SelectMany(x => x.Value.Errors)
                        .Select(x => x.ErrorMessage)
                        .ToArray();

                    var errorResponse = new ApiValidationErrorResponse
                    {
                        Errors = errors
                    };

                    return new BadRequestObjectResult(errorResponse);
                };
            });
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
