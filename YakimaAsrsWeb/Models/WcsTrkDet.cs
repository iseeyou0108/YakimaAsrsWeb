using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YakimaAsrsWeb.Models
{
    public class WcsTrkDet
    {
        public int ASRS_ID { get; set; }
        public string WH_NO { get; set; }
        public string DEV_NO { get; set; }
        public string PROD_NO { get; set; }
        public string PROD_NAME { get; set; }
        public string UN { get; set; }
        public string LOT_NO { get; set; }
        public string QC_LOT { get; set; }
        public decimal PROD_TYPE { get; set; }
        public decimal QTY { get; set; }
        public decimal OUT_QTY { get; set; }
        public string MOLD_NO { get; set; }
        public string MOLD_STATUS { get; set; }
        public DateTime? SDATE { get; set; }
        public DateTime? PDATE { get; set; }
    }
}