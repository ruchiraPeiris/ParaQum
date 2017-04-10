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
using System.Configuration;
using System.Data;
using System.Web.UI;
using DocumentFormat.OpenXml.InkML;

namespace ParaQum.Controllers
{
    public class OrderBomController : Controller
    {

        Dbfinal newdb = new Dbfinal();
        string conStr = ConfigurationManager.ConnectionStrings["Dbfinal"].ConnectionString;
        List<HttpPostedFileBase> fileList1 = new List<HttpPostedFileBase>();
        public ActionResult Index()
        {
            List<Bom> b = new List<Bom>();
            try
            {
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    con.Open();
                    string sqlCommand = "select [ExcelFile] from Bom where State='PENDING' ";
                    SqlCommand cmd2 = new SqlCommand(sqlCommand, con);
                    SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd2);
                    DataTable resultSet = new DataTable();
                    sqlAdapter.Fill(resultSet);
                    var num = resultSet.Select();


                    for (int i = 0; i < num.Length; i++)
                    {
                        Bom b1 = new Bom();
                        b1.ExcelFile = resultSet.Rows[i][0].ToString();
                        b.Add(b1);
                    }
                    ViewBag.exFileNames = new SelectList(b, "ExcelFile", "ExcelFile");

                    ViewBag.BomNames = b;
                    return View("Index");
                }
            }
            catch (Exception e)
            {
                return View("Index");
            }
        }
        //updating required qty in componenet tables
        //update order table
        //exporting excel sheets
        //deleting the bomimport table
        public ActionResult newExport()
        {
            return View();
        }

        public ActionResult ABC()
        {

            try
            {
                OleDbConnection conn = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\Users\HARITH\Documents\PQDataBook (1).mdb");

                OleDbCommand cmd = new OleDbCommand();
                conn.Open();
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    con.Open();
                    string sqlCommand = "select * from [BomImport]";
                    SqlCommand cmd2 = new SqlCommand(sqlCommand, con);
                    SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd2);
                    DataTable resultSet = new DataTable();
                    sqlAdapter.Fill(resultSet);
                    int rowCount = Convert.ToInt32(resultSet.Rows[0][0]);
                    var num = resultSet.Select();
                    int a=0, b=0, itemNo=0, qty1=0, qty10=0;
                    string s, mpn;

                    //updating db
                    for (int i = 0; i < num.Length; i++)
                    {
                        try
                        {
                            a = int.Parse(resultSet.Rows[i][1].ToString().Substring(2));
                            b = Convert.ToInt32(resultSet.Rows[i][3]);//qty10 value
                            s = resultSet.Rows[i][1].ToString();
                            itemNo = Convert.ToInt32(resultSet.Rows[i][0]);
                            mpn = resultSet.Rows[i][4].ToString();
                            qty10 = Convert.ToInt32(resultSet.Rows[i][3]);
                            qty1 = Convert.ToInt32(resultSet.Rows[i][2]);
                            //string sqlInsertCmd = "insert into [dbo].[BomOrderTemp](itemNo,customerRef,qty1,qty10,mpn) values(" + itemNo + ",'" + s + "'," + qty1 + "," + qty10 + ",'" + mpn + "')";
                            //SqlCommand insertCmd = new SqlCommand(sqlInsertCmd, con);
                            //insertCmd.ExecuteNonQuery();

                            if (a <= 1000 && a > 0)
                            {

                            }
                            else if (a > 1000 && a <= 2000)
                            {

                                string cmd1 = "select [Stock] from [Capacitor_local] where [Part Label]='" + s + "'";
                                OleDbCommand com2 = new OleDbCommand(cmd1, conn);
                                com2.Parameters.AddWithValue("?", s);
                                int paraQumStock = Convert.ToInt32(com2.ExecuteScalar());
                                string newcmd1 = "select [RequiredQty] from [Capacitor_local] where [Part Label]='" + s + "'";
                                OleDbCommand newcom2 = new OleDbCommand(newcmd1, conn);
                                newcom2.Parameters.AddWithValue("?", s);
                                int requiredQty = Convert.ToInt32(newcom2.ExecuteScalar());

                                if (requiredQty == 0)
                                {
                                    if (paraQumStock - b > 0)
                                    {
                                        requiredQty = 0;
                                        paraQumStock = paraQumStock - b;
                                    }
                                    else
                                    {
                                        requiredQty = b - paraQumStock;

                                        string sqlInsertCommand = "insert into [dbo].[BomOrderTemp](itemNo,customerRef,qty10,mpn) values(" + itemNo + ",'" + s + "'," + requiredQty + ",'" + mpn + "')";
                                        SqlCommand insCommand = new SqlCommand(sqlInsertCommand, con);
                                        insCommand.ExecuteNonQuery();
                                    }
                                }
                                else
                                {
                                    requiredQty = requiredQty+ b;

                                    string sqlInsertCommand = "insert into [dbo].[BomOrderTemp](itemNo,customerRef,qty10,mpn) values(" + itemNo + ",'" + s + "'," + b + ",'" + mpn + "')";
                                    SqlCommand insCommand = new SqlCommand(sqlInsertCommand, con);
                                    insCommand.ExecuteNonQuery();
                                }

                                string cmd3 = "UPDATE [Capacitor_local] SET [Stock]=@paraStock,[RequiredQty] = @stock WHERE [Part Label] = @label";

                                OleDbCommand com3 = new OleDbCommand(cmd3, conn);
                                com3.Parameters.AddWithValue("@paraStock", paraQumStock);
                                com3.Parameters.AddWithValue("@stock", requiredQty);
                                com3.Parameters.AddWithValue("@label", s);
                                com3.ExecuteNonQuery();

                            }


                        }
                        catch (Exception e)
                        {

                        }

                    }


                    List<string> list1 = Session["data"] as List<string>;

                    foreach (string list in list1)
                    {

                        string sqlCommand1 = "update [dbo].[Bom] set State='PROCESSED' where ExcelFile ='" + list.ToString() + "'";

                        SqlCommand command = new SqlCommand(sqlCommand1, con);
                        command.ExecuteNonQuery();


                    }
                    TempData["Componenet"] = "DB updated...";
                }
            }
            catch (Exception e)
            {

            }
            return RedirectToAction("Index", "BomOrderTemps");

        }
        public ActionResult ExportToExcel(Models.Order bomOrd)
        {
            try
            {
                var grid = new System.Web.UI.WebControls.GridView();
                grid.DataSource = from data in Models.BomOrderTemp.GenerateListPro()
                                  select new
                                  {

                                      id = data.itemNo,
                                      name = data.customerRef,
                                      qty1 = data.qty1,
                                      mpn = data.mpn
                                  };
                grid.DataBind();
                Response.ClearContent();
                Response.AddHeader("content-disposition", "attachment;filename=" + bomOrd.OrderExcelFile + ".xls");
                Response.ContentType = "application/excel";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htmlTextWriter = new HtmlTextWriter(sw);
                grid.RenderControl(htmlTextWriter);
                Response.Write(sw.ToString());
                Response.End();



                //using (SqlConnection con = new SqlConnection(conStr))
                //{
                //    con.Open();
                //    // Delete the Temp table
                //    string sqlString2 = "delete from [BomImport]";
                //    SqlCommand sqlCommand2 = new SqlCommand(sqlString2, con);
                //    sqlCommand2.ExecuteNonQuery();


                //}
                return View("Index");
            }
            catch (Exception e)
            {

            }
            List<string> list2 = Session["data"] as List<string>;

            return View("Index");
        }
        public ActionResult OrderConformation(HttpPostedFileBase file)
        {
            String path = Server.MapPath("~/ProjectBOM/" + file.FileName);
            string ecxelFileName = Path.GetFileName(path);

            byte[] t = new byte[file.ContentLength];
            file.InputStream.Read(t, 0, file.ContentLength);
            Models.Order ord = new Models.Order();
            ord.Date = DateTime.Now;
            ord.OrderExcelFile = file.FileName;
            ord.data = t;


            List<string> list1 = Session["data"] as List<string>;
            using (SqlConnection con = new SqlConnection(conStr))
            {
                con.Open();
                string sqlFiles = "select ExcelFile from[Bom] where State='PROCESSED'";
                SqlCommand fileCmd = new SqlCommand(sqlFiles, con);
                SqlDataAdapter sqlFileAdapter = new SqlDataAdapter(fileCmd);
                DataTable resultFileSet = new DataTable();
                sqlFileAdapter.Fill(resultFileSet);
                var fileList = resultFileSet.Select();


                string sqlCommand = "select * from [Bom]";
                SqlCommand cmd2 = new SqlCommand(sqlCommand, con);
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd2);
                DataTable resultSet = new DataTable();
                sqlAdapter.Fill(resultSet);
                var num = resultSet.Select();
                int bomId;
                string sqlCommand3 = "insert into [dbo].[Order](OrderExcelFile,date,data) values('" + ord.OrderExcelFile + "','" + ord.Date + "',@FileData)";
                SqlCommand command2 = new SqlCommand(sqlCommand3, con);
                command2.Parameters.Add("@FileData", SqlDbType.VarBinary);
                command2.Parameters["@FileData"].Value = t;
                command2.ExecuteNonQuery();
                string sqlString2 = "select Max(OrderId) from [Order]";
                SqlCommand sqlCommand2 = new SqlCommand(sqlString2, con);
                bomId = (int)sqlCommand2.ExecuteScalar();
                //

                //delete BomOrderTemp

                string sqldel = "delete from [BomOrderTemp]";
                SqlCommand sqlCmdDel = new SqlCommand(sqldel, con);
                sqlCmdDel.ExecuteNonQuery();

                for (int i = 0; i < num.Length; i++)
                {
                    if (list1 == null)
                    {
                        for (int list = 0; list < fileList.Length; list++)
                        {
                            string sqlCommand1 = "update [dbo].[Bom] set State='COMPLETED',OrderId=" + bomId + " where ExcelFile ='" + resultFileSet.Rows[i][0].ToString() + "'";

                            SqlCommand command = new SqlCommand(sqlCommand1, con);
                            command.ExecuteNonQuery();
                        }
                    }
                    else {
                        foreach (string list in list1)
                        {
                            if (resultSet.Rows[i][3].ToString() == "PROCESSED" && resultSet.Rows[i][1].ToString() == list.ToString())
                            {
                                string sqlCommand1 = "update [dbo].[Bom] set State='COMPLETED',OrderId=" + bomId + " where ExcelFile ='" + list.ToString() + "'";

                                SqlCommand command = new SqlCommand(sqlCommand1, con);
                                command.ExecuteNonQuery();


                            }

                        }
                    }
                }
            }
            return View("Index");
        }
        // GET: OrderBom
        public ActionResult selectFiles(IEnumerable<HttpPostedFileBase> files, Bom bm, FormCollection f, Context c)
        {

            List<HttpPostedFileBase> fileList = new List<HttpPostedFileBase>();
            List<BomModel> listProducts1 = new List<BomModel>();
            // Check data is available in the temp table
            using (SqlConnection con = new SqlConnection(conStr))
            {
                con.Open();
                string sqlString = "select count(*) as recordCount from [BomOrderTemp]";
                SqlCommand sqlCommand = new SqlCommand(sqlString, con);
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(sqlCommand);
                DataTable resultSet = new DataTable();
                sqlAdapter.Fill(resultSet);
                int rowCount = Convert.ToInt32(resultSet.Rows[0][0]);

                if (rowCount > 0)
                {
                    return View("ExportToExcel");
                }
                string sql = "select count(*) as recordCount from [BomImport]";
                SqlCommand sqlCmd = new SqlCommand(sql, con);
                SqlDataAdapter sqlAd = new SqlDataAdapter(sqlCmd);
                DataTable resultSet1 = new DataTable();
                sqlAd.Fill(resultSet1);
                int rowCount1 = Convert.ToInt32(resultSet1.Rows[0][0]);

                if (rowCount1 > 0)
                {
                    // Delete the Temp table
                    string sqlString1 = "delete from [BomImport]";
                    SqlCommand sqlCommand1 = new SqlCommand(sqlString1, con);
                    sqlCommand1.ExecuteNonQuery();
                }
            }
            List<string> sn = new List<string>();
            var listvals = f["exFileNames"].ToString();
            string[] splitVals = listvals.Split(new char[] { ',' });

            foreach (var exfile in splitVals)
            {
                sn.Add(exfile);
            }
            int count1 = 1;
            List<BomModel> listProducts = new List<BomModel>();
            foreach (string i in sn)
            {


                using (SqlConnection con = new SqlConnection(conStr))
                {
                    con.Open();

                    string sqlString2 = "select data from [Bom] where ExcelFile='" + i + "'";
                    SqlCommand sqlCommand2 = new SqlCommand(sqlString2, con);
                    SqlDataAdapter ada = new SqlDataAdapter(sqlCommand2);
                    DataTable file = new DataTable();
                    ada.Fill(file);
                    DataRow row = file.Rows[0];
                    Byte[] data = (byte[])sqlCommand2.ExecuteScalar();


                    System.IO.File.WriteAllBytes("C:\\Users\\HARITH\\Documents\\3.30.2017\\ParaQum\\ParaQum\\ProjectBom" + i, data);
                    String path = Server.MapPath("~/ProjectBOM/" + i);

                    //read data from the file
                    Excel.Application application = new Excel.Application();
                    Excel.Workbook worlbook = application.Workbooks.Open(path);
                    Excel.Worksheet worksheet1 = worlbook.ActiveSheet;
                    Excel.Range range = worksheet1.UsedRange;

                    List<HttpPostedFileBase> fileList1 = new List<HttpPostedFileBase>();
                    try
                    {
                        for (int j = 2; j < range.Rows.Count; j++)
                        {

                            BomModel b = new BomModel();
                            Models.BomImport bomImp = new Models.BomImport();

                            b.itemNo = ((Excel.Range)range.Cells[j, 1]).Text;
                            b.customerRef = ((Excel.Range)range.Cells[j, 2]).Text;
                            b.refDesignator = ((Excel.Range)range.Cells[j, 3]).Text;
                            b.qty1 = ((Excel.Range)range.Cells[j, 4]).Text;
                            b.qty10 = ((Excel.Range)range.Cells[j, 5]).Text;
                            b.description = ((Excel.Range)range.Cells[j, 6]).Text;
                            b.value = ((Excel.Range)range.Cells[j, 7]).Text;
                            b.manufacture = ((Excel.Range)range.Cells[j, 8]).Text;
                            b.mpn = ((Excel.Range)range.Cells[j, 9]).Text;
                            b.vsNo = ((Excel.Range)range.Cells[j, 10]).Text;
                            b.vs_TdComment = ((Excel.Range)range.Cells[j, 11]).Text;
                            b.parConfromation = ((Excel.Range)range.Cells[j, 12]).Text;
                            listProducts1.Add(b);
                            bomImp.itemNo = count1;
                            bomImp.customerRef = b.customerRef;
                            bomImp.qty1 = b.qty1;
                            bomImp.qty10 = b.qty10;
                            bomImp.mpn = b.mpn;


                            BomImport newOb = new BomImport();
                            //check whether component is already exists in the table
                            string sqlComp = "select customerRef from [BomImport] where mpn='" + bomImp.mpn + "'";
                            SqlCommand sqlCommandComp = new SqlCommand(sqlComp, con);
                            SqlDataAdapter sqlAdapter = new SqlDataAdapter(sqlCommandComp);

                            DataTable resultSet = new DataTable();
                            sqlAdapter.Fill(resultSet);
                            try
                            {
                                string custRef = Convert.ToString(resultSet.Rows[0][0]);

                                if (b.customerRef == custRef)
                                {
                                    string sqlCommandUpdate = "update [dbo].[BomImport] set qty1=qty1+" + b.qty1 + ",qty10=qty10+" + b.qty10 + " where customerRef ='" + b.customerRef + "'";
                                    SqlCommand commandUpdate = new SqlCommand(sqlCommandUpdate, con);
                                    commandUpdate.ExecuteNonQuery();

                                }
                            }
                            catch (Exception e)
                            {
                                {
                                    string sqlCommand = "insert into [dbo].[BomImport](itemNo,customerRef,qty1,qty10,mpn) values(" + bomImp.itemNo + ",'" + bomImp.customerRef + "'," + b.qty1 + "," + bomImp.qty10 + ",'" + bomImp.mpn + "')";
                                    SqlCommand command = new SqlCommand(sqlCommand, con);
                                    command.ExecuteNonQuery();
                                }
                            }
                            count1++;
                        }
                        ViewBag.ListProduct = listProducts1;

                    }

                    catch (Exception ex)
                    {
                        throw ex;
                    }

                }

            }

            Session["data"] = sn;
            return View("Success");

        }

        public ActionResult Cancel(UpdateComp uc)
        {
            try
            {
                OleDbConnection conn = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\Users\HARITH\Documents\PQDataBook (1).mdb");

                OleDbCommand cmd = new OleDbCommand();
                conn.Open();
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    con.Open();
                    string sqlCommand = "select * from [BomOrderTemp]";
                    SqlCommand cmd2 = new SqlCommand(sqlCommand, con);
                    SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd2);
                    DataTable resultSet = new DataTable();
                    sqlAdapter.Fill(resultSet);
                    int rowCount = Convert.ToInt32(resultSet.Rows[0][0]);
                    var num = resultSet.Select();
                    int a, b, itemNo, qty1, qty10;
                    string s, mpn;


                    //updating db
                    for (int i = 0; i < num.Length; i++)
                    {
                        a = int.Parse(resultSet.Rows[i][1].ToString().Substring(2));
                        b = Convert.ToInt32(resultSet.Rows[i][3]);
                        s = resultSet.Rows[i][1].ToString();
                        itemNo = Convert.ToInt32(resultSet.Rows[i][0]);
                        mpn = resultSet.Rows[i][4].ToString();
                        qty10 = Convert.ToInt32(resultSet.Rows[i][3]);
                        //qty1 = Convert.ToInt32(resultSet.Rows[i][2]);
                        try
                        {



                            if (a <= 1000 && a > 0)
                            {

                            }
                            else if (a > 1000 && a <= 2000)
                            {

                                string cmd1 = "select [ParaQum Stock] from [Capacitor_local] where [Part Label]='" + s + "'";
                                OleDbCommand com2 = new OleDbCommand(cmd1, conn);
                                com2.Parameters.AddWithValue("?", s);
                                int paraQumStock = Convert.ToInt32(com2.ExecuteScalar());
                                string newcmd1 = "select [RequiredQty] from [Capacitor_local] where [Part Label]='" + s + "'";
                                OleDbCommand newcom2 = new OleDbCommand(newcmd1, conn);
                                newcom2.Parameters.AddWithValue("?", s);
                                int requiredQty = Convert.ToInt32(newcom2.ExecuteScalar());

                                requiredQty = requiredQty - qty10;


                                string cmd3 = "UPDATE [Capacitor_local] SET [Stock]=@paraStock,[RequiredQty] = @stock WHERE [Part Label] = @label";

                                OleDbCommand com3 = new OleDbCommand(cmd3, conn);
                                com3.Parameters.AddWithValue("@paraStock", paraQumStock);
                                com3.Parameters.AddWithValue("@stock", requiredQty);
                                com3.Parameters.AddWithValue("@label", s);
                                com3.ExecuteNonQuery();

                                string sqlInsertCommand = "delete from [dbo].[BomOrderTemp] where itemNo=" + itemNo;
                                SqlCommand insCommand = new SqlCommand(sqlInsertCommand, con);
                                insCommand.ExecuteNonQuery();
                            }


                        }
                        catch (Exception e)
                        {

                        }

                    }


                    string sqlCommand1 = "update [dbo].[Bom] set State='PENDING' where State='PROCESSED'";

                    SqlCommand command = new SqlCommand(sqlCommand1, con);
                    command.ExecuteNonQuery();

                }
            }
            catch (Exception e)
            {

            }




            TempData["Componenet"] = "Rollback completed...";
            return View("ExportToExcel");
        }
    }
}