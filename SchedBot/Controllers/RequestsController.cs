using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SchedBot;
using SchedbotDTOs;
using SchedBot.Models;
using SchedBot.Areas.Management.Models;
using Microsoft.AspNet.Identity;
using System.Text;
using SchedBot.Helpers;

namespace SchedBot.Controllers
{
    [Authorize]

    public class RequestsController : Controller
    {
        private SchedBotContext db = new SchedBotContext();

        
        // GET: Management/Requests
        public async Task<ActionResult> Index()
        {
           

            RequestViewModel vm = new RequestViewModel();
            vm.Requests = db.Requests.ToList().Select(x => new Models.Requests.RequestDetailsViewModel(x, db)).ToList();
            //vm.Requests.ForEach(x => x.SendingUser = db.Users.Find(x.SendingUserId) ?? null);

            return View(vm);
        }


        [HttpGet]
        public async Task<ActionResult> PopulateTradeRequest()
        {
            RequestViewModel vm = new RequestViewModel();
            vm.Users = db.Users.Select(x => new SelectListItem() { Text = x.FirstName + " " + x.LastName, Value = x.UserId.ToString() }).ToList();
            string acctId = User.Identity.GetUserId();

            User loggedIn = db.Users.FirstOrDefault(x => x.AccountId == acctId);
            if (loggedIn != null)
            {
                List<User_Shift_Schedule> uss = db.UserShiftSchedules.Where(x => x.UserId == loggedIn.UserId && (x.Schedule.Flag == Schedule.Flags.Current || x.Schedule.Flag == Schedule.Flags.Final)).ToList();
                vm.UserShifts = uss.Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Shift.Day.ToString() + "(" + Helper.TimeSpanToString(x.Shift.Start) + "-" + Helper.TimeSpanToString(x.Shift.End) + ")" }).ToList();
            }
            return PartialView("tradePartial", vm);
        }



        [HttpPost]
        public async Task<ActionResult> PopulateTradeRequestSelectedUserShifts(int userId)
        {
            RequestViewModel vm = new RequestViewModel();
            vm.Users = db.Users.Select(x => new SelectListItem() { Text = x.FirstName + " " + x.LastName, Value = x.UserId.ToString() }).ToList();
            string acctId = User.Identity.GetUserId();

            User loggedIn = db.Users.FirstOrDefault(x => x.AccountId == acctId);
            if (loggedIn != null)
            {
                List<User_Shift_Schedule> uss = db.UserShiftSchedules.Where(x => x.UserId == loggedIn.UserId && (x.Schedule.Flag == Schedule.Flags.Current || x.Schedule.Flag == Schedule.Flags.Final)).ToList();
                vm.UserShifts = uss.Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Shift.Day.ToString() + "(" + Helper.TimeSpanToString(x.Shift.Start) + "-" + Helper.TimeSpanToString(x.Shift.End) + ")" }).ToList();
            }

            if (userId != 0)
            {
                List<User_Shift_Schedule> otherUss = db.UserShiftSchedules.Where(x => x.UserId == userId && (x.Schedule.Flag == Schedule.Flags.Current || x.Schedule.Flag == Schedule.Flags.Final)).ToList();
                vm.OtherUserShifts = otherUss.Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Shift.Day.ToString() + "(" + Helper.TimeSpanToString(x.Shift.Start) + "-" + Helper.TimeSpanToString(x.Shift.End) + ")" }).ToList();
            }
            return PartialView("TradableShiftsPartial", vm);
        }

        [HttpPost]
        public async Task<ActionResult> ApproveRequest(int requestId)
        {
            Request req = db.Requests.FirstOrDefault(x => x.RequestId == requestId);
            req.Status = "Employee Approved";
            if(User.IsInRole("Manager"))
                req.Status = "Manager Approved";
            if(req.RequestType == "Trade")
            {
                int orignalId = req.OriginalUSSId;
                int tradingId = req.TradingUSSId;
                User_Shift_Schedule original = db.UserShiftSchedules.FirstOrDefault(x => x.Id == orignalId);
                User_Shift_Schedule trading = db.UserShiftSchedules.FirstOrDefault(x => x.Id == tradingId);
                original.UserId = tradingId;
                trading.UserId = orignalId;
                db.Entry(original).State = EntityState.Modified;
                db.Entry(trading).State = EntityState.Modified;

            }
            else
            {
                //todo:update schedule here
            }
            await  db.SaveChangesAsync();
          return  RedirectToAction("Index");
        }

        [HttpPost]

        public async Task<ActionResult> DenyRequest(int requestId, string reason)
        {
            Request req = db.Requests.FirstOrDefault(x => x.RequestId == requestId);
            req.Status = "Employee Denied";

            if (User.IsInRole("Manager"))
                req.Status = "Manager Denied";
            req.Reason = reason;
            db.Entry(req).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<ActionResult> CreateRequest(FormCollection collection)
        {
            SchedbotDTOs.Request request = new SchedbotDTOs.Request();
            string acntId = User.Identity.GetUserId();
            request.SendingUserId = db.Users.First(x => x.AccountId == acntId).UserId;
            request.Status = "Pending";
            string requestType = collection.Get("requestType");
            request.RequestType = requestType;
            request.Reason = collection.GetValue("requestReason").AttemptedValue.Trim();
            request.StartTimeOff = null;
            request.EndTimeOff = null;
            if (requestType == "trade")
            {

                request.RequestType = "Trade";
                request.ReceivingUserId = int.Parse(collection.Get("Users"));
                request.OriginalUSSId = int.Parse(collection.Get("UserShifts"));
                request.TradingUSSId = int.Parse(collection.Get("OtherUserShifts"));
            }
            else if (requestType == "timeOff")
            {
                if (collection.AllKeys.Contains("daterange"))
                {
                    //request.SendingUserId = db.Users.First(x => x.AccountId == User.Identity)
                    request.RequestType = "Time Off";

                    string[] dateRange = collection.GetValue("daterange").AttemptedValue.Split('-');
                    request.StartTimeOff = DateTime.Parse(dateRange[0].Trim());
                    request.EndTimeOff = DateTime.Parse(dateRange[1].Trim());
                }


            }
            db.Requests.Add(request);
            db.SaveChanges();
            return RedirectToAction("Index");
        }



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
