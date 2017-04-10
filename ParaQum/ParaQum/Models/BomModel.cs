using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ParaQum.Models
{
    public class BomModel
    {
        

        public int orderId { get; set; }
        public string itemNo { get; set; }
        public string customerRef { get; set; }
        public string refDesignator { get; set; }
        public string qty1 { get; set; }
        public string qty10 { get; set; }
        public string description { get; set; }
        public string value { get; set; }
        public string manufacture { get; set; }
        public string mpn { get; set; }
        public string vsNo { get; set; }
        public string vs_TdComment { get; set; }
        public string parConfromation { get; set; }
    }
}