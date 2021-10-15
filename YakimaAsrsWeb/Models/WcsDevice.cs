using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YakimaAsrsWeb.Models
{
    public class WcsDevice
    {
        public int ASRS_ID { get; set; }
        public string DEV_NO { get; set; }
        public int SER_NO { get; set; }
        public string DEST { get; set; }
        public bool MANU { get; set; }
        public bool AUTO { get; set; }
        public bool AUTO_START { get; set; }
        public bool ERR { get; set; }
        public bool IDLE { get; set; }
        public bool LOAD { get; set; }
        public bool STKIN_REQ { get; set; }
        public int IO_MODE { get; set; }
        public string IN_ENABLE { get; set; }
        public string OUT_ENABLE { get; set; }
        public decimal WEIGHT { get; set; }
        public decimal SIZE_CHK { get; set; }
        public decimal OP_MODE { get; set; }
        public decimal DISPLAY_ITEM { get; set; }

    }
}
