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
using DapperTutorial.DemoConsole.Helpers;

namespace DapperTutorial.DemoConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            //BasicRead();
            //BasicReadWithoutModel();
            //SelectOneColumn();
            //SelectByLastName("Fernandez");
            //SelectByLastNameWithAnonymousParameter("Torres");
            Console.ReadKey();
        }

        private static void BasicRead()
        {
            using (IDbConnection connection = new SqlConnection(ConnectionStringHelper.GetConnectionString()))
            {
                var sql = "SELECT * FROM dbo.Customer";

                var customers = connection.Query<Customer>(sql).ToList();

                customers.ForEach(customer =>
                {
                    Console.WriteLine($"{customer.Name} {customer.LastName}");
                });
            }
        }

        private static void BasicReadWithoutModel()
        {
            using (IDbConnection connection = new SqlConnection(ConnectionStringHelper.GetConnectionString()))
            {
                var sql = "SELECT * FROM dbo.Customer";

                var customers = connection.Query(sql).ToList();

                customers.ForEach(customer =>
                {
                    Console.WriteLine($"{customer.Name} {customer.LastName}");
                });
            }
        }

        private static void SelectOneColumn()
        {
            using (IDbConnection connection = new SqlConnection(ConnectionStringHelper.GetConnectionString()))
            {
                var sql = "SELECT Name FROM dbo.Customer";

                var customers = connection.Query<Customer>(sql).ToList();

                customers.ForEach(customer =>
                {
                    Console.WriteLine($"{customer.Name} {customer.LastName}");
                });
            }
        }

        private static void SelectByLastName(string lastName)
        {
            using (IDbConnection connection = new SqlConnection(ConnectionStringHelper.GetConnectionString()))
            {
                var parameter = new DynamicParameters();
                parameter.Add("@LastName", lastName);

                var sql = "SELECT * FROM dbo.Customer WHERE LastName = @LastName";

                var customers = connection.Query<Customer>(sql, parameter).ToList();

                customers.ForEach(customer =>
                {
                    Console.WriteLine($"{customer.Name} {customer.LastName}");
                });
            }
        }

        private static void SelectByLastNameWithAnonymousParameter(string lastName)
        {
            using (IDbConnection connection = new SqlConnection(ConnectionStringHelper.GetConnectionString()))
            {
                var parameter = new { LastName = lastName };

                var sql = "SELECT * FROM dbo.Customer WHERE LastName = @LastName";

                var customers = connection.Query<Customer>(sql, parameter).ToList();

                customers.ForEach(customer =>
                {
                    Console.WriteLine($"{customer.Name} {customer.LastName}");
                });
            }
        }


    }
}
