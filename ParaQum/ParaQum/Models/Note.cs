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
    
    public partial class Note
    {
        public int Note_Id { get; set; }
        public string Added_Date { get; set; }
        public Nullable<int> Project_Id { get; set; }
        public string Description { get; set; }
    
        public virtual Project Project { get; set; }
    }
}
