﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ParaQum.Models
{
    public class LoginModel
    {


        [Required(ErrorMessage = "User name is required")]
        public string userName { get; set; }

        [Required(ErrorMessage = "password is required")]
        public string password { get; set; }

        public string repeatPassword { get; set; }

        public string userId { get; set; }


        [Required(ErrorMessage = "Verification code is required")]
        public string verificationCode { get; set; }
    }
}