using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Objects;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using systems.Helpers;
using systems.Models;

namespace systems.Controllers
{
    public class SpacesController : Controller
    {
        private RoomResEntities spacesdb = new RoomResEntities();
        //
        // GET: /Spaces/

        public ActionResult Index()
        {
            List<BasicReservation> MyReservations = spacesdb.BasicReservations.Where(q => q.UserId == User.Identity.Name).OrderBy(p => p.EventDate).ToList();
            ViewBag.MyReservations = MyReservations;
            return View(spacesdb.ReservableSpaces.Where(q => q.Online == true).OrderBy(item => item.Building).ThenBy(item => item.RoomNum).ToList());
        }

        public ActionResult SpaceDetails(string id)
        {
            ObjectResult<spFetchSpaceByRmCode_Result> space = spacesdb.spFetchSpaceByRmCode(id);

            return View(space);
        }

        // Login versions of forms - all rooms available
        [Authorize]
        public ActionResult RequestReservation(string id)
        {
            ObjectResult<spFetchSpaceByRmCode_Result> space = spacesdb.spFetchSpaceByRmCode(id);
            
            ViewBag.RmCode = id;
            ViewBag.Details = space;
            
            if (space == null)
            {
                // not a valid space. return to spaces list? TODO
                return View();
//                return HttpException(HttpNotFound);
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RequestReservation(BasicReservation reservation)
        {
            if (reservation.ReqUPD == null)
            {
                reservation.ReqUPD = "";
            }
            if (reservation.ResNotes == null)
            {
                reservation.ResNotes = "";
            }
            if (ModelState.IsValid)
            {
                bool isAdmin = fns.SpacesApprover(User.Identity.Name,reservation.RmCode);
                if (isAdmin)
                {
                    reservation.Status = "approved";
                    reservation.ApprovedBy = User.Identity.Name;
                    reservation.ApprovedOn = DateTime.Now;
                }
                spacesdb.BasicReservations.AddObject(reservation);
                spacesdb.SaveChanges();
                var id = reservation.RmCode;
                var recur = Request.Form["DoesRecur"];
                if (fns.IsExtendedRoom(id))
                {
                    return RedirectToAction("RequestReservationExtended", new { id = id, resId = reservation.ResId, recur = recur });
                }
                else if (recur == "Yes")
                {
                    return RedirectToAction("RequestReservationRecur", new { id = id, resId = reservation.ResId, recur = recur });
                }
                else
                {
                    bool sent = fns.Notify_Accept(reservation, isAdmin, true);
                    return RedirectToAction("Success", new { msg = "Entered" });
                }
            }
            else
            {
                return View();
            }
            
        }

        [Authorize]
        public ActionResult RequestReservationRecur(string id, int resId, string recur)
        {
            ViewBag.BaseRes = resId;
            return View();
        }

        [HttpPost]
        public ActionResult RequestReservationRecur()
        {
            DateTime endDT = new DateTime();
            string end = Request.Form["EndDate"];
            endDT = Convert.ToDateTime(end);
            // InjectRecurrences includes notification, so don't need to do that separately.
            bool success = fns.InjectRecurrences(Convert.ToInt32(Request.Form["BaseRes"]), Request.Form["Interval"], endDT);
            if (success)
            {
                return RedirectToAction("Success", new { msg = "Entered" });
            }
            else
            {
                return View();
            }
        }

        [Authorize]
        public ActionResult RequestReservationExtended(string id, int resId, string recur)
        {
            ObjectResult<spFetchSpaceByRmCode_Result> space = spacesdb.spFetchSpaceByRmCode(id);

            ViewBag.RmCode = id;
            ViewBag.Details = space;
            ViewBag.BaseRes = resId;
            ViewBag.DoesRecur = recur;
            return View();
        }

        [HttpPost]
        public ActionResult RequestReservationExtended(ExtendedReservation reservation)
        {
            if (reservation.FacilityReqs == null)
            {
                reservation.FacilityReqs = "";
            }
            if (ModelState.IsValid)
            {
                spacesdb.ExtendedReservations.AddObject(reservation);
                spacesdb.SaveChanges();
                bool success = false;
                if (Request.Form["Interval"] != "" && Request.Form["Interval"] != null)
                {
                    DateTime endDT = new DateTime();
                    string end = Request.Form["EndDate"];
                    endDT = Convert.ToDateTime(end);
                    success = fns.InjectRecurrences(Convert.ToInt32(Request.Form["BaseRes"]), Request.Form["Interval"], endDT);
                }
                else
                {
                    bool isAdmin = fns.SpacesApprover(User.Identity.Name, reservation.BasicReservation.RmCode);
                    bool sent = fns.Notify_Accept(reservation.BasicReservation, isAdmin, true);
                    success = true;
                }
                if (success)
                {
                    return RedirectToAction("Success", new { msg = "Entered" });
                }
                else
                {
                    return View();
                }
            }
            else
            {
                return View();
            }
        }
        // End internal request forms

        //Public versions of forms - locked to EvCtrGW ONLY
        public ActionResult OutsideRequestPartOne()
        {
            ObjectResult<spFetchSpaceByRmCode_Result> space = spacesdb.spFetchSpaceByRmCode("EvCtrGW");

            ViewBag.RmCode = "EvCtrGW";
            ViewBag.Details = space;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult OutsideRequestPartOne(BasicReservation reservation)
        {
            if (reservation.ReqUPD == null)
            {
                reservation.ReqUPD = "";
            }
            if (reservation.ResNotes == null)
            {
                reservation.ResNotes = "";
            }
            if (ModelState.IsValid)
            {
                spacesdb.BasicReservations.AddObject(reservation);
                spacesdb.SaveChanges();
                var id = reservation.RmCode;
                var recur = Request.Form["DoesRecur"];
                return RedirectToAction("OutsideRequestPartTwo", new { id = id, resId = reservation.ResId, recur = recur });
            }
            else
            {
                return View();
            }

        }

        public ActionResult OutsideRequestPartTwo(string id, int resId, string recur)
        {
            ObjectResult<spFetchSpaceByRmCode_Result> space = spacesdb.spFetchSpaceByRmCode(id);

            ViewBag.RmCode = id;
            ViewBag.Details = space;
            ViewBag.BaseRes = resId;
            ViewBag.DoesRecur = recur;
            return View();
        }

        [HttpPost]
        public ActionResult OutsideRequestPartTwo(ExtendedReservation reservation)
        {
            if (ModelState.IsValid)
            {
                spacesdb.ExtendedReservations.AddObject(reservation);
                spacesdb.SaveChanges();
                bool success = false;
                if (Request.Form["Interval"] != "" && Request.Form["Interval"] != null)
                {
                    DateTime endDT = new DateTime();
                    string end = Request.Form["EndDate"];
                    endDT = Convert.ToDateTime(end);
                    success = fns.InjectRecurrences(Convert.ToInt32(Request.Form["BaseRes"]), Request.Form["Interval"], endDT);
                }
                else
                {
                    success = true;
                }
                if (success)
                {
                    return RedirectToAction("PublicSuccess");
                }
                else
                {
                    return View();
                }
            }
            else
            {
                return View();
            }
        }

        public ActionResult PublicSuccess()
        {
            return View();
        }

        // End public forms

        [Authorize]
        public ActionResult Success(string msg)
        {
            // Interpret message string, pass as ViewBag.SystemMessage
            if (msg != "" && msg != null)
            {
                ViewBag.SystemMessage = fns.SystemMessage(msg);
            }
            else
            {
                ViewBag.SystemMessage = "";
            }

            return View();
        }

        [Authorize]
        public ActionResult EditReservation(int id)
        {
            FullReservation res = new FullReservation();
            res.basic = spacesdb.BasicReservations.Single(m => m.ResId == id);
            try
            {
                res.extended = spacesdb.ExtendedReservations.Single(m => m.ResID == id);
            }
            catch (Exception)
            {
                res.extended = null;
            }            
            return View(res);
        }

        [HttpPost]
        public ActionResult EditReservation(FullReservation reservation)
        {
            if (ModelState.IsValid)
            {
                FullReservation saveMe = new FullReservation();
                saveMe.basic = spacesdb.BasicReservations.Single(m => m.ResId == reservation.basic.ResId);
                try
                {
                    saveMe.extended = spacesdb.ExtendedReservations.Single(m => m.ResID == reservation.basic.ResId);
                }
                catch (Exception)
                {
                    saveMe.extended = null;
                }
                bool requiresApproval = false;
                saveMe.basic.EventName = reservation.basic.EventName;
                saveMe.basic.EventDesc = reservation.basic.EventDesc;
                if (saveMe.basic.EventDate != reservation.basic.EventDate)
                {
                    requiresApproval = true;
                }
                saveMe.basic.EventDate = reservation.basic.EventDate;
                if (saveMe.basic.EndTime != reservation.basic.EndTime)
                {
                    requiresApproval = true;
                }
                saveMe.basic.EndTime = reservation.basic.EndTime;
                if (saveMe.basic.StartTime != reservation.basic.StartTime)
                {
                    requiresApproval = true;
                }
                saveMe.basic.StartTime = reservation.basic.StartTime;
                saveMe.basic.SponsOrg = reservation.basic.SponsOrg;
                saveMe.basic.SponsOrgAdv = reservation.basic.SponsOrgAdv;
                saveMe.basic.ResNotes = reservation.basic.ResNotes;
                saveMe.basic.EventDate = reservation.basic.EventDate;
                saveMe.basic.ReqUPD = reservation.basic.ReqUPD;
                saveMe.basic.ReqGuests = reservation.basic.ReqGuests;
                if (saveMe.basic.ExtendedReservation != null)
                {
                    if (saveMe.basic.ExtendedReservation.EventAdm != reservation.extended.EventAdm)
                    {
                        requiresApproval = true;
                    }
                    saveMe.basic.ExtendedReservation.EventAdm = reservation.extended.EventAdm;
                    saveMe.extended.EventAdm = reservation.extended.EventAdm;
                    if (saveMe.basic.ExtendedReservation.FacilityReqs != reservation.extended.FacilityReqs)
                    {
                        requiresApproval = true;
                    } 
                    saveMe.basic.ExtendedReservation.FacilityReqs = reservation.extended.FacilityReqs;
                    saveMe.extended.FacilityReqs = reservation.extended.FacilityReqs;// Request.Form["FacilityReqs"];
                    if (saveMe.basic.ExtendedReservation.FoodServed != reservation.extended.FoodServed)
                    {
                        requiresApproval = true;
                    }
                    saveMe.basic.ExtendedReservation.FoodServed = reservation.extended.FoodServed;
                    saveMe.extended.FoodServed = reservation.extended.FoodServed;
                    if (saveMe.basic.ExtendedReservation.AlcoholServed != reservation.extended.AlcoholServed)
                    {
                        requiresApproval = true;
                    }
                    saveMe.basic.ExtendedReservation.AlcoholServed = reservation.extended.AlcoholServed;
                    saveMe.extended.AlcoholServed = reservation.extended.AlcoholServed;
                    //if (saveMe.basic.ExtendedReservation.GatewayA != reservation.extended.GatewayA)
                    //{
                    //    requiresApproval = true;
                    //}
                    //saveMe.basic.ExtendedReservation.GatewayA = reservation.extended.GatewayA;
                    //saveMe.extended.GatewayA = reservation.extended.GatewayA;
                    //if (saveMe.basic.ExtendedReservation.FacilityReqs != reservation.extended.FacilityReqs)
                    //{
                    //    requiresApproval = true;
                    //}
                    //saveMe.basic.ExtendedReservation.GatewayB = reservation.extended.GatewayB;
                    //saveMe.extended.GatewayB = reservation.extended.GatewayB;
                    //if (saveMe.basic.ExtendedReservation.FacilityReqs != reservation.extended.FacilityReqs)
                    //{
                    //    requiresApproval = true;
                    //}
                    //saveMe.basic.ExtendedReservation.GatewayC = reservation.extended.GatewayC;
                    //saveMe.extended.GatewayC = reservation.extended.GatewayC;
                }
                bool isAdmin = fns.SpacesApprover(User.Identity.Name, reservation.basic.RmCode);
                if (isAdmin)
                {
                    requiresApproval = false;
                }

                // IF admin status, can make changes without submitting for approval.
                // IF not... if date/time/extended changes, set as 
                string msg = "";
                if (requiresApproval)
                {
                    saveMe.basic.Status = "pending";
                    saveMe.basic.ApprovedBy = null;
                    saveMe.basic.ApprovedOn = null;
                    msg = "EditedPending";

                    bool sent = fns.Notify_Edit(saveMe.basic, User.Identity.Name,false,true);
                }
                else
                {
                    if (isAdmin)
                    {
                        msg = "EditedAdmin";
                        bool sent = fns.Notify_Edit(saveMe.basic, User.Identity.Name,true,false);
                    }
                    else
                    {
                        msg = "Edited";
                        bool sent = fns.Notify_Edit(saveMe.basic, User.Identity.Name, false, false);
                    }
                }
                spacesdb.SaveChanges();

                return RedirectToAction("Success", new { msg = msg });
            }
            else
            {
                FullReservation res = new FullReservation();
                res.basic = spacesdb.BasicReservations.Single(m => m.ResId == reservation.basic.ResId);
                res.extended = spacesdb.ExtendedReservations.Single(m => m.ResID == reservation.basic.ResId);
                return View(res);
            }
        }

        [Authorize]
        public ActionResult ApproveList()
        {
            string permitted = fns.SpacesPermissions(User.Identity.Name);
            if (permitted == "none")
            {
                // no permissions
                return RedirectToAction("Index");
            }
            else
            {
                // split the return string and get viewer/editor/approver permissions and what group
                var perms = permitted.Split('/');
                ViewBag.RmGroup = perms[0];
                ViewBag.Viewer = Convert.ToBoolean(perms[1]);
                ViewBag.Editor = Convert.ToBoolean(perms[2]);
                ViewBag.Approver = Convert.ToBoolean(perms[3]);
                return View();
            }
        }

        [Authorize]
        public ActionResult AdminCalendarList()
        {
            string permitted = fns.SpacesPermissions(User.Identity.Name);
            if (permitted == "none")
            {
                // no permissions
                return RedirectToAction("Index");
            }
            else
            {
                // split the return string and get viewer/editor/approver permissions and what group
                var perms = permitted.Split('/');
                ViewBag.RmGroup = perms[0];
                ViewBag.Viewer = Convert.ToBoolean(perms[1]);
                ViewBag.Editor = Convert.ToBoolean(perms[2]);
                ViewBag.Approver = Convert.ToBoolean(perms[3]);
                return View();
            }
        }

        // Administrative home landing page. Optionally takes a message
        [Authorize]
        public ActionResult Admin(string msg)
        {
            // Interpret message string, pass as ViewBag.SystemMessage
            if (msg != "" && msg != null)
            {
                ViewBag.SystemMessage = "";
            }
            else
            {
                ViewBag.SystemMessage = fns.SystemMessage(msg);
            }
            string permitted = fns.SpacesPermissions(User.Identity.Name);
            if (permitted == "none")
            {
                // no permissions
                return RedirectToAction("Index");
            }
            else
            {
                // split the return string and get viewer/editor/approver permissions and what group
                var perms = permitted.Split('/');
                ViewBag.RmGroup = perms[0];
                ViewBag.Viewer = Convert.ToBoolean(perms[1]);
                ViewBag.Editor = Convert.ToBoolean(perms[2]);
                ViewBag.Approver = Convert.ToBoolean(perms[3]);
                return View();
            }            
        }

        [Authorize]
        public ActionResult AdminCalendar(string id, DateTime monthStart)
        {
            string permitted = fns.SpacesPermissions(User.Identity.Name);
            if (permitted == "none")
            {
                // no permissions
                return RedirectToAction("Index");
            }
            else
            {
                // split the return string and get viewer/editor/approver permissions and what group
                var perms = permitted.Split('/');
                ViewBag.RmGroup = perms[0];
                ViewBag.Viewer = Convert.ToBoolean(perms[1]);
                ViewBag.Editor = Convert.ToBoolean(perms[2]);
                ViewBag.Approver = Convert.ToBoolean(perms[3]);

                List<BasicReservation> reservations = spacesdb.spFetchReservations(id).ToList();
                ReservableSpace space = spacesdb.ReservableSpaces.Single(n => n.RmCode == id);
                ViewBag.Reservations = fns.FullCalendarList(reservations,true);
                ViewBag.SpaceInfo = space;
                ViewBag.MonthStart = monthStart;
                CatalogEntities ts = new CatalogEntities();
                List<string> semesters = ts.spDistinctSemestersTS().ToList();
                string TSOverlay = "";
                foreach (string sem in semesters)
                {
                    if (sem.Substring(0, 6) != "Summer")
                    {

                        if (space.RmCode == "MarshAud")
                        {
                            if (TSOverlay.Length > 5)
                            {
                                TSOverlay = TSOverlay + "," + fns.FullCalendarTSOverlay(sem, space.Building, "AUD");
                            }
                            else
                            {
                                TSOverlay = TSOverlay + fns.FullCalendarTSOverlay(sem, space.Building, "AUD");
                            }
//                            TSOverlay = TSOverlay + fns.FullCalendarTSOverlay(sem, space.Building, "AUD");
                        }
                        else if (space.RmCode == "Nif" || space.RmCode == "Rot" || space.RmCode == "EvCtrGW")
                        {
                            TSOverlay = "";
                        }
                        else
                        {
                            if (TSOverlay.Length > 5)
                            {
                                TSOverlay = TSOverlay + "," + fns.FullCalendarTSOverlay(sem, space.Building, space.RoomNum);
                            }
                            else
                            {
                                TSOverlay = TSOverlay + fns.FullCalendarTSOverlay(sem, space.Building, space.RoomNum);
                            }
                        }
                        
                    }
                }
                if (TSOverlay.Length < 5)
                {
                    TSOverlay = "";
                }
                ViewBag.TSOverlay = TSOverlay;
                return View();
            }
        }

        public ActionResult Calendar(string id, DateTime monthStart)
        {
            string permitted = fns.SpacesPermissions(User.Identity.Name);
            if (permitted == "none")
            {
                ViewBag.Viewer = false;
                ViewBag.Editor = false;
                ViewBag.Approver = false;
            }
            else
            {
                var perms = permitted.Split('/');
                ViewBag.RmGroup = perms[0];
                ViewBag.Viewer = Convert.ToBoolean(perms[1]);
                ViewBag.Editor = Convert.ToBoolean(perms[2]);
                ViewBag.Approver = Convert.ToBoolean(perms[3]);
            }
            List<BasicReservation> reservations = spacesdb.spFetchReservations(id).ToList();
            ReservableSpace space = spacesdb.ReservableSpaces.Single(n => n.RmCode == id);
            ViewBag.Reservations = fns.FullCalendarList(reservations,false);
            ViewBag.SpaceInfo = space;
            ViewBag.MonthStart = monthStart;
            CatalogEntities ts = new CatalogEntities();
            List<string> semesters = ts.spDistinctSemestersTS().ToList();
            string TSOverlay = "";
            foreach (string sem in semesters)
            {
                if (space.RmCode == "MarshAud")
                {
                    if (TSOverlay.Length > 5)
                    {
                        TSOverlay = TSOverlay + "," + fns.FullCalendarTSOverlay(sem, space.Building, "AUD");
                    }
                    else
                    {
                        TSOverlay = TSOverlay + fns.FullCalendarTSOverlay(sem, space.Building, "AUD");
                    }
                }
                else if (space.RmCode == "Nif" || space.RmCode == "Rot")
                {
                    TSOverlay = "";
                }
                else
                {
                    if (TSOverlay.Length > 5)
                    {
                        TSOverlay = TSOverlay + "," + fns.FullCalendarTSOverlay(sem, space.Building, space.RoomNum);
                    }
                    else
                    {
                        TSOverlay = TSOverlay + fns.FullCalendarTSOverlay(sem, space.Building, space.RoomNum);
                    }
                }
            }
            if (TSOverlay.Length < 5)
            {
                TSOverlay = "";
            }
            ViewBag.TSOverlay = TSOverlay;
            return View();
        }

        // Calendar by building
        [Authorize]
        public ActionResult AdminBldgCalendar(string id, DateTime monthStart)
        {
            // id in this case is the *building* name, not just a room.
            string permitted = fns.SpacesPermissions(User.Identity.Name);
            if (permitted == "none")
            {
                // no permissions
                return RedirectToAction("Index");
            }
            else
            {
                // split the return string and get viewer/editor/approver permissions and what group
                var perms = permitted.Split('/');
                ViewBag.RmGroup = perms[0];
                ViewBag.Viewer = Convert.ToBoolean(perms[1]);
                ViewBag.Editor = Convert.ToBoolean(perms[2]);
                ViewBag.Approver = Convert.ToBoolean(perms[3]);
                
                List<ReservableSpace> spacesInBuilding = spacesdb.spFetchSpacesByBuilding(id).ToList();
                List<BasicReservation> reservations = new List<BasicReservation>();
                string Bldg = "";
                string Room = "";
                CatalogEntities ts = new CatalogEntities();
                List<string> semesters = ts.spDistinctSemestersTS().ToList();
                foreach (ReservableSpace space in spacesInBuilding)
                {
                    List<BasicReservation> thisSpaceList = spacesdb.spFetchReservations(space.RmCode).ToList();
                    foreach (BasicReservation res in thisSpaceList)
                    {
                        reservations.Add(res);
                    }
                    Bldg = space.Building;
                    Room = space.RoomNum;
                }
                ViewBag.Reservations = fns.FullCalendarList(reservations,true);
                ViewBag.BuildingName = Bldg;

                return View();
            }
        }

        // Calendar by building
        public ActionResult AllLabsCalendar(string id = "Baker309")
        {
            
            List<ReservableSpace> spacesInBuilding = new List<ReservableSpace>();
            spacesInBuilding.Add(spacesdb.ReservableSpaces.First(m => m.RmCode == id));
            
            List<BasicReservation> reservations = new List<BasicReservation>();
            string Bldg = "";
            string Room = "";
            CatalogEntities ts = new CatalogEntities();
            List<string> semesters = ts.spDistinctSemestersTS().ToList();
            string TSOverlay = "";
            ReservableSpace space = spacesdb.ReservableSpaces.Single(m => m.RmCode == id);
            List<BasicReservation> thisSpaceList = spacesdb.spFetchReservations(space.RmCode).ToList();
            foreach (BasicReservation res in thisSpaceList)
            {
                reservations.Add(res);
            }
            Bldg = space.Building;
            Room = space.RoomNum;
                
            foreach (string sem in semesters)
            {
                if (TSOverlay.Length > 5)
                {
                    TSOverlay = TSOverlay + "," + fns.FullCalendarTSOverlayShort(sem, space.Building, space.RoomNum);
                }
                else
                {
                    TSOverlay = TSOverlay + fns.FullCalendarTSOverlayShort(sem, space.Building, space.RoomNum);
                }
            }
            if (TSOverlay.Length < 5)
            {
                TSOverlay = "";
            }

            ViewBag.Reservations = fns.FullCalendarListShort(reservations, false);
            ViewBag.BuildingName = Bldg;
            ViewBag.TSOverlay = TSOverlay;
            ViewBag.SpaceInfo = space;

            return View();
        }


        // Pending room detail view (takes in ResId)
        [Authorize]
        public ActionResult ViewPending(int id)
        {
            BasicReservation res = spacesdb.BasicReservations.SingleOrDefault(p => p.ResId == id);
            return View(res);
        }

        // Reservation detail view (takes in ResId)
        [Authorize]
        public ActionResult ViewReservation(int id)
        {
            try
            {
                string permitted = fns.SpacesPermissions(User.Identity.Name);
                if (permitted == "none")
                {
                    ViewBag.Viewer = false;
                    ViewBag.Editor = false;
                    ViewBag.Approver = false;
                }
                else
                {
                    var perms = permitted.Split('/');
                    ViewBag.RmGroup = perms[0];
                    ViewBag.Viewer = Convert.ToBoolean(perms[1]);
                    ViewBag.Editor = Convert.ToBoolean(perms[2]);
                    ViewBag.Approver = Convert.ToBoolean(perms[3]);
                }
                BasicReservation res = spacesdb.BasicReservations.Single(p => p.ResId == id);
                return View(res);
            }
            catch (Exception)
            {
                BasicReservation res = new BasicReservation();
                return View(res);
            }
        }

        // Approve reservation
        [Authorize]
        public ActionResult ApproveReservation(int id)
        {
            BasicReservation reservation = spacesdb.BasicReservations.SingleOrDefault(p => p.ResId == id);
            if (fns.SpacesApprover(User.Identity.Name,reservation.RmCode))
            {
                try
                {
                    reservation.Status = "approved";
                    reservation.ApprovedBy = User.Identity.Name;
                    reservation.ApprovedOn = DateTime.Now;
                    spacesdb.SaveChanges();
                    bool notify = fns.Notify_Approval(reservation, User.Identity.Name);
                    if (notify)
                    {
                        return RedirectToAction("Admin", new { msg = "ApproveSuccess" });
                    }
                    else
                    {
                        return RedirectToAction("Admin", new { msg = "ApproveFail1" });
                    }
                }
                catch (Exception)
                {
                    return RedirectToAction("Admin", new { msg = "ApproveFail2" });
                }
            }
            else
            {
                return RedirectToAction("Error", new { msg = "AccessDenied" });
            }
        }

        // Approve Reservation Series - id is the RecurInit (first reservation in series)
        [Authorize]
        public ActionResult ApproveReservationSeries(int id)
        {
            try
            {
                List<BasicReservation> reservations = spacesdb.BasicReservations.Where(m => m.RecurInit == id).ToList();
                bool notify = false;
                foreach (BasicReservation res in reservations)
                {
                    res.Status = "approved";
                    res.ApprovedBy = User.Identity.Name;
                    res.ApprovedOn = DateTime.Now;
                    spacesdb.SaveChanges();
                    notify = fns.Notify_Approval(res, User.Identity.Name);
                }
                if (notify)
                {
                    return RedirectToAction("Admin", new { msg = "ApproveSuccess" });
                }
                else
                {
                    return RedirectToAction("Admin", new { msg = "ApproveFail1" });
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Admin", new { msg = "ApproveFail2" });
            }
        }

        // Cancel reservation
        [Authorize]
        public ActionResult CancelReservation(int id)
        {
            try
            {
                BasicReservation reservation = spacesdb.BasicReservations.SingleOrDefault(p => p.ResId == id);
                reservation.Status = "cancelled";
                reservation.ApprovedBy = User.Identity.Name;
                reservation.ApprovedOn = DateTime.Now;
                spacesdb.SaveChanges();
                bool notify = fns.Notify_Cancellation(reservation, User.Identity.Name, false);
                if (notify)
                {
                    return RedirectToAction("Admin", new { msg = "CancelSuccess" });
                }
                else
                {
                    return RedirectToAction("Admin", new { msg = "CancelFail" });
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Admin", new { msg = "CancelFail" });
            }
        }

        // Cancel reservation
        [Authorize]
        public ActionResult CancelReservationSeries(int id)
        {
            try
            {
                List<BasicReservation> reservations = spacesdb.BasicReservations.Where(m => m.RecurInit == id).ToList();
                bool notify = false;
                foreach (BasicReservation res in reservations)
                {
                    res.Status = "cancelled";
                    res.ApprovedBy = User.Identity.Name;
                    res.ApprovedOn = DateTime.Now;
                    spacesdb.SaveChanges();
                    notify = fns.Notify_Cancellation(res, User.Identity.Name, false);
                } 
                if (notify)
                {
                    return RedirectToAction("Admin", new { msg = "CancelSuccess" });
                }
                else
                {
                    return RedirectToAction("Admin", new { msg = "CancelFail" });
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Admin", new { msg = "CancelFail" });
            }
        }

        // Cancel reservation
        [Authorize]
        public ActionResult UserCancelReservation(int id)
        {
            try
            {
                BasicReservation reservation = spacesdb.BasicReservations.SingleOrDefault(p => p.ResId == id);
                reservation.Status = "cancelled";
                reservation.ApprovedBy = User.Identity.Name;
                reservation.ApprovedOn = DateTime.Now;
                spacesdb.SaveChanges();
                bool notify = fns.Notify_Cancellation(reservation, User.Identity.Name, true);
                if (notify)
                {
                    return RedirectToAction("Success", new { msg = "CancelSuccess" });
                }
                else
                {
                    return RedirectToAction("Success", new { msg = "CancelFail" });
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Success", new { msg = "CancelFail" });
            }
        }

        // Deny reservation (mostly same as CancelReservation)
        [Authorize]
        public ActionResult DenyReservation(int id)
        {
            try
            {
                BasicReservation reservation = spacesdb.BasicReservations.SingleOrDefault(p => p.ResId == id);
                reservation.Status = "denied";
                reservation.ApprovedBy = User.Identity.Name;
                reservation.ApprovedOn = DateTime.Now;
                spacesdb.SaveChanges();
                bool notify = fns.Notify_Denial(reservation, User.Identity.Name);
                if (notify)
                {
                    return RedirectToAction("Admin", new { msg = "DenySuccess" });
                }
                else
                {
                    return RedirectToAction("Admin", new { msg = "DenyFail" });
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Admin", new { msg = "DenyFail" });
            }
        }

        // Deny reservation series (mostly same as CancelReservationSeries)
        [Authorize]
        public ActionResult DenyReservationSeries(int id)
        {
            try
            {
                List<BasicReservation> reservations = spacesdb.BasicReservations.Where(m => m.RecurInit == id).ToList();
                bool notify = false;
                foreach (BasicReservation res in reservations)
                {
                    res.Status = "denied";
                    res.ApprovedBy = User.Identity.Name;
                    res.ApprovedOn = DateTime.Now;
                    spacesdb.SaveChanges();
                    notify = fns.Notify_Denial(res, User.Identity.Name);
                }
                if (notify)
                {
                    return RedirectToAction("Admin", new { msg = "DenySuccess" });
                }
                else
                {
                    return RedirectToAction("Admin", new { msg = "DenyFail" });
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Admin", new { msg = "DenyFail" });
            }
        }

    }
}
