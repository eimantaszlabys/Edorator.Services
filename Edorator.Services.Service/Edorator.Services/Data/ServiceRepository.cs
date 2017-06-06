using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using Edorator.Services.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Edorator.Services.Data
{
    public interface IServiceRepository
    {
        Task<List<Service>> GetAllServices();
        Task AddService(Service item);
    }

    public class ServiceRepository : IServiceRepository
    {
        private readonly ServicesContext _context = null;

        private readonly IPrincipal _principal;

        public ServiceRepository(IOptions<Settings> settings, IPrincipal principal)
        {
            _principal = principal;
            _context = new ServicesContext(settings);
        }

        public Task<List<Service>> GetAllServices()
        {
            try
            {
                return _context.Services.Find(service => service.CreatedBy == _principal.Identity.Name) .ToListAsync();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task AddService(Service item)
        {
            try
            {
                item.CreatedBy = _principal.Identity.Name;

                await _context.Services.InsertOneAsync(item);
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }
    }
}
