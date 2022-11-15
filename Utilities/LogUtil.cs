using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SolgisFotos.Utilities
{
    public class LogUtil
    {

        public static void LogErrorOnDb(Exception e)
        {
            var parameters = new { ServerName = "SolgisFoto", Error = e.Message ?? "", ErrorExtra = e.StackTrace ?? "" };
            const string query =
                "EXECUTE TestSolmar.dbo.AppSolgis_Log_Error @ServerName, @Error, @ErrorExtra;";
            using (var connection = BDConnection.GetConnection())
            {
                connection.Query(query, parameters);
            }
        }



    }
}
