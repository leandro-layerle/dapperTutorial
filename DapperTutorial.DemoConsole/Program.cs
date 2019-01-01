using Dapper;
using DapperTutorial.DemoConsole.Domain;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DapperTutorial.DemoConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            BasicRead();
            Console.ReadKey();
        }

        private static void BasicRead()
        {
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DapperTutorialDB"].ConnectionString))
            {
                var sql = "SELECT * FROM dbo.Customer";

                var customers = connection.Query<Customer>(sql).ToList();

                customers.ForEach(customer =>
                {
                    Console.WriteLine($"{customer.Name} {customer.LastName}");
                });
            }
        }
    }
}
