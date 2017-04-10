using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ParaQum.Models;
using LoginValidation;

namespace ParaQum.Controllers
{
    public class HomeController : Controller
    {
      
        public ActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Login(Login lg)
        {
         
            //if (ModelState.IsValid)
            //{
            //    using (ParaQumDBEntities ue = new ParaQumDBEntities())
            //    {

<<<<<<< HEAD
=======
            if (ModelState.IsValid)
            {
                using (PQDataBookSQLEntities ue = new PQDataBookSQLEntities())
                {
>>>>>>> 830f7c9b7a715105f5d2e0a92298c9e310bd9f59

            //        var log = ue.Users.Where(a => a.UserName.Equals(lg.username) && a.Password.Equals(lg.password)).FirstOrDefault();
            //        if (log != null)
            //        {

<<<<<<< HEAD
            //            return RedirectToAction("AdminDashboard", "Home");
=======
                    var log = ue.Users.Where(a => a.User_Name.Equals(lg.username) && a.Password.Equals(lg.password)).FirstOrDefault();
                    if (log != null)
                    {
>>>>>>> 830f7c9b7a715105f5d2e0a92298c9e310bd9f59

            //        }

            //        else {

            //            return RedirectToAction("Login", "Home");
            //        }

            //    }
            //}

            return View();

        }


        public ActionResult AdminDashboard() {
            return View();

        }
        public ActionResult AddComponent() {

            return View();


        }


    }
}