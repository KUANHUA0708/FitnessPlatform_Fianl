using FitnessPlatform.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FitnessPlatform.Controllers
{
    public class FitnessController : Controller
    {
        // GET: Fitness
        public ActionResult Index()
        {
            int i = 0;
            FitnessEntities db = new FitnessEntities();
            var q = (from items in db.tOrder
                     join itemscoach in db.tCoach
                     on items.fCoachID equals itemscoach.fCoachID 
                    where items.fOrderlike == 1 
                    group items by items.fCoachID into g
                    orderby g.Sum(m=>m.fOrderlike) descending
                    select g);
            List<string> img = new List<string>();
            foreach(var w in q)
            {
                foreach(var w1 in w)
                {
                    
                    var x = (from y in db.tCoach
                            where y.fCoachID == w1.fCoachID
                            select y.fCoachimg).ToArray();
                    var a= x[0];
                    img.Add(a);
                    
                }
                if (i >= 3)
                    break;
                i++;
            }
            var t = img.Distinct();
            ViewBag.coachimg = t.ToList() ;
            return View();
        }
        [HttpPost]
        public ActionResult Index(tMember member)
        {
            FitnessEntities db = new FitnessEntities();
            db.tMember.Add(member);
            db.SaveChanges();
            return View();
        }
    }
}