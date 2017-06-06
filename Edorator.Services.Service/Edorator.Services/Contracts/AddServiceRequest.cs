using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Edorator.Services.Contracts
{
    public class AddServiceRequest : BaseRequest
    {
        [Required]
        [StringLength(30, MinimumLength = 5)]
        public string Name { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 5)]
        public string Address { get; set; }
    }
}