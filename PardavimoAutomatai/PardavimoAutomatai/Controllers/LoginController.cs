using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Cryptography;

using System.Configuration;
using System.Data;
using MySql.Data.MySqlClient;


namespace PardavimoAutomatai.Controllers
{
    public class LoginController : Controller
    {
        // Window makers
        //
        public ActionResult Login()
        {
            return View();
        }
        

        // Operations
        //
        public ActionResult CreateWindow()
        {
            return RedirectToAction("Login");
        }
        public ActionResult CheckLoginInformation(string loginName, string password)
        {
            // validation
            //
            bool errorFound = false;
            if (loginName.Equals(""))
            {
                Session["layout_popup"] = "Username or password can't be empty.";
                errorFound = true;
            }
            if (password.Equals(""))
            {
                Session["layout_popup"] = "Username or password can't be empty.";
                errorFound = true;
            }
            if (errorFound)
            {
                return RedirectToAction("Login");
            }
            
            // get account by loginname
            //
            string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
            MySqlConnection mySqlConnection = new MySqlConnection(conn);
            string sqlquery = "SELECT id,password,role FROM `user` WHERE LoginName='"+loginName+"'";
            MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
            mySqlConnection.Open();
            MySqlDataAdapter mda = new MySqlDataAdapter(mySqlCommand);
            DataTable dt = new DataTable();
            mda.Fill(dt);
            mySqlConnection.Close();

            if(dt.Rows.Count == 0)
            {
                Session["layout_popup"] = "The username or password was incorrect.";
                return RedirectToAction("Login");
            }

            // checking password
            //
            DataRow row = dt.Rows[0];
            string savedPasswordHash = (string)row.ItemArray[1];
            byte[] hashBytes = Convert.FromBase64String(savedPasswordHash);
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000);
            byte[] hash = pbkdf2.GetBytes(20);
            for (int i = 0; i < 20; i++)
                if (hashBytes[i + 16] != hash[i])
                {
                    Session["layout_popup"] = "The username or password was incorrect.";
                    return RedirectToAction("Login");
                }

            // information is correct
            //
            Session["userid"] = (string)row.ItemArray[0].ToString();
            Session["userrole"] = row.ItemArray[2].ToString();
            Session["layout_popup"] = "Successfully logged in.";
            return RedirectToAction("OpenMainMenu", "Home");
        }
    }
}