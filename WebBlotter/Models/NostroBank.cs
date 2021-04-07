using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebBlotter.Models
{
    public class NostroBank
    {
        public long ID { get; set; }
        [StringLength(50)]
        [Required(ErrorMessage = "Bank Name is Required. It cannot be empty")]
        public string BankName { get; set; }
        [Required]
        [Display(Name = "Nostro Limit")]
        [DataType(DataType.Currency, ErrorMessage = "Number Only")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal NostroLimit { get; set; }
        public string NostroDescription { get; set; }
        public bool isActive { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
    }
}