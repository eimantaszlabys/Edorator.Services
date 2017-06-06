using Edorator.Services.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Edorator.Services.Data
{
    public class ServicesContext
    {
        private readonly IMongoDatabase _database = null;

        public ServicesContext(IOptions<Settings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            if (client != null)
                _database = client.GetDatabase(settings.Value.Database);
        }

        public IMongoCollection<Service> Services
        {
            get
            {
                return _database.GetCollection<Service>("Service");
            }
        }
    }
}