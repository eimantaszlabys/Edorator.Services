using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Edorator.Services.Models
{
    public class Service
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public DateTime UpdatedOn { get; set; } = DateTime.Now;
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public string   CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public string Name { get; set; }
        public string Ip { get; set; }
    }
}