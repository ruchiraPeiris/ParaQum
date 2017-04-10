using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ParaQum.Models;
using System.Data.OleDb;
using DataAccess;
using System.Data;
using System.IO;
using MVCEmail.Models;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Configuration;


namespace Notes.Controllers
{
    public class ProjectManagementController : Controller
    {

        SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Dbfinal"].ConnectionString);
        SqlCommand cmd;
        SqlDataAdapter myDataAdapter;
        DataSet myDataSet;
        string strSQL;
        int rowcount;

        public ActionResult AdminDashboard()
        {
            //please assign your user id in here
            //Session["userId"] = "1";
            if (Session["type"].ToString()=="4") {


                return RedirectToAction("otherDashboard","ProjectManagement");


            }


            var c = RowCount();

            if (c == 0)

                Session["count"] = null;



            else

                Session["count"] = c;

            var Notification = new List<DashboardData>();

            //conn.Open();


            strSQL = "Select Id,Title,SendDate,SRSurl,SenderName from SRSdoc where IsChecked='false' AND ReceiverId='" + Session["userId"] + "'";
            cmd = new SqlCommand(strSQL, conn);
            myDataAdapter = new SqlDataAdapter(cmd);
            myDataSet = new DataSet();
            myDataAdapter.Fill(myDataSet, "Protop");

            foreach (DataRow r in myDataSet.Tables[0].Rows)
            {
                var alert2 = new DashboardData();

                alert2.SrsId = r[0].ToString();
                alert2.SrsTitle = r[1].ToString();
                alert2.SrsSendDate = DateTime.Parse(r[2].ToString()).ToShortDateString();
                alert2.srsPath = r[3].ToString();
                alert2.sender = r[4].ToString();

                Notification.Insert(0,alert2);

            }

            strSQL = "Select NotID,MsgHead,senddate FROM Notifications WHERE unread='False' AND ReciveUserID='" + Session["userId"] + "'";
            cmd = new SqlCommand(strSQL, conn);
            myDataAdapter = new SqlDataAdapter(cmd);
            myDataSet = new DataSet();
            myDataAdapter.Fill(myDataSet, "Protop");

            foreach (DataRow r in myDataSet.Tables[0].Rows)
            {
                var alert = new DashboardData();
                alert.NotifID = r[0].ToString();
                alert.NotifHead = r[1].ToString();
                alert.NotifDate = DateTime.Parse(r[2].ToString()).ToShortDateString();

                Notification.Add(alert);

            }



            conn.Close();

            return View(Notification);


        }

        public ActionResult AsignProjectNameWindow()
        {


            var users = new UserData();

            conn.Open();
            strSQL = "SELECT ProjectID, ProjectName FROM Project WHERE ProjectID IN (SELECT ProjectID FROM UserProject WHERE UserId='" + Session["userId"] + "')";
            cmd = new SqlCommand(strSQL, conn);
            myDataAdapter = new SqlDataAdapter(cmd);
            myDataSet = new DataSet();
            myDataAdapter.Fill(myDataSet, "Protop1");

            foreach (DataRow r in myDataSet.Tables["Protop1"].Rows)
            {
                users.ProjectID.Add(r[0].ToString());
                users.ProName.Add(r[1].ToString());
            }

            strSQL = "SELECT UserID, UserName FROM [User]";
            cmd = new SqlCommand(strSQL, conn);
            myDataAdapter = new SqlDataAdapter(cmd);
            myDataSet = new DataSet();
            myDataAdapter.Fill(myDataSet, "Protop2");

            foreach (DataRow r in myDataSet.Tables["Protop2"].Rows)
            {
                users.UserID.Add(r[0].ToString());
                users.UserName.Add(r[1].ToString());
            }

            strSQL = "SELECT [User].UserName,Project.ProjectName FROM Project INNER JOIN UserProject ON Project.ProjectId = UserProject.ProjectId INNER JOIN [User] ON UserProject.UserId = [User].UserID WHERE UserProject.ProjectId IN(SELECT ProjectId FROM  UserProject WHERE UserId='" + Session["userId"] + "')";
            cmd = new SqlCommand(strSQL, conn);
            myDataAdapter = new SqlDataAdapter(cmd);
            myDataSet = new DataSet();
            myDataAdapter.Fill(myDataSet, "Protop3");

            foreach (DataRow L in myDataSet.Tables["Protop3"].Rows)
            {
                users.AsignUser.Add(L["UserName"].ToString());
                users.AsignProject.Add(L["ProjectName"].ToString());
            }

            conn.Close();


            return View("AsignNewProjects", users);
        }

        [HttpPost]
        public ActionResult AsignProject(string mnuprojects, string mnuusers)
        {

            if (mnuprojects == "" || mnuusers == "")
                return null;


            conn.Open();

            strSQL = "DELETE FROM UserProject WHERE UserId='" + mnuusers + "' AND ProjectId='" + mnuprojects + "'";

            cmd = new SqlCommand(strSQL, conn);
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();

            strSQL = "INSERT INTO UserProject(ProjectId,UserId) VALUES('" + mnuprojects + "','" + mnuusers + "')";

            cmd = new SqlCommand(strSQL, conn);
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();

            conn.Close();

            return RedirectToAction("AsignProjectNameWindow");

        }

        public ActionResult AllNotoficationsWindow()
        {


            var AlertsData = new NotificationData();

            conn.Open();
            strSQL = "Select NotID,MsgHead,senddate,[User].UserName AS sendto,CASE WHEN unread=0 THEN 'UNREAD' ELSE 'READ' END AS isread  FROM Notifications,[User] WHERE Notifications.SendUserID=[User].UserID AND ReciveUserID='" + Session["userId"] + "' ORDER BY senddate DESC";
            cmd = new SqlCommand(strSQL, conn);
            myDataAdapter = new SqlDataAdapter(cmd);
            myDataSet = new DataSet();
            myDataAdapter.Fill(myDataSet, "Protop1");

            foreach (DataRow r in myDataSet.Tables["Protop1"].Rows)
            {
                AlertsData.InNotID.Add(r[0].ToString());
                AlertsData.InNotName.Add(r[1].ToString());
                AlertsData.InNotDate.Add(DateTime.Parse(r[2].ToString()).ToShortDateString());
                AlertsData.InNotSendto.Add(r[3].ToString());
                AlertsData.InNotRead.Add(r[4].ToString());
            }

            strSQL = "Select NotID,MsgHead,senddate,[User].UserName AS sendby  FROM Notifications,[User] WHERE Notifications.ReciveUserID=[User].UserID AND SendUserID='" + Session["userId"] + "' ORDER BY senddate DESC";
            cmd = new SqlCommand(strSQL, conn);
            myDataAdapter = new SqlDataAdapter(cmd);
            myDataSet = new DataSet();
            myDataAdapter.Fill(myDataSet, "Protop2");

            foreach (DataRow r in myDataSet.Tables["Protop2"].Rows)
            {
                AlertsData.OutNotID.Add(r[0].ToString());
                AlertsData.OutNotName.Add(r[1].ToString());
                AlertsData.OutNotDate.Add(DateTime.Parse(r[2].ToString()).ToShortDateString());
                AlertsData.OutNotSendby.Add(r[3].ToString());
            }

            strSQL = "Select UserID,UserName  FROM [User] WHERE UserID !='" + Session["userId"] + "' ORDER BY UserName ASC";
            cmd = new SqlCommand(strSQL, conn);
            myDataAdapter = new SqlDataAdapter(cmd);
            myDataSet = new DataSet();
            myDataAdapter.Fill(myDataSet, "Protop3");

            foreach (DataRow r in myDataSet.Tables["Protop3"].Rows)
            {
                AlertsData.UserID.Add(r[0].ToString());
                AlertsData.UserName.Add(r[1].ToString());
            }


            conn.Close();


            return View("AllNotifications", AlertsData);
        }

        public ActionResult OpenNewProjectNameWindow(FormCollection form)
        {
            var project = new List<ProjectData>();

            conn.Open();
            strSQL = "SELECT ProjectID, ProjectName FROM Project WHERE ProjectID IN (SELECT ProjectID FROM UserProject WHERE UserId='" + Session["userId"] + "')";
            cmd = new SqlCommand(strSQL, conn);
            myDataAdapter = new SqlDataAdapter(cmd);
            myDataSet = new DataSet();
            myDataAdapter.Fill(myDataSet, "Protop");

            foreach (DataRow r in myDataSet.Tables[0].Rows)
            {
                var prodata = new ProjectData();
                prodata.intProjectID = int.Parse(r[0].ToString());
                prodata.ProName = r[1].ToString();

                project.Add(prodata);
            }
            conn.Close();



            return View("NewProjectName", project);

        }
        public ActionResult SearchProjectsWindow(FormCollection form)
        {


            string connectionString = ConfigurationManager.ConnectionStrings["PqContext"].ConnectionString;
            SqlConnection connection = new SqlConnection(connectionString);
            if (form["Key"] == null)
            {
                //Checking all the Excel files
                DirectoryInfo Info = new DirectoryInfo(@"D:\New folder\");
                FileInfo[] Files = Info.GetFiles("*.xlsx");
                List<string> fileNames = new List<string>();
                foreach (FileInfo file in Files)
                {
                    fileNames.Add(file.Name);
                }
                //End Checking

                //Adding Excel Data

                connection.Open();
                for (int i = 1; i <= fileNames.Count; i++)
                {
                    System.Data.DataTable dt12 = new System.Data.DataTable();

                    var dt =
                        DataAccess.DataTable.New.ReadExcel(@"D:\New folder\" +
                                                           fileNames[i - 1] + "");
                    string sqlQuery = "Select ProjectId as ProjectId from bom where ExcelFile='" + fileNames[i - 1] +
                                      "'";
                    SqlCommand cmd2 = new SqlCommand(sqlQuery, connection);
                    cmd2.ExecuteNonQuery();
                    SqlDataAdapter adt = new SqlDataAdapter(cmd2);
                    adt.Fill(dt12);

                    if (dt12.Rows.Count == 0) continue;
                    string ProjectId = dt12.Rows[0]["ProjectId"].ToString();
                    foreach (Row row in dt.Rows)
                    {
                        // ProjectId = row["ProjectId"].ToString();
                        string sqlQuery2 = "IF (not exists(select * from Referance where ProjectId = '" + ProjectId + "'and PqNos='" + row["Customer Reference"] + "'))" +
                                           "BEGIN " +
                                           "Insert into Referance(ProjectId, PqNos)" +
                                           " values ('" + ProjectId + "','" + row["Customer Reference"] + "')" +
                                           "END";
                        SqlCommand cmd1 = new SqlCommand(sqlQuery2, connection);
                        cmd1.ExecuteNonQuery();
                    }

                }
                //End Adding
            }
            //Start Search by PGNumbers
            List<ProjectData> projectList = new List<ProjectData>();
            if (form["Key"] != null)
            {
                connection.Open();
                string Keyword = form["Key"].ToString();
                string sqlQueryForSearch1 = "Select ProjectId as ProjectID from Referance where PqNos='" + Keyword + "'";
                System.Data.DataTable dtSearch = new System.Data.DataTable();
                SqlCommand cmdSearch1 = new SqlCommand(sqlQueryForSearch1, connection);
                cmdSearch1.ExecuteNonQuery();
                SqlDataAdapter adpt = new SqlDataAdapter(cmdSearch1);
                adpt.Fill(dtSearch);

                System.Data.DataTable dtSearch1 = new System.Data.DataTable();
                for (int i = 1; i <= dtSearch.Rows.Count; i++)
                {
                    string sqlQueryForSearch2 = "Select * from Project where ProjectId ='" + dtSearch.Rows[i - 1]["ProjectId"] + "'";
                    SqlCommand cmdSearch2 = new SqlCommand(sqlQueryForSearch2, connection);
                    cmdSearch2.ExecuteNonQuery();
                    SqlDataAdapter adpt1 = new SqlDataAdapter(cmdSearch2);
                    adpt1.Fill(dtSearch1);

                }
                for (int i = 1; i <= dtSearch1.Rows.Count; i++)
                {

                    ProjectData x = new ProjectData();
                    x.intProjectID = Convert.ToInt32(dtSearch1.Rows[i - 1]["ProjectId"]);
                    x.ProName = dtSearch1.Rows[i - 1]["ProjectName"].ToString();
                    projectList.Insert(i - 1, x);
                }

            }
            //End Search



            var project = new List<ProjectData>();

            conn.Open();
            strSQL = "SELECT ProjectID, ProjectName FROM Project WHERE ProjectID IN (SELECT ProjectID FROM UserProject WHERE UserId='" + Session["userId"] + "')";
            cmd = new SqlCommand(strSQL, conn);
            myDataAdapter = new SqlDataAdapter(cmd);
            myDataSet = new DataSet();
            myDataAdapter.Fill(myDataSet, "Protop");

            foreach (DataRow r in myDataSet.Tables[0].Rows)
            {
                var prodata = new ProjectData();
                prodata.intProjectID = int.Parse(r[0].ToString());
                prodata.ProName = r[1].ToString();

                project.Add(prodata);
            }
            conn.Close();
            //Replace with PG Search Results
            if (projectList != null)
            {
                project = projectList;
            }
            //

            return View("SearchByComponent", project);
        }
        [HttpPost]
        public ActionResult CreateNewProject(string sProjectName)
        {

            if (sProjectName == "")
                return null;

            string intProjectID = "1";


            conn.Open();

            SqlCommand SQLCMD = new SqlCommand("(SELECT ISNULL(MAX(CAST(ProjectId AS int)) + 1, 1) FROM Project)", conn);
            SqlDataAdapter SDA = new SqlDataAdapter(SQLCMD);
            System.Data.DataTable DT = new System.Data.DataTable();
            SDA.Fill(DT);

            if (DT.Rows.Count == 1)
            {
                intProjectID = DT.Rows[0][0].ToString();
            }

            strSQL = "SELECT ProjectID, ProjectName FROM Project WHERE ProjectName = '" + sProjectName + "' ORDER BY ProjectID DESC";
            cmd = new SqlCommand(strSQL, conn);
            myDataAdapter = new SqlDataAdapter(cmd);
            myDataSet = new DataSet();
            myDataAdapter.Fill(myDataSet, "Project");

            if (myDataSet.Tables["Project"].Rows.Count == 0)
            {
                strSQL = "INSERT INTO Project(ProjectId,ProjectName,Date_time,UserId) VALUES('" + intProjectID + "','" + sProjectName + "','" + DateTime.Now + "','" + Session["userId"] + "')";

                cmd = new SqlCommand(strSQL, conn);
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();

                strSQL = "INSERT INTO UserProject(UserId,ProjectId) VALUES('" + Session["userId"] + "','" + intProjectID + "')";

                cmd = new SqlCommand(strSQL, conn);
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();

                //craete project fille& note folder
                string projectFilePath = System.Configuration.ConfigurationManager.AppSettings.Get("ProjectFilePath") + sProjectName + "(" + intProjectID.ToString() + ")/Files";
                string projectNotePath = System.Configuration.ConfigurationManager.AppSettings.Get("ProjectFilePath") + sProjectName + "(" + intProjectID.ToString() + ")/Notes";
                string projecCadPath = System.Configuration.ConfigurationManager.AppSettings.Get("ProjectFilePath") + sProjectName + "(" + intProjectID.ToString() + ")/Design";
                try
                {

                    if (!Directory.Exists(projectNotePath))
                        Directory.CreateDirectory(projectNotePath);
                }
                catch (Exception ex) { }

                myDataSet.Dispose();
                conn.Close();
                return RedirectToAction("NeWProjectWindow", new { intProjectID });
            }
            else
            {
                myDataSet.Dispose();
                conn.Close();
                return Content("<script language='javascript' type='text/javascript'>alert('This Project Name Already Exsist!');window.location.href = 'OpenNewProjectNameWindow';</script>");
            }

        }

        public ActionResult NeWProjectWindow(int intProjectID)
        {
            var project = new ProjectViewModels();

            conn.Open();
            strSQL = "SELECT ProjectID, ProjectName FROM Project WHERE ProjectID =" + intProjectID.ToString();
            cmd = new SqlCommand(strSQL, conn);
            myDataAdapter = new SqlDataAdapter(cmd);
            myDataSet = new DataSet();
            myDataAdapter.Fill(myDataSet, "Project");

            //int intProjectID = Convert.ToInt32(myDataSet.Tables["Project"].Rows[0][0]);
            string strProjectName = myDataSet.Tables["Project"].Rows[0][1].ToString();
            project.intProjectID = intProjectID;
            project.strProjectName = strProjectName;

            strSQL = "SELECT * FROM [Note] WHERE ProjectID =" + intProjectID.ToString();
            cmd = new SqlCommand(strSQL, conn);
            myDataAdapter = new SqlDataAdapter(cmd);
            myDataSet = new DataSet();
            myDataAdapter.Fill(myDataSet, "Note");

            foreach (DataRow r in myDataSet.Tables[0].Rows)
            {
                project.ProjectNotes.Add(r[3].ToString());
                project.ProjectNotesDate.Add(r[1].ToString());
                project.ProjectNotesID.Add(r[0].ToString());
            }

            DirectoryInfo projectDirectory = null;
            FileInfo[] files = null;
            try
            {
                //string projectFilePath = "D:/Projects/DotNet2105/Notes/Projects/TestProject(11)";
                string projectFilePath = Server.MapPath("~/Projects/Pro_" + intProjectID.ToString() + "/Files/");
                projectDirectory = new DirectoryInfo(projectFilePath);
                files = projectDirectory.GetFiles();
                foreach (FileInfo f in files)
                {
                    project.ProjectFiles.Add(f.Name);
                    project.ProjectFilesPath.Add("/Projects/Pro_" + intProjectID.ToString() + "/Files/");
                }

                projectFilePath = Server.MapPath("~/Projects/Pro_" + intProjectID.ToString() + "/Design/");
                projectDirectory = new DirectoryInfo(projectFilePath);
                files = projectDirectory.GetFiles();
                foreach (FileInfo f in files)
                {
                    project.ProjectFilesCad.Add(f.Name);
                    project.ProjectFilesPathCad.Add("/Projects/Pro_" + intProjectID.ToString() + "/Design/");
                }
            }
            catch (DirectoryNotFoundException exp)
            {
            }
            catch (IOException exp)
            {
            }

            myDataSet.Dispose();
            conn.Close();
            return View("NewProject", project);
        }

        public ActionResult DeleteProjet(int intProjectID)
        {

            conn.Open();

            strSQL = "DELETE FROM UserProject WHERE ProjectId='" + intProjectID + "'";
            cmd = new SqlCommand(strSQL, conn);
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();

            strSQL = "DELETE FROM Project WHERE ProjectId='" + intProjectID + "'";
            cmd = new SqlCommand(strSQL, conn);
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();

            strSQL = "DELETE FROM Note WHERE ProjectId='" + intProjectID + "'";
            cmd = new SqlCommand(strSQL, conn);
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();

            string folderPath = Server.MapPath("~/Projects/Pro_" + intProjectID.ToString());

            Directory.Delete(folderPath, true);

            conn.Close();
            return Content("<script language='javascript' type='text/javascript'>alert('This Project Delete Success!');window.location.href = 'OpenNewProjectNameWindow';</script>");
        }

        public ActionResult DeleteProjetFile(string item, int intProjectID)
        {
            string folderPath = Server.MapPath("~/Projects/Pro_" + intProjectID.ToString() + "/Files/" + item);
            if ((System.IO.File.Exists(folderPath)))
            {
                System.IO.File.Delete(folderPath);
            }

            return RedirectToAction("NeWProjectWindow", "ProjectManagement", new { intProjectID });
        }
        
        public ActionResult DeleteProjetCadFile(string item, int intProjectID)
        {
            string folderPath = Server.MapPath("~/Projects/Pro_" + intProjectID.ToString() + "/Design/" + item);
            if ((System.IO.File.Exists(folderPath)))
            {
                System.IO.File.Delete(folderPath);
            }

            return RedirectToAction("NeWProjectWindow", "ProjectManagement", new { intProjectID });
        }

        public ActionResult DeleteProjetNote(string noteid, int intProjectID)
        {
            conn.Open();
            strSQL = "DELETE FROM Note WHERE NoteId='" + noteid + "'";
            cmd = new SqlCommand(strSQL, conn);
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            conn.Close();

            return RedirectToAction("NeWProjectWindow", "ProjectManagement", new { intProjectID });
        }


        [HttpPost]
        public ActionResult Upload(string projectId, string projectName)
        {
            string folderPath = Server.MapPath("~/Projects/Pro_" + projectId.ToString() + "/Files/");

            conn.Open();
            strSQL = "UPDATE Project SET ProjectFolder='" + folderPath + "' WHERE ProjectId='" + projectId + "'";
            cmd = new SqlCommand(strSQL, conn);
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            conn.Close();

            //Check whether Directory (Folder) exists.
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
                    var fileName = Path.GetFileName(file.FileName);
                    var path = strProjectFilePath + "/" + fileName;
                    file.SaveAs(path);
                }
            }
            int intProjectID = Convert.ToInt32(projectId);
            return RedirectToAction("NeWProjectWindow", "ProjectManagement", new { intProjectID });
        }

        [HttpPost]
        public ActionResult Upload1(string projectId, string projectName)
        {
            string folderPath = Server.MapPath("~/Projects/Pro_" + projectId.ToString() + "/Design/");

            conn.Open();
            strSQL = "UPDATE Project SET ProjectDesign='" + folderPath + "' WHERE ProjectId='" + projectId + "'";
            cmd = new SqlCommand(strSQL, conn);
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            conn.Close();

            //Check whether Directory (Folder) exists.
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
                    var fileName = Path.GetFileName(file.FileName);
                    var path = strProjectFilePath + "/" + fileName;
                    file.SaveAs(path);
                }
            }
            int intProjectID = Convert.ToInt32(projectId);
            return RedirectToAction("NeWProjectWindow", "ProjectManagement", new { intProjectID });
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult SaveNote(string projectId1, string projectName1, string notes)
        {
            conn.Open();


            string strNoteName = projectName1 + "_" + projectId1.ToString() + "_" + DateTime.Now.ToString("yyyy-mmm-dd-hh-mm-ss");
            strSQL = "INSERT INTO Note(NoteId,AddedDate,projectid,Descriptin) VALUES ((SELECT ISNULL(MAX(NoteId) + 1, 1) FROM note),'" + DateTime.Now + "','" + projectId1 + "','" + notes.Replace("'", "''") + "')";
            cmd = new SqlCommand(strSQL, conn);
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();


            conn.Close();
            int intProjectID = Convert.ToInt32(projectId1);
            return RedirectToAction("NeWProjectWindow", "ProjectManagement", new { intProjectID });
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult SentMessage(string mnuusers, string msghead, string msgbody)
        {
            conn.Open();


            strSQL = "INSERT INTO Notifications(SendUserID,ReciveUserID,MsgHead,MsgBody,senddate,unread)" +
                " VALUES ('" + Session["userId"] + "','" + mnuusers + "','" + msghead + "','" + msgbody + "','" + DateTime.Now + "','False')";
            cmd = new SqlCommand(strSQL, conn);
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();


            conn.Close();
            return RedirectToAction("AllNotoficationsWindow");
        }

        public ActionResult ViewAlertInbox(int item)
        {
            var Notification = new Alertdata();

            conn.Open();
            strSQL = "UPDATE Notifications SET unread='True' WHERE NotID='" + item + "' AND ReciveUserID='" + Session["userId"] + "'";

            cmd = new SqlCommand(strSQL, conn);
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();

            strSQL = "SELECT     dbo.Notifications.NotID, User_1.UserName, dbo.[User].UserName AS userName2, dbo.Notifications.MsgHead, dbo.Notifications.MsgBody, dbo.Notifications.senddate, dbo.Notifications.unread " +
"FROM         dbo.Notifications INNER JOIN " +
"dbo.[User] AS User_1 ON dbo.Notifications.SendUserID = User_1.UserID INNER JOIN " +
"dbo.[User] ON dbo.Notifications.ReciveUserID = dbo.[User].UserID " +
"WHERE     (dbo.Notifications.NotID = " + item + ")";
            cmd = new SqlCommand(strSQL, conn);
            myDataAdapter = new SqlDataAdapter(cmd);
            myDataSet = new DataSet();
            myDataAdapter.Fill(myDataSet, "Note");

            foreach (DataRow r in myDataSet.Tables[0].Rows)
            {
                Notification.NotifID = r[0].ToString();
                Notification.NotifUsersend = r[1].ToString();
                Notification.NotifUserRecive = r[2].ToString();
                Notification.NotifHead = r[3].ToString();
                Notification.NotifBody = r[4].ToString();
                Notification.NotifDate = r[5].ToString();
            }

            conn.Close();
            return View("ViewNotification", Notification);
        }

        public ActionResult Sent()
        {
            return View();
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Contact(EmailFormModel model)
        {
            if (ModelState.IsValid)
            {
                var body = "<p>Email From: {0} ({1})</p><p>Message:</p><p>{2}</p>";
                var message = new MailMessage();
                message.To.Add(new MailAddress("noelmirani@gmail.com"));  // replace with valid value 
                message.From = new MailAddress("welikalajs@gmail.com");  // replace with valid value
                message.Subject = "Your email subject";
                message.Body = string.Format(body, model.FromName, model.FromEmail, model.Message);
                message.IsBodyHtml = true;

                using (var smtp = new SmtpClient())
                {
                    var credential = new NetworkCredential
                    {
                        UserName = "welikalajs@gmail.com",  // replace with valid value
                        Password = "Juwelikala@55"  // replace with valid value
                    };
                    smtp.Credentials = credential;
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    await smtp.SendMailAsync(message);
                    return RedirectToAction("Sent");
                }
            }
            return View(model);
        }

        public ActionResult OtherDashboard()
        {
            var users = new SrsData();

            conn.Open();
            strSQL = "select UserID,UserName from [User]";
            cmd = new SqlCommand(strSQL, conn);
            myDataAdapter = new SqlDataAdapter(cmd);
            myDataSet = new DataSet();
            myDataAdapter.Fill(myDataSet, "Protop1");

            foreach (DataRow r in myDataSet.Tables["Protop1"].Rows)
            {

                users.receiverId.Add(r[0].ToString());
                users.receiverName.Add(r[1].ToString());
            }


            //PqContext pc = new PqContext();
            //ViewBag.user = new SelectList(pc.User, "UserID", "UserName");



            return View(users);


        }


        public int RowCount()
        {


            strSQL = "select COUNT(*) from SRSdoc where State='" + 1 + "' AND ReceiverId=" + Session["userId"];
            cmd = new SqlCommand(strSQL, conn);


            try
            {
                conn.Open();

                rowcount = (Int32)cmd.ExecuteScalar();
                if (rowcount == 0)
                {

                    Session["count"] = null;

                }
            }

            catch (Exception ex)
            {



            }




            return rowcount;















        }

    }

}