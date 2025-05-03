using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce_System_API.Application.Interfaces
{
    public interface IDapperContext
    {
        IDbConnection CreateConnection();
    }
}
