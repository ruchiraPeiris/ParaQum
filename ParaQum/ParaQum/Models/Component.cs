//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ParaQum.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Component
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Component()
        {
            this.Projects = new HashSet<Project>();
        }
    
        public string Part_Label { get; set; }
        public string Manufacturer_Part_Number { get; set; }
        public string Part_Name { get; set; }
        public string Type { get; set; }
        public string Quantity { get; set; }
        public string Added_Date { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Project> Projects { get; set; }
    }
}