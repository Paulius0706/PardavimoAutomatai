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
    public class VendorNeedsController : Controller
    {
        public void addRow(string sqlquery) {
            string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
            MySqlConnection mySqlConnection = new MySqlConnection(conn);
            MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
            mySqlConnection.Open();
            mySqlCommand.ExecuteNonQuery();
            mySqlConnection.Close();
        }
        public DataTable getList(string sqlquery) {
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
        public int getNumber(string sqlquery) {
            int ats = 1;
            string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
            MySqlConnection mySqlConnection = new MySqlConnection(conn);
            MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
            mySqlConnection.Open();
            MySqlDataAdapter mda = new MySqlDataAdapter(mySqlCommand);
            DataTable dt = new DataTable();
            mda.Fill(dt);

            if (dt.Rows.Count !=0) {
                DataRow dr = dt.Rows[0];
                ats = Convert.ToInt32(dr["id"]);
            }
            
            mySqlConnection.Close();
            return ats;
        }

        // GET: VendorNeeds
        public ActionResult Index()
        {
            string sqlquery = "" +
                "SELECT v.`Id` as `Id`, v.`SlotsCount` as `SlotCount`, v.`Model` as `Model`, s.`Name` as `State`, l.`Address` as `Location` " +
                "FROM ((`vendormachine` v " +
                "       LEFT JOIN `vendorstate` s ON v.`State`       = s.`id`) " +
                "       LEFT JOIN `location`    l ON v.`fk_Location` = l.`id`) " +
                "WHERE 1";

            DataTable vnedors = getList(sqlquery);
            List<VendorNeedUnit> list = new List<VendorNeedUnit>();
            foreach (DataRow vendor in vnedors.Rows)
            {
                VendorNeedUnit vendorNeedUnit = new VendorNeedUnit();
                vendorNeedUnit.id        = Convert.ToInt32(vendor["Id"]);
                vendorNeedUnit.location  = Convert.ToString(vendor["Location"]);
                vendorNeedUnit.model     = Convert.ToString(vendor["Model"]);
                vendorNeedUnit.state     = Convert.ToString(vendor["State"]);
                vendorNeedUnit.slotCount = Convert.ToInt32(vendor["SlotCount"]);

                string slotSqlquery = "" +
                    "SELECT s.`Id` as `Id`, s.`Count` as `Count`, s.`Name` as `Name`, s.`fk_ProductType` as `ProductID`, p.`Name` as `Product`, s.`fk_VendorMachine` as `Vendor` " +
                    "FROM (`slot` s LEFT JOIN `producttype` p ON s.`fk_ProductType` = p.`id`) " +
                    "WHERE s.`fk_VendorMachine` = " + vendorNeedUnit.id;
                DataTable slots = getList(slotSqlquery);
                foreach(DataRow slot in slots.Rows)
                {
                    VendorNeedUnitSlot vendorNeedUnitSlot = new VendorNeedUnitSlot();
                    vendorNeedUnitSlot.id        = Convert.ToInt32(slot["Id"]);
                    vendorNeedUnitSlot.name      = Convert.ToString(slot["Name"]);
                    vendorNeedUnitSlot.product   = Convert.ToString(slot["Product"]);
                    vendorNeedUnitSlot.productID = Convert.ToInt32(slot["ProductID"]);
                    vendorNeedUnitSlot.vendor    = Convert.ToInt32(slot["Vendor"]);
                    vendorNeedUnitSlot.count     = Convert.ToInt32(slot["Count"]);
                    
                    string slotProductsSqlquery = "SELECT `id` FROM `product` WHERE `fk_Slot` = " + vendorNeedUnitSlot.id;
                    vendorNeedUnitSlot.left = getList(slotProductsSqlquery).Rows.Count;
                    vendorNeedUnitSlot.need = vendorNeedUnitSlot.count - vendorNeedUnitSlot.left; //incorrect
                    vendorNeedUnit.slots.Add(vendorNeedUnitSlot);
                }
                list.Add(vendorNeedUnit);
            }
            Session.Add("List", list);
            return View();
        }


        public ActionResult CreateShipment(string vendorID) {
            string sqlquery = "SELECT `id` FROM `shipment` ORDER BY `id` DESC";
            int id = getNumber(sqlquery) + 1;
            //incorrect needs fk_Admin
            sqlquery = "" +
                "INSERT INTO `shipment`(`id`   ,`StartDate`                                     , `Done`, `fk_VendorMachine`   ) " +
                "VALUES                ( "+id+", \"" + DateTime.Now.ToString("yyyy-MM-dd") + "\", 1 , " + vendorID + ")";
            addRow(sqlquery);
            
            string slotSqlquery = "" +
                     "SELECT s.`Id` as `Id`, s.`Count` as `Count`, s.`Name` as `Name`, s.`fk_ProductType` as `ProductID`, p.`Name` as `Product`, s.`fk_VendorMachine` as `Vendor` " +
                     "FROM (`slot` s LEFT JOIN `producttype` p ON s.`fk_ProductType` = p.`id`) " +
                     "WHERE s.`fk_VendorMachine` = " + vendorID;
            DataTable slots = getList(slotSqlquery);
            foreach (DataRow slot in slots.Rows)
            {
                VendorNeedUnitSlot vnus = new VendorNeedUnitSlot();
                vnus.id = Convert.ToInt32(slot["Id"]);
                vnus.name = Convert.ToString(slot["Name"]);
                vnus.productID = Convert.ToInt32(slot["ProductID"]);
                vnus.vendor = Convert.ToInt32(slot["Vendor"]);
                vnus.count = Convert.ToInt32(slot["Count"]);

                string slotProductsSqlquery = "SELECT `id` FROM `product` WHERE `fk_Slot` = " + vnus.id;
                vnus.left = getList(slotProductsSqlquery).Rows.Count;
                vnus.need = vnus.count - vnus.left; //incorrect needs existing shipmnet

                string needsSqlquery = "" +
                    "INSERT INTO `shipmentneeds`(`SlotName`, `Count`, `State`, `fk_Shipment`, `fk_ProductType`) " +
                    "VALUES ( \"" + vnus.name + "\" ," + vnus.need + "," + 1 + "," + id + "," + vnus.productID + ")";
                addRow(needsSqlquery);
            }
            return RedirectToAction("Index");
        }
        
    }
}
