﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class PQDataBookSQLEntities : DbContext
    {
        public PQDataBookSQLEntities()
            : base("name=PQDataBookSQLEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Bom> Boms { get; set; }
        public virtual DbSet<BomImport> BomImports { get; set; }
        public virtual DbSet<BomOrderTemp> BomOrderTemps { get; set; }
        public virtual DbSet<Component> Components { get; set; }
        public virtual DbSet<Login_Details> Login_Details { get; set; }
        public virtual DbSet<Note> Notes { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserLogin> UserLogins { get; set; }
    }
}
