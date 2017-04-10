
using ParaQum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UpdateStock;



namespace ParaQum.Controllers
{
    public class UsersController : Controller
    {
        PqContext Db = new PqContext();
        // GET: Users
        public ActionResult Index()
        {

            return View(Db.User.ToList());
        }
        public ActionResult Create() {


            return View();


        }
        [HttpPost]
        public ActionResult Create(userModel usermodel) {

            Db.User.Add(usermodel);//add new raw to table
            Db.SaveChanges();

           

            return RedirectToAction("Index");



        }

        public ActionResult Edit() {
            return View();


        }



        public ActionResult Delete() {


            return View();


        }

     
    }
}