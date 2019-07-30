using Microsoft.Extensions.Configuration;
using System;
using System.Configuration;

namespace Sparrow.Core.Domain.Uow
{
    public class DefaultConnectionStringResolver : IConnectionStringResolver
    {
        private readonly IConfiguration _configuration;

        public DefaultConnectionStringResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public virtual string GetNameOrConnectionString(ConnectionStringResolveArgs args)
        {
            var defaultConnectionString = _configuration.GetConnectionString("Default");
            if (!string.IsNullOrWhiteSpace(defaultConnectionString))
            {
                return defaultConnectionString;
            }

            if (ConfigurationManager.ConnectionStrings["Default"] != null)
            {
                return "Default";
            }

            if (ConfigurationManager.ConnectionStrings.Count == 1)
            {
                return ConfigurationManager.ConnectionStrings[0].ConnectionString;
            }

            throw new Exception("Could not find a connection string definition for the application. Add a 'Default' connection string to application .config file.");
        }
    }
}
