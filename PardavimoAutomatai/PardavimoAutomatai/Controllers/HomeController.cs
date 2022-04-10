using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PardavimoAutomatai.Controllers
{
    public class HomeController : Controller
    {
        // Window makers
        //
        public ActionResult Index()
        {
            return View();
        }

        /*public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }*/

        // Operations
        //
        public ActionResult OpenMainMenu()
        {
            return RedirectToAction("Index");
        }
        public ActionResult RequestLogoutUser()
        {
            Session["userid"] = null;
            Session["userrole"] = null;
            Session["layout_popup"] = "Successfully logged out.";
            return RedirectToAction("CreateWindow", "Login");
        }
    }
}