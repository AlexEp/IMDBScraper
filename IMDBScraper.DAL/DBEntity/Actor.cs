using IMDBScraper.Shared;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace IMDBScraper.DAL.DBEntity
{
    internal class Actor
    {
        [BsonId]
        public ObjectId InternalId { get; set; }
        //public string Id { get; set; }
        public string Name { get; set; }
        public string ImageURL { get; set; }
        public DateTime Birthday { get; set; }
        public IEnumerable<string> Roles { get; set; }
        public int Gender { get; set; }
        public bool IsHide { get; set; }
    }
}
