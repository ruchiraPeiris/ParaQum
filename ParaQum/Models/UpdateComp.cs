using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ParaQum.Models
{
    public class UpdateComp
    {
        public string customerRef { get; set; }
        public int paraQumStock { get; set; }
        public int ReqiredQty { get; set; }
        public int stock { get; set; }
        public int reservedQty { get; set; }
    }
}