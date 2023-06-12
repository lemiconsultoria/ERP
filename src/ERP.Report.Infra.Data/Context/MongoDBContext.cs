using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace ERP.Report.Infra.Data.Context
{
    public class MongoDBContext
    {
        public readonly IMongoDatabase DataBase;

        public MongoDBContext(IConfiguration configuration)
        {
            string connectionString = configuration.GetSection("MongoDB").GetSection("ConnectionString").Value ?? ""; ;

            var mongoUrl = new MongoUrl(connectionString);
            var dbname = mongoUrl.DatabaseName;
            DataBase = new MongoClient(mongoUrl).GetDatabase(dbname);
        }
    }
}