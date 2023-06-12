using ERP.Consolidator.Domain.Entities;
using ERP.Consolidator.Domain.Interfaces.Repositories;
using ERP.Consolidator.Infra.Data.Context;
using MongoDB.Driver;

namespace ERP.Consolidator.Infra.Data.Repositories.Base
{
    public class EntryRepositoryBase<T> : IEntryRepositoryBase<T> where T : Entry
    {
        protected IMongoDatabase _database;

        public EntryRepositoryBase(MongoDBContext context)
        {
            _database = context.DataBase;
        }

        public void Add(T entity)
        {
            var collection = _database.GetCollection<T>(typeof(T).Name);
            collection.InsertOne(entity);
        }

        public void Delete(T entity)
        {
            var collection = _database.GetCollection<T>(typeof(T).Name);
            collection.DeleteOne(c => c.Id == entity.Id);
        }

        public void Update(T entity)
        {
            var collection = _database.GetCollection<T>(typeof(T).Name);
            collection.ReplaceOne(Builders<T>.Filter.Eq(r => r.Id, entity.Id), entity);
        }

        public T GetById(long id)
        {
            var collection = _database.GetCollection<T>(typeof(T).Name);
            return collection.Find(x => x.Id == id).FirstOrDefault();
        }

        public decimal SumAllByDate(DateOnly dateRef)
        {
            var collection = _database.GetCollection<T>(typeof(T).Name);
            return collection.Find(x => x.DateRef == dateRef).ToList().Sum(x => x.Value);
        }
    }
}