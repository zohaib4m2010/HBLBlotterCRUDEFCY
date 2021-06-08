using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using WebBlotter.Models;
using WebBlotter.Repository;

namespace WebBlotter.Controllers
{
    public class BlotterOpenBalController : Controller
    {
        UtilityClass UC = new UtilityClass();
        // GET: BlotterOpenBal

        public ActionResult OpeningBalance()
        {
            try
            {
                ServiceRepository serviceObj = new ServiceRepository();
                HttpResponseMessage response = serviceObj.GetResponse("/api/BlotterOpenBal/GetAllBlotterOpenBal?UserID=" + Session["UserID"].ToString() + "&BranchID=" + Session["BranchID"].ToString() + "&CurID=" + Session["SelectedCurrency"].ToString() + "&BR=" + Session["BR"].ToString());
                response.EnsureSuccessStatusCode();
                List<Models.SBP_BlotterOpeningBalance> BlotterOpenBal = response.Content.ReadAsAsync<List<Models.SBP_BlotterOpeningBalance>>().Result;
                var PAccess = Session["CurrentPagesAccess"].ToString().Split('~');
                UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(BlotterOpenBal), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());

                ViewData["isDateChangable"] = Convert.ToBoolean(PAccess[2]);
                ViewData["isEditable"] = Convert.ToBoolean(PAccess[3]);
                ViewData["IsDeletable"] = Convert.ToBoolean(PAccess[4]);
                ViewBag.Title = "All Blotter Setup";
                return View(BlotterOpenBal);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        [HttpGet]
        public ActionResult Create()
        {
            UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), "", this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
            SBP_BlotterOpeningBalance model = new SBP_BlotterOpeningBalance();
            
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SBP_BlotterOpeningBalance BlotterOpenBal)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    BlotterOpenBal.BalDate = BlotterOpenBal.BalDate;
                    BlotterOpenBal.UserID = Convert.ToInt16(Session["UserID"].ToString());
                    BlotterOpenBal.BID = Convert.ToInt16(Session["BranchID"].ToString());
                    BlotterOpenBal.BR = Convert.ToInt16(Session["BR"].ToString());
                    BlotterOpenBal.CurID = Convert.ToInt16(Session["SelectedCurrency"].ToString());
                    BlotterOpenBal.CreateDate = DateTime.Now;
                    ServiceRepository serviceObj = new ServiceRepository();
                    HttpResponseMessage response = serviceObj.PostResponse("api/BlotterOpenBal/InsertOpenBal", BlotterOpenBal);
                    response.EnsureSuccessStatusCode();
                    UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(BlotterOpenBal), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
                    return RedirectToAction("OpeningBalance");
                }
            }
            catch (Exception ex) { }
            return View(BlotterOpenBal);

        }

        public ActionResult Edit(int id)
        {
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.GetResponse("/api/BlotterOpenBal/GetBlotterOpenBalById?id=" + id.ToString());
            response.EnsureSuccessStatusCode();
            Models.SBP_BlotterOpeningBalance BlotterOpenBal = response.Content.ReadAsAsync<Models.SBP_BlotterOpeningBalance>().Result;
            UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(BlotterOpenBal), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
            var isDateChangable = Convert.ToBoolean(Session["CurrentPagesAccess"].ToString().Split('~')[2]);
            ViewData["isDateChangable"] = isDateChangable;
            return View(BlotterOpenBal);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(Models.SBP_BlotterOpeningBalance BlotterOpenBal)
        {
            BlotterOpenBal.UpdateDate = DateTime.Now;
            if (BlotterOpenBal.BalDate == null)
                BlotterOpenBal.BalDate = DateTime.Now;
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.PutResponse("api/BlotterOpenBal/UpdateOpenBal", BlotterOpenBal);
            response.EnsureSuccessStatusCode();
            UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(BlotterOpenBal), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
            return RedirectToAction("OpeningBalance");
        }

        public ActionResult Delete(int id)
        {
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.DeleteResponse("api/BlotterOpenBal/DeleteOpenBal?id=" + id.ToString());
            response.EnsureSuccessStatusCode();
            UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(id), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
            return RedirectToAction("OpeningBalance");
        }
    }
}