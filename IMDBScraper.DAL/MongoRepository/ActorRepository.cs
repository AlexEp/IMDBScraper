using IMDBScraper.DAL.Repository;
using DTO = IMDBScraper.Shared.DTO;
using DBEntity = IMDBScraper.DAL.DBEntity;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace IMDBScraper.DAL.MongoRepository
{
    public class ActorRepository : MongoRepository, IActorRepository
    {
     
        public ActorRepository(MongoDBConfig config) : base(config)
        {
  
        }

        private IMongoCollection<DBEntity.Actor> ActorsContext =>
            _db.GetCollection  <DBEntity.Actor> ("Actors");

        public async Task<bool> DeleteAsync(DTO.Actor entityToDelete)
        {
            return await DeleteAsync(entityToDelete.Id);
        }

        public async Task<bool> DeleteAsync(string id)
        {
            FilterDefinition<DBEntity.Actor> filter = Builders<DBEntity.Actor>.Filter.Eq(m => m.InternalId, ObjectId.Parse(id));
            DeleteResult deleteResult = await ActorsContext
                                                .DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged
                && deleteResult.DeletedCount > 0;
        }

        public async Task<bool> HideAsync(string id)
        {
            FilterDefinition<DBEntity.Actor> filter = Builders<DBEntity.Actor>.Filter.Eq(m => m.InternalId, ObjectId.Parse(id));
            var multiUpdateDefinition = Builders<DBEntity.Actor>.Update.Set(u => u.IsHide, true);

            UpdateResult updateResult = await ActorsContext
                                                .UpdateManyAsync(filter, multiUpdateDefinition);

            return updateResult.IsAcknowledged
                && updateResult.ModifiedCount > 0;
        }

        public async Task<IEnumerable<DTO.Actor>> GetAllAsync()
        {
            var dbEntity = await ActorsContext
                     .Find(_ => true)
                     .ToListAsync();

            var entity = _mapper.Map<IEnumerable<DTO.Actor>>(dbEntity);

            return entity;
        }

        public async Task<IEnumerable<DTO.Actor>> GetAllUnHidedAsync()
        {
             var dbEntity = await ActorsContext
                      .Find(a => !a.IsHide)
                      .ToListAsync();

            var entity  = _mapper.Map<IEnumerable<DTO.Actor>>(dbEntity);

            return entity;
        }
 

        public async Task<DTO.Actor> GetAsync(string id)
        {
            FilterDefinition<DBEntity.Actor> filter = Builders <DBEntity.Actor>.Filter.Eq(m => m.InternalId.ToString(), id);
            var dbEntity = await ActorsContext
                    .Find(filter)
                    .FirstOrDefaultAsync();

            var entity = _mapper.Map<DTO.Actor>(dbEntity);
            return entity;
        }

        public async Task InsertAsync(DTO.Actor entity)
        {
            var dbEntity = _mapper.Map<IMDBScraper.DAL.DBEntity.Actor>(entity);
            await ActorsContext.InsertOneAsync(dbEntity);
        }

        public async Task InsertAsync(IEnumerable<DTO.Actor> entities)
        {
          var dbEntities = _mapper.Map<IEnumerable<IMDBScraper.DAL.DBEntity.Actor>>(entities);
          await ActorsContext.InsertManyAsync(dbEntities);
        }

        public async Task<bool> UpdateAsync(DTO.Actor entity)
        {
            //var dbEntity = _mapper.Map<IMDBScraper.DAL.DBEntity.Actor>(entity);

            var filter = Builders<DBEntity.Actor>.Filter.Eq(s => s.Name, entity.Name);
            var actor = await ActorsContext.Find<DBEntity.Actor>(filter).FirstOrDefaultAsync();

            actor.Birthday = entity.Birthday;
            actor.Roles = entity.Roles;
            actor.ImageURL = entity.ImageURL;

            ReplaceOneResult updateResult =
                await ActorsContext
                        .ReplaceOneAsync(
                            filter: filter,
                            replacement: actor);


            return updateResult.IsAcknowledged
                    && updateResult.ModifiedCount > 0;
        }

        public async Task<bool> UnHideAllAsync()
        {
            FilterDefinition<DBEntity.Actor> filter = Builders<DBEntity.Actor>.Filter.Empty;
            var multiUpdateDefinition = Builders<DBEntity.Actor>.Update.Set(u => u.IsHide, false);

            UpdateResult updateResult = await ActorsContext
                                                .UpdateManyAsync(filter, multiUpdateDefinition);

            return updateResult.IsAcknowledged
                && updateResult.ModifiedCount > 0;
        }
    }
}
