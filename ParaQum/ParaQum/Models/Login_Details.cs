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
    
    public partial class Login_Details
    {
        public int Login_Id { get; set; }
        public string User_Id { get; set; }
        public string State { get; set; }
        public Nullable<System.DateTime> Date_time { get; set; }
    
        public virtual User User { get; set; }
    }
}
