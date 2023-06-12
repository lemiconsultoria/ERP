using ERP.Consolidator.Domain.Entities;
using ERP.Consolidator.Domain.Interfaces.Repositories;
using ERP.Consolidator.Infra.Data.Context;
using ERP.Core.Helpers;
using MongoDB.Driver;

namespace ERP.Consolidator.Infra.Data.Repositories
{
    public class EntryBalanceRepository : IEntryBalanceRepository
    {
        protected IMongoDatabase _database;

        public EntryBalanceRepository(MongoDBContext context)
        {
            _database = context.DataBase;
        }

        public void Add(EntryBalance entity)
        {
            try
            {
                var collection = _database.GetCollection<EntryBalance>(typeof(EntryBalance).Name);
                collection.InsertOne(entity);
            }
            catch (Exception ex)
            {
                LogHelper.RiseError(ex);
                throw;
            }
        }

        public EntryBalance GetByDate(DateOnly dateRef)
        {
            try
            {
                var collection = _database.GetCollection<EntryBalance>(typeof(EntryBalance).Name);

                var entity = collection.Find(x => x.ReferenceDate == dateRef).FirstOrDefault();

                return entity;
            }
            catch (Exception ex)
            {
                LogHelper.RiseError(ex);
                throw;
            }
        }

        public void Update(EntryBalance entity)
        {
            try
            {
                var collection = _database.GetCollection<EntryBalance>(typeof(EntryBalance).Name);
                collection.ReplaceOne(Builders<EntryBalance>.Filter.Eq(r => r.Id, entity.Id), entity);
            }
            catch (Exception ex)
            {
                LogHelper.RiseError(ex);
                throw;
            }
        }
    }
}