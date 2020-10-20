using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FitnessPlatform;
using FitnessPlatform.Models;
using FitnessPlatform.ViewModel;
using Newtonsoft.Json;

namespace body_fit.Controllers
{
    public class CanlendarController : Controller
    {
        Resource resource = new Resource();
        // GET: Canlendar行事曆首頁
        public ActionResult Booking(string coachid = "7")
        {
            if (Session[CDictionary.user_using_id] == null)
            {
                return RedirectToAction("ViewCoach", "Coach");
            }
            int a = Convert.ToInt32(Session[CDictionary.user_using_id]);
            FitnessEntities db = new FitnessEntities();
            Ctdatatable ct = new Ctdatatable();
            var q = (from itemsmember in db.tMember
                     where itemsmember.fMemberId == a
                     select itemsmember).First();
            var q1 = from itemsorder in db.tOrder
                     where itemsorder.fMemberId == a
                     select itemsorder;
            var q2 = from membertime in db.tTime
                     join memberrecord in db.tRecord
                     on membertime.fTimeId equals memberrecord.fTimeId
                     where memberrecord.fMemberId == a
                     select membertime;
            var q3 = from membertime in db.tTime
                     join memberrecord in db.tRecord
                     on membertime.fTimeId equals memberrecord.fTimeId
                     where memberrecord.fMemberId == a
                     select memberrecord;
            //var q4 = from aa in db.tTime
            //         select aa;

            ct.Cmember = q;
            ct.Corder = q1;
            ct.Ctime = q2;
            ct.Crecord = q3;
            ViewBag.CoachID = coachid;
            ViewBag.MemberID = Session[CDictionary.user_using_id].ToString();
            return View(ct);
        }

        //更新事件到資料庫
        public string UpdateOrder(string datestr, string sdate, string edate, string gymId, string tposition, string smdate, string dmdate, string eventid,string coachid="1", string memberid = "2")
        {
            return resource.UpdatetBookings(datestr, sdate, edate, gymId, tposition, smdate, dmdate, eventid, coachid, memberid);
        }
        //從資料庫刪除事件
        public string DeleteTbookData(string id)
        {
            resource.DeleteOrder(id);
            return "";
        }

        //拿整個行事曆的資料，丟到前端
        public JsonResult GetTbookData(string memberid = "2", string coachid="1")
        {           
            var callendarLst = resource.GetTbookData(memberid, coachid);
            return Json(callendarLst, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTbookDataByid(string id, string coachid = "1", string memberid = "2")
        {
            var callendarLst = resource.GetTbookDataByid(id, coachid, memberid);
            return Json(callendarLst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetGymName() //JsonResult是資料庫拿資料，丟到前端
        {
            List<tGymVM> gymNameList = resource.GetGymName();
            return Json(gymNameList, JsonRequestBehavior.AllowGet);
        }

        public string LoadApplyCount(string coachid) {
            return resource.LoadApplyCount(coachid);
        }

    }
}