
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FinalTryDemo.Models;
using System.Text.RegularExpressions;

namespace FinalTryDemo.Controllers
{


    public class FetchDataController : Controller
    {
        // GET: Home
        string partNumber;
        string partName;
        string footPrint;

        public ActionResult Index()
        {
            return View();
        }

        //resistor part name and footprint
        private string getFootprintName(string tableType, string package, string mpn)
        {


            if (string.Equals(tableType, "Resistor"))
            {
                if (string.Equals(package, "0201 (0603 Metric)"))
                {
                    footPrint = "RESC0603_IS0201L";
                    partNumber = "R0201";
                }
                else if (string.Equals(package, "0402 (1005 Metric)"))
                {
                    footPrint = "RESC1005_IS0402L";
                    partNumber = "R0402";
                }
                else if (string.Equals(package, "0603 (1608 Metric)"))
                {
                    footPrint = "RESC1608_IS0603N";
                    partNumber = "R0603";
                }
                else if (string.Equals(package, "0805 (2012 Metric)"))
                {
                    footPrint = "RESC2012_IS0805N";
                    partNumber = "R0805";
                }
                else if (string.Equals(package, "1206 (3216 Metric)"))
                {
                    footPrint = "RESC3216_IS1206N";
                    partNumber = "R1206";
                }
                else
                {
                    footPrint = mpn;
                    partNumber = mpn;
                }
            }

            else if (string.Equals(tableType, "Capacitor"))
            {

                if (string.Equals(package, "0201 (0603 Metric)"))
                {
                    footPrint = "CAPC0603_IS0201L";
                    partNumber = "C0201";
                }
                else if (string.Equals(package, "0402 (1005 Metric)"))
                {
                    footPrint = "CAPC1005_IS0402L";
                    partNumber = "C0402";
                }

                else if (string.Equals(package, "0603 (1608 Metric)"))
                {
                    footPrint = "CAPC1608_IS0603N";
                    partNumber = "C0603";
                }
                else if (string.Equals(package, "0805 (2012 Metric)"))
                {
                    footPrint = "CAPC2012_IS0805N";
                    partNumber = "C0805";
                }
                else if (string.Equals(package, "1206 (3216 Metric)"))
                {
                    footPrint = "CAPC3216_IS1206N";
                    partNumber = "C1206";
                }
                else if (string.Equals(package, "2917 (7343 Metric)"))
                {
                    footPrint = "CAPMP7343N";
                    partNumber = "C2917";
                }
                else
                {
                    footPrint = mpn;
                    partNumber = mpn;
                }

            }

            else if (string.Equals(tableType, "Inductor"))
            {

                if (string.Equals(package, "0201 (0603 Metric)"))
                {
                    footPrint = "INDC0603_IS0201L";
                    partNumber = "L0201";
                }
                else if (string.Equals(package, "0402 (1005 Metric)"))
                {
                    footPrint = "INDC1005_IS0402L";
                    partNumber = "L0402";
                }
                else if (string.Equals(package, "0603 (1608 Metric)"))
                {
                    footPrint = "INDC1608_IS0603N";
                    partNumber = "L0603";
                }
                else if (string.Equals(package, "0805 (2012 Metric)"))
                {
                    footPrint = "INDC2012_IS0805N";
                    partNumber = "L0805";
                }
                else if (string.Equals(package, "1206 (3216 Metric)"))
                {
                    footPrint = "INDC3216_IS1206N";
                    partNumber = "L1206";
                }
                else if (string.Equals(package, "1210 (3225 Metric)"))
                {
                    footPrint = "INDS2_APID_S1210N";
                    partNumber = "L1210";
                }
                else
                {
                    footPrint = mpn;
                    partNumber = mpn;
                }
            }

            else if (string.Equals(tableType, "Filter"))
            {

                if (string.Equals(package, "0201 (0603 Metric)"))
                {
                    footPrint = "FILC0603_IS0201L";
                    partNumber = "FIL0201";
                }
                else if (string.Equals(package, "0402 (1005 Metric)"))
                {
                    footPrint = "FILC1005_IS0402L";
                    partNumber = "FIL0402";
                }
                else if (string.Equals(package, "0603 (1608 Metric)"))
                {
                    footPrint = "FILC1608_IS0603N";
                    partNumber = "FIL0603";
                }
                else if (string.Equals(package, "0805 (2012 Metric)"))
                {
                    footPrint = "FILC2012_IS0805N";
                    partNumber = "FIL0805";
                }
                else if (string.Equals(package, "1206 (3216 Metric)"))
                {
                    footPrint = "FILC3216_IS1206N";
                    partNumber = "FIL1206";
                }
                else
                {
                    footPrint = mpn;
                    partNumber = mpn;
                }

            }



            else
            {
                footPrint = mpn;
                partNumber = mpn;
            }
            return footPrint;
        }




