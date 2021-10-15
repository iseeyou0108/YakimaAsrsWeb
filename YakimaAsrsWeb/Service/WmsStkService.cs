using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace YakimaAsrsWeb.Service
{
    public class WmsStkService
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

            result = await service.GetDbData("select a.*,b.STUS_CTR_DESC, " +
                                             "       case when a.PROD_TYPE = '0' then '成品/原物料' when a.PROD_TYPE = '1' then '五金加工件' when a.PROD_TYPE = '2' then '模具' when a.PROD_TYPE = '3' then '載具' when a.PROD_TYPE = '4' then '暫存品' " +
                                             "       end PROD_TYPE_DESC " +
                                             "from WMS_STK a " +
                                             "left join WMS_STK_STUS_CTR_REF b on a.STUS_CTR = b.STUS_CTR " +
                                             "" +
                                             "order by a.ASRS_ID, a.BIN_NO ", new List<DBService.DBParameter>());

            return await Task.FromResult(result);
        }

        public async Task<DBService.DBResult> GetProdListData()
        {
            DBService.DBResult result = new DBService.DBResult();

            result = await service.GetDbData("select a.PROD_NO, a.PROD_NAME " +
                                             "from WMS_PROD a " +
                                             "" +
                                             "order by a.PROD_NO ", new List<DBService.DBParameter>());

            return await Task.FromResult(result);
        }

        public async Task<DBService.DBResult> DeleteWmsStks(List<Models.WmsStk> Items, string UserID)
        {
            DBService.DBResult result = new DBService.DBResult();

            SqlConnection Conn = new SqlConnection(DBService.GetDBConnStr());

            try
            {
                Conn.Open();

                var cmd = Conn.CreateCommand();

                foreach (var Item in Items)
                {
                    cmd.CommandText = "delete WMS_STK where FIFO_NO = @FIFO_NO ";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("FIFO_NO", (object)Item.FIFO_NO ?? DBNull.Value);
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = "insert into WMS_USER_OPR_HIST(ACT_PROG, ACT_REMARK, HAVE_IMG, LOG_NO, CREATE_BY, CREATE_DATE) values (@ACT_PROG,@ACT_REMARK,@HAVE_IMG,@LOG_NO,@CREATE_BY,getdate())";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("ACT_PROG", "WMS.DeleteWmsStks");
                    cmd.Parameters.AddWithValue("ACT_REMARK", $"delete WMS_STK where FIFO_NO = {Item.FIFO_NO}");
                    cmd.Parameters.AddWithValue("HAVE_IMG", "N");
                    cmd.Parameters.AddWithValue("LOG_NO", "");
                    cmd.Parameters.AddWithValue("CREATE_BY", UserID);
                    cmd.ExecuteNonQuery();
                }

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
