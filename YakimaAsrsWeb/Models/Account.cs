using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YakimaAsrsWeb.Models
{
    public class Account
    {
        public string UserNo { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public class LoginResult
        {
            public bool Successed { get; set; }
            public string Message { get; set; }
        }
    }
}
