using ERP.Core.Helpers;
using ERP.Report.Domain.Entitites;
using ERP.Report.Domain.Interfaces.Repositories;
using ERP.Report.Infra.Data.Context;
using MongoDB.Driver;

namespace ERP.Report.Infra.Data.Repositories
{
    public class EntryBalanceRepository : IEntryBalanceRepository
    {
        protected IMongoDatabase _database;

        public EntryBalanceRepository(MongoDBContext context)
        {
            _database = context.DataBase;
        }

        public async Task<EntryBalance> GetByDateAsync(DateOnly dateRef)
        {
            try
            {
                var collection = _database.GetCollection<EntryBalance>(typeof(EntryBalance).Name);

                var entity = await collection.Find(x => x.ReferenceDate == dateRef).FirstOrDefaultAsync();

                return entity;
            }
            catch (Exception ex)
            {
                LogHelper.RiseError(ex);
                throw;
            }
        }

        public async Task<IList<EntryBalance>> GetByPeriodAsync(DateOnly dateStart, DateOnly dateEnd)
        {
            try
            {
                var collection = _database.GetCollection<EntryBalance>(typeof(EntryBalance).Name);

                var entities = await collection.Find(x => x.ReferenceDate >= dateStart && x.ReferenceDate <= dateEnd).ToListAsync();

                return entities;
            }
            catch (Exception ex)
            {
                LogHelper.RiseError(ex);
                throw;
            }
        }
    }
}