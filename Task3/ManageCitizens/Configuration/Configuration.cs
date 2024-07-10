using Microsoft.Extensions.Configuration;

namespace ManageCitizens
{
    public class Configuration
    {

        public string GetConfigurationString()
        {
            IConfiguration builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();

            return builder.GetRequiredSection("AppSettings:SQLDbConnection").Value.ToString();
        }
    }
}