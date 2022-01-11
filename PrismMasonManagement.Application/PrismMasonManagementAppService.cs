using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using PrismMasonManagement.Application.Contracts;
using PrismMasonManagement.Infrastructure.PrismMasonManagementDomainService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrismMasonManagement.Application
{
    public abstract class PrismMasonManagementAppService : IPrismMasonManagementAppService, ITransientDependency
    {
        private IMapper _mapper;

        protected PrismMasonManagementAppService(
            )
        {
        }

        protected IMapper ObjectMapper => ObjectMapperRegistration(out _mapper);

        private IMapper ObjectMapperRegistration(out IMapper mapper)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<PrismMasonManagementAutoMapperProfile>();
            });

            mapper = config.CreateMapper();
            return mapper;
        }
    }
}
