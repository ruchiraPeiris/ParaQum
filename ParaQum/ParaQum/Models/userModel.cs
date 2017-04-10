using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ParaQum.Models
{
    [Table("user")]
    public class userModel
    {
        [Key]
        public string userId { get; set; }
      
        public string userName { get; set; }
      
        public string password { get; set; }
       
        public string email { get; set; }
     
        public string contactNo { get; set; }
     
        public string inventoryManager { get; set; }
        public string admin { get; set; }
       
        public string designer { get; set; }
      
        public string other { get; set; }
    }
}