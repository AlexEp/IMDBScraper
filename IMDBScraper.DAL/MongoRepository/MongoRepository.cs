using AutoMapper;
using IMDBScraper.DAL.AutoMapper;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace IMDBScraper.DAL.MongoRepository
{
    public class MongoRepository
    {
        protected MongoDBConfig _config;
        protected readonly IMongoDatabase _db;

        protected static readonly IMapper _mapper;

        static MongoRepository()
        {
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AutoMapperProfiles());
            });

            _mapper = mapperConfig.CreateMapper();
        }

        public MongoRepository(MongoDBConfig config)
        {
            _config = config;
            var client = new MongoClient(config.ConnectionString);
            _db = client.GetDatabase(config.Database);
        }


    }
}
