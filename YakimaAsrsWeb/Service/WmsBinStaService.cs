using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace YakimaAsrsWeb.Service
{
    public class WmsBinStaService
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

            result = await service.GetDbData("select a.*,b.BIN_STA_DESC " +
                                             "from WMS_BINSTA a " +
                                             "left join WMS_BIN_STA_REF b on a.BIN_STA = b.BIN_STA " +
                                             "" +
                                             "order by a.ASRS_ID, a.BIN_NO ", new List<DBService.DBParameter>());

            return await Task.FromResult(result);
        }
    }
}
