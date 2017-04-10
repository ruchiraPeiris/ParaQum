using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ParaQum.Models
{
    [Table("Order")]
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        public string OrderExcelFile { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public byte[] data { get; set; }
    }
}