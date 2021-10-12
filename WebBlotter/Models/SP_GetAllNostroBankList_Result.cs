using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBlotter.Models
{
    public class SP_GetAllNostroBankList_Result
    {
        public int ID { get; set; }
        public string BankName { get; set; }
        public Nullable<decimal> NostroLimit { get; set; }
        public string NostroDescription { get; set; }
        public Nullable<bool> isActive { get; set; }
        public int CurId { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
    }
}