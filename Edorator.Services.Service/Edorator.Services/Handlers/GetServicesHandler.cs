using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edorator.Services.Contracts;
using Edorator.Services.Data;
using Edorator.Services.Handlers.Base;
using Edorator.Services.Models;
using Microsoft.Extensions.Logging;

namespace Edorator.Services.Handlers
{
    public class GetServicesHandler : BaseHandler<GetServicesRequest, GetServicesResponse>
    {
        private readonly IServiceRepository _repository;

        public GetServicesHandler(
            ILoggerFactory logger,
            IServiceRepository repository
            ) : base(logger)
        {
            _repository = repository;
        }

        protected override async Task<GetServicesResponse> HandleCore(GetServicesRequest request)
        {
            List<Service> result = await _repository.GetAllServices();

            var response = new GetServicesResponse
            {
                Items = result.Select(x => new ServiceDto
                {
                    Name = x.Name,
                    Address = x.Ip,
                    Status = ServiceStatus.Active
                }).ToList()
            };

            return response;
        }
    }
}