        //generate symbol name
        private string createSymbolName(string Description)
        {
            string stringDescription = Description.ToUpper();
            string symbolName;

            if (stringDescription.Contains("CAP") && (stringDescription.Contains("CER") || stringDescription.Contains("FILM")))
                symbolName = "cap";

            else if (stringDescription.Contains("CAP") && (stringDescription.Contains("TANT") || (stringDescription.Contains("POLYMER")) || (stringDescription.Contains("ALUM"))))
                symbolName = "cap-pol";

            else if (stringDescription.Contains("RES"))
                symbolName = "res";

            else if (stringDescription.Contains("IND"))
                symbolName = "ind";

            else if (stringDescription.Contains("FERRITE"))
                symbolName = "bead";
            else if (stringDescription.Contains("MOSFET N-CH"))
                symbolName = "mosfet-n";
            else if (stringDescription.Contains("MOSFET P-CH"))
                symbolName = "mosfet-p";

            else if (stringDescription.Contains("DIODE SCHOTTKY"))
                symbolName = "diode-schtky";
            else if (stringDescription.Contains("DIODE ZENER"))
                symbolName = "diode-schtky";
            else if (stringDescription.Contains("DIODE GEN"))
                symbolName = "diode";

            else symbolName = "";

            return symbolName;

        }



        //generate part name
        private string createPartName(string tableType, string description, string mpn)
        {

            if (string.Equals(tableType, "Capacitor"))
            {
                partName = description.Replace(" ", "-");
            }
            else if (string.Equals(tableType, "Resistor"))
            {
                partName = description.Replace(" ", "-");
            }
            else if (string.Equals(tableType, "Inductors"))
            {
                partName = description.Replace(" ", "-");
            }
            else if (string.Equals(tableType, "Filter"))
            {
                partName = description.Replace(" ", "-");
            }
            else
                partName = mpn;

            return partName;
        }





        //find package metric
        public string remapCasepackage(String package)
        {
            if (string.Equals(package, "0201"))
            {
                package = "0201 (0603 Metric)";
            }
            else if (string.Equals(package, "0402"))
            {
                package = "0402 (1005 Metric)";
            }
            else if (string.Equals(package, "0603"))
            {
                package = "0603 (1608 Metric)";
            }
            else if (string.Equals(package, "0805"))
            {
                package = "0805 (2012 Metric)";
            }
            else if (string.Equals(package, "1206"))
            {
                package = "1206 (3216 Metric)";
            }
            else if (string.Equals(package, "1210"))
            {
                package = "1210 (3225 Metric)";
            }
            else if (string.Equals(package, "1806"))
            {
                package = "1806 (4516 Metric)";
            }
            return package;
        }


