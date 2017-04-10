using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Excel = Microsoft.Office.Interop.Excel;
using ParaQum.Models;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.IO;

namespace ParaQum.Controllers
{

    public class BomImportController : Controller
    {
        Dbfinal newdb = new Dbfinal();
        string conStr = ConfigurationManager.ConnectionStrings["Dbfinal"].ConnectionString;
        // GET: BomImport
        public ActionResult Index()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    List<Bom> b = new List<Bom>();
                    con.Open();
                    string sqlCommand = "select [ProjectName] FROM [Project] where [ProjectName] not IN (SELECT [ProjectName] FROM [Bom] )";
                    SqlCommand cmd2 = new SqlCommand(sqlCommand, con);
                    SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd2);
                    DataTable resultSet = new DataTable();
                    sqlAdapter.Fill(resultSet);
                    var pro = resultSet.Select();
                    for (int i = 0; i < pro.Length; i++)
                    {
                        Bom b1 = new Bom();
                        b1.ProjectName = resultSet.Rows[i][0].ToString();
                        b.Add(b1);
                    }
                    ViewBag.proNames = new SelectList(b, "ProjectName", "ProjectName");
                    return View("Index");
                }
            }
            catch (Exception e)
            {
                return View("Index");
            }
        }
        public ActionResult DeleteBom(string item, int intProjectID)
        {
            string folderPath =Server.MapPath("~/ProjectBOM/" + item);
            if ((System.IO.File.Exists(folderPath)))
            {
                System.IO.File.Delete(folderPath);
            }

            return RedirectToAction("NeWProjectWindow", "ProjectManagement", new { intProjectID });
        }
        public ActionResult Success()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    con.Open();
                    // Delete the Temp table
                    string sqlString2 = "delete from [BomImport]";
                    SqlCommand sqlCommand2 = new SqlCommand(sqlString2, con);
                    sqlCommand2.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {

            }
            return View("Index");
        }
        public ActionResult ImportBom(HttpPostedFileBase excelfile, string projectName)
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                con.Open();
                string sqlString2 = " select count(*) as recordCount from [Bom] where ProjectName='" + projectName + "'";
                SqlCommand sqlCommand2 = new SqlCommand(sqlString2, con);
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(sqlCommand2);
                DataTable resultSet = new DataTable();
                sqlAdapter.Fill(resultSet);
                int rowCount = Convert.ToInt32(resultSet.Rows[0][0]);
                int reult = sqlCommand2.ExecuteNonQuery();
                if (rowCount>0)
                {
                    TempData["BomUploadFail"] = "Project BOM already uploaded";
                    return View("Index");
                }
            }

            using (SqlConnection con = new SqlConnection(conStr))
            {
                con.Open();
                string sqlString = "select count(*) as recordCount from [BomImport]";
                SqlCommand sqlCommand = new SqlCommand(sqlString, con);
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(sqlCommand);
                DataTable resultSet = new DataTable();
                sqlAdapter.Fill(resultSet);
                int rowCount = Convert.ToInt32(resultSet.Rows[0][0]);

                if (rowCount > 0)
                {
                    // Delete the Temp table
                    string sqlString1 = "delete from [BomImport]";
                    SqlCommand sqlCommand1 = new SqlCommand(sqlString1, con);
                    sqlCommand1.ExecuteNonQuery();
                }

            }

            if (excelfile == null || excelfile.ContentLength == 0)
            {
                ViewBag.Error = "file type is incorrect <br />";
                return View("index");
            }
            else
            {


                if (excelfile.FileName.EndsWith("xls") || excelfile.FileName.EndsWith("xlsx"))
                {
                    Bom bom = new Bom();
                    try
                    {
                        String path = Server.MapPath("~/ProjectBOM/" + excelfile.FileName);
                        string ecxelFileName = Path.GetFileName(path);
                        byte[] t = new byte[excelfile.ContentLength];
                        excelfile.InputStream.Read(t, 0, excelfile.ContentLength);
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

                                bomImp.itemNo = row;
                                bomImp.customerRef = b.customerRef;
                                bomImp.qty1 = b.qty1;
                                bomImp.qty10 = b.qty10;
                                bomImp.mpn = b.mpn;
                                using (SqlConnection con = new SqlConnection(conStr))
                                {
                                    con.Open();
                                    string sqlCommand = "insert into [dbo].[BomImport](itemNo,customerRef,qty1,qty10,mpn) values(" + bomImp.itemNo + ",'" + bomImp.customerRef + "'," + b.qty1 + "," + bomImp.qty10 + ",'" + bomImp.mpn + "')";

                                    SqlCommand command = new SqlCommand(sqlCommand, con);
                                    command.ExecuteNonQuery();
                                }
                            }

                            bom.ExcelFile = excelfile.FileName;
                            bom.AddedBy = Session["userName"].ToString();
                            bom.State = "PENDING";
                            bom.OrderId = 0;
                            bom.ProjectName = projectName;
                            bom.date = DateTime.Now;
                            bom.data = t;

                            using (SqlConnection con = new SqlConnection(conStr))
                            {
                                con.Open();
                                string sqlCommand = "SELECT[ProjectId] FROM Project WHERE[ProjectName] = '" + bom.ProjectName + "'";
                                SqlCommand cmd1 = new SqlCommand(sqlCommand, con);
                                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd1);
                                DataTable resultSet = new DataTable();
                                sqlAdapter.Fill(resultSet);
                                int rowCount = Convert.ToInt32(resultSet.Rows[0][0]);
                                string insCommand = "insert into [Bom](ExcelFile,AddedBy,State,ProjectId,ProjectName,OrderId,date,data) values('" + bom.ExcelFile + "','" + bom.AddedBy + "','" + bom.State + "'," + rowCount + ",'" + bom.ProjectName + "'," + bom.OrderId + ",'" + bom.date + "',@FileData)";
                                SqlCommand cmd = new SqlCommand(insCommand, con);
                                cmd.Parameters.Add("@FileData", SqlDbType.VarBinary);
                                cmd.Parameters["@FileData"].Value = t;
                                cmd.ExecuteNonQuery();
                                TempData["BomUpload"] = "Project BOM successfully uploaded";
                            }
                        }
                        catch (Exception e)
                        {
                            TempData["BomUploadFail"] = "Failed!....\n"+e;
                        }
                        ViewBag.ListProduct = listProducts;
                        return View("Success");
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                else
                {
                    ViewBag.Error = "file type is incorrect <br />";
                    return View("index");
                }
            }
        }
        public ActionResult Import(HttpPostedFileBase excelfile, Bom bh, FormCollection f)
        {
            string name = f["proNames"].ToString();
            using (SqlConnection con = new SqlConnection(conStr))
            {
                con.Open();
                string sqlString2 = "select ExcelFile from [Bom] where ProjectName='" + name + "'";
                SqlCommand sqlCommand2 = new SqlCommand(sqlString2, con);
                int reult = sqlCommand2.ExecuteNonQuery();
                if (reult != -1)
                {
                    TempData["BomUploadFail"] = "Project BOM is already uploaded";
                    return Redirect("Index");
                }
            }
            using (SqlConnection con = new SqlConnection(conStr))
            {
                con.Open();
                string sqlString = "select count(*) as recordCount from [BomImport]";
                SqlCommand sqlCommand = new SqlCommand(sqlString, con);
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(sqlCommand);
                DataTable resultSet = new DataTable();
                sqlAdapter.Fill(resultSet);
                int rowCount = Convert.ToInt32(resultSet.Rows[0][0]);

                if (rowCount > 0)
                {
                    // Delete the Temp table
                    string sqlString1 = "delete from [BomImport]";
                    SqlCommand sqlCommand1 = new SqlCommand(sqlString1, con);
                    sqlCommand1.ExecuteNonQuery();
                }
            }

            if (excelfile == null || excelfile.ContentLength == 0)
            {
                ViewBag.Error = "file type is incorrect <br />";
                return View("index");
            }
            else
            {
                if (excelfile.FileName.EndsWith("xls") || excelfile.FileName.EndsWith("xlsx"))
                {
                    try
                    {
                        String path = Server.MapPath("~/ProjectBOM/" + excelfile.FileName);
                        string ecxelFileName = Path.GetFileName(path);

                        byte[] t = new byte[excelfile.ContentLength];
                        excelfile.InputStream.Read(t, 0, excelfile.ContentLength);

                        //read data from the file
                        Excel.Application application = new Excel.Application();
                        Excel.Workbook worlbook = application.Workbooks.Open(path);
                        Excel.Worksheet worksheet1 = worlbook.ActiveSheet;
                        Excel.Range range = worksheet1.UsedRange;
                        List<BomModel> listProducts = new List<BomModel>();



                        for (int row = 2; row < range.Rows.Count; row++)
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

                            bomImp.itemNo = row;
                            bomImp.customerRef = b.customerRef;
                            bomImp.qty1 = b.qty1;
                            bomImp.qty10 = b.qty10;
                            bomImp.mpn = b.mpn;

                            using (SqlConnection con = new SqlConnection(conStr))
                            {
                                con.Open();
                                string sqlCommand = "insert into [dbo].[BomImport](itemNo,customerRef,qty1,qty10,mpn) values(" + bomImp.itemNo + ",'" + bomImp.customerRef + "'," + b.qty1 + "," + bomImp.qty10 + ",'" + bomImp.mpn + "')";

                                SqlCommand command = new SqlCommand(sqlCommand, con);
                                command.ExecuteNonQuery();
                            }
                        }
                        Bom bom = new Bom();
                        try
                        {
                            bom.ExcelFile = excelfile.FileName;
                            bom.AddedBy = Session["userName"].ToString();
                            bom.State = "PENDING";
                            bom.OrderId = 0;
                            bom.ProjectName = f["proNames"].ToString();
                            bom.date = DateTime.Now;
                            bom.data = t;

                            using (SqlConnection con = new SqlConnection(conStr))
                            {
                                con.Open();
                                string sqlCommand = "SELECT[ProjectId] FROM Project WHERE[ProjectName] = '" + bom.ProjectName + "'";
                                SqlCommand cmd1 = new SqlCommand(sqlCommand, con);
                                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd1);
                                DataTable resultSet = new DataTable();
                                sqlAdapter.Fill(resultSet);
                                int rowCount = Convert.ToInt32(resultSet.Rows[0][0]);

                                string insCommand = "insert into [Bom](ExcelFile,AddedBy,State,ProjectId,ProjectName,OrderId,date,data) values('" + bom.ExcelFile + "','" + bom.AddedBy + "','" + bom.State + "'," + rowCount + ",'" + bom.ProjectName + "'," + bom.OrderId + ",'" + bom.date + "',@FileData)";
                                SqlCommand cmd = new SqlCommand(insCommand, con);
                                cmd.Parameters.Add("@FileData", SqlDbType.VarBinary);
                                cmd.Parameters["@FileData"].Value = t;
                                cmd.ExecuteNonQuery();
                                TempData["BomUpload"] = "Project BOM successfully uploaded";
                            }
                        }
                        catch (Exception e)
                        {
                            TempData["BomUploadFail"] = "Failed!....\n" + e;
                        }
                        ViewBag.ListProduct = listProducts;
                        return View("Success");
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
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
