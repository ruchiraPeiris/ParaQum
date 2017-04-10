using ParaQum.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ParaQum.Controllers
{
    public class BomOrderTempsController : Controller
    {

        // GET: BomImports
        Dbfinal Db = new Dbfinal();
        string conStr = ConfigurationManager.ConnectionStrings["Dbfinal"].ConnectionString;

        // GET: Users
        public ActionResult Index()
        {

            return View(Db.BomOrderTemps);
        }
        [HttpPost]
        public ActionResult edit(BomOrderTemp bom)
        {//update user

            Dbfinal db = new Dbfinal();
            BomOrderTemp bot = db.BomOrderTemps.SingleOrDefault(s => s.itemNo == bom.itemNo);
            bot.customerRef = bom.customerRef;
            bot.qty1 = bom.qty1;
            bot.qty10 = bom.qty10;
            bot.mpn = bom.mpn;

            //Update Access DB
            int qty10, newQty10;
            newQty10 = Convert.ToInt32(bot.qty10.ToString());
            OleDbConnection conn = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\Users\HARITH\Documents\PQDataBook (1).mdb");
            OleDbCommand cmd = new OleDbCommand();
            conn.Open();

            int pqNum = int.Parse(bot.customerRef.Substring(2));
            try
            {
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    con.Open();
                    string sqlCommand = "select qty10 from [BomOrderTemp] where customerRef='"+ bot.customerRef.ToString()+"'";
                    SqlCommand cmd2 = new SqlCommand(sqlCommand, con);
                    SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd2);
                    DataTable resultSet = new DataTable();
                    sqlAdapter.Fill(resultSet);
                    qty10 = Convert.ToInt32(resultSet.Rows[0][0]);
                }


                if (pqNum <= 1000 && pqNum > 0)
                {

                }
                else if (pqNum > 1000 && pqNum <= 2000)
                {

                    string cmd1 = "select [Stock] from [Capacitor_local] where [Part Label]='" + bot.customerRef + "'";
                    OleDbCommand com2 = new OleDbCommand(cmd1, conn);
                    com2.Parameters.AddWithValue("?", bot.customerRef);
                    int paraQumStock = Convert.ToInt32(com2.ExecuteScalar());
                    string newcmd1 = "select [RequiredQty] from [Capacitor_local] where [Part Label]='" + bot.customerRef + "'";
                    OleDbCommand newcom2 = new OleDbCommand(newcmd1, conn);
                    newcom2.Parameters.AddWithValue("?", bot.customerRef);
                    int requiredQty = Convert.ToInt32(newcom2.ExecuteScalar());
                    requiredQty = newQty10;
                    
                    

                    string cmd3 = "UPDATE [Capacitor_local] SET [RequiredQty] = @stock WHERE [Part Label] = @label";

                    OleDbCommand com3 = new OleDbCommand(cmd3, conn);
                    com3.Parameters.AddWithValue("@stock", requiredQty);
                    com3.Parameters.AddWithValue("@label", bot.customerRef);
                    com3.ExecuteNonQuery();

                }


            }
            catch (Exception e)
            {
                ModelState.AddModelError("", "Quantity update Fails....");
            }


            db.SaveChanges();
            ModelState.AddModelError("", "Quantity updated successfully");

            return View(bot);


        }

        public ActionResult Edit(int? id)
        {//display user informations

            Dbfinal db = new Dbfinal();
            BomOrderTemp bom = db.BomOrderTemps.SingleOrDefault(a => a.itemNo == id);
            return View(bom);




        }

        [HttpPost, ActionName("Delete")]
        public ActionResult Delete(int id)
        {


            Dbfinal db = new Dbfinal();
            BomOrderTemp bot = db.BomOrderTemps.Find(id);
            db.BomOrderTemps.Remove(bot);//remove a raw from the user table
            db.SaveChanges();

            ViewBag.message = "BomOrder removed successfully";

            return View(bot);


        }

        public ActionResult Delete(int? id)
        {

            if (id == 0)
            {

                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);


            }

            Dbfinal db = new Dbfinal();
            BomOrderTemp bom = db.BomOrderTemps.SingleOrDefault(a => a.itemNo == id);
            return View(bom);



        }



    }
}