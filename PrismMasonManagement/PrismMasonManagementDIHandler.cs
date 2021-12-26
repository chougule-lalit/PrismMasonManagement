using Microsoft.Extensions.DependencyInjection;
using PrismMasonManagement.Application.Authorization.Services;
using PrismMasonManagement.Application.Contracts.Authorization.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrismMasonManagement.Api
{
    public static class PrismMasonManagementDIHandler
    {
        public static void AddPrismMasonDependencies(this IServiceCollection services)
        {
            services.AddScoped<IUserRoleAppService, UserRoleAppService>();
        }
    }
}
