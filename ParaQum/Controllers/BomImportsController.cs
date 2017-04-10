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
    public class BomImportsController : Controller
    {
        // GET: BomImports
        Dbfinal Db = new Dbfinal();
        string conStr = ConfigurationManager.ConnectionStrings["Dbfinal"].ConnectionString;

        // GET: Users
        public ActionResult Index()
        {

            return View(Db.BomImports);
        }
        [HttpPost]
        public ActionResult edit(BomImport bom)
        {//update user

            Dbfinal db = new Dbfinal();
            BomImport student_to_update = db.BomImports.SingleOrDefault(s => s.itemNo == bom.itemNo);
            student_to_update.customerRef = bom.customerRef;
            student_to_update.qty1 = bom.qty1;
            student_to_update.qty10 = bom.qty10;
            student_to_update.mpn = bom.mpn;

            db.SaveChanges();
            ModelState.AddModelError("", "Quantity updated successfully");

            return View(student_to_update);


        }

        public ActionResult Edit(int? id)
        {//display user informations

            Dbfinal db = new Dbfinal();
            BomImport bom = db.BomImports.SingleOrDefault(a => a.itemNo == id);
            return View(bom);




        }

        [HttpPost]
        public ActionResult Delete(BomImport bom)
        {


            Dbfinal db = new Dbfinal();

            db.BomImports.Remove(bom);//remove a raw from the user table
            db.SaveChanges();

            ViewBag.message = "Bom removed successfully";

            return View("index");


        }
        [HttpPost, ActionName("Delete")]
        public ActionResult Delete(int tempId)
        {


            Dbfinal db = new Dbfinal();
            BomImport bomImport = db.BomImports.Find(tempId);
            db.BomImports.Remove(bomImport);//remove a raw from the user table
            db.SaveChanges();

            ViewBag.message = "Bom removed successfully";

            return View(bomImport);


        }
        public ActionResult Delete(int? id)
        {

            if (id == 0)
            {

                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);


            }

            Dbfinal db = new Dbfinal();
            BomImport bom = db.BomImports.SingleOrDefault(a => a.itemNo == id);
            return View(bom);



        }



    }
}