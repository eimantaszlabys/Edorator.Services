using System;
using System.Threading.Tasks;
using Edorator.Services.Contracts;
using Edorator.Services.Handlers.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Edorator.Services.Controllers
{
    [Route("api/[controller]")]
    public class ServicesController : Controller
    {
        private readonly IHandler<AddServiceRequest, AddServiceResponse> _addServiceHandler;
        private readonly IHandler<GetServicesRequest, GetServicesResponse> _getServiceHandler;

        public ServicesController(
            IHandler<AddServiceRequest, AddServiceResponse> addServiceHandler, 
            IHandler<GetServicesRequest, GetServicesResponse> getServiceHandler)
        {
            if (addServiceHandler == null) throw new ArgumentNullException(nameof(addServiceHandler));
            _addServiceHandler = addServiceHandler;
            _getServiceHandler = getServiceHandler;
        }

        [HttpGet]
        [EnableCors("EdoratorUI")]
        [Authorize("Bearer")]
        public async Task<ActionResult> Get()
        {
            GetServicesRequest request = new GetServicesRequest();

            GetServicesResponse response = await _getServiceHandler.Handle(request);

            return Ok(response);
        }
        
        
        [HttpPost]
        [EnableCors("EdoratorUI")]
        [Authorize("Bearer")]
        public async Task<ActionResult> Post([FromBody]AddServiceRequest request)
        {
            AddServiceResponse response = await _addServiceHandler.Handle(request);

            return Ok(response);
        }
    }
}
