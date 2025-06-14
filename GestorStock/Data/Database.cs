using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using System.Data;

namespace GestorStock.Data
{
        public class Database
        {
            private static readonly string connectionString = "Host=localhost;Username=postgres;Password=462676;Database=GestorStock_001";

            public static IDbConnection GetConnection()
            {
                return new NpgsqlConnection(connectionString);
            }
        }
    
}