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

        public async Task<DBService.DBResult> GetProdListData(string SearchText)
        {
            DBService.DBResult result = new DBService.DBResult();

            List<DBService.DBParameter> parameters = new List<DBService.DBParameter>();
            string strSql = string.Empty;
            strSql = "select a.PROD_NO, a.PROD_NAME from WMS_PROD A where 1 = 1 ";
            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                strSql += "and (A.PROD_NO like @SearchText or A.PROD_NAME like @SearchText ) ";
                parameters.Add(new DBService.DBParameter() { ParameterName = "SearchText", Value = string.Format("%{0}%", SearchText) });
            }
            strSql += " order by A.PROD_NO ";
            result = await service.GetDbData(strSql, parameters);

            return await Task.FromResult(result);
        }

        public async Task<DBService.DBResult> AddWmsStks(List<Models.WmsStk> Items, string UserID)
        {
            DBService.DBResult result = new DBService.DBResult();

            SqlConnection Conn = new SqlConnection(DBService.GetDBConnStr());

            try
            {
                Conn.Open();

                var cmd = Conn.CreateCommand();
                int FifoSeq = 0;
                foreach (var Item in Items)
                {
                    FifoSeq += 1;
                    Item.FIFO_NO = string.Format("{0}{1:00}", DateTime.Now.ToString("yyyyMMddHHmmssfff"), FifoSeq);
                    Item.AREA_NO = "ASRS";

                    cmd.CommandText = $"insert into WMS_STK ( FIFO_NO, ASRS_ID, AREA_NO, WH_NO, BIN_NO, STUS_CTR, PROD_TYPE, PROD_NO, LOT_NO, QC_LOT, QTY, LINE_ID, " +
                                      $"                      STOREIN_DATE, PDATE, UNITS, REMARK, AUFNR, VORNR, VORNR_NEXT, MOLD_NO, MOLD_STATUS) values (" +
                                      $"                     @FIFO_NO,@ASRS_ID,@AREA_NO,@WH_NO,@BIN_NO,@STUS_CTR,@PROD_TYPE,@PROD_NO,@LOT_NO,@QC_LOT,@QTY,@LINE_ID, " +
                                      $"                     @STOREIN_DATE,@PDATE,@UNITS,@REMARK,@AUFNR,@VORNR,@VORNR_NEXT,@MOLD_NO,@MOLD_STATUS)";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("FIFO_NO", (object)Item.FIFO_NO ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("ASRS_ID", (object)Item.ASRS_ID ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("AREA_NO", (object)Item.AREA_NO ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("WH_NO", (object)Item.WH_NO ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("BIN_NO", (object)Item.BIN_NO ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("STUS_CTR", (object)Item.STUS_CTR ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("PROD_TYPE", (object)Item.PROD_TYPE ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("PROD_NO", (object)Item.PROD_NO ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("LOT_NO", (object)Item.LOT_NO ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("QC_LOT", (object)Item.QC_LOT ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("QTY", (object)Item.QTY ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("LINE_ID", 0);
                    cmd.Parameters.AddWithValue("STOREIN_DATE", (object)Item.STOREIN_DATE ?? DateTime.Now);
                    cmd.Parameters.AddWithValue("PDATE", (object)Item.PDATE ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("UNITS", (object)Item.UNITS ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("REMARK", (object)Item.REMARK ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("AUFNR", (object)Item.AUFNR ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("VORNR", (object)Item.VORNR ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("VORNR_NEXT", (object)Item.VORNR_NEXT ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("MOLD_NO", (object)Item.MOLD_NO ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("MOLD_STATUS", (object)Item.MOLD_STATUS ?? DBNull.Value);
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = "insert into WMS_USER_OPR_HIST(ACT_PROG, ACT_REMARK, HAVE_IMG, LOG_NO, CREATE_BY, CREATE_DATE) values (@ACT_PROG,@ACT_REMARK,@HAVE_IMG,@LOG_NO,@CREATE_BY,getdate())";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("ACT_PROG", "WMS.DeleteWmsStks");
                    cmd.Parameters.AddWithValue("ACT_REMARK", $"insert into WMS_STK, FIFO_NO = {Item.FIFO_NO}, ASRS_ID = {Item.ASRS_ID}, AREA_NO = {Item.AREA_NO}, WH_NO = {Item.WH_NO}, BIN_NO = {Item.BIN_NO}");
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
