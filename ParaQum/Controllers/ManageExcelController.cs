using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;

namespace ParaQum.Controllers
{
    public class ManageExcelController : Controller
    {
        // GET: ManageExcel
        public ActionResult Index()
        {

            return View();
        }
    }
}