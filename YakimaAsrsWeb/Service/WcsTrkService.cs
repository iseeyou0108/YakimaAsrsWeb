using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace YakimaAsrsWeb.Service
{
    public class WcsTrkService
    {
        DBService service = new DBService();
        public class PostResponse
        {
            public bool Successed { get; set; }
            public string Message { get; set; }
        }

        public class GetTrkSeqNoResult
        {
            public bool Successed { get; set; }
            public string Message { get; set; }
            public int SeqNo { get; set; }
        }

        public async Task<DBService.DBResult> GetData()
        {
            DBService.DBResult result = new DBService.DBResult();

            result = await service.GetDbData("select a.*, b.STEP_DESC, c.STATUS_DESC, d.OPN_DESC, e.USER_NAME " +
                "from WCS_TRK a " +
                " left join WCS_TRK_STEP_REF b on a.STEP = b.STEP " +
                " left join WCS_TRK_STATUS_REF c on a.STATUS = c.STATUS " +
                " left join WCS_TRK_OPN_REF d on a.OPN = d.OPN " +
                " left join HRS_USER e on a.CREATE_BY = e.USER_NO " +
                                             " order by a.ASRS_ID, a.SER_NO ", new List<DBService.DBParameter>());

            return await Task.FromResult(result);
        }

        public async Task<DBService.DBResult> CancelOrComplteTrk(Models.WcsTrk Item, string UserID, bool IsCancelTrk)
        {
            DBService.DBResult result = new DBService.DBResult();

            SqlConnection Conn = new SqlConnection(DBService.GetDBConnStr());

            try
            {
                Conn.Open();

                var cmd = Conn.CreateCommand();
                int STEP = IsCancelTrk == true ? 0 : 91;
                cmd.CommandText = "update WCS_TRK set STEP = @STEP where ASRS_ID = @ASRS_ID and SER_NO = @SER_NO and CREATE_DATE = @CREATE_DATE ";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("STEP", STEP);
                cmd.Parameters.AddWithValue("ASRS_ID", (object)Item.ASRS_ID ?? 1);
                cmd.Parameters.AddWithValue("SER_NO", (object)Item.SER_NO ?? DBNull.Value);
                cmd.Parameters.AddWithValue("CREATE_DATE", (object)Item.CREATE_DATE ?? DBNull.Value);
                cmd.ExecuteNonQuery();

                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = IsCancelTrk == true ? "ASRS_CANCEL_SP" : "ASRS_COMPLETE_SP";
                SqlCommandBuilder.DeriveParameters(cmd);
                cmd.Parameters["@I_SER_NO"].Value = Item.SER_NO;
                cmd.Parameters["@I_SER_NO"].Direction = System.Data.ParameterDirection.Input;

                cmd.Parameters["@O_ERR_CODE"].Value = 0;
                cmd.Parameters["@O_ERR_CODE"].Direction = System.Data.ParameterDirection.InputOutput;
                cmd.Parameters["@O_ERR_DESC"].Value = "";
                cmd.Parameters["@O_ERR_DESC"].Direction = System.Data.ParameterDirection.InputOutput;

                cmd.ExecuteNonQuery();

                if (cmd.Parameters["@O_ERR_CODE"].Value.ToString() != "0")
                    throw new Exception(cmd.Parameters["@O_ERR_DESC"].Value.ToString());

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

        public async Task<DBService.DBResult> ChangeTrkStep(Models.WcsTrk Item, string UserID, int STEP)
        {
            DBService.DBResult result = new DBService.DBResult();

            SqlConnection Conn = new SqlConnection(DBService.GetDBConnStr());

            try
            {
                Conn.Open();

                var cmd = Conn.CreateCommand();
                
                cmd.CommandText = "update WCS_TRK set STEP = @STEP where ASRS_ID = @ASRS_ID and SER_NO = @SER_NO and CREATE_DATE = @CREATE_DATE ";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("STEP", STEP);
                cmd.Parameters.AddWithValue("ASRS_ID", (object)Item.ASRS_ID ?? 1);
                cmd.Parameters.AddWithValue("SER_NO", (object)Item.SER_NO ?? DBNull.Value);
                cmd.Parameters.AddWithValue("CREATE_DATE", (object)Item.CREATE_DATE ?? DBNull.Value);
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

        /// <summary>
        /// 產生空棧板入庫工作檔
        /// </summary>
        /// <param name="Item">工作主檔</param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public async Task<DBService.DBResult> CreatePalletInTrk(Models.WcsTrk Item, string UserID)
        {
            DBService.DBResult result = new DBService.DBResult();

            SqlConnection Conn = new SqlConnection(DBService.GetDBConnStr());

            try
            {
                Conn.Open();

                var cmd = Conn.CreateCommand();

                cmd.CommandText = $"insert into WCS_TRK (ASRS_ID, WH_NO, SER_NO, CREATE_DATE, DEV_NO, BIN_NO, IO, OPN, STEP, STATUS, EMERGE, CREATE_BY ) " +
                                  $"            values (@ASRS_ID,@WH_NO,@SER_NO,@CREATE_DATE,@DEV_NO,@BIN_NO,@IO,@OPN,@STEP,@STATUS,@EMERGE,@CREATE_BY) ";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("ASRS_ID", (object)Item.ASRS_ID ?? Item.DEV_NO.Substring(0, 1));
                cmd.Parameters.AddWithValue("WH_NO","A");
                cmd.Parameters.AddWithValue("SER_NO", (object)Item.SER_NO ?? DBNull.Value);
                cmd.Parameters.AddWithValue("CREATE_DATE", (object)Item.CREATE_DATE ?? DBNull.Value);
                cmd.Parameters.AddWithValue("DEV_NO", (object)Item.DEV_NO ?? DBNull.Value);
                cmd.Parameters.AddWithValue("BIN_NO", (object)Item.BIN_NO ?? DBNull.Value);
                cmd.Parameters.AddWithValue("IO", (object)Item.IO ?? "I");
                cmd.Parameters.AddWithValue("OPN", (object)Item.OPN ?? 104);
                cmd.Parameters.AddWithValue("STEP", (object)Item.STEP ?? 0);
                cmd.Parameters.AddWithValue("STATUS", (object)Item.STATUS ?? 0);
                cmd.Parameters.AddWithValue("EMERGE", (object)Item.EMERGE ?? 0);
                cmd.Parameters.AddWithValue("CREATE_BY", (object)Item.CREATE_BY ?? UserID);
                cmd.ExecuteNonQuery();

                cmd.CommandText = $"insert into WCS_TRK_DET (ASRS_ID, WH_NO, SER_NO, CREATE_DATE, TRK_COUNT, PROD_NO, PROD_TYPE, PROD_NAME, CREATE_BY, QTY, OUT_QTY, UN) values " +
                                  $"                       (@ASRS_ID,@WH_NO,@SER_NO,@CREATE_DATE,@TRK_COUNT,@PROD_NO,@PROD_TYPE,@PROD_NAME,@CREATE_BY, 1,   0,       'PCS' )";

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("ASRS_ID", (object)Item.ASRS_ID ?? Item.DEV_NO.Substring(0, 1));
                cmd.Parameters.AddWithValue("WH_NO", "A");
                cmd.Parameters.AddWithValue("SER_NO", (object)Item.SER_NO ?? DBNull.Value);
                cmd.Parameters.AddWithValue("CREATE_DATE", (object)Item.CREATE_DATE ?? DBNull.Value);
                cmd.Parameters.AddWithValue("TRK_COUNT", 1);
                cmd.Parameters.AddWithValue("PROD_NO", "PALLET");
                cmd.Parameters.AddWithValue("PROD_TYPE", 3);
                cmd.Parameters.AddWithValue("PROD_NAME", "PALLET");
                cmd.Parameters.AddWithValue("CREATE_BY", (object)Item.CREATE_BY ?? UserID);

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

        /// <summary>
        /// 產生成品入庫工作檔
        /// </summary>
        /// <param name="Item">工作主檔</param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public async Task<DBService.DBResult> CreateProdInTrk(Models.WcsTrk Item, Models.WcsTrkDet Detail, string UserID)
        {
            DBService.DBResult result = new DBService.DBResult();

            SqlConnection Conn = new SqlConnection(DBService.GetDBConnStr());

            try
            {
                Conn.Open();

                var cmd = Conn.CreateCommand();

                cmd.CommandText = $"insert into WCS_TRK (ASRS_ID, WH_NO, SER_NO, CREATE_DATE, DEV_NO, BIN_NO, IO, OPN, STEP, STATUS, EMERGE, CREATE_BY ) " +
                                  $"            values (@ASRS_ID,@WH_NO,@SER_NO,@CREATE_DATE,@DEV_NO,@BIN_NO,@IO,@OPN,@STEP,@STATUS,@EMERGE,@CREATE_BY) ";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("ASRS_ID", (object)Item.ASRS_ID ?? Item.DEV_NO.Substring(0, 1));
                cmd.Parameters.AddWithValue("WH_NO", "A");
                cmd.Parameters.AddWithValue("SER_NO", (object)Item.SER_NO ?? DBNull.Value);
                cmd.Parameters.AddWithValue("CREATE_DATE", (object)Item.CREATE_DATE ?? DBNull.Value);
                cmd.Parameters.AddWithValue("DEV_NO", (object)Item.DEV_NO ?? DBNull.Value);
                cmd.Parameters.AddWithValue("BIN_NO", (object)Item.BIN_NO ?? DBNull.Value);
                cmd.Parameters.AddWithValue("IO", (object)Item.IO ?? "I");
                cmd.Parameters.AddWithValue("OPN", (object)Item.OPN ?? 101);        //單板入庫
                cmd.Parameters.AddWithValue("STEP", (object)Item.STEP ?? 0);
                cmd.Parameters.AddWithValue("STATUS", (object)Item.STATUS ?? 0);
                cmd.Parameters.AddWithValue("EMERGE", (object)Item.EMERGE ?? 0);
                cmd.Parameters.AddWithValue("CREATE_BY", (object)Item.CREATE_BY ?? UserID);
                cmd.ExecuteNonQuery();

                cmd.CommandText = $"insert into WCS_TRK_DET (ASRS_ID, WH_NO, SER_NO, CREATE_DATE, TRK_COUNT, PROD_NO, PROD_TYPE, PROD_NAME, CREATE_BY, QTY, OUT_QTY, UN, LOT_NO, STOREIN_DATE, QC_LOT) values " +
                                  $"                       (@ASRS_ID,@WH_NO,@SER_NO,@CREATE_DATE,@TRK_COUNT,@PROD_NO,@PROD_TYPE,@PROD_NAME,@CREATE_BY,@QTY,@OUT_QTY,@UN,@LOT_NO,@STOREIN_DATE,@QC_LOT)";

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("ASRS_ID", (object)Item.ASRS_ID ?? Item.DEV_NO.Substring(0, 1));
                cmd.Parameters.AddWithValue("WH_NO", "A");
                cmd.Parameters.AddWithValue("SER_NO", (object)Item.SER_NO ?? DBNull.Value);
                cmd.Parameters.AddWithValue("CREATE_DATE", (object)Item.CREATE_DATE ?? DBNull.Value);
                cmd.Parameters.AddWithValue("TRK_COUNT", 1);
                cmd.Parameters.AddWithValue("PROD_NO", (object)Detail.PROD_NO ?? DBNull.Value);
                cmd.Parameters.AddWithValue("PROD_TYPE", 0);
                cmd.Parameters.AddWithValue("PROD_NAME", (object)Detail.PROD_NAME ?? DBNull.Value);
                cmd.Parameters.AddWithValue("CREATE_BY", (object)Item.CREATE_BY ?? UserID);
                cmd.Parameters.AddWithValue("QTY", (object)Detail.QTY ?? 1);
                cmd.Parameters.AddWithValue("OUT_QTY", (object)Detail.OUT_QTY ?? 0);
                cmd.Parameters.AddWithValue("UN", (object)Detail.UN ?? "PCS");
                cmd.Parameters.AddWithValue("LOT_NO", (object)Detail.LOT_NO ?? DBNull.Value);
                cmd.Parameters.AddWithValue("STOREIN_DATE", (object)Detail.SDATE ?? DateTime.Now);
                cmd.Parameters.AddWithValue("QC_LOT", (object)Detail.QC_LOT ?? DBNull.Value);
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

        public async Task<GetTrkSeqNoResult> GetTrkSeqNo()
        {
            GetTrkSeqNoResult result = new GetTrkSeqNoResult() { Successed = true, Message = "", SeqNo = 0 };

            SqlConnection Conn = new SqlConnection(DBService.GetDBConnStr());
            
            try
            {
                Conn.Open();

                var cmd = Conn.CreateCommand();

                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "ASRS_TRK_SER_SP ";
                SqlCommandBuilder.DeriveParameters(cmd);
                cmd.Parameters["@O_SER_NO"].Value = 0;
                cmd.Parameters["@O_SER_NO"].Direction = System.Data.ParameterDirection.InputOutput;

                cmd.Parameters["@O_ERR_CODE"].Value = 0;
                cmd.Parameters["@O_ERR_CODE"].Direction = System.Data.ParameterDirection.InputOutput;

                cmd.Parameters["@O_ERR_DESC"].Value = "";
                cmd.Parameters["@O_ERR_DESC"].Direction = System.Data.ParameterDirection.InputOutput;

                cmd.ExecuteNonQuery();

                if (cmd.Parameters["@O_ERR_CODE"].Value.ToString() != "0")
                    throw new Exception(cmd.Parameters["@O_ERR_DESC"].ToString());

                result.SeqNo = Convert.ToInt16(cmd.Parameters["@O_SER_NO"].Value);

                cmd.Dispose();
            }
            catch (SqlException ex)
            {
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

        public string GetIODesc(string IO)
        {
            string result = string.Empty;
            switch (IO)
            {
                case "I":
                    result = "入庫";
                    break;
                case "O":
                    result = "出庫";
                    break;
                case "M":
                    result = "吊車動作";
                    break;
                case "T":
                    result = "站間移載";
                    break;
                case "C":
                    result = "庫間移載";
                    break;
                default:
                    result = IO;
                    break;
            }

            return result;
        }
    }
}