        //convert spec values
        public double getNumericalValue(string stringValue)
        {
            double value = Double.NaN;
            string tmpVal;
            if (stringValue != null)
            {


                if (stringValue.Contains("mOhm"))
                {
                    tmpVal = Regex.Replace(stringValue, "[A-Za-zΩ]", "");
                    value = double.Parse(tmpVal) / 1000;
                }
                if (stringValue.Contains("Ohm"))
                {
                    tmpVal = Regex.Replace(stringValue, "[A-Za-zΩ]", "");
                    value = double.Parse(tmpVal);
                }
                else if (stringValue.Contains("a"))
                {
                    tmpVal = Regex.Replace(stringValue, "[A-Za-zΩ]", "");
                    value = double.Parse(tmpVal) / 1000000000000000000;
                }
                else if (stringValue.Contains("f"))
                {
                    tmpVal = Regex.Replace(stringValue, "[A-Za-zΩ]", "");
                    value = double.Parse(tmpVal) / 1000000000000000;
                }
                else if (stringValue.Contains("p"))
                {
                    tmpVal = Regex.Replace(stringValue, "[A-Za-zΩ]", "");
                    value = double.Parse(tmpVal) / 1000000000000;
                }
                else if (stringValue.Contains("n"))
                {
                    tmpVal = Regex.Replace(stringValue, "[A-Za-zΩ]", "");
                    value = double.Parse(tmpVal) / 1000000000;
                }
                else if (stringValue.Contains("µ") || stringValue.Contains("U") || stringValue.Contains("u"))
                {
                    tmpVal = Regex.Replace(stringValue, "[A-Za-zµΩ]", "");
                    value = double.Parse(tmpVal) / 1000000;
                }
                else if (stringValue.Contains("m"))
                {
                    tmpVal = Regex.Replace(stringValue, "[A-Za-zΩ]", "");
                    value = double.Parse(tmpVal) / 1000;
                }
                else if (stringValue.Contains("k"))
                {
                    tmpVal = Regex.Replace(stringValue, "[A-Za-zΩ]", "");
                    value = double.Parse(tmpVal) * 1000;
                }
                else if (stringValue.Contains("%"))
                {
                    tmpVal = Regex.Replace(stringValue, "[A-Za-zΩ%±]", "");
                    value = double.Parse(tmpVal) / 100;
                }
                else if (stringValue.Contains("M"))
                {
                    tmpVal = Regex.Replace(stringValue, "[A-Za-zΩ]", "");
                    value = double.Parse(tmpVal) * 1000000;
                }
                else if (stringValue.Contains("G"))
                {
                    tmpVal = Regex.Replace(stringValue, "[A-Za-zΩ]", "");
                    value = double.Parse(tmpVal) * 1000000000;
                }
                else if (stringValue.Contains("T"))
                {
                    tmpVal = Regex.Replace(stringValue, "[A-Za-zΩ]", "");
                    value = double.Parse(tmpVal) / 1000000000000;
                }
                else if (stringValue.Contains("V") || stringValue.Contains("A") || stringValue.Contains("H") || stringValue.Contains("Ω"))
                {

                    tmpVal = Regex.Replace(stringValue, "[A-Za-z()/Ω]", "");
                    value = double.Parse(tmpVal) * 1.0;
                }
                else
                {
                    tmpVal = Regex.Replace(stringValue, "[A-Za-zΩ]", "");
                    value = double.Parse(tmpVal) * 1.0;
                }

            }
            else
                value = 0;

            return value;
        }




        //main interface
        public ActionResult Interface()
        {
            OctopartModel op = new OctopartModel();
            return View(op);

        }

        [HttpPost]
        public ActionResult AddData(OctopartModel octopart)
        {


            // put the directory path to the Source=
            OleDbConnection conn = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=F:\repositary\database\PQDataBook (1).mdb");

            OleDbCommand cmd = new OleDbCommand();

            string tab = octopart.tab;
            string partLabel = "";
            float currency = Convert.ToSingle(octopart.currencyRate);
            string pq = "";


            // float one = Convert.ToSingle(octopart.one);
            // float ten = Convert.ToSingle(octopart.ten);
            //float hundred = Convert.ToSingle(octopart.hundred);
            // float thousand = Convert.ToSingle(octopart.thousand);
            //we call this separately because it also changes the partNumber
            string footprint = getFootprintName(octopart.tab, remapCasepackage(octopart.package), octopart.mpn);
            string symbol = createSymbolName(octopart.description);
            cmd.Connection = conn;
            conn.Open();

            if (tab != null)
            {
                string cmd2 = "SELECT MAX([Part Label]) FROM [" + tab + "_local]";

                OleDbCommand com1 = new OleDbCommand(cmd2, conn);

                string count = "SELECT [Part Label] FROM [" + tab + "_local]";
                OleDbCommand rows = new OleDbCommand(count, conn);
                OleDbDataAdapter da = new OleDbDataAdapter(rows);
                System.Data.DataSet ds = new System.Data.DataSet();

                da.Fill(ds);


                if (ds.Tables[0].Rows.Count == 0)
                {
                    if (tab == "Capacitor")
                        pq = "PQ1000";

                    else if (tab == "Circuit Protection")
                        pq = "PQ9000";

                    else if (tab == "Connector")
                        pq = "PQ5000";

                    else if (tab == "Diode")
                        pq = "PQ3000";

                    else if (tab == "Display")
                        pq = "PQ10000";

                    else if (tab == "Filter")
                        pq = "PQ15000";

                    else if (tab == "IC")
                        pq = "PQ7000";

                    else if (tab == "Inductor")
                        pq = "PQ4000";

                    else if (tab == "Mechanical")
                        pq = "PQ16000";

                    else if (tab == "Oscillator")
                        pq = "PQ8000";

                    else if (tab == "Resistor")
                        pq = "PQ2000";

                    else if (tab == "Switch")
                        pq = "PQ11000";

                    else if (tab == "Transistor")
                        pq = "PQ6000";
                    else if (tab == "Audio")
                        pq = "PQ12000";
                    else if (tab == "Battery")
                        pq = "PQ13000";
                    else if (tab == "TestPoints")
                        pq = "PQ14000";
                    else if (tab == "Transformer")
                        pq = "PQ17000";
                    else
                        pq = "PQ18000";
                }

                else
                    pq = (String)com1.ExecuteScalar();


                string[] pqArr = pq.Split('Q');

                int pqNum = Int32.Parse(pqArr[1]);
                pqNum++;

                partLabel = "PQ" + pqNum;


            }

            string cmd3 = "SELECT TOP 1 [Manufacturer Part Number] FROM [" + tab + "_local] WHERE [Manufacturer Part Number] ='" + octopart.mpn + "'";
            OleDbCommand com3 = new OleDbCommand(cmd3, conn);



            string cmd1 = "insert into [" + tab + "_local]([Part Number],[Part Name],[Part Label],[Symbol],[Footprint],[Manufacturer Part Description],[Manufacturer Name],[Manufacturer Part Number],[Manufacturer Link],[Supplier Name],[Supplier Part Number],[Supplier Category],[Supplier Sub-Category],[Supplier Link],[Datasheet Link 1],[Datasheet Link 2],[Datasheet Link 3],[Datasheet Link 4],[Datasheet Link 5],[Datasheet Link 6],[Photo Link],[Value],[Resistance],[Capacitance],[Inductance],[Tolerance],[Rated Voltage],[Rated Current],[Rated Power],[Package],[Status],[Digikey Stock],[Unit Cost],[x10],[x100],[x1000],[Author]) values(?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)";
            OleDbCommand com2 = new OleDbCommand(cmd1, conn);
            //   if (octopart.type != "Capacitor")
            //   {
            com2.Parameters.AddWithValue("?", partNumber);
            com2.Parameters.AddWithValue("?", createPartName(octopart.tab, octopart.description, octopart.mpn));
            // }
            /*    else
                {
                    com2.Parameters.AddWithValue("?", octopart.mpn);
                    com2.Parameters.AddWithValue("?", octopart.mpn);
                }*/
            com2.Parameters.AddWithValue("?", partLabel);
            com2.Parameters.AddWithValue("?", symbol);
            com2.Parameters.AddWithValue("?", footprint);
            com2.Parameters.AddWithValue("?", octopart.description);
            com2.Parameters.AddWithValue("?", octopart.manufacturer);
            com2.Parameters.AddWithValue("?", octopart.mpn);
            com2.Parameters.AddWithValue("?", (octopart.manufacturerLink == null) ? Convert.DBNull : octopart.manufacturerLink);
            com2.Parameters.AddWithValue("?", octopart.supplier);
            com2.Parameters.AddWithValue("?", (octopart.suppPartNo == null) ? Convert.DBNull : octopart.suppPartNo);
            com2.Parameters.AddWithValue("?", (octopart.type == null) ? Convert.DBNull : octopart.type);
            com2.Parameters.AddWithValue("?", (octopart.suppSubCategory == null) ? Convert.DBNull : octopart.suppSubCategory);
            com2.Parameters.AddWithValue("?", (octopart.suppLink == null) ? Convert.DBNull : octopart.suppLink);
            com2.Parameters.AddWithValue("?", (octopart.dsLink1 == null) ? Convert.DBNull : octopart.dsLink1);
            com2.Parameters.AddWithValue("?", (octopart.dsLink2 == null) ? Convert.DBNull : octopart.dsLink2);
            com2.Parameters.AddWithValue("?", (octopart.dsLink3 == null) ? Convert.DBNull : octopart.dsLink3);
            com2.Parameters.AddWithValue("?", (octopart.dsLink4 == null) ? Convert.DBNull : octopart.dsLink4);
            com2.Parameters.AddWithValue("?", (octopart.dsLink5 == null) ? Convert.DBNull : octopart.dsLink5);
            com2.Parameters.AddWithValue("?", (octopart.dsLink6 == null) ? Convert.DBNull : octopart.dsLink6);
            com2.Parameters.AddWithValue("?", (octopart.photolink == null) ? Convert.DBNull : octopart.photolink);
            com2.Parameters.AddWithValue("?", (octopart.value == null) ? Convert.DBNull : octopart.value);
            com2.Parameters.AddWithValue("?", getNumericalValue(octopart.resistance));
            com2.Parameters.AddWithValue("?", getNumericalValue(octopart.capacitance));
            com2.Parameters.AddWithValue("?", getNumericalValue(octopart.inductance));
            com2.Parameters.AddWithValue("?", getNumericalValue(octopart.tolerance));
            com2.Parameters.AddWithValue("?", getNumericalValue(octopart.voltage));
            com2.Parameters.AddWithValue("?", getNumericalValue(octopart.current));
            com2.Parameters.AddWithValue("?", getNumericalValue(octopart.power));
            com2.Parameters.AddWithValue("?", (remapCasepackage(octopart.package) == null) ? Convert.DBNull : remapCasepackage(octopart.package));

            if (octopart.stockqty != null)
            {
                if (Int32.Parse(octopart.stockqty) > 0)
                {
                    com2.Parameters.AddWithValue("?", "InStock");
                }
                else
                {
                    com2.Parameters.AddWithValue("?", "NonStock");
                }
            }

            com2.Parameters.AddWithValue("?", octopart.stockqty);

            com2.Parameters.AddWithValue("?", octopart.one);
            com2.Parameters.AddWithValue("?", octopart.ten);
            com2.Parameters.AddWithValue("?", octopart.hundred);
            com2.Parameters.AddWithValue("?", octopart.thousand);

            com2.Parameters.AddWithValue("?", Session["userName"]);

            if ((String)com3.ExecuteScalar() != null)
            {
                TempData["notice"] = "Component already exists.Ignored";
            }
            else
            {
                com2.ExecuteNonQuery();
                TempData["notice"] = "successful.Added to " + tab;
            }


            return RedirectToAction("Interface");
        }

    }
}
