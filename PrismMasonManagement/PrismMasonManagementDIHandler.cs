using Microsoft.Extensions.DependencyInjection;
using PrismMasonManagement.Application.Authorization.Services;
using PrismMasonManagement.Application.Contracts.Authorization.Interfaces;
using PrismMasonManagement.Infrastructure.PrismMasonManagementDomainService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrismMasonManagement.Api
{
    public static class PrismMasonManagementDIHandler
    {
        public static void AddPrismMasonManagementDependencies(this IServiceCollection services)
        {
            services.AddTransient<IUserRoleAppService, UserRoleAppService>();
            services.AddTransient<ICurrentUser, CurrentUser>();
        }
    }
}
