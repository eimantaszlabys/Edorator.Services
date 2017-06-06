using System;
using System.Security.Principal;
using System.Threading.Tasks;
using Edorator.Services.Contracts;
using Edorator.Services.Data;
using Edorator.Services.Handlers.Base;
using Edorator.Services.Models;
using Microsoft.Extensions.Logging;

namespace Edorator.Services.Handlers
{
    public class AddServiceHandler : BaseHandler<AddServiceRequest, AddServiceResponse>
    {
        private readonly IServiceRepository _repository;

        public AddServiceHandler(
            ILoggerFactory logger,
            IServiceRepository repository) : base(logger)
        {
            if (repository == null) throw new ArgumentNullException(nameof(repository));
            _repository = repository;
        }

        protected override async Task<AddServiceResponse> HandleCore(AddServiceRequest request)
        {
            if (String.IsNullOrEmpty(request.Name) || String.IsNullOrEmpty(request.Address))
                throw new ArgumentNullException(nameof(request));

            Service service = new Service
            {
                Name = request.Name,
                Ip = request.Address
            };
            await _repository.AddService(service);

            return new AddServiceResponse();
        }
    }
}