using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace YakimaAsrsWeb.Service
{
    public class WcsDeviceService
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

            result = await service.GetDbData("select a.* from WCS_DEVICE a order by a.ASRS_ID, a.DEV_NO ", new List<DBService.DBParameter>());

            return await Task.FromResult(result);
        }
    }
}
