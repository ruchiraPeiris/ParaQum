using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdateStock
{
    public class DbConn
    {
        public OleDbConnection connDb()
        {
<<<<<<< HEAD

            //OleDbConnection conn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\HARITH\Documents\database\PQDataBook.mdb");
            OleDbConnection conn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\HARITH\Documents\newParaDb.mdb");


            //OleDbConnection conn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\gest1\Desktop\newParaDb.accdb");

=======
<<<<<<< HEAD
            OleDbConnection conn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=Provider=Microsoft.Jet.OLEDB.4.0;Data Source=E:\MORA\L2 project\DB\newParaDb.mdb");
=======
<<<<<<< HEAD
            //OleDbConnection conn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\HARITH\Documents\database\PQDataBook.mdb");
            OleDbConnection conn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\HARITH\Documents\newParaDb.mdb");

=======
            OleDbConnection conn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\gest1\Desktop\newParaDb.accdb");
>>>>>>> 2c32efd204509873af8faebd2cae766f00f67796
>>>>>>> 830f7c9b7a715105f5d2e0a92298c9e310bd9f59
>>>>>>> c68a2a8acfb6681036236e174eca743cd5867684
            return conn;
        }
    }
}
