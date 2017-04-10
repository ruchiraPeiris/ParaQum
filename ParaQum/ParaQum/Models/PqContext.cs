using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ParaQum.Models
{
    public class PqContext:DbContext
    {
        public DbSet<userModel> User { get; set; }

      
    }
}