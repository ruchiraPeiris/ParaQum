using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace ParaQum.Models
{
    class BomTb
    {
        public int BomId { get; set; }
        public string ExcelFile { get; set; }
        public string AddedBy { get; set; }
        public string State { get; set; }
        public string Project { get; set; }
        public Nullable<int> OrderId { get; set; }
        public DateTime? date { get; set; }
    }
}
