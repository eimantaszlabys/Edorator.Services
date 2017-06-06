using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Edorator.Services.Contracts
{
    public class GetServicesResponse : BaseResponse
    {
        public List<ServiceDto> Items { get; set; }
    }

    public class ServiceDto
    {
        public string Name { get; set; }

        public string Address { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public ServiceStatus Status { get; set; }
    }

    public enum ServiceStatus
    {
        Active,
        Pending,
        Error
    }
}