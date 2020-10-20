using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FitnessPlatform.Controllers
{
    public class TDEEController : Controller
    {
        // GET: TDEE
        public ActionResult Calculate()
        {
            return View();
        }
    }
}