using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace ERP.Crud.Infra.Messaging.Tests
{
    public static class TestHelper
    {
        public static IConfigurationRoot BuildConfiguration(string testDirectory, string fileConfiguration)
        {
            return new ConfigurationBuilder()
                .SetBasePath(testDirectory)
                .AddJsonFile(fileConfiguration, optional: true)
                .Build();
        }

        public static string GetPathOfTest()
        {
            var location = typeof(TestHelper).GetTypeInfo().Assembly.Location;
            var dirPath = Path.GetDirectoryName(location);
            return dirPath ?? "";
        }
    }
}