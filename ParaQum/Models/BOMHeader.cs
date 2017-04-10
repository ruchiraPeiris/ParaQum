using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ParaQum.Models
{
    public class BOMHeader
    {
        public int BomId { get; set; }
        public string ExcelFile { get; set; }
        public string AddedBy { get; set; }
        public string State { get; set; }
        public string Project { get; set; }
        public Nullable<int> OrderId { get; set; }
        public Nullable<System.DateTime> date { get; set; }
    }
}