using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ParaQum.Models
{
    [Table("BomOrderTemp")]
    public class BomOrderTemp
    {
        [Key]
        public int itemNo { get; set; }
        public string customerRef { get; set; }
        public string qty1 { get; set; }
        public string qty10 { get; set; }
        public string mpn { get; set; }

        private static Dbfinal db = new Dbfinal();
        public static List<BomOrderTemp> GenerateListPro()
        {
            List<BomOrderTemp> pro = db.BomOrderTemps.ToList();

            return pro;
        }
    }
}