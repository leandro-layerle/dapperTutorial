using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DapperTutorial.DemoConsole.Helpers
{
    public static class ConnectionStringHelper
    {
        public static string GetConnectionString(string name = "DapperTutorialDB")
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }
    }
}
