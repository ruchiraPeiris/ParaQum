using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Excel = Microsoft.Office.Interop.Excel;
using ParaQum.Models;
using UpdateStock;
using System.Data.OleDb;


namespace ParaQum.Controllers
{
    public class BomImportController : Controller
    {
        

        // GET: BomImport
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Success()
        {
            /* DbConn db = new DbConn();
             OleDbConnection conn = db.connDb();
             string cmdstr = "delete from BomImport";
             OleDbCommand com = new OleDbCommand(cmdstr, conn);
             conn.Open();
             com.ExecuteNonQuery();
             conn.Close();*/
            PQDataBookSQLEntities db2 = new PQDataBookSQLEntities();
            IEnumerable< BomImport> b= db2.BomImports.SqlQuery("select * from BomImport");


            db2.BomImports.RemoveRange(b);
            db2.SaveChanges();
            return View("Index");
        }
        public ActionResult Import(HttpPostedFileBase excelfile, BOMHeader bomHeader)
        {
           /* DbConn db = new DbConn();
            OleDbConnection conn = db.connDb();
            conn.Open();*/


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
                    Bom bom = new Bom();
                    //BomTb bomTb = new BomTb();
                    //string cmd = "Select BomId from Bom";
                    //OleDbCommand com = new OleDbCommand(cmd, conn);
                    bom.AddedBy = bomHeader.AddedBy;
                    bom.ExcelFile = excelfile.FileName;
                    bom.OrderId = 0;
                    bom.Project = bomHeader.Project;
                    bom.date = DateTime.Now;
                    bom.State = "PENDING";
                    bom.BomId= bomHeader.BomId;
                    db1.Boms.Add(bom);
                    db1.SaveChanges();
         //           string cmd1 = "insert into Bom(BomId,ExcelFile,AddedBy,State,Project,OrderId,date)values(?,?,?,?,?,?,?)";
           /*         OleDbCommand com2 = new OleDbCommand(cmd1, conn);
                    com2.Parameters.AddWithValue("?", bomImport.BomNo);
                    com2.Parameters.AddWithValue("?", bomImport.link);
                    com2.Parameters.AddWithValue("?", bomImport.EnteredBy);
                    com2.Parameters.AddWithValue("?", bomImport.Status);
                    com2.Parameters.AddWithValue("?", bomImport.project);
                    com2.Parameters.AddWithValue("?", bomImport.orderId);
                    com2.Parameters.AddWithValue("?", bomImport.ImportDate);
                    com2.ExecuteNonQuery();*/
                    String path = Server.MapPath("~/Content/" + excelfile.FileName);
                    //if (System.IO.File.Exists(path))
                        //System.IO.File.Delete(path);
                        //excelfile.SaveAs(path);

                    //read data from the file
                    Excel.Application application = new Excel.Application();
                    Excel.Workbook worlbook = application.Workbooks.Open(path);
                    Excel.Worksheet worksheet1 = worlbook.ActiveSheet;
                    Excel.Range range = worksheet1.UsedRange;
                    List<BomModel> listProducts = new List<BomModel>();
                    

                    try
                    {
                        for (int row = 2; row < range.Rows.Count; row++)
                        {
                           BomModel b = new BomModel();
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
                            BomImport bomImp = new BomImport();
                            //string cmdstr = "insert into BomImport(customerRef,qty1,qty10)values(?,?,?)";
                           // OleDbCommand com1 = new OleDbCommand(cmdstr, conn);
                            

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
                            //com1.Parameters.AddWithValue("?", b.customerRef);
                            //.Parameters.AddWithValue("?", b.qty1);
                            //com1.Parameters.AddWithValue("?", b.qty10);
                            bomImp.itemNo = row;
                            bomImp.customerRef = b.customerRef;
                            bomImp.qty1 = b.qty1;
                            bomImp.qty10 = b.qty10;
                            db1.BomImports.Add(bomImp);
                            db1.SaveChanges();
                            //com1.ExecuteNonQuery();
                           // conn.Close();

                        }
                        
                        ViewBag.ListProduct = listProducts;
                        return View("Success");
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
                



            }
           
        }
    }
}