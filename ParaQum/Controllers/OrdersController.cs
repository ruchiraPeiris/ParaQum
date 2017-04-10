using ParaQum.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Excel = Microsoft.Office.Interop.Excel;

namespace ParaQum.Controllers
{
    public class OrdersController : Controller
    {
        // GET: Boms
        Dbfinal Db = new Dbfinal();
        string conStr = ConfigurationManager.ConnectionStrings["Dbfinal"].ConnectionString;

        // GET: Users
        public ActionResult Index()
        {
            return View(Db.Orders);
        }
        [HttpPost]
        public ActionResult edit(Order ord)
        {//update user

            Dbfinal db = new Dbfinal();
            Order student_to_update = db.Orders.SingleOrDefault(s => s.OrderId == ord.OrderId);
            student_to_update.OrderExcelFile = ord.OrderExcelFile;
            student_to_update.Date = ord.Date;
            student_to_update.data = ord.data;

            db.SaveChanges();
            ModelState.AddModelError("", "Order updated successfully");

            return View(student_to_update);


        }

        public ActionResult Edit(int? id)
        {//display user informations

            Dbfinal db = new Dbfinal();
            Order ord = db.Orders.SingleOrDefault(a => a.OrderId == id);
            return View(ord);




        }

        [HttpPost, ActionName("Delete")]
        public ActionResult Delete(int order_Id)
        {


            Dbfinal db = new Dbfinal();
            Order orderFile = db.Orders.Find(order_Id);
            db.Orders.Remove(orderFile);//remove a raw from the user table
            db.SaveChanges();

            ViewBag.message = "Bom removed successfully";

            return View(orderFile);


        }

        public ActionResult delete(int? id)
        {

            if (id == 0)
            {

                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);


            }

            Dbfinal db = new Dbfinal();
            Order ord = db.Orders.SingleOrDefault(a => a.OrderId == id);
            return View(ord);



        }
        
        public ActionResult ViewBom(Order ord)
        {
            List<HttpPostedFileBase> f = new List<HttpPostedFileBase>(); ;
            string file = ord.OrderExcelFile;
            Bom bom = new Bom();
            try
            {
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    con.Open();

                    string sqlString2 = "select data from [Bom] where ExcelFile='" + file + "'";
                    SqlCommand sqlCommand2 = new SqlCommand(sqlString2, con);
                    SqlDataAdapter ada = new SqlDataAdapter(sqlCommand2);
                    DataTable newFile = new DataTable();
                    ada.Fill(newFile);
                    DataRow row = newFile.Rows[0];
                    Byte[] data = (byte[])sqlCommand2.ExecuteScalar();


                    System.IO.File.WriteAllBytes("C:\\Users\\HARITH\\Documents\\3.30.2017\\ParaQum\\ParaQum\\ProjectBom" + file, data);
                    String path = Server.MapPath("~/ProjectBOM/" + file);

                    //read data from the file
                    Excel.Application application = new Excel.Application();
                    Excel.Workbook worlbook = application.Workbooks.Open(path);
                    Excel.Worksheet worksheet1 = worlbook.ActiveSheet;
                    Excel.Range range = worksheet1.UsedRange;
                    List<BomModel> listProducts = new List<BomModel>();


                    for (int r = 2; r < range.Rows.Count; r++)
                    {
                        BomModel b = new BomModel();

                        Models.BomImport bomImp = new Models.BomImport();
                        b.itemNo = ((Excel.Range)range.Cells[row, 1]).Text;
                        b.customerRef = ((Excel.Range)range.Cells[row, 2]).Text;
                        b.refDesignator = ((Excel.Range)range.Cells[row, 3]).Text;
                        b.qty1 = ((Excel.Range)range.Cells[row, 4]).Text;
                        b.qty10 = ((Excel.Range)range.Cells[row, 5]).Text;
                        b.description = ((Excel.Range)range.Cells[row, 6]).Text;
                        b.value = ((Excel.Range)range.Cells[row, 7]).Text;
                        b.manufacture = ((Excel.Range)range.Cells[row, 8]).Text;
                        b.mpn = ((Excel.Range)range.Cells[row, 9]).Text;
                        b.vsNo = ((Excel.Range)range.Cells[row, 10]).Text;
                        b.vs_TdComment = ((Excel.Range)range.Cells[row, 11]).Text;
                        b.parConfromation = ((Excel.Range)range.Cells[row, 12]).Text;
                        listProducts.Add(b);

                        bomImp.itemNo = r;
                        bomImp.customerRef = b.customerRef;
                        bomImp.qty1 = b.qty1;
                        bomImp.qty10 = b.qty10;
                        bomImp.mpn = b.mpn;

                    }

                    ViewBag.ListProduct = listProducts;
                }
            }
            catch (Exception e)
            {
                TempData["BomUploadFail"] = e;
            }


            return View("ViewBom");
        }
    }




}

