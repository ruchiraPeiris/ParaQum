using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ParaQum.Models
{
    [Table("Bom")]
    public class Bom
    {

        private static Dbfinal db = new Dbfinal();
        [Key]
        public int BomId { get; set; }
        public string ExcelFile { get; set; }
        public string AddedBy { get; set; }
        public string State { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public Nullable<int> OrderId { get; set; }
        public Nullable<System.DateTime> date { get; set; }
        public byte[] data { get; set; }

        public Project Project { get; set; }

        public static List<Bom> GenerateListPro()
        {
            List<Bom> pro = db.Boms.ToList();

            return pro;
        }
    }
}
