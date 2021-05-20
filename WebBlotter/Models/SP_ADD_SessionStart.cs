using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBlotter.Models
{
    public class SP_ADD_SessionStart
    {
        public string pSessionID { get; set; }
        public int pUserID { get; set; }
        public string pIP { get; set; }
        public string pLoginGUID { get; set; }
        public string pActivity { get; set; }
        public string pData { get; set; }
        public string pURL { get; set; }
        public Nullable<DateTime> pLoginTime { get; set; }
        public Nullable<DateTime> pExpires { get; set; }
    }
}