using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YakimaAsrsWeb.Models
{
    public class WmsStk
    {
        public string AREA_NO { get; set; }
        public string WH_NO { get; set; }
        public int ASRS_ID { get; set; }
        public string FIFO_NO { get; set; }
        public string BIN_NO { get; set; }
        public string PROD_NO { get; set; }
        public string PROD_NAME { get; set; }
        public string LOT_NO { get; set; }
        public decimal QTY { get; set; }
        public string MOLD_NO { get; set; }
        public decimal STUS_CTR { get; set; }
        public string STUS_CTR_DESC { get; set; }
        public decimal PROD_TYPE { get; set; }
        public string PROD_TYPE_DESC { get; set; }
        public string UNITS { get; set; }
        public string M_WIP_NO { get; set; }
        public string SCHEDULE_REMARK { get; set; }
        public string QC_LOT { get; set; }
        public string SUPPLIER { get; set; }
        public decimal? RES_QTY { get; set; }
        public string LIST_NO { get; set; }
        public decimal? LINE_ID { get; set; }
        public string STOREIN_DATE { get; set; }
        public string PDATE { get; set; }
        public string REMARK { get; set; }
        public string AUFNR { get; set; }
        public string VORNR { get; set; }
        public string EBPM_NO { get; set; }
        public string MOLD_GROUP { get; set; }
        public string MOLD_STATUS { get; set; }
        public string VORNR_NEXT { get; set; }
        public string AREA { get; set; }
        public string SRC_BIN_NO { get; set; }
        public string MOVE_OPN { get; set; }
    }
}
