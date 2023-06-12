using ERP.Consolidator.Domain.Entities;
using ERP.Consolidator.Domain.Interfaces.Repositories;
using ERP.Consolidator.Infra.Data.Context;
using ERP.Core.Helpers;
using MongoDB.Driver;

namespace ERP.Consolidator.Infra.Data.Repositories
{
    public class DateToProcessRepository : IDateToProcessRepository
    {
        protected IMongoDatabase _database;

        public DateToProcessRepository(MongoDBContext context)
        {
            _database = context.DataBase;
        }

        public IList<DateToProcess> GetDatesToProcess()
        {
            try
            {
                var collection = _database.GetCollection<DateToProcess>(typeof(DateToProcess).Name);

                var daysOfPeriod = collection.Find(Builders<DateToProcess>.Filter.Empty).ToList();

                var daysOfPeriodDistinct = daysOfPeriod.ToList();

                return daysOfPeriod;
            }
            catch (Exception ex)
            {
                LogHelper.RiseError(ex);
                throw;
            }
        }

        public bool DateNotExists(DateOnly dateRef)
        {
            try
            {
                var collection = _database.GetCollection<DateToProcess>(typeof(DateToProcess).Name);

                var exists = collection.Find(x => x.ReferenceDate == dateRef).Any();

                return !exists;
            }
            catch (Exception ex)
            {
                LogHelper.RiseError(ex);
                throw;
            }
        }

        public void Add(DateToProcess entity)
        {
            var collection = _database.GetCollection<DateToProcess>(typeof(DateToProcess).Name);
            collection.InsertOne(entity);
        }

        public void RemoveDate(DateOnly dateRef)
        {
            var collection = _database.GetCollection<DateToProcess>(typeof(DateToProcess).Name);
            collection.DeleteMany(c => c.ReferenceDate == dateRef);
        }
    }
}