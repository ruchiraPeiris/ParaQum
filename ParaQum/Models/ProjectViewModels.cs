using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Data;


namespace ParaQum.Models
{
    public class ProjectViewModels
    {

        public int intProjectID { get; set; }
        public string strProjectName { get; set; }

        public int srsId { get; set; }
        public string srsurl { get; set; }
        public string srsDisplayName { get; set; }
        public string srsTitle { get; set; }
        public List<string> ProjectFiles { get; set; }
        public List<string> ProjectFilesPath { get; set; }
        public List<string> ProjectFilesCad { get; set; }
        public List<string> ProjectFilesPathCad { get; set; }
        public List<string> ProjectNotes { get; set; }
        public List<string> ProjectNotesDate { get; set; }
        public List<string> ProjectNotesID { get; set; }
        public DataSet ProjectTable { get; set; }

        public List<string> SrsFiles { get; set; }

        public List<string> SrsFilesPath { get; set; }




        public ProjectViewModels()
        {
            ProjectFiles = new List<string>();
            ProjectNotes = new List<string>();
            ProjectFilesPath = new List<string>();
            ProjectNotesDate = new List<string>();
            ProjectNotesID = new List<string>();
            ProjectTable = new DataSet();
            ProjectFilesCad = new List<string>();
            ProjectFilesPathCad = new List<string>();
            SrsFiles = new List<string>();

            SrsFilesPath = new List<string>();
        }




    }

    public class ProjectData
    {
        public int intProjectID { get; set; }
        public string ProName { get; set; }


    }

    public class DashboardData
    {
        public string sender { get; set; }

        public string NotifID { get; set; }
        public string NotifHead { get; set; }
        public string NotifDate { get; set; }

        public string SrsId { get; set; }
        public string SrsTitle { get; set; }

        public string SrsSendDate { get; set; }

        public string srsPath { get; set; }
    }

    public class UserData
    {
        public List<string> ProjectID { get; set; }
        public List<string> ProName { get; set; }
        public List<string> UserID { get; set; }
        public List<string> UserName { get; set; }
        public List<string> AsignUser { get; set; }
        public List<string> AsignProject { get; set; }

        public UserData()
        {
            ProjectID = new List<string>();
            ProName = new List<string>();
            UserID = new List<string>();
            UserName = new List<string>();
            AsignUser = new List<string>();
            AsignProject = new List<string>();
        }
    }

    public class SrsData
    {
        public int Id { get; set; }
      
        public string senderId { get; set; }
        public List<string> receiverId { get; set; }
        public List<string> receiverName { get; set; }

        public string title { get; set; }
        public bool isChecked { get; set; }
        public DateTime date { get; set; }

        public SrsData()
        {

            receiverId = new List<string>();
            receiverName = new List<string>();

        }

    }

    public class NotificationData
    {
        public List<string> InNotID { get; set; }
        public List<string> InNotName { get; set; }
        public List<string> InNotDate { get; set; }
        public List<string> InNotSendto { get; set; }
        public List<string> InNotRead { get; set; }
        public List<string> OutNotID { get; set; }
        public List<string> OutNotName { get; set; }
        public List<string> OutNotDate { get; set; }
        public List<string> OutNotSendby { get; set; }
        public List<string> UserID { get; set; }
        public List<string> UserName { get; set; }

        public NotificationData()
        {
            InNotID = new List<string>();
            InNotName = new List<string>();
            InNotDate = new List<string>();
            InNotSendto = new List<string>();
            InNotRead = new List<string>();
            OutNotID = new List<string>();
            OutNotName = new List<string>();
            OutNotDate = new List<string>();
            OutNotSendby = new List<string>();
            UserID = new List<string>();
            UserName = new List<string>();
        }
    }

    public class Alertdata
    {
        public string NotifID { get; set; }
        public string NotifUsersend { get; set; }
        public string NotifUserRecive { get; set; }
        public string NotifHead { get; set; }
        public string NotifBody { get; set; }
        public string NotifDate { get; set; }
    }
}
