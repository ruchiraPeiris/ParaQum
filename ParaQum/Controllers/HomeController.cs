using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ParaQum.Models;
using System.Configuration;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Data;
using System.IO;
using MVCEmail.Models;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;
using System.Data.Sql;
using System.Web.Helpers;
using System.Web.Security;

namespace ParaQum.Controllers
{
    public class HomeController : Controller
    {

        SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Dbfinal"].ConnectionString);

        bool doesPasswordMatch;

        Dbfinal DB = new Dbfinal();

        public ActionResult Login()
        {

            if (User.Identity.IsAuthenticated)
            {

                return RedirectToAction("Admindashboard", "ProjectManagement");

            }


            return View();
        }


        [HttpPost]
        public ActionResult Login(Login lg)
        {



            if (ModelState.IsValid)
            {
                using (Dbfinal ue = new Dbfinal())
                {





                    var log = ue.User.Where(a => a.userName.Equals(lg.userName)).FirstOrDefault();


                    if (log != null)

                        doesPasswordMatch = Crypto.VerifyHashedPassword(log.password, lg.password);




                    if (log != null && doesPasswordMatch)
                    {
                        Session["admin"] = log.admin.ToString();
                        Session["userId"] = log.userId.ToString();
                        Session["userName"] = log.userName.ToString();
                        Session["isAdmin"] = log.admin.ToString();
                        Session["isDesigner"] = log.designer.ToString();
                        Session["isIm"] = log.inventoryManager.ToString();



                        if (Session["userName"] == null)
                        {


                            return RedirectToAction("Login", "Home");

                        }



                        if (log.admin == true)
                        {

                            Session["type"] = "1";
                            return RedirectToAction("AdminDashboard", "ProjectManagement");

                        }


                        if (log.designer == true)
                        {
                            Session["type"] = "2";

                            return RedirectToAction("AdminDashboard", "ProjectManagement");
                        }

                        if (log.inventoryManager == true)
                        {
                            Session["type"] = "3";

                            return RedirectToAction("AdminDashboard", "ProjectManagement");
                        }

                        if (log.other == true)
                        {
                            Session["type"] = "4";
                            return RedirectToAction("OtherDashboard", "ProjectManagement");

                        }



                    }


                    else
                    {
                        TempData["loginError"] = "Invalid login details";

                        return RedirectToAction("Login", "Home");
                    }



                }
            }

            return View();

        }

        public ActionResult Logout()
        {


            Session.RemoveAll();

            return RedirectToAction("Login");


        }
        public ActionResult Error()
        {

            return View();


        }



        public JsonResult doesUserNameExist(string UserName)
        {
            return Json(!DB.User.Any(user => user.userName == UserName), JsonRequestBehavior.AllowGet);

        }

        public JsonResult CheckPrivilege(Boolean admin, Boolean im, Boolean designer, Boolean other)
        {


            return Json(!((admin==false)&&(im == false) &&(designer == false) &&(other == false)),JsonRequestBehavior.AllowGet);


        }














    }
}