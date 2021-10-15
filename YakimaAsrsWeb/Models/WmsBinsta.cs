using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YakimaAsrsWeb.Models
{
    public class WmsBinsta
    {
        public int ASRS_ID { get; set; }
        public string WH_NO { get; set; }
        public string AREA_NO { get; set; }
        public string BIN_NO { get; set; }
        public string BIN_STA { get; set; }
        public string BIN_STA_DESC { get; set; }
        public string INHIBIT_IN_FLG { get; set; }
        public string INHIBIT_OUT_FLG { get; set; }
        public string AREA { get; set; }
        public string SIZE_CHK { get; set; }
    }
}