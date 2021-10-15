using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YakimaAsrsWeb.Models
{
    public class WcsTrk
    {
        public int ASRS_ID { get; set; }
        public int SER_NO { get; set; }
        public string BIN_NO { get; set; }
        public string DEV_NO { get; set; }
        public string NEXT_DEV_NO { get; set; }
        public string CUR_DEV_NO { get; set; }
        public int STEP { get; set; }
        public int STATUS { get; set; }
        public string STEP_DESC { get; set; }
        public string STATUS_DESC { get; set; }
        public int USE_CRN_ID { get; set; }
        public int OPN { get; set; }
        public string OPN_DESC { get; set; }
        public int EMERGE { get; set; }
        public int SIZE_CHK { get; set; }
        public string IO { get; set; }
        public string IO_DESC { get; set; }
        public string CREATE_DATE { get; set; }
        public string CREATE_BY { get; set; }
        public string USER_NAME { get; set; }
    }
}
