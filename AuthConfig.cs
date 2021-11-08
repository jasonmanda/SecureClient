using System;
using System.IO;
using System.Globalization;
using Microsoft.Extensions.Configuration;

namespace SecureClient
{
    public class AuthConfig
    {
        public string InstanceId { get; set; }
        public string TenantId { get; set; }
        public string ClientId { get; set; }
        public string Authority
        {
            get
            {
                return String.Format(CultureInfo.InvariantCulture, InstanceId, TenantId);
            }
        }
        public string ClientSecret { get; set; }
        public string BaseAdress { get; set; }
        public string ResourceId { get; set; }
        public static AuthConfig ReadJsonFromFile(
            string path
        )
        {
            IConfiguration configuration;
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(path);
            configuration = builder.Build();
            return configuration.Get<AuthConfig>();
        }
    }
}
