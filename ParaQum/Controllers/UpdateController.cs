using ParaQum.Models;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UpdateStock;

namespace ParaQum.Controllers
{
    public class UpdateController : Controller
    {
        // GET: Update
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult updating(UpdateComp uc)
        {
            UpdateComp updateComp = new UpdateComp();
            OleDbConnection conn = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\Users\HARITH\Documents\PQDataBook (1).mdb");
            OleDbCommand cmd = new OleDbCommand();
            conn.Open();

            string custRef = uc.customerRef;
            int stock = uc.paraQumStock;

            try
            {
                int pqNum = int.Parse(uc.customerRef.Substring(2));

                //conn.Open();

                //conn.Close();
                if (pqNum <= 1000 && pqNum > 0)
                {

                }
                else
                if (pqNum > 1000 && pqNum <= 2000)
                {
                    string cmd1 = "select [ParaQum Stock] from [Capacitor_local] where [Part Label]= @c";
                    OleDbCommand com1 = new OleDbCommand(cmd1, conn);
                    com1.Parameters.AddWithValue("@c", uc.customerRef);
                    int paraQumStock = Convert.ToInt32(com1.ExecuteScalar().ToString());

                    string cmd2 = "select [RequiredQty] from [Capacitor_local] where [Part Label]= @c";
                    OleDbCommand com2 = new OleDbCommand(cmd2, conn);
                    com2.Parameters.AddWithValue("@c", uc.customerRef);
                    int requiredQty = Convert.ToInt32(com2.ExecuteScalar().ToString());

                    string cmd3 = "select [Stock] from [Capacitor_local] where [Part Label]= @c";
                    OleDbCommand com3 = new OleDbCommand(cmd3, conn);
                    com3.Parameters.AddWithValue("@c", uc.customerRef);
                    int stock1 = Convert.ToInt32(com3.ExecuteScalar().ToString());

                    updateComp.paraQumStock = paraQumStock + stock;
                    updateComp.ReqiredQty = requiredQty - stock;
                    updateComp.stock = updateComp.paraQumStock;
                    string cmd4 = "UPDATE [Capacitor_local] SET [ParaQum Stock] =@pq , [RequiredQty] =@rq, [Stock]=@s WHERE [Part Label] = @c";
                    OleDbCommand com4 = new OleDbCommand(cmd4, conn);

                    com4.Parameters.AddWithValue("@pq", updateComp.paraQumStock);
                    com4.Parameters.AddWithValue("@rq", updateComp.ReqiredQty);
                    com4.Parameters.AddWithValue("@s", updateComp.stock);
                    com4.Parameters.AddWithValue("@c", uc.customerRef);
                    int a = com4.ExecuteNonQuery();
                    TempData["Comp"] = "Stock Updated";



                }
                else if (pqNum > 2000 && pqNum <= 3000)
                {

                }
                else if (pqNum > 3000 && pqNum <= 4000)
                {

                }
                else if (pqNum > 4000 && pqNum <= 5000)
                {

                }
                else if (pqNum > 5000 && pqNum <= 6000)
                {

                }
                else if (pqNum > 7000 && pqNum <= 8000)
                {

                }
                else if (pqNum > 8000 && pqNum <= 9000)
                {

                }
            }
            catch (Exception e)
            {

            }
            return RedirectToAction("Index", "Update");

        }
        
    }
}