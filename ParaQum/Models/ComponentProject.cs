using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ParaQum.Models
{
    [Table("ComponentProject")]
    public class ComponentProject
    {

        [Key]
        public string Component_Id { get; set; }
        public int Project_Id { get; set; }
        public Nullable<int> qty { get; set; }
        public Nullable<System.DateTime> date { get; set; }

        //public virtual Component Component { get; set; }
        public Project Project { get; set; }
    }

}