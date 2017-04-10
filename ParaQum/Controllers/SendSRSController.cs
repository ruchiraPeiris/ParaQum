using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ParaQum.Models;
using System.IO;
using System.Data.SqlClient;
using System.Data;

namespace ParaQum.Controllers
{
    public class SendSRSController : Controller


    {

        SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["dbfinal"].ConnectionString);
        SqlCommand cmd;
        SqlDataAdapter myDataAdapter;
        DataSet myDataSet;
        string strSQL;


        // GET: SendSRS
        [HttpPost]
        public ActionResult SendFile(string listusers, SrsData data)

        {
            try
            {

                if (data.title == null)
                {

                    TempData["fileNameEmptyMsg"] = "Please give a file name";
                    return RedirectToAction("otherdashboard", "ProjectManagement");


                }



                string folderPath = Server.MapPath("/SRSDocument/" + listusers);


                if (!Directory.Exists(folderPath))
                {
                    //If Directory (Folder) does not exists. Create it.
                    Directory.CreateDirectory(folderPath);
                }

                var strProjectFilePath = folderPath;

                if (Request.Files.Count > 0)
                {
                    var file = Request.Files[0];

                    if (file != null && file.ContentLength > 0)
                    {



                        var fileName = data.title.ToString() + System.IO.Path.GetExtension(file.FileName);

                        var path = strProjectFilePath + "/" + fileName;

                        conn.Open();
                        strSQL = "insert into [SRSdoc](SRSurl,SenderId,ReceiverId,Title,IsChecked,SendDate,State,SenderName)values('" + path + "','" + Session["userID"] + "','" + listusers + "','" + data.title + "','false','" + DateTime.Now + "','" + 1 + "','" + Session["userName"] + "')";
                        cmd = new SqlCommand(strSQL, conn);
                        myDataAdapter = new SqlDataAdapter(cmd);
                        myDataSet = new DataSet();
                        myDataAdapter.Fill(myDataSet, "Protop1");

                        conn.Close();
                        DirectoryInfo projectDirectory = new DirectoryInfo(folderPath);



                        FileInfo[] files = projectDirectory.GetFiles(data.title + ".*");

                        if (files.Length != 0)
                        {

                            TempData["fileExistMsg"] = "File name already exists,use a different name";

                            return RedirectToAction("otherdashboard", "ProjectManagement");


                        }



                        file.SaveAs(path);




                    }

                    else {


                        TempData["fileEmptyMsg"] = "Please choose a file";


                        return RedirectToAction("otherdashboard", "ProjectManagement");


                    }

                }

                TempData["sentmsg"] = "file sent";


            }

            catch (Exception ex)
            {

                TempData["sentmsg"] = "file uploading falied";

            }
            return RedirectToAction("OtherDashboard", "ProjectManagement");
        }


        public ActionResult DownloadSrs(string srstitle)
        {


            Session["count"] = null;


            conn.Open();
            strSQL = "update SRSdoc set State='" + 0 + "' where ReceiverId=" + Session["userId"];
            cmd = new SqlCommand(strSQL, conn);
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();

            conn.Close();



            var srs = new ProjectViewModels();

            srs.srsDisplayName = srstitle;



            DirectoryInfo projectDirectory = null;
            FileInfo[] files = null;
            try
            {
                if (srstitle == null)
                {
                    string SRSpath2 = Server.MapPath("/SRSDocument/" + Session["userId"] + "/");
                    projectDirectory = new DirectoryInfo(SRSpath2);

                    files = projectDirectory.GetFiles().OrderByDescending(p => p.CreationTime).ToArray();

                }

                else {
                    string SRSpath = Server.MapPath("/SRSDocument/" + Session["userId"] + "/");
                    projectDirectory = new DirectoryInfo(SRSpath);
                    //files = projectDirectory.GetFiles("1*.pdf");
                    files = projectDirectory.GetFiles(srstitle + ".*");
                }
                foreach (FileInfo f in files)
                {

                    srs.SrsFiles.Add(f.Name);
                    srs.SrsFilesPath.Add("/SRSDocument/" + Session["userId"] + "/");
                }


            }
            catch (DirectoryNotFoundException exp)
            {
            }
            catch (IOException exp)
            {
            }




            //myDataSet.Dispose();
            conn.Close();
            return View(srs);


        }

        public ActionResult deleteSrsFile(string item)

        {
            string[] srsfile = item.Split('.');




            try
            {
                conn.Open();

                strSQL = "DELETE FROM SRSdoc WHERE Title='" + srsfile[0] + "'";
                cmd = new SqlCommand(strSQL, conn);
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();

                string folderPath = Server.MapPath("/SRSDocument/" + Session["userId"] + "/" + item);
                if ((System.IO.File.Exists(folderPath)))
                {
                    System.IO.File.Delete(folderPath);
                }
                TempData["srsDeleteMsg"] = "File deleted";

            }

            catch (Exception ex)
            {

                TempData["srsDeleteMsg"] = "Error Occured";

            }
            return RedirectToAction("Admindashboard", "ProjectManagement");

        }




        public void ClearCount()
        {



            try
            {

                conn.Open();

                strSQL = "update SRSdoc set State='" + 0 + "' ";
                cmd = new SqlCommand(strSQL, conn);
                cmd.ExecuteNonQuery();

                TempData["rowCount"] = 0;

            }

            catch (Exception ex)
            {




            }




        }


        public ActionResult DeleteAlLConfirm()
        {


            return View();


        }

        [HttpPost]
        public ActionResult DeleteAll()
        {




            try
            {
                conn.Open();

                strSQL = "DELETE FROM SRSdoc WHERE ReceiverId='" + Session["userId"] + "'";
                cmd = new SqlCommand(strSQL, conn);
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();

                string folderPath = Server.MapPath("/SRSDocument/" + Session["userId"]);
                if (Directory.Exists(folderPath))
                {

                    Directory.Delete(folderPath, true);
                    TempData["srsDeleteMsg"] = "All Files deleted";
                }


            }

            catch (Exception ex)
            {

                TempData["srsDeleteMsg"] = "Error Occured";

            }

            return RedirectToAction("Downloadsrs");

        }

    }
}