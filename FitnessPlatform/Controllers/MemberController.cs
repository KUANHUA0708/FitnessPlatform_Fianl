using FitnessPlatform.Models;
using FitnessPlatform.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FitnessPlatform.Controllers
{
    public class MemberController : Controller
    {
        //GET: Member
       Resource resource = new Resource();
        public ActionResult Member(int coachid)
        {
            coachid = Convert.ToInt32(Session[CDictionary.user_using_id]);
            ViewBag.CoachID = coachid;
            return View();
        }


        public JsonResult GetTbookData()
        {
            int a = Convert.ToInt32(Session[CDictionary.user_using_id]);
            var callendarLst = resource.GetMemberbookData(a.ToString());
            return Json(callendarLst, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetGymName() //JsonResult是資料庫拿資料，丟到前端
        {
            List<tGymVM> gymNameList = resource.GetGymName();
            return Json(gymNameList, JsonRequestBehavior.AllowGet);
        }

        //註冊
        public ActionResult Signup()
        {
            return View("index");
        }
        [HttpPost]
        public ActionResult Signup(tMember items)
        {
            FitnessEntities db = new FitnessEntities();
            items.fMember_time = DateTime.Now + TimeSpan.FromMinutes(10);
            string number = randnum();
            items.fMember_Num = number;
            CSendEmail.sendemail(new string[] { items.fMemberEmail }, CSendEmail.getnum(number));
            items.fMemberPicture = "https://cdn4.iconfinder.com/data/icons/small-n-flat/24/user-group-512.png";
            db.tMember.Add(items);
            db.SaveChanges();
            return RedirectToAction("../Fitness/Index");
        }
        public string randnum()
        {

            string VerificationCode = "";
            Random ramd = new Random();
            for (int i = 0; i < 10; i++)
            {
                switch (ramd.Next(3))
                {
                    case 0://數字
                        VerificationCode += (char)ramd.Next(48, 58);
                        break;
                    case 1://大寫字母
                        VerificationCode += (char)ramd.Next(65, 91);
                        break;
                    case 2://小寫字母
                        VerificationCode += (char)ramd.Next(97, 123);
                        break;
                }
            }
            return VerificationCode;
        }

        [HttpGet]
        public ActionResult CheckMember()
        {
            FitnessEntities db = new FitnessEntities();
            if (Session[CDictionary.user_using_id] != null)
            {
                int userid = (int)Session[CDictionary.user_using_id];
                string nums = HttpContext.Request.QueryString["fMember_Num"];
                var bumbs = (from i in db.tMember
                             where i.fMemberId == userid
                             select i).First();
                if (bumbs != null)
                {
                    if (bumbs.fMember_Num == nums)
                    {

                        bumbs.fMember_Num = null;
                        if (DateTime.Now < bumbs.fMember_time)
                        {
                            ViewBag.message = "驗證成功";
                            bumbs.fMember_approced = "y";
                            db.SaveChanges();
                            return View("CheckMember");
                        }
                        db.SaveChanges();
                    }
                }
            }
            ViewBag.message = "驗證成功";
            return View("CheckMember");
        }
        public ActionResult Login()
        {
            if (Session[CDictionary.user_using_id] ==null)
            {
                return RedirectToAction("Index", "Fitness");
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
            //q2 =======這段不要啟用，會影響到飲食紀錄時間
            //var q2 = from membertime in db.tTime
            //         join memberrecord in db.tRecord
            //         on membertime.fTimeId equals memberrecord.fTimeId
            //         where memberrecord.fMemberId == a
            //         select membertime;
            var q3 = from membertime in db.tTime
                     join memberrecord in db.tRecord
                     on membertime.fTimeId equals memberrecord.fTimeId
                     where memberrecord.fMemberId == a
                     select memberrecord;
            var q4 = from aa in db.tTime
                     select aa;

            var q5 = (from itemsorder in db.tOrder
                      join itemscoach in db.tCoach
                      on itemsorder.fCoachID equals itemscoach.fCoachID
                      where itemsorder.fMemberId == a && itemsorder.fEndTime < q.fMember_time && itemsorder.fOrderlike == -1
                      select itemscoach).FirstOrDefault();


            ct.Cmember = q;
            ct.Corder = q1;
            ct.Ctime = q4;
            ct.Crecord = q3;
            ct.Ccoach = q5;
            return View(ct);
        }
        public ActionResult CoachLogin()
        {
            if (Session[CDictionary.user_using_id] == null)
            {
                return RedirectToAction("Index", "Fitness");
            }
            Ctdatatable ct = new Ctdatatable();
            int a = Convert.ToInt32(Session[CDictionary.user_using_id]);
            FitnessEntities db = new FitnessEntities();
            var q = (from items in db.tCoach
                     where items.fCoachID == a
                     select items).First();
            ViewBag.CoachID = a;
            return View(q);
        }
        [HttpPost]
        public ActionResult Login(string account, string password)
        {
            FitnessEntities db = new FitnessEntities();
            if (string.IsNullOrWhiteSpace(account) || string.IsNullOrWhiteSpace(password)) {
                return RedirectToAction("Index", "Fitness");
            }
            if (Session[CDictionary.account] != null && Session[CDictionary.password] != null)
            {
                account = Session[CDictionary.account].ToString();
                password = Session[CDictionary.password].ToString();
            }
            else {
                Session[CDictionary.account] = account;
                Session[CDictionary.password] = password;
            }
              
            string result = "";
            var q = from items in db.tMember
                    where items.fMemberEmail == account && items.fMemberPassword == password
                    select items;

            var q1 = from items in db.tCoach
                     where items.fCoachMail == account && items.fCoachPassword == password
                     select items;

            if (q.Count() > 0)
            {
                ViewBag.message = q.First().fMemberName + "登入成功";
                Session[CDictionary.user_using_id] = q.First().fMemberId;
                q.First().fMember_time = DateTime.Now;
                db.SaveChanges();
                result = "/Login";
            }
            else if (q1.Count() > 0)
            {
                ViewBag.message = q1.First().fCoachName + "登入成功";
                Session[CDictionary.user_using_id] = q1.First().fCoachID;
                ViewBag.img = q1.First().fCoachimg;
                result = "/CoachLogin";
            }
            else
            {
                result = "登入失敗";
            }     
            return RedirectToAction(result);
        }


        public ActionResult Editmember()
        {
            int a = Convert.ToInt32(Session[CDictionary.user_using_id]);
            FitnessEntities db = new FitnessEntities();
            var q = (from p in db.tMember
                     where p.fMemberId == a
                     select p).First();
            return View(q);
        }

        public ActionResult Logout()
        {
            Session[CDictionary.account] = null;
            Session[CDictionary.password] = null;
            Session.Clear();
            return RedirectToAction("Login");
        }



        [HttpPost]
        public ActionResult Editmember(Ctdatatable member)
        {
            //string photoName = Guid.NewGuid().ToString();
            //photoName += Path.GetExtension(member.Cmember.image.FileName);
            //member.Cmember.image.SaveAs(Server.MapPath("../Images/" + photoName));
            //member.Cmember.fMemberPicture = "../Images/" + photoName;

            FitnessEntities db = new FitnessEntities();
            var q = from items in db.tMember
                    where items.fMemberId == member.Cmember.fMemberId
                    select items;
            q.First().fMemberName = member.Cmember.fMemberName;
            q.First().fMemberGender = member.Cmember.fMemberGender;
            q.First().fMemberPhone = member.Cmember.fMemberPhone;
            q.First().MemberAddress = member.Cmember.MemberAddress;
            q.First().fMemberEmail = member.Cmember.fMemberEmail;
            q.First().fMemberPassword = member.Cmember.fMemberPassword;
            q.First().fMemberPicture = member.Cmember.fMemberPicture;
            db.SaveChanges();
            return RedirectToAction("Login");
        }

        public JsonResult GetAutoComplete(string fname)
        {
            FitnessEntities db = new FitnessEntities();
            bool data = true;
            if (string.IsNullOrWhiteSpace(fname) == false)
            {
                data = false;
            }
            var p = from q in db.tFoodList
                    where q.fFoodName.StartsWith(fname) || data
                    select new { label = q.fId, value = q.fFoodName };
            return Json(p, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public string AddGetGymNameArray(List<string> values)
        {
            FitnessEntities db = new FitnessEntities();
            if (values != null)
            {
                if (values.Count > 2)
                {
                    string flag = "";
                    List<string> a = new List<string>();

                    for (int i = 0; i < values.Count - 2; i++)
                    {
                        if (values[i + 2] != null)
                        {
                            a.Add(values[i + 2]);
                        }

                    }

                    foreach (string foodName in a)
                    {

                        var q = (from p in db.tFoodList
                                 where p.fFoodName == foodName
                                 select p).FirstOrDefault();

                        tRecord tl = new tRecord();
                        tl.fId = q.fId;
                        tl.fRecordFoodCalories = q.fFoodCalories;
                        tl.fRecordFoodFat = q.fFoodFat;
                        tl.fRecordFoodName = q.fFoodName;
                        tl.fRecordFoodProtein = q.fFoodProtein;
                        tl.fRecordCarbohydrate = q.fFoodCarbohydrate;
                        int x = values.Count();
                        tl.fRecordDate = DateTime.Parse(values[0]);
                        tl.fTimeId = Int32.Parse(values[1]);
                        tl.fMemberId = Convert.ToInt32(Session[CDictionary.user_using_id]);
                        db.tRecord.Add(tl);
                        db.SaveChanges();
                    }
                    if (flag != "")
                    {
                        return flag;
                    }
                    else
                    {
                        return "新增成功";
                    }
                }
            }
            return "新增失敗";
        }




        public JsonResult GetBreakfast(string fDateBk)
        {
            //Debug.WriteLine("後段test");
            FitnessEntities db = new FitnessEntities();
            int a = Convert.ToInt32(Session[CDictionary.user_using_id]);
            //DateTime key = Convert.ToDateTime("2020/10/8").Date;
            bool data = true;
            if (string.IsNullOrWhiteSpace(fDateBk) == false)
            {
                data = false;
            }
            DateTime key = Convert.ToDateTime(fDateBk).Date;


            var p = from q in db.tRecord
                    where q.fRecordDate == key && q.fTimeId == 1 && q.fMemberId == a
                    select new { foodname = q.fRecordFoodName, foodprotein = q.fRecordFoodProtein, foodfat = q.fRecordFoodFat, foodcarbohydrate = q.fRecordCarbohydrate };

            //string p = "re";

            return Json(p, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetLunch(string fDateLc)
        {
            //Debug.WriteLine("後段test");
            FitnessEntities db = new FitnessEntities();
            int a = Convert.ToInt32(Session[CDictionary.user_using_id]);
            //DateTime key = Convert.ToDateTime("2020/10/8").Date;
            bool data = true;
            if (string.IsNullOrWhiteSpace(fDateLc) == false)
            {
                data = false;
            }
            DateTime key = Convert.ToDateTime(fDateLc).Date;


            var p = from q in db.tRecord
                    where q.fRecordDate == key && q.fTimeId == 2 && q.fMemberId == a
                    select new { foodname = q.fRecordFoodName, foodprotein = q.fRecordFoodProtein, foodfat = q.fRecordFoodFat, foodcarbohydrate = q.fRecordCarbohydrate };

            //string p = "re";

            return Json(p, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetDinner(string fDateDn)
        {
            //Debug.WriteLine("後段test");
            FitnessEntities db = new FitnessEntities();
            int a = Convert.ToInt32(Session[CDictionary.user_using_id]);
            //DateTime key = Convert.ToDateTime("2020/10/8").Date;
            bool data = true;
            if (string.IsNullOrWhiteSpace(fDateDn) == false)
            {
                data = false;
            }
            DateTime key = Convert.ToDateTime(fDateDn).Date;


            var p = from q in db.tRecord
                    where q.fRecordDate == key && q.fTimeId == 3 && q.fMemberId == a
                    select new { foodname = q.fRecordFoodName, foodprotein = q.fRecordFoodProtein, foodfat = q.fRecordFoodFat, foodcarbohydrate = q.fRecordCarbohydrate };

            //string p = "re";

            return Json(p, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetDessert(string fDateDs)
        {
            //Debug.WriteLine("後段test");
            FitnessEntities db = new FitnessEntities();
            int a = Convert.ToInt32(Session[CDictionary.user_using_id]);
            //DateTime key = Convert.ToDateTime("2020/10/8").Date;
            bool data = true;
            if (string.IsNullOrWhiteSpace(fDateDs) == false)
            {
                data = false;
            }
            DateTime key = Convert.ToDateTime(fDateDs).Date;


            var p = from q in db.tRecord
                    where q.fRecordDate == key && q.fTimeId == 4 && q.fMemberId == a
                    select new { foodname = q.fRecordFoodName, foodprotein = q.fRecordFoodProtein, foodfat = q.fRecordFoodFat, foodcarbohydrate = q.fRecordCarbohydrate };

            //string p = "re";

            return Json(p, JsonRequestBehavior.AllowGet);

        }

        public ActionResult FoodCaloriesToday()
        {
            FitnessEntities db = new FitnessEntities();
            int a = Convert.ToInt32(Session[CDictionary.user_using_id]);
            var today = DateTime.Today;

            var data = (from c in db.tRecord

                        where c.fRecordDate.Value == today &&c.fMemberId==a


                        group c by new { a = c.fRecordDate.Value, b = c.fTimeId.Value } into g
                        //group c by c.fListDate into g 


                        select new { name = g.Key.a.ToString(), gro = g.Key.b, count = g.Sum(w => w.fRecordFoodCalories) }).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FoodCaloriesYesterday()
        {
            FitnessEntities db = new FitnessEntities();
            int a = Convert.ToInt32(Session[CDictionary.user_using_id]);
            var td = DateTime.Today.AddDays(-1);

            var data = (from c in db.tRecord

                        where c.fRecordDate.Value == td && c.fMemberId == a


                        group c by new { a = c.fRecordDate.Value, b = c.fTimeId.Value } into g
                        //group c by c.fListDate into g 


                        select new { name = g.Key.a.ToString(), gro = g.Key.b, count = g.Sum(w => w.fRecordFoodCalories) }).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FoodDietCaloriesThisWeek()
        {
            FitnessEntities db = new FitnessEntities();
            int a = Convert.ToInt32(Session[CDictionary.user_using_id]);
            var t1 = DateTime.Today.AddDays(Convert.ToDouble((0 - Convert.ToInt16(DateTime.Today.DayOfWeek))));
            var t2 = DateTime.Today.AddDays(Convert.ToDouble((6 - Convert.ToInt16(DateTime.Today.DayOfWeek))));
            var t3 = DateTime.Today.AddDays(-4);

            var data = (from c in db.tRecord

                        where c.fRecordDate.Value >= t1 && c.fRecordDate.Value <= t2 && c.fMemberId == a


                        group c by new { a = c.fRecordDate.Value, b = c.fTimeId.Value } into g
                        //group c by c.fListDate into g 


                        select new { name = g.Key.a.ToString(), gro = g.Key.b, count = g.Sum(w => w.fRecordFoodCalories) }).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FoodDietCaloriesLastWeek()
        {
            FitnessEntities db = new FitnessEntities();
            int a = Convert.ToInt32(Session[CDictionary.user_using_id]);
            var t1 = DateTime.Now.AddDays(Convert.ToDouble((0 - Convert.ToInt16(DateTime.Now.DayOfWeek)) - 7));
            var t2 = DateTime.Now.AddDays(Convert.ToDouble((6 - Convert.ToInt16(DateTime.Now.DayOfWeek)) - 7));
            var t3 = DateTime.Today.AddDays(-4);

            var data = (from c in db.tRecord

                        where c.fRecordDate.Value >= t1 && c.fRecordDate.Value <= t2 && c.fMemberId == a


                        group c by new { a = c.fRecordDate.Value, b = c.fTimeId.Value } into g
                        //group c by c.fListDate into g 


                        select new { name = g.Key.a.ToString(), gro = g.Key.b, count = g.Sum(w => w.fRecordFoodCalories) }).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }


        //儲存評論
        [HttpPost]
        public string getcomment(int forderid,string fOrderAbout)
        {
            FitnessEntities db = new FitnessEntities();
            var q = from p in db.tOrder
                    where p.forderid == forderid
                    select p;
            q.First().fOrderAbout = fOrderAbout;
            q.First().fOrderlike = 1;
            db.SaveChanges();
            return "匯入成功";
        }
        [HttpPost]
        public string nogetcomment(int forderid)
        {
            FitnessEntities db = new FitnessEntities();
            var q = from p in db.tOrder
                    where p.forderid == forderid
                    select p;
            q.First().fOrderlike = 0;
            db.SaveChanges();
            return "匯入成功";
        }
    }
}