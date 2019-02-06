using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebSubHub.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToAction(actionName: "Edit", controllerName: "Card");
            //ViewBag.Title = "Home Page";

            //return View();
        }
    }
}
