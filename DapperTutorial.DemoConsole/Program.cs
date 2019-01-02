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

            //Console.WriteLine("Ingrese un nombre o apellido a buscar");
            //var term = Console.ReadLine();
            //ExecuteStoreProcedure(term);

            //BasicInsert("Leandro", "Layerle");
            //CountRowsAffected();
            //InsertGroupOfCustomers(GetCustomers());

            // ----- ADVANCED --------------------------------
            //MapTablesWithForeingKey();
            //GetMultipleTables();
            //GetMultipleTablesWithParameters("Fernandez", "1235");
            GetOutputParameters("Leandro", "Layerle");

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

        private static void ExecuteStoreProcedure(string term)
        {
            using (IDbConnection connection = new SqlConnection(ConnectionStringHelper.GetConnectionString()))
            {
                var parameter = new DynamicParameters();
                parameter.Add("@searchTerm", term);

                var sql = "dbo.SearchCustomer";

                var customers = connection.Query<Customer>(sql,
                    parameter,
                    commandType: CommandType.StoredProcedure)
                    .ToList();

                customers.ForEach(customer =>
                {
                    Console.WriteLine($"{customer.CustomerId} {customer.Name} {customer.LastName}");
                });
            }
        }

        private static void BasicInsert(string name, string lastName)
        {
            using (IDbConnection connection = new SqlConnection(ConnectionStringHelper.GetConnectionString()))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Name", name);
                parameters.Add("@LastName", lastName);

                var sql = @"INSERT INTO Customer (Name,LastName)
                            VALUES (@Name, @LastName)";

                connection.Execute(sql, parameters);

                SelectByLastName(lastName);
            }
        }

        private static void CountRowsAffected()
        {
            using (IDbConnection connection = new SqlConnection(ConnectionStringHelper.GetConnectionString()))
            {
                var sql = @"UPDATE dbo.Customer SET LastName = UPPER(LastName)";

                var rowsAffected = connection.Execute(sql);

                Console.Write($"Filas Afectadas por el update: {rowsAffected}");
            }
        }

        private static void InsertGroupOfCustomers(List<Customer> customers)
        {
            using (IDbConnection connection = new SqlConnection(ConnectionStringHelper.GetConnectionString()))
            {
                var sql = "INSERT INTO dbo.Customer (Name,LastName) VALUES(@Name, @LastName)";

                connection.Execute(sql, customers);

                BasicRead();
            }
        }

        private static void MapTablesWithForeingKey()
        {
            using (IDbConnection connection = new SqlConnection(ConnectionStringHelper.GetConnectionString()))
            {
                var sql = @"SELECT CU.*, 
                                   AD.* 
                            FROM dbo.Customer CU 
                            LEFT JOIN dbo.Address AD 
                            ON CU.AddressId = AD.AddressId";

                var customers = connection.Query<Customer, Address, Customer>(sql,
                    (customer, address) => { customer.Address = address; return customer; }, splitOn: "AddressId").ToList();

                customers.ForEach(customer =>
                {
                    Console.WriteLine($"Name: {customer.Name} LastName: {customer.LastName} Address: {customer.Address?.Street}-{customer.Address?.Number}");
                });
            }
        }

        private static void GetMultipleTables()
        {
            using (IDbConnection connection = new SqlConnection(ConnectionStringHelper.GetConnectionString()))
            {
                var sql = @"SELECT * FROM Customer;
                            SELECT * FROM Address;";

                var customers = new List<Customer>();
                var addresses = new List<Address>();

                using (var lists = connection.QueryMultiple(sql))
                {
                    customers = lists.Read<Customer>().ToList();
                    addresses = lists.Read<Address>().ToList();
                }

                Console.WriteLine("---CLIENTES---");
                customers.ForEach(customer => { Console.WriteLine($"{customer.Name} {customer.LastName}"); });
                Console.WriteLine();
                Console.WriteLine("---DOMICILIO---");
                addresses.ForEach(address => { Console.WriteLine($"{address.Street} {address.Number}"); });
            }
        }

        private static void GetMultipleTablesWithParameters(string lastName, string addressTerm)
        {
            using (IDbConnection connection = new SqlConnection(ConnectionStringHelper.GetConnectionString()))
            {
                var sql = @"SELECT * FROM Customer WHERE LastName = @LastName;
                            SELECT * FROM Address WHERE Street LIKE '%' + @address + '%'
                                                  OR Number LIKE '%' + @address + '%';";

                var parameters = new { LastName = lastName, Address = addressTerm
 };

                var customers = new List<Customer>();
                var addresses = new List<Address>();

                using (var lists = connection.QueryMultiple(sql, parameters))
                {
                    customers = lists.Read<Customer>().ToList();
                    addresses = lists.Read<Address>().ToList();
                }

                Console.WriteLine("---CLIENTES---");
                customers.ForEach(customer => { Console.WriteLine($"{customer.Name} {customer.LastName}"); });
                Console.WriteLine();
                Console.WriteLine("---DOMICILIO---");
                addresses.ForEach(address => { Console.WriteLine($"{address.Street} {address.Number}"); });
            }

        }

        private static void GetOutputParameters(string name, string lastName)
        {
            using (IDbConnection connection = new SqlConnection(ConnectionStringHelper.GetConnectionString()))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Id", 0, DbType.Int32, ParameterDirection.Output);
                parameters.Add("@Name", name);
                parameters.Add("@LastName", lastName);

                var sql = @"INSERT INTO dbo.Customer (Name, LastName)
                            VALUES (@Name, @LastName)
                            SELECT @Id = @@IDENTITY";

                connection.Execute(sql, parameters);

                var identity = parameters.Get<int>("@Id");

                Console.WriteLine($"El nuevo id de Cliente es: {identity}");
            }
        }

        private static List<Customer> GetCustomers()
        {
            var customers = new List<Customer>();

            for (int i = 0; i < 5; i++)
            {
                customers.Add(new Customer { Name = $"Name{i}", LastName = $"LastName{i}" });
            }

            return customers;
        }
    }
}
