using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using E_Commerce_System_API.Application.Interfaces;

namespace E_Commerce_System_API.Infrastructure.Context
{
    public class DatabaseInitializer
    {
        public static void RunSqlScript(IDapperContext context, string scriptPath)
        {   
            //this was used to initialize the database .sql file  
            
            //string fullPath = Path.GetFullPath(scriptPath);

            //try
            //{
            //    string sql = File.ReadAllText(fullPath);
            //    using IDbConnection conn = context.CreateConnection();
            //    conn.Open();
            //    conn.Execute(sql);
            //    Console.WriteLine("Sql script executed successfully.");
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine("Error executing sql script:");
            //    Console.WriteLine(ex.Message);
            //}
        }
    }
}
