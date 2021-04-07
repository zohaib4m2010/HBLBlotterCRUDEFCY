using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBlotter.Models
{
    public class UserPageRelation
    {
        public int UPRID { get; set; }
        public int URID { get; set; }
        public int WPID { get; set; }
        public Nullable<bool> DateChangeAccess { get; set; }
        public Nullable<bool> EditAccess { get; set; }
        public Nullable<bool> DeleteAccess { get; set; }
    }
}