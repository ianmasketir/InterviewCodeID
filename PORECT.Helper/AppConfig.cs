﻿using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace PORECT.Helper
{
    public static class AppConfig
    {
        private static IConfigurationRoot _configuration;
        public static IConfigurationRoot Configuration
        {
            get
            {
                if (_configuration != null)
                    return _configuration;

                //var basePath = Directory.GetCurrentDirectory();
                var basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                var builder = new ConfigurationBuilder().SetBasePath(basePath).AddJsonFile("appsettings.json");
                _configuration = builder.Build();
                return _configuration;
            }
        }

        public static IConfigurationRoot GetCurrentDeirectory(string directory)
        {
            if (_configuration != null)
                return _configuration;

            var builder = new ConfigurationBuilder().SetBasePath(directory).AddJsonFile("appsettings.json");
            _configuration = builder.Build();
            return _configuration;
        }

        public static AppConfiguration Config
        {
            get
            {
                return new AppConfiguration();
            }
        }

        public class AppConfiguration
        {
            public AppConfiguration()
            {
                //string basePath = Directory.GetCurrentDirectory();
                string basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                IConfigurationRoot configurationRoot = new ConfigurationBuilder()
                   .SetBasePath(basePath)
                   .AddJsonFile("appsettings.json")
                   .Build();
                GetConfig(configurationRoot);
            }

            #region Get Configuration
            private void GetConfig(IConfiguration configuration)
            {
                Environment = configuration.GetSection("Environment").Value;
                ConfigDev = configuration.GetSection("Dev").Get<Cfg_Dev>();
                ConfigProd = configuration.GetSection("PROD").Get<Cfg_Prod>();
                ConfigAPI = configuration.GetSection("API").Get<Cfg_API>();
                ConfigJwt = configuration.GetSection("Jwt").Get<Cfg_Jwt>();
            }

            public string Environment { get; set; }
            public Cfg_Dev ConfigDev { get; set; }
            public Cfg_Prod ConfigProd { get; set; }
            public Cfg_Jwt ConfigJwt { get; set; }
            public Cfg_API ConfigAPI { get; set; }

            public class Cfg_Dev
            {
                public string SQLConnection { get; set; }
            }
            public class Cfg_Prod
            {
                public string connection { get; set; }
            }
            public class Cfg_Jwt
            {
                public string Username { get; set; }
                public string Password { get; set; }
            }
            public class Cfg_BaseAPI
            {
                public string BaseUrl { get; set; }
                public string Endpoint { get; set; }
            }
            public class Cfg_API
            {
                public Cfg_APIAuth Auth { get; set; }
                public Cfg_APIUser User { get; set; }
                public Cfg_APIProduct Product { get; set; }
            }
            public class Cfg_APIAuth
            {
                public string BaseUrl { get; set; }
                public Cfg_BaseAPI GenerateKey { get; set; }
                public Cfg_BaseAPI Login { get; set; }
            }
            public class Cfg_APIUser
            {
                public string BaseUrl { get; set; }
                public Cfg_BaseAPI List { get; set; }
                public Cfg_BaseAPI Submit { get; set; }
            }
            public class Cfg_APIProduct
            {
                public string BaseUrl { get; set; }
                public Cfg_BaseAPI List { get; set; }
                public Cfg_BaseAPI Submit { get; set; }
            }
            #endregion Get Configuration

        }
    }
}