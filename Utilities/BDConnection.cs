using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SolgisFotos.Utilities
{
    public class BDConnection
    {

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(Startup.Configuration.GetSection("ConnectionString").Value);
        }
        
    }
}
