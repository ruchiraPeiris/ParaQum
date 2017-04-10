using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ParaQum.Models
{

    public class Dbfinal : DbContext
    {
        public DbSet<userModel> User { get; set; }
        public DbSet<BomOrderTemp> BomOrderTemps { get; set; }
        public DbSet<BomImport> BomImports { get; set; }
        public DbSet<Bom> Boms { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ComponentProject> ComponentProjects { get; set; }





    }
}