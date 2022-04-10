using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Data;
using MySql.Data.MySqlClient;
using PardavimoAutomatai.Models;

namespace PardavimoAutomatai.Controllers
{
    public class ShipmentController : Controller
    {
        public void addRow(string sqlquery)
        {
            string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
            MySqlConnection mySqlConnection = new MySqlConnection(conn);
            MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
            mySqlConnection.Open();
            mySqlCommand.ExecuteNonQuery();
            mySqlConnection.Close();
        }
        public DataTable getList(string sqlquery)
        {
            string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
            MySqlConnection mySqlConnection = new MySqlConnection(conn);
            MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
            mySqlConnection.Open();
            MySqlDataAdapter mda = new MySqlDataAdapter(mySqlCommand);
            DataTable dt = new DataTable();
            mda.Fill(dt);
            mySqlConnection.Close();
            return dt;
        }
        public int getNumber(string sqlquery)
        {
            int ats = 1;
            string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
            MySqlConnection mySqlConnection = new MySqlConnection(conn);
            MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
            mySqlConnection.Open();
            MySqlDataAdapter mda = new MySqlDataAdapter(mySqlCommand);
            DataTable dt = new DataTable();
            mda.Fill(dt);

            if (dt.Rows.Count != 0)
            {
                DataRow dr = dt.Rows[0];
                ats = Convert.ToInt32(dr["id"]);
            }

            mySqlConnection.Close();
            return ats;
        }

        // GET: Shipment
        public ActionResult Index()
        {
            string sqlquery = "SELECT `Id`, `StartDate`, `ComfirmDate`, `EndDate`, `Done`, `fk_Admin`, `fk_VendorMachine`, `fk_Supplier` FROM `shipment` WHERE 1";

            DataTable shipmentsTable = getList(sqlquery);
            List<Shipment> list = new List<Shipment>();
            foreach (DataRow shipmentRow in shipmentsTable.Rows) {
                //public int id { get; set; }
                //public DateTime start { get; set; }
                //public DateTime comfirm { get; set; }
                //public DateTime done { get; set; }
                //public int admin { get; set; }
                //public int vendor { get; set; }
                //public int supplier { get; set; }
                Shipment shipment = new Shipment();
                shipment.id       = !shipmentRow.IsNull("id")               ? Convert.ToInt32(shipmentRow["Id"]) : 0;
                shipment.start    = !shipmentRow.IsNull("StartDate")        ? Convert.ToDateTime(shipmentRow["StartDate"]).ToString("yyyy-MM-dd") : "";
                shipment.comfirm  = !shipmentRow.IsNull("ComfirmDate")      ? Convert.ToDateTime(shipmentRow["ComfirmDate"]).ToString("yyyy-MM-dd") : "";
                shipment.end      = !shipmentRow.IsNull("EndDate")          ? Convert.ToDateTime(shipmentRow["EndDate"]).ToString("yyyy-MM-dd") : "";
                shipment.done     = !shipmentRow.IsNull("Done")             ? Convert.ToString(shipmentRow["Done"]) : "";
                shipment.admin    = !shipmentRow.IsNull("fk_Admin")         ? Convert.ToInt32(shipmentRow["fk_Admin"]) : 0;
                shipment.vendor   = !shipmentRow.IsNull("fk_VendorMachine") ? Convert.ToInt32(shipmentRow["fk_VendorMachine"]) : 0;
                shipment.supplier = !shipmentRow.IsNull("fk_Supplier")      ? Convert.ToInt32(shipmentRow["fk_Supplier"]) : 0;
                list.Add(shipment);
            }
            Session.Add("List", list);
            return View();
        }
        public ActionResult Create()
        {

            return View("Index");
        }
    }
}