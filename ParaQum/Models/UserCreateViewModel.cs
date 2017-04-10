using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ParaQum.Models
{
    [Table("User")]
    public class UserCreateViewModel
    {
        [Key]
        [Required(ErrorMessage = "user id is required")]
        public string userId { get; set; }

        [Required(ErrorMessage = "user name is required")]

        public string userName { get; set; }
        [Required(ErrorMessage = "password  is required")]

        public string password = null;

        public string email { get; set; }

        public string contactNo { get; set; }



        public Boolean inventoryManager { get; set; }
        public Boolean admin { get; set; }

        public Boolean designer { get; set; }

        public Boolean other { get; set; }

        public string emailSubject = "This is the verification code for your account";
        public string verificationCode { get; set; }
    }
}