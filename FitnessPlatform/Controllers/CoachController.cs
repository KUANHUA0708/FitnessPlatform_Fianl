using FitnessPlatform.Models;
using FitnessPlatform.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FitnessPlatform.Controllers
{
    public class CoachController : Controller
    {
        // GET: Coach
        public ActionResult ViewCoach()
        {
            FitnessEntities db = new FitnessEntities();
            var q = from items in db.tCoach
                    where items.fCoachSuccess == 1
                    select items;
            ViewBag.Login = "";
            if (Session[CDictionary.user_using_id] == null)
            {
                ViewBag.Login = "N";
            }
            else{
                ViewBag.Login = "Y";
            }
            return View(q.ToList());
        }
              
       
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Signup(tCoach coach)
        {
            FitnessEntities db = new FitnessEntities();
            coach.fCoachimg = "https://cdn4.iconfinder.com/data/icons/small-n-flat/24/user-group-512.png";
            db.tCoach.Add(coach);
            db.SaveChanges();
            return View();

        }
        public ActionResult Login(int fCoachID)
        {
            return View();
        }


        Resource resource = new Resource();
        // GET: Canlendar行事曆首頁
        public ActionResult Coach(string coachid = "1")
        {
            //前端呼叫資料，此處無東西
            ViewBag.CoachID = coachid;
            return View();
        }


        public ActionResult MemberCheck(string coachid = "1")
        {
            //前端呼叫資料，此處無東西
            ViewBag.CoachID = coachid;
            List<tBookingVM> memberLst = resource.GetMemBerList(coachid);
            return View(memberLst);
        }


        //更新事件到資料庫
        public string UpdateOrder(string StartTime, string EndTime, string eventid, string coachid, string description)
        {
            return resource.UpdatetCoachRest(StartTime, EndTime, eventid, coachid, description);
        }

        //從資料庫刪除事件
        public string DeleteCourseRest(string id, string coachid)
        {
            resource.DeleteCourseRest(id, coachid);
            return "";
        }

        //拿整個行事曆的資料，丟到前端
        public JsonResult GetTbookData(string memberid = "2", string coachid = "1")
        {
            var callendarLst = resource.GetTbookData(memberid, coachid);
            return Json(callendarLst, JsonRequestBehavior.AllowGet);
        }



        public JsonResult GetGymName() //JsonResult是資料庫拿資料，丟到前端
        {
            List<tGymVM> gymNameList = resource.GetGymName();
            return Json(gymNameList, JsonRequestBehavior.AllowGet);
        }

        public string CompleteTbookData(List<tOrder> applyObj)
        {
            return resource.CompleteTbookData(applyObj);
        }
    }
}