using FitnessPlatform.ViewModel;
using FitnessPlatform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FitnessPlatform.Models
{//取資料的地方
    public class Resource
    {
        FitnessEntities db = new FitnessEntities();
        internal List<CallendarVM> GetTbookData(string memberid, string coachid)
        //撈所有事件資料
        {

            int _memberid = int.Parse(memberid);
            int _coachid = int.Parse(coachid);
            bool isCoach = false;
            if (memberid == "-1")
            {
                isCoach = true;
            }

            //會員預定            
            var memberLst = (from q in db.tOrder
                             join m in db.tMember on q.fMemberId equals m.fMemberId
                             join c in db.tCoach on q.fCoachID equals c.fCoachID
                             where ((q.fMemberId == _memberid) || isCoach) && q.fCoachID == _coachid && q.fSuccessed.ToString() != "2"
                             select new tBookingVM
                             {
                                 GymId = q.fGynId,
                                 OrderID = q.forderid,
                                 MemberId = q.fMemberId,
                                 MemberName = m.fMemberName,
                                 CoachId = q.fCoachID,
                                 CoachName = c.fCoachName,
                                 OrderStatus = q.fSuccessed.ToString(),
                                 StartTime = q.fStartTime,
                                 Endtime = q.fEndTime,
                                 TrainingName = q.fTrainingName,
                                 GymName = (from p in db.tStudio
                                            where p.fId == q.fGynId
                                            select p.fCorpName).FirstOrDefault()
                             }).ToList();
            //教練畫休
            var coachLst = (from q in db.tCoachRest
                            where q.fCoachId == _coachid
                            select new tBookingVM
                            {
                                OrderID = q.fRestId,
                                RestID = q.fRestId,
                                CoachId = q.fCoachId,
                                StartTime = q.fStartTime,
                                Endtime = q.fEndTime,
                                GymName = q.Description,
                                Type = "Coach"
                            }).ToList();

            //會員預定+教練畫休
            var resultLst = memberLst.Union(coachLst).ToList();

            List<CallendarVM> callendarLst = new List<CallendarVM>();
            foreach (var p in resultLst)
            {
                CallendarVM c = new CallendarVM();
                c.type = string.IsNullOrWhiteSpace(p.Type) ? "member" : "coach";
                c.groupId = p.OrderID.ToString();
                c.coachid = p.CoachId.ToString();
                c.coachrestid = p.RestID;
                c.membername = p.MemberName;
                c.title = p.TrainingName;
                c.start = p.StartTime.ToString("yyyy-MM-dd HH:mm");
                c.end = p.Endtime.ToString("yyyy-MM-dd HH:mm");
                c.color = "steelblue";
                c.GymName = p.GymName;
                c.GymId = p.GymId;
                c.TrainingName = p.TrainingName;
                c.OrderStatus = p.OrderStatus;
                c.CoachName = p.CoachName;
                callendarLst.Add(c);
            }
            return callendarLst;
        }

        internal List<CallendarVM> GetMemberbookData(string memberid = "2")
        //會員檢視自己排的課
        {
            int _memberid = int.Parse(memberid);
            var resultLst = (from q in db.tOrder
                             join m in db.tMember on q.fMemberId equals m.fMemberId
                             join c in db.tCoach on q.fCoachID equals c.fCoachID
                             where (q.fMemberId == _memberid)
                             select new tBookingVM
                             {
                                 GymId = q.fGynId,
                                 OrderID = q.forderid,
                                 MemberId = q.fMemberId,
                                 MemberName = m.fMemberName,
                                 CoachName = c.fCoachName,
                                 CoachId = q.fCoachID,
                                 OrderStatus = q.fSuccessed.ToString(),
                                 StartTime = q.fStartTime,
                                 Endtime = q.fEndTime,
                                 TrainingName = q.fTrainingName,
                                 GymName = (from p in db.tStudio
                                            where p.fId == q.fGynId
                                            select p.fCorpName).FirstOrDefault()
                             }).ToList();

            List<CallendarVM> callendarLst = new List<CallendarVM>();
            foreach (var p in resultLst)
            {
                CallendarVM c = new CallendarVM();
                c.type = string.IsNullOrWhiteSpace(p.Type) ? "member" : "coach";
                c.groupId = p.OrderID.ToString();
                c.coachid = p.CoachId.ToString();
                c.coachrestid = p.RestID;
                c.membername = p.MemberName;
                c.title = p.TrainingName;
                c.start = p.StartTime.ToString("yyyy-MM-dd HH:mm");
                c.end = p.Endtime.ToString("yyyy-MM-dd HH:mm");
                c.color = "steelblue";
                c.GymName = p.GymName;
                c.GymId = p.GymId;
                c.TrainingName = p.TrainingName;
                c.OrderStatus = p.OrderStatus;
                c.CoachName = p.CoachName;
                callendarLst.Add(c);
            }
            return callendarLst;
        }

        internal List<CallendarVM> GetTbookDataByid(string id, string coachid, string memberid)
        //撈所有事件資料
        {
            int _id = int.Parse(id);
            int _memberid = int.Parse(memberid);
            int _coachid = int.Parse(coachid);

            //會員預定            
            var memberLst = (from q in db.tOrder
                             join m in db.tMember on q.fMemberId equals m.fMemberId
                             where (q.fMemberId == _memberid) && q.fCoachID == _coachid && q.forderid == _id
                             select new tBookingVM
                             {
                                 GymId = q.fGynId,
                                 OrderID = q.fLessonId,
                                 MemberId = q.fMemberId,
                                 MemberName = m.fMemberName,
                                 CoachId = q.fCoachID,
                                 OrderStatus = q.fSuccessed.ToString(),
                                 StartTime = q.fStartTime,
                                 Endtime = q.fEndTime,
                                 TrainingName = q.fTrainingName,
                                 GymName = (from p in db.tStudio
                                            where p.fId == q.fGynId
                                            select p.fCorpName).FirstOrDefault()
                             }).ToList();


            List<CallendarVM> callendarLst = new List<CallendarVM>();
            foreach (var p in memberLst)
            {
                CallendarVM c = new CallendarVM();
                c.type = string.IsNullOrWhiteSpace(p.Type) ? "member" : "coach";
                c.groupId = p.OrderID.ToString();
                c.membername = p.MemberName;
                c.title = p.TrainingName;
                c.start = p.StartTime.ToString("yyyy-MM-dd HH:mm");
                c.end = p.Endtime.ToString("yyyy-MM-dd HH:mm");
                c.color = "steelblue";
                c.GymName = p.GymName;
                c.GymId = p.GymId;
                c.TrainingName = p.TrainingName;
                callendarLst.Add(c);
            }
            return callendarLst;
        }


        public string UpdatetBookings(string datestr, string sdate, string edate, string gymId, string tposition, string smdate, string emdate, string eventid, string coachid, string memberid)
        {

            int _coachid = int.Parse(coachid);

            if (string.IsNullOrWhiteSpace(eventid))
            {
                //新增到資料庫
                DateTime StartTime = Convert.ToDateTime(datestr + " " + sdate + ":" + smdate);
                DateTime Endtime = Convert.ToDateTime(datestr + " " + edate + ":" + emdate);

                //判斷跟教練排休時間有沒有衝到
                bool conflictCoachTime = (from p in db.tCoachRest
                                          where p.fCoachId == _coachid
                                          && (
                                            (p.fStartTime <= StartTime && p.fEndTime >= Endtime) ||
                                            (p.fStartTime <= Endtime && p.fEndTime >= Endtime))
                                          select p).Any();
                if (conflictCoachTime)
                {
                    return "與教練排休時間衝突!";
                }
                //原寫死需修改
                tOrder t = new tOrder();
                int _fid = (from p in db.tOrder select p.fid).FirstOrDefault();
                int _fOrder = (from p in db.tOrder select p.forderid).FirstOrDefault();
                if (_fid == 0)
                {
                    _fid = 1;
                }
                else
                {
                    _fid = (from p in db.tOrder select p.fid).Max() + 1;
                }
                if (_fOrder == 0)
                {
                    _fOrder = 1;
                }
                else
                {
                    _fOrder = (from p in db.tOrder select p.forderid).Max() + 1;
                }
                t.fid = _fid;
                t.forderid = _fOrder;
                t.fMemberId = int.Parse(memberid);
                t.fCoachID = Convert.ToInt32(coachid);
                t.fGynId = Convert.ToInt32(gymId);
                t.fLessonId = _fOrder;
                t.fSuccessed = 0;
                t.fStartTime = StartTime;
                t.fEndTime = Endtime;
                t.fApplyTime = DateTime.Now;
                t.fOrderlike = -1;
                if (sdate == "23")
                {
                    t.fEndTime = t.fEndTime.AddDays(1);
                }
                t.fTrainingName = tposition;
                db.tOrder.Add(t);
                db.SaveChanges();
            }
            else
            {
                //修改資料庫
                int _eventid = int.Parse(eventid);
                tOrder t = (from p in db.tOrder where p.forderid == _eventid select p).FirstOrDefault();
                DateTime StartTime = Convert.ToDateTime(t.fStartTime.ToString("yyyy/MM/dd") + " " + sdate + ":" + smdate);
                DateTime Endtime = Convert.ToDateTime(t.fEndTime.ToString("yyyy/MM/dd") + " " + edate + ":" + emdate);
                if (t != null)
                {
                    t.fMemberId = int.Parse(memberid);
                    t.fCoachID = _coachid;
                    t.fGynId = Convert.ToInt32(gymId);
                    t.fSuccessed = 0;
                    t.fStartTime = StartTime;
                    t.fEndTime = Endtime;
                    t.fApplyTime = DateTime.Now;
                    t.fLessonId = t.forderid;
                    if (sdate == "23")
                    {
                        t.fEndTime = t.fEndTime.AddDays(1);
                    }
                    t.fTrainingName = tposition;
                    db.Entry(t).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
            }
            return "";
        }

        public string UpdatetCoachRest(string STime, string ETime, string eventid, string coachid, string description)
        {
            DateTime StartTime = Convert.ToDateTime(STime);
            DateTime Endtime = Convert.ToDateTime(ETime);
            int _coachid = int.Parse(coachid);

            //判斷跟自己排休時間有沒有衝到
            bool conflictCoachTime = (from p in db.tCoachRest
                                      where p.fCoachId == _coachid
                                      && (
                                        (p.fStartTime <= StartTime && p.fEndTime >= Endtime) ||
                                        (p.fStartTime <= Endtime && p.fEndTime >= Endtime))
                                      select p).Any();
            //判斷跟學員預約時間有沒有衝到
            bool conflictBookingTime = (from p in db.tOrder
                                        where p.fCoachID == _coachid
                                        && (
                                          (p.fStartTime <= StartTime && p.fEndTime >= Endtime) ||
                                          (p.fStartTime <= Endtime && p.fEndTime >= Endtime))
                                        select p).Any();
            if (conflictBookingTime)
            {
                return "與學員預約時間衝突";
            }
            if (conflictCoachTime && string.IsNullOrWhiteSpace(eventid))
            {
                return "與自己排休時間重複";
            }

            if (string.IsNullOrWhiteSpace(eventid))
            {
                int restid = (from p in db.tCoachRest where p.fCoachId == _coachid select p.fRestId).FirstOrDefault();
                if (restid == 0)
                {
                    bool hasrestid = (from p in db.tCoachRest select p).Any();
                    if (hasrestid == false)
                    {
                        restid = 1;
                    }
                    else {
                        restid = (from p in db.tCoachRest  select p.fRestId).Max() + 1;
                    }                 
                }
                else
                {
                    restid = (from p in db.tCoachRest  select p.fRestId).Max() + 1;
                }
                //新增到資料庫
                tCoachRest t = new tCoachRest();
                t.fRestId = restid;
                t.fCoachId = _coachid;
                t.fStartTime = StartTime;
                t.fEndTime = Endtime;
                t.Description = description;
                db.tCoachRest.Add(t);
                db.SaveChanges();
            }
            else
            {
                //修改資料庫
                int _eventid = int.Parse(eventid);
                tCoachRest t = (from p in db.tCoachRest where p.fRestId == _eventid && p.fCoachId == _coachid select p).FirstOrDefault();
                if (t != null)
                {
                    t.fStartTime = StartTime;
                    t.fEndTime = Endtime;
                    t.Description = description;
                    db.Entry(t).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
            }
            return "";
        }


        //刪除資料庫
        public void DeleteOrder(string id)
        {
            int _id = Convert.ToInt32(id);
            tOrder t = (from p in db.tOrder where p.fLessonId == _id select p).FirstOrDefault();
            db.tOrder.Remove(t);
            db.SaveChanges();
        }
        public void DeleteCourseRest(string id, string coachid)
        {
            int _id = Convert.ToInt32(id);
            int _coachid = int.Parse(coachid);
            tCoachRest t = (from p in db.tCoachRest
                            where p.fRestId == _id
    && p.fCoachId == _coachid
                            select p).FirstOrDefault();
            db.tCoachRest.Remove(t);
            db.SaveChanges();
        }

        internal List<tGymVM> GetGymName()
        {
            var gymList = (from q in db.tStudio
                           select new tGymVM { GymId = q.fId, GymName = q.fCorpName }).ToList();
            List<tGymVM> gymNameList = new List<tGymVM>();
            foreach (var item in gymList)
            {
                tGymVM t = new tGymVM();
                t.GymId = item.GymId;
                t.GymName = item.GymName;
                gymNameList.Add(t);

            }
            return gymNameList;
        }

        internal List<tBookingVM> GetOrderbyID(int id)
        {
            var g = (from q in db.tOrder
                     where q.fGynId == id
                     select new tBookingVM
                     {
                         GymId = q.fGynId,
                         OrderID = q.fLessonId,
                         MemberId = q.fMemberId,
                         CoachId = q.fCoachID,
                         OrderStatus = q.fSuccessed.ToString(),
                         StartTime = q.fStartTime,
                         Endtime = q.fEndTime,
                         TrainingName = q.fTrainingName,
                         GymName = (from p in db.tStudio
                                    where p.fId == q.fGynId
                                    select p.fCorpName).FirstOrDefault()
                     }).ToList();
            return g;
        }

        internal string LoadApplyCount(string coachid)
        {
            int _coachid = int.Parse(coachid);
            string count = (from p in db.tOrder 
                            where p.fSuccessed == 0  && p.fCoachID == _coachid
                            select p).Count().ToString();
            return count;
        }

        internal List<tBookingVM> GetMemBerList(string coachid, string type = "0")
        {
            int _coachid = int.Parse(coachid);
            List<tBookingVM> memberLst = (from p in db.tOrder
                                          join m in db.tMember on p.fMemberId equals m.fMemberId
                                          join g in db.tStudio on p.fGynId equals g.fId
                                          where p.fSuccessed == 0 && p.fCoachID == _coachid
                                          orderby p.fApplyTime descending
                                          select new tBookingVM
                                          {
                                              OrderID = p.forderid,
                                              MemberId = p.fMemberId,
                                              MemberName = m.fMemberName,
                                              fMemberAge = m.fMemberAge,
                                              fMemberGender = m.fMemberGender,
                                              fMemberPhone = m.fMemberPhone,
                                              fMemberEmail = m.fMemberEmail,
                                              TrainingName = p.fTrainingName,
                                              StartTime = p.fStartTime,
                                              Endtime = p.fEndTime,
                                              Applytime = p.fApplyTime,
                                              BirthDay = m.fMemberBirthday,
                                              fCorpName = g.fCorpName

                                          }).ToList();
            if (memberLst == null)
            {
                return new List<tBookingVM>();
            }
            else
            {
                foreach (var p in memberLst)
                {
                
                    try
                    {
                        int age = DateTime.Now.Year - Convert.ToDateTime(p.BirthDay).Year;
                        p.fMemberAge = Byte.Parse(age.ToString());
                    }
                    catch (Exception)
                    {
                        p.fMemberAge = 24;
                    }

                }
            }

            return memberLst;
        }

        public string CompleteTbookData(List<tOrder> applyObj)
        {
            if (applyObj != null)
            {
                foreach (tOrder t in applyObj)
                {
                    tOrder data = (from p in db.tOrder where p.forderid == t.forderid select p).FirstOrDefault();
                    data.fSuccessed = t.fSuccessed;
                    db.Entry(data).Property(x => x.fSuccessed).IsModified = true;
                    db.Entry(data).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
            }
            return "";
        }

    }
}