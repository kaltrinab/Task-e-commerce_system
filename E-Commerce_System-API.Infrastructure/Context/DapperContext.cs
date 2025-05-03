using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Commerce_System_API.Application.Interfaces;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;

namespace E_Commerce_System_API.Infrastructure.Context
{
    public class DapperContext : IDapperContext
    {
        private readonly IConfiguration _configuration;

        public DapperContext(IConfiguration configuration)
        {
            _configuration = configuration;
          
        }

       public IDbConnection CreateConnection() 
        =>  new SqliteConnection(_configuration.GetConnectionString("DefaultConnection"));
    }
}
