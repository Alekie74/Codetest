using Codetest.Models;
using Microsoft.Data.Sqlite;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace Codetest.Repository
{
    public class ProductsDatabase
    {
        static string Name = "Pizza Palace";
        static string DbDir = $"C:\\{Name}";
        static string DbFile = $"{DbDir}\\{Name}.db";
        static string connectionString = $"Data Source={DbFile}";

        public static void SeedDatabase()
        {
            try
            {
                if (!Directory.Exists(DbDir))
                {
                    Directory.CreateDirectory(DbDir);
                    Logger.WriteLog("SeedDatabase", $"Database Path={DbFile}");
                }
                if (!File.Exists(DbFile))
                {
                    File.Create(DbFile).Dispose();
                }
                if (File.Exists(DbFile))
                {
                    // initialise the database  
                    using (SqliteConnection connection = new SqliteConnection(connectionString))
                    {
                        try
                        {
                            connection.Open();

                            using (SqliteCommand command = connection.CreateCommand())
                            {
                                command.CommandText = $"CREATE TABLE IF NOT EXISTS [Pizzas] ('ID' INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,'Size' TEXT,'Price' Double)"; ;
                                command.ExecuteNonQuery();
                                command.CommandText = $"CREATE TABLE IF NOT EXISTS [Toppings] ('ID' INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,'Type' TEXT,'Item' TEXT,'Size' TEXT,'Price' Double)";
                                command.ExecuteNonQuery();
                                command.CommandText = $"CREATE TABLE IF NOT EXISTS [Orders] ('ID' INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,'Pizzas' TEXT,'Toppings' TEXT,'Price' Double,'DateCreated' DATETIME)";
                                command.ExecuteNonQuery();
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.WriteLog("SeedDatabase", ex.Message);
                        }
                        finally
                        {
                            connection.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("SeedDatabase", ex.Message);
            }
        }

        public static List<Order> GetOrders(int count)
        {
            SeedDatabase();
            List<Order> orders = new List<Order>();
            string sqlQuery = count > 0 ? $"SELECT * FROM [Orders] ORDER BY ID DESC LIMIT {count}" : $"SELECT * FROM [Orders] ORDER BY ID DESC";

            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    using (SqliteCommand command = connection.CreateCommand())
                    {
                        command.CommandText = sqlQuery;

                        using (SqliteDataReader dataReader = command.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                orders.Add(new Order
                                {
                                    ID = dataReader.GetInt32(0),
                                    Pizzas = JsonConvert.DeserializeObject<Pizza>(dataReader.GetString(1)),
                                    Toppings = JsonConvert.DeserializeObject<List<Topping>>(dataReader.GetString(2)),
                                    TotalPrice = dataReader.GetDouble(3),
                                    //DateCreated = dataReader.GetDateTime(4)
                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.WriteLog("GetOrders", ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }

            return orders;
        }

        public static bool PostOrders(OrderResponse order)
        {
            SeedDatabase();
            bool posted = false;

            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                try
                {
                    string Toppings = JsonConvert.SerializeObject(order.Toppings);
                    string pizzas = JsonConvert.SerializeObject(new Pizza { Size = order.Size, Price = order.Price });
                    string sqlQuery = $"INSERT INTO [Orders] ('Pizzas', 'Toppings', 'Price', 'DateCreated') VALUES('{pizzas}', '{Toppings}', '{order.TotalPrice}', '{DateTime.Now}')";

                    connection.Open();

                    using (SqliteCommand command = connection.CreateCommand())
                    {
                        command.CommandText = sqlQuery;
                        posted = (command.ExecuteNonQuery() > 0);
                    }
                }
                catch (Exception ex)
                {
                    Logger.WriteLog("PostOrders", ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }

            return posted;
        }

        public static bool ClrearOrders()
        {
            SeedDatabase();
            bool posted = false;

            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                try
                {
                    string sqlQuery = "DELETE FROM [Orders]";

                    connection.Open();

                    using (SqliteCommand command = connection.CreateCommand())
                    {
                        command.CommandText = sqlQuery;
                        posted = (command.ExecuteNonQuery() > 0);
                    }
                }
                catch (Exception ex)
                {
                    Logger.WriteLog("PostOrders", ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }

            return posted;
        }
    }
}
