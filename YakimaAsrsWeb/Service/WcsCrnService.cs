using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace YakimaAsrsWeb.Service
{
    public class WcsCrnService
    {
        DBService service = new DBService();
        public class PostResponse
        {
            public bool Successed { get; set; }
            public string Message { get; set; }
        }

        public async Task<DBService.DBResult> GetData()
        {
            DBService.DBResult result = new DBService.DBResult();

            result = await service.GetDbData("select a.* from WCS_CRN a order by a.ASRS_ID, a.CRN_ID ", new List<DBService.DBParameter>());

            return await Task.FromResult(result);
        }

        public async Task<DBService.DBResult> ResetCrn(Models.WcsCrn Crn, string UserID)
        {
            DBService.DBResult result = new DBService.DBResult();

            SqlConnection Conn = new SqlConnection(DBService.GetDBConnStr());
            
            try
            {
                Conn.Open();

                var cmd = Conn.CreateCommand();

                cmd.CommandText = "delete WCS_UI_COMMAND_ONLINE where CMD_ID = 1 ";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "insert into WCS_UI_COMMAND_ONLINE (ASRS_ID, CMD_ID, VAL, PARAM1, CREATE_BY) values (@ASRS_ID, @CMD_ID, @VAL, @PARAM1, @CREATE_BY) ";
                cmd.Parameters.AddWithValue("ASRS_ID", (object)Crn.ASRS_ID ?? 1);
                cmd.Parameters.AddWithValue("CMD_ID", 1);
                cmd.Parameters.AddWithValue("VAL", 1);
                cmd.Parameters.AddWithValue("PARAM1", Crn.CRN_ID);
                cmd.Parameters.AddWithValue("CREATE_BY", (object)UserID ?? "admin");

                cmd.ExecuteNonQuery();

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
