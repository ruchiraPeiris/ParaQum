
using DocumentFormat.OpenXml.Drawing;
using Microsoft.AspNet.Identity;
using ParaQum.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;
using UpdateStock;
using Path = System.IO.Path;



namespace ParaQum.Controllers
{
    public class UsersController : Controller
    {


        bool doesPasswordMatch;
        Dbfinal Db = new Dbfinal();

        // GET: Users
        public ActionResult Index()
        {

            return View(Db.User);
        }
        public ActionResult Create()
        {


            return View();


        }





        [HttpPost]
        public ActionResult Create(userModel usermodel)
        {




            Random randomno = new Random();
            usermodel.verificationCode = randomno.Next(1000, 9000).ToString();




            var hashedPassword = Crypto.HashPassword(usermodel.password);




            if ((usermodel.admin == false) && (usermodel.designer == false) && (usermodel.inventoryManager == false) && (usermodel.other == false))
            {

                TempData["checkBoxNullMsg"] = "Failed,All checkboxes cannot be null";


                return RedirectToAction("Index");




            }

            string constr = ConfigurationManager.ConnectionStrings["Dbfinal"].ConnectionString;


            using (SqlConnection con = new SqlConnection(constr))
            {

                try
                {
                    string sqlCommand = "insert into [dbo].[User](UserName,Password,email,contactNo,InventoryManager,Admin,Designer,Other,verificationCode)values('" + usermodel.userName + "','" + hashedPassword + "','" + usermodel.email + "','" + usermodel.contactNo + "','" + usermodel.inventoryManager + "','" + usermodel.admin + "','" + usermodel.designer + "','" + usermodel.other + "','" + usermodel.verificationCode + "')";


                    SqlCommand command = new SqlCommand(sqlCommand, con);
                    con.Open();
                    command.ExecuteNonQuery();
                    TempData["createmsg"] = "New user added succesfuly";





                }

                catch (Exception ex)
                {


                    ViewBag.message = "An error ocured while adding  user";


                }
            }





            //Db.User.Add(usermodel);
            //Db.SaveChanges();



            SendMail(usermodel);
            return RedirectToAction("index", "Users");



        }

        private void SendMail(userModel um)
        {

            if (ModelState.IsValid)
            {
                string smtpUserName = "vqimsystem@gmail.com";
                string smtpPassword = "imsystemvq";
                string smtpHost = "smtp.gmail.com";
                int smtpPort = 25;

                string emailTo = um.email;
                string subject = um.emailSubject;
                string body = string.Format("User name: <b>{0}</b><br/>Email: {1}<br/>Password: </br>{2}",
                    um.userName, um.email, um.password);

                EmailService service = new EmailService();

                bool kq = service.Send(smtpUserName, smtpPassword, smtpHost, smtpPort,
                    emailTo, subject, body);

                if (kq) ModelState.AddModelError("", "Password sent successfully");
                else ModelState.AddModelError("", "Error occured");
            }




        }

        //Added on 3/16/2017
        public ActionResult changePassword()
        {

            return View();


        }

        [HttpPost]

        public ActionResult changePassword(changePasswordModel cpm)
        {


            if (ModelState.IsValid)
            {

                using (Dbfinal pc = new Dbfinal())
                {

                    var log = pc.User.Where(a => a.userName.Equals(cpm.userName)).FirstOrDefault();


                    if (log != null)

                        doesPasswordMatch = Crypto.VerifyHashedPassword(log.password, cpm.password);


                    if (log != null && doesPasswordMatch)
                    {


                        string constr = ConfigurationManager.ConnectionStrings["Dbfinal"].ConnectionString;

                        var hashedPassword = Crypto.HashPassword(cpm.newPassword);
                        using (SqlConnection con = new SqlConnection(constr))
                        {


                            try
                            {
                                string sqlCommand = "update [dbo].[User] set Password='" + hashedPassword + "' where userName='" + cpm.userName + "'";

                                SqlCommand command = new SqlCommand(sqlCommand, con);
                                con.Open();
                                command.ExecuteNonQuery();
                                TempData["createmsg"] = "Password updated Succesfully";

                            }
                            catch (Exception EX)
                            {


                                TempData["createmsg"] = "Error Occured";

                            }





                        }








                    }


                    else
                    {
                        TempData["createmsg"] = "Error Occured";



                    }




                }







            }


            return View();


        }








        [HttpPost]
        public ActionResult edit(userModel user)
        {//update user

            Dbfinal db = new Dbfinal();
            userModel student_to_update = db.User.SingleOrDefault(s => s.userId == user.userId);
            student_to_update.userName = user.userName;
            student_to_update.email = user.email;
            student_to_update.contactNo = user.contactNo;
            student_to_update.inventoryManager = user.inventoryManager;
            student_to_update.admin = user.admin;
            student_to_update.designer = user.designer;
            student_to_update.other = user.other;
            db.SaveChanges();
            ModelState.AddModelError("", "User updated successfully");

            return View(student_to_update);


        }

        public ActionResult Edit(int id)
        {//display user informations

            Dbfinal db = new Dbfinal();
            userModel user = db.User.SingleOrDefault(a => a.userId == id);
            return View(user);




        }

        [HttpPost, ActionName("Delete")]
        public ActionResult Delete_conf(int id)
        {


            Dbfinal db = new Dbfinal();
            userModel student_to_delete = db.User.Find(id);

            db.User.Remove(student_to_delete);//remove a raw from the user table
            db.SaveChanges();

            ModelState.AddModelError("", "User deleted successfully");

            return View(student_to_delete);






        }

        public ActionResult Delete(int id)
        {

            if (id == 0)
            {

                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);


            }

            Dbfinal db = new Dbfinal();
            userModel user = db.User.SingleOrDefault(a => a.userId == id);
            return View(user);



        }

        public ActionResult ForgotPassword()
        {


            return View();


        }


        public ActionResult ChangeProPic()
        {



            return View();



        }




    }
}