using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementApp
{
    public class ConfigInitializer
    {
        public static IConfigurationRoot InitConfig()
        {
            var builder = new ConfigurationBuilder().AddJsonFile($"appsettings.json", true, true);
            return builder.Build();
        }
    }
}
