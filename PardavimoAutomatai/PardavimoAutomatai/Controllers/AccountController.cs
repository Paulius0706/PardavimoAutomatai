using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Data;
using MySql.Data.MySqlClient;
using System.Security.Cryptography;

namespace PardavimoAutomatai.Controllers
{
    public class AccountController : Controller
    {
        // Window makers
        //
        public ActionResult Settings(string name, string surname, string loginName, string phone, string email)
        {
            return View(new Models.User() { Name=name, Surname=surname, LoginName=loginName, Phone=phone, Email=email });
        }
        public ActionResult Register()
        {
            return View();
        }
        public ActionResult AllAccounts()
        {
            List<Models.User> users = TempData["account_AllAccounts"] as List<Models.User>;
            return View(users);
        }
        // Operations
        //
        public ActionResult CreateWindow()
        {
            Models.User user = new Models.User();
            int userid = int.Parse(Session["userid"].ToString());
            // get account by id
            //
            string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
            MySqlConnection mySqlConnection = new MySqlConnection(conn);
            string sqlquery = "SELECT Name, Surname, LoginName, Phone, Email FROM `user` WHERE Id=" + userid.ToString() + "";
            MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
            mySqlConnection.Open();
            MySqlDataAdapter mda = new MySqlDataAdapter(mySqlCommand);
            DataTable dt = new DataTable();
            mda.Fill(dt);
            mySqlConnection.Close();

            DataRow row = dt.Rows[0];
            user.Name = row[0].ToString();
            user.Surname = row[1].ToString();
            user.LoginName = row[2].ToString();
            user.Phone = row[3].ToString();
            user.Email = row[4].ToString();

            return RedirectToAction("Settings", "Account", new { name = user.Name, surname = user.Surname, loginName = user.LoginName, phone = user.Phone, email = user.Email });
        }
        public ActionResult CreateAllAccountsWindow()
        {
            List<Models.User> users=new List<Models.User>();
            // get all accounts
            string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
            MySqlConnection mySqlConnection = new MySqlConnection(conn);
            string sqlquery = "SELECT Id, Name, Surname, LoginName, Phone, Email, Role FROM `user`";
            MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
            mySqlConnection.Open();
            MySqlDataAdapter mda = new MySqlDataAdapter(mySqlCommand);
            DataTable dt = new DataTable();
            mda.Fill(dt);
            mySqlConnection.Close();

            foreach(DataRow row in dt.Rows)
            {
                int id = int.Parse(row[0].ToString());
                string name = row[1].ToString();
                string surname = row[2].ToString();
                string loginName = row[3].ToString();
                string phone = row[4].ToString();
                string email = row[5].ToString();
                int role = int.Parse(row[6].ToString());

                users.Add(new Models.User() { Id = id, Name = name, Surname = surname, LoginName = loginName, Phone = phone, Email = email, Role = role });
            }


            TempData["account_AllAccounts"] = users;
            return RedirectToAction("AllAccounts");
        }
        public ActionResult CreateRegisterWindow()
        {
            return RedirectToAction("Register");
        }
        public ActionResult CreateAccount(Models.User user, string selectedRole, string confirmPassword)
        {
            // minor validation
            Session["reg_confirmPasswordError"] = "";
            if (!user.Password.Equals(confirmPassword))
            {
                Session["reg_confirmPasswordError"] = "Confirmation password doesn't match with password";
                return RedirectToAction("Register");
            }
            int role = -1;
            if(selectedRole.Equals("Administrator"))
            {
                role = 0;
            }
            else if (selectedRole.Equals("Supplier"))
            {
                role = 1;
            }
            // password hashing
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);

            var pbkdf2 = new Rfc2898DeriveBytes(user.Password, salt, 100000);
            byte[] hash = pbkdf2.GetBytes(20);

            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            string hashedPassword = Convert.ToBase64String(hashBytes);
            user.Password = "";

            // insertion into database
            string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
            MySqlConnection mySqlConnection = new MySqlConnection(conn);
            string sqlquery = "INSERT INTO `user` (`Name`, `Surname`, `Phone`, `Email`, `LoginName`, `Password`, `Role`) VALUES ('" + user.Name + "', '" + user.Surname + "', '" + user.Phone + "', '" + user.Email + "', '" + user.LoginName + "', '" + hashedPassword + "',"+role+")";
            MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
            mySqlConnection.Open();
            mySqlCommand.ExecuteNonQuery();
            mySqlConnection.Close();

            Session["layout_popup"] = "Successfully created account.";
            return RedirectToAction("CreateAllAccountsWindow");
        }
        public ActionResult DeleteAccount(int id)
        {
            string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
            MySqlConnection mySqlConnection = new MySqlConnection(conn);
            string sqlquery = "DELETE FROM `user` WHERE id="+id;
            MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
            mySqlConnection.Open();
            mySqlCommand.ExecuteNonQuery();
            mySqlConnection.Close();

            Session["layout_popup"] = "Successfully deleted account.";
            return RedirectToAction("CreateAllAccountsWindow");
        }
    }
}