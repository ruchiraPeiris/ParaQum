using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Excel = Microsoft.Office.Interop.Excel;
using UpdateStock;
using System.Data.OleDb;
using ParaQum.Models;
using System.IO;
using System.Data.SqlClient;

namespace ParaQum.Controllers
{
    public class OrderBomController : Controller
    {
        public ActionResult Index()
        {
            PQDataBookSQLEntities newdb = new PQDataBookSQLEntities();
            newdb.BomImports.SqlQuery("select customerRef from BomImport");
            List<BomImport>list=newdb.BomImports.ToList();

            DbConn db = new DbConn();
            OleDbConnection conn = db.connDb();
            conn.Open();


            foreach (var item in list)
            {
                int a = Int16.Parse(item.customerRef.Substring(2));
                if (a<=1000 && a>0)
                {

                }else if(a>1000 && a >= 2000)
                {
                    //cap
                    //SqlCommand cmd = new SqlCommand("UPDATE Registration SET [UpdatedStock] = 1 WHERE [Email] = '@Email' and [AttendingCode] LIKE '%@AttendingCode%'", conn);
                    //cmd.Parameters.Add("@Email", SqlDbType.VarChar).Value = Email;
                    //cmd.Parameters.Add("@AttendingCode", SqlDbType.VarChar).Value = AttendingCode;
                    //string cmdstr = "update Capacitor_local set UpdatedStock= ";
                    //OleDbCommand com = new OleDbCommand(cmdstr, conn);
                    
                    //com.ExecuteNonQuery();
                    
                }
                else if (a > 2000 && a >= 3000)
                {
                    
                }
                else if (a > 3000 && a >= 4000)
                {

                }
                else if (a > 4000 && a >= 5000)
                {

                }
                else if (a > 5000 && a >= 6000)
                {

                }
                else if (a > 7000 && a >= 8000)
                {

                }
                else if (a > 8000 && a >= 9000)
                {

                }

            }
            conn.Close();
            return View();
        }
        // GET: OrderBom
        public ActionResult selectFiles(IEnumerable<HttpPostedFileBase> files)
        {
            int count = 1;
            List<BomModel> listProducts = new List<BomModel>();
            List<HttpPostedFileBase> fileList = new List<HttpPostedFileBase>();
            try
            {
                foreach (var excelfile in files)
                {
                    fileList.Add(excelfile);
                    //HttpPostedFileBase excelfile = files.ElementAt(i);


                    if (excelfile == null || excelfile.ContentLength == 0)
                    {
                        ViewBag.Error = "file type is incorrect <br />";
                        return View("index");
                    }
                    else
                    {
                        PQDataBookSQLEntities db1 = new PQDataBookSQLEntities();
                        if (excelfile.FileName.EndsWith("xls") || excelfile.FileName.EndsWith("xlsx"))
                        {
                            String path = Server.MapPath("~/Content/" + excelfile.FileName);
                            //if (System.IO.File.Exists(path))
                            //System.IO.File.Delete(path);
                            //excelfile.SaveAs(path);

                            //read data from the file
                            Excel.Application application = new Excel.Application();
                            Excel.Workbook worlbook = application.Workbooks.Open(path);
                            Excel.Worksheet worksheet1 = worlbook.ActiveSheet;
                            Excel.Range range = worksheet1.UsedRange;
                            
                            
                            //List<BomTb> listProducts = new List<BomTb>();

                            try
                            {
                                for (int row = 2; row < range.Rows.Count; row++)
                                {
                                    
                                    //BomTb b = new Bom.BomTb();
                                    /*if (((Excel.Range)range.Cells[row, 2]).Text != "" && ((Excel.Range)range.Cells[row, 5]).Text != "")
                                    {
                                        bomImportDetail = new BomImportDetail();
                                        bomImportDetail.BomNo = bomImport.BomNo;
                                        bomImportDetail.CustomerReference = ((Excel.Range)range.Cells[row, 2]).Text;
                                        bomImportDetail.Qty = Convert.ToInt32(((Excel.Range)range.Cells[row, 5]).Text);
                                        BOMDB.BomImportDetails.Add(bomImportDetail);
                                        BOMDB.SaveChanges();
                                    }*/
                                    BomModel b = new BomModel();
                                    BomImport bomImp = new BomImport();

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
                                    bomImp.itemNo = count;
                                    bomImp.customerRef = b.customerRef;
                                    bomImp.qty1 = b.qty1;
                                    bomImp.qty10 = b.qty10;
                                    db1.BomImports.Add(bomImp);
                                    db1.SaveChanges();
                                    count++;
                                }
                                ViewBag.ListProduct = listProducts;
                                
                            }

                            catch (Exception ex)
                            {
                                throw ex;
                            }

                            // return View()


                        }
                        else
                        {
                            ViewBag.Error = "file type is incorrect <br />";
                            return View("index");
                        }

                        //return View();
                    }


                }
                //return View();
            
            }catch(Exception e)
            {

            }
           
            return View("Success");
           

        }
        
    }
}