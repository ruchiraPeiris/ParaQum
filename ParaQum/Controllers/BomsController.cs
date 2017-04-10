using Microsoft.AspNet.Identity;
using ParaQum.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ParaQum.Controllers
{
    public class BomsController : Controller
    {
        // GET: Boms
        Dbfinal Db = new Dbfinal();
        string conStr = ConfigurationManager.ConnectionStrings["Dbfinal"].ConnectionString;

        // GET: Users
        public ActionResult Index()
        {

            return View(Db.Boms);
        }
        [HttpPost]
        public ActionResult edit(Bom bom)
        {//update user

            Dbfinal db = new Dbfinal();
            Bom student_to_update = db.Boms.SingleOrDefault(s => s.BomId == bom.BomId);
            student_to_update.AddedBy = bom.AddedBy;
            student_to_update.ExcelFile = bom.ExcelFile;
            student_to_update.State = bom.State;
            student_to_update.ProjectId = bom.ProjectId;
            student_to_update.ProjectName = bom.ProjectName;
            student_to_update.ProjectId = bom.ProjectId;
            student_to_update.date = bom.date;
            db.SaveChanges();
            ModelState.AddModelError("", "Bom updated successfully");

            return View(student_to_update);


        }

        public ActionResult Edit(int? id)
        {//display user informations

            Dbfinal db = new Dbfinal();
            Bom bom = db.Boms.SingleOrDefault(a => a.BomId == id);
            return View(bom);




        }

        [HttpPost, ActionName("Delete")]
        public ActionResult Delete_Bom(int id)
        {


            Dbfinal db = new Dbfinal();
            Bom bomdelete = db.Boms.Find(id);
            db.Boms.Remove(bomdelete);//remove a raw from the user table
            db.SaveChanges();

            ViewBag.message = "Bom removed successfully";


            return View(bomdelete);

        }

        public ActionResult Delete(int? id)
        {

            if (id == 0)
            {

                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);


            }

            Dbfinal db = new Dbfinal();
            Bom bom = db.Boms.SingleOrDefault(a => a.BomId == id);
            return View(bom);



        }


        [HttpPost]
        public ActionResult Create(userModel usermodel)
        {






            string constr = ConfigurationManager.ConnectionStrings["PqContext"].ConnectionString;


            using (SqlConnection con = new SqlConnection(constr))
            {

                try
                {
                    string sqlCommand = "insert into [dbo].[User](UserId,UserName,Password,email,contactNo,InventoryManager,Admin,Designer,Other,verificationCode)values('" + usermodel.userId + "','" + usermodel.userName + "','','" + usermodel.email + "','" + usermodel.contactNo + "','" + usermodel.inventoryManager + "','" + usermodel.admin + "','" + usermodel.designer + "','" + usermodel.other + "','" + usermodel.verificationCode + "')";


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




            return RedirectToAction("index", "Users");



        }



    }
}