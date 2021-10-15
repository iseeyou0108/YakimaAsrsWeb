using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YakimaAsrsWeb.Models
{
    public class WcsCrn
    {
        public int ASRS_ID { get; set; }
        public int CRN_ID { get; set; }
        public string CRN_NO { get; set; }
        public int SER_NO { get; set; }
        public bool LOADING { get; set; }
        public bool CRN_READY { get; set; }
        public bool CRN_ERROR { get; set; }
        public bool CRN_AUTO { get; set; }
        public bool CRN_INITAL { get; set; }
        public string ERR_CODE { get; set; }
    }
}
