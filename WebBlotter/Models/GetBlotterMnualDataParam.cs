using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBlotter.Models
{
    public class GetBlotterMnualDataParam
    {
        public int BR { get; set; }
        public System.DateTime DateFor { get; set; }
        public bool Recon { get; set; }
    }
}