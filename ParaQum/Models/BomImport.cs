using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace ParaQum.Models
{
    [Table("BomImport")]
    public class BomImport
    {
        [Key]
        public int itemNo { get; set; }
        public string customerRef { get; set; }
        public string qty1 { get; set; }
        public string qty10 { get; set; }
        public string mpn { get; set; }

        private static Dbfinal db = new Dbfinal();
        public static List<BomImport> GenerateListPro()
        {
            List<BomImport> pro = db.BomImports.ToList();

            return pro;
        }
    }
}