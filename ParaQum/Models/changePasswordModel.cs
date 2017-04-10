using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ParaQum.Models
{
    public class changePasswordModel
    {

        [Required(ErrorMessage = "User name is required")]
        public string userName { get; set; }

        [Required(ErrorMessage = "password is required")]
        [StringLength(255, ErrorMessage = "Must be between 5 and 255 characters", MinimumLength = 5)]
        public string password { get; set; }


        [Required(ErrorMessage = "required")]
        public string newPassword { get; set; }


        [Required(ErrorMessage = "Confirm Password is required")]
        [StringLength(255, ErrorMessage = "Must be between 5 and 255 characters", MinimumLength = 5)]
     
        [Compare("newPassword")]
        public string repeatNewPassword { get; set; }
    }
}