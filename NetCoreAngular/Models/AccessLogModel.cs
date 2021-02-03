using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace NetCoreAngular.Models
{
    #region "Enums"
    public enum LogType
    {
          Info = 1
        , Debug = 2
        , Error = 3
    }
    #endregion
    //
    #region "Entities"
    public class AccessLogEntity
    {
        public int      ID_column;
        public string   PageName;
        public DateTime AccessDate;
        public string   IpValue;
        public LogType  LogType;
    }
    #endregion

    #region "Classes"
    public class LogModel
    {
        #region "campos"
        private string constring;
        #endregion

        #region "Constructor"
        public LogModel(string aConnString)
        {
            this.constring = aConnString;
        }
        #endregion 

        #region "Metodos"
        //
        private string Build_6_Tsql_SelectLog(LogType logType)
        {
            return string.Format(@" SELECT 
                       [ID_column]
                      ,[PageName]
                      ,[AccessDate]
                      ,[IpValue]
                 FROM 
                       [dbo].[accessLogs] 
                 WHERE
                       [LogType] = {0}
                 order by 
                       [ID_column] desc", (uint)logType);
        }

        //
        public List<AccessLogEntity> GetAccessLog()
        {
            //
            List<AccessLogEntity> listLog = new List<AccessLogEntity>();
            //
            using (var connection = new SqlConnection(constring))
            {
                //
                connection.Open();
                //
                string tsql = Build_6_Tsql_SelectLog(LogType.Info);
                //
                using (var command = new SqlCommand(tsql, connection))
                {
                    //
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        //
                        while (reader.Read())
                        {
                            //
                            int id_Column               = Convert.ToInt32(reader["ID_Column"]);
                            string pageName             = (string)reader["PageName"];
                            System.DateTime accessDate  = (DateTime)reader["AccessDate"];
                            string ipValue              = (string)reader["IPValue"];
                            //
                            AccessLogEntity Obj         = new AccessLogEntity();
                            //
                            Obj.ID_column    = id_Column;
                            Obj.PageName     = pageName;
                            Obj.AccessDate   = accessDate;
                            Obj.IpValue      = ipValue;
                            //
                            listLog.Add(Obj);
                        }
                    }
                }
            }
            //
            return listLog;
        }
        //
        private string InsertAccessLog
        (
              string pageName
            , string ipValue
            , LogType logType
        )
            {
                //
                pageName = pageName.Replace("'", "''");
                //
                if (pageName.Length >= 128)
                    pageName = pageName.Substring(0, 128);
                //
                return string.Format(@"
                           INSERT INTO accessLogs
                           (PageName,IpValue,LogType)
                              VALUES
                           ('{0}','{1}',{2});", pageName, ipValue, (uint)logType);
            }
            //
        private void Submit_Tsql_NonQuery
            (
                SqlConnection connection,
                string tsqlSourceCode,
                string parameterName = null,
                string parameterValue = null
            )
        {
            //    
            using (var command = new SqlCommand(tsqlSourceCode, connection))
            {
                if (parameterName != null)
                {
                    command.Parameters.AddWithValue(  // Or, use SqlParameter class.
                        parameterName,
                        parameterValue);
                }
                int rowsAffected = command.ExecuteNonQuery();
                //Console.WriteLine(rowsAffected + " = rows affected.");
            }
        }
        //
        public void Log
                (
                      string msg
                    , string ipValue  = ""
                    , LogType logType = LogType.Info
                )
        {
            //
            using (var connection = new SqlConnection(constring))
            {
                //
                connection.Open();
                //
                this.Submit_Tsql_NonQuery(connection,
                   this.InsertAccessLog(
                        msg
                       , ipValue
                       , logType
                       ));
            }
            //
        }
        #endregion
    }
    #endregion
}


