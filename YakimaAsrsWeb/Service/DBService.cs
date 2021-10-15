using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Data.SqlClient;

namespace YakimaAsrsWeb.Service
{
    public class DBService
    {
        private static string ConnStr = "Data Source=192.168.61.11;Initial Catalog=ASRS_YAKIMA;User ID=yakimaasrs;Password=yakima;";
        //private static string ConnStr = "Data Source=192.168.1.65;Initial Catalog=ASRS_YAKIMA;User ID=sa;Password=123;";
        public class DBResult
        {
            public bool Successed { get; set; }
            public string Message { get; set; }
            public int ErrorCode { get; set; }
            public int EffectRows { get; set; }
            public System.Data.DataTable Data { get; set; }

            public DBResult()
            {
                Successed = true;
                ErrorCode = 0;
                Message = "";
                EffectRows = 0;
                Data = new System.Data.DataTable();
            }
        }

        public static string GetDBConnStr()
        {
            return ConnStr;
        }

        public class DBParameter
        {
            public string ParameterName { get; set; }
            public object Value { get; set; }
        }

        /// <summary>
        /// 查詢資料庫資料
        /// </summary>
        /// <param name="SqlCmd">查詢語法</param>
        /// <param name="parameters">查詢參數</param>
        /// <returns></returns>
        public async Task<DBResult> GetDbData(string SqlCmd, List<DBParameter> parameters)
        {
            DBResult result = new DBResult();
            SqlConnection Conn = new SqlConnection(ConnStr);
            
            try
            {
                Conn.Open();

                var cmd = Conn.CreateCommand();
                cmd.CommandText = SqlCmd;
                if (parameters != null)
                {
                    if (parameters.Count > 0)
                    {
                        foreach (var Item in parameters)
                        {
                            cmd.Parameters.AddWithValue(Item.ParameterName, (object)Item.Value ?? DBNull.Value);
                        }
                    }
                }
                
                SqlDataAdapter dap = new SqlDataAdapter(cmd);

                dap.Fill(result.Data);

                dap.Dispose();
                cmd.Dispose();
            }
            catch (SqlException ex)
            {
                result.ErrorCode = ex.ErrorCode;
                result.Successed = false;
                result.Message = ex.Message;
            }
            finally
            {
                if (Conn.State == System.Data.ConnectionState.Open)
                    Conn.Close();
                Conn.Dispose();
            }

            return await Task.FromResult(result);
        }
    }
}
