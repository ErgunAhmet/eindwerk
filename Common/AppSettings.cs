using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Runtime;

namespace Common
{
    public class AppSettings
    {
        public AzureAdB2C AzureAdB2C { get; private set; }
        public MongoSettings Mongo { get; private set; }
        public static void InitAppSettings(IConfiguration configuration)
        {
            Instance = new AppSettings
            {
                AzureAdB2C = new AzureAdB2C(configuration),
                Mongo = new MongoSettings(configuration),
            };
        }

        public static AppSettings Instance { get; private set; }
    }



    public class AzureAdB2C
    {
        private readonly IConfiguration _configuration;

        public AzureAdB2C(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string ClientId => _configuration["AzureAdB2C:ClientId"];
        public string TenantId => _configuration["AzureAdB2C:TenantId"];
        public string Scopes => _configuration["AzureAdB2C:Scopes"];
        public string ClientSecret => _configuration["AzureAdB2C:ClientSecret"];
    }
    public class MongoSettings
    {
        private readonly IConfiguration _configuration;

        public MongoSettings(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string ConnectionString => _configuration["Mongo:ConnectionString"];
    }
}
