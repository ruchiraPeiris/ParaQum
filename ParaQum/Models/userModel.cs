using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ParaQum.Models
{
    [Table("User")]
    public class userModel
    {
        [Key]
        [Required(ErrorMessage ="user id is required")]
        public int userId { get; set; }

        [Required(ErrorMessage = "user name is required")]

        [Remote("doesUserNameExist","Home",ErrorMessage ="User name exists,use a different name")]
        public string userName { get; set; }
        [Required(ErrorMessage = "password  is required")]
        [StringLength(255, ErrorMessage = "Must be between 5 to 255 characters", MinimumLength = 5)]
        public string password { get; set; }


        [Required(ErrorMessage = "email  is required")]
        [RegularExpression("^[a-zA-Z0-9_.-]+@[a-zA-Z0-9-]+.[a-zA-Z0-9-.]+$", ErrorMessage = "Invalid email")]
        public string email { get; set; }
     
        public string contactNo { get; set; }



    
        public Boolean inventoryManager { get; set; }
        public Boolean admin { get; set; }
       
        public Boolean designer { get; set; }
        [Remote("CheckPrivilege", "Home", AdditionalFields = "admin,designer,inventoryManager", ErrorMessage = "Please select user type")]
        public Boolean other { get; set; }

        public string emailSubject = "This is the password for your ParaQum account";
        public string verificationCode { get; set; }
    }
}