using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FitnessPlatform.Controllers
{
    public class GoogleMapForStudioController : Controller
    {
        public ActionResult displayPage() => View();

        public string defaultPage()
        {
            //Models.期末專題_Test資料庫Entities temp = new Models.期末專題_Test資料庫Entities();
            FitnessEntities temp = new FitnessEntities();
            var x = from i in temp.tStudio
                    select new { i.fCorpName, i.fCounty, i.fDistrict, i.cfImage, i.fLatitude, i.fLongitude, i.fAddress };

            string json = JsonConvert.SerializeObject(x);
            return json;
        }
    }
}