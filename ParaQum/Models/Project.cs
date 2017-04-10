using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ParaQum.Models
{
    [Table("Project")]
    public class Project
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Project()
        {
            this.Boms = new HashSet<Bom>();
            this.ComponentProjects = new HashSet<ComponentProject>();
            //this.Notes = new HashSet<Note>();
        }
        [Key]
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        //public string ClientDocument { get; set; }
        public string ProjectDesign { get; set; }
        public string ProjectFolder { get; set; }
        public Nullable<System.DateTime> Date_time { get; set; }
        public string UserId { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<Bom> Boms { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<ComponentProject> ComponentProjects { get; set; }
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        // public virtual ICollection<Note> Notes { get; set; }
    }
}