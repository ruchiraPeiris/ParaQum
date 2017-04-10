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

            //OleDbConnection conn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\HARITH\Documents\database\PQDataBook.mdb");
            OleDbConnection conn = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=F:\repositary\database\PQDataBook(1).mdb");


            //OleDbConnection conn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\gest1\Desktop\newParaDb.accdb");

            return conn;
        }
    }
}
