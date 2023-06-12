using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Context;
using Serilog.Core;
using Serilog.Sinks.MariaDB;
using Serilog.Sinks.MariaDB.Extensions;

namespace ERP.Core.Helpers
{
    public static class LogHelper
    {
        private static Logger? Logger;

        private static LoggerConfiguration? _configuration;

        public static void Register(string serilogUrlMysql = "")
        {
            var configuration = new LoggerConfiguration()
               .Enrich.FromLogContext()
               .MinimumLevel.Debug();

            if (string.IsNullOrEmpty(serilogUrlMysql))
                configuration.WriteTo.Console();
            else
                configuration.WriteTo.Async(async => async.MariaDB(
                    connectionString: serilogUrlMysql,
                    tableName: "log_serilog",
                    autoCreateTable: true,
                    useBulkInsert: false,
                    options: new MariaDBSinkOptions()
                ));

            _configuration = configuration;
            Logger = _configuration.CreateLogger();
        }

        public static void RiseError(Exception ex, string errorCode)
        {
            if (Logger != null && _configuration != null)
            {
                LogContext.PushProperty("ErrorCode", errorCode);
                Logger.Error(ex, ex.Message);
            }
        }

        public static void RiseError(Exception ex)
        {
            RiseError(ex, "");
        }

        public static void RegisterLogHelper(this IConfiguration configuration)
        {
            var connection = configuration.GetConnectionString("SerilogMySQL");

            if (!string.IsNullOrEmpty(connection))
                LogHelper.Register(connection);
            else
                LogHelper.Register();
        }
    }
}