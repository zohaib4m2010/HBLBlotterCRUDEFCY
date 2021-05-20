using WebBlotter.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using WebBlotter.Models;
using WebBlotter.Classes;
using Newtonsoft.Json;

namespace WebBlotter.Controllers
{
    [AuthAccess]
    public class BlotterTBOController : Controller
    {
        UtilityClass UC = new UtilityClass();
        // GET: BlotterTBO

        private List<Models.SP_GETAllTransactionTitles_Result> GetAllTBOTransactionTitles()
        {
            try
            {
                ServiceRepository serviceObj = new ServiceRepository();
                HttpResponseMessage response = serviceObj.GetResponse("/api/BlotterTBO/GetAllTBOTransactionTitles");
                response.EnsureSuccessStatusCode();
                List<Models.SP_GETAllTransactionTitles_Result> blotterTTT = response.Content.ReadAsAsync<List<Models.SP_GETAllTransactionTitles_Result>>().Result;
                UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(blotterTTT), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());

                return blotterTTT;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult BlotterTBO()
        {
            try
            {
                ServiceRepository serviceObj = new ServiceRepository();
                HttpResponseMessage response = serviceObj.GetResponse("/api/BlotterTBO/GetAllBlotterTBO?UserID=" + Session["UserID"].ToString() + "&BranchID=" + Session["BranchID"].ToString() + "&CurID=" + Session["SelectedCurrency"].ToString() + "&BR=" + Session["BR"].ToString());
                response.EnsureSuccessStatusCode();
                List<Models.SP_GetAll_SBPBlotterTBO_Result> blotterTBO = response.Content.ReadAsAsync<List<Models.SP_GetAll_SBPBlotterTBO_Result>>().Result;
                var PAccess = Session["CurrentPagesAccess"].ToString().Split('~');

                ViewData["isDateChangable"] = Convert.ToBoolean(PAccess[2]);
                ViewData["isEditable"] = Convert.ToBoolean(PAccess[3]);
                ViewData["IsDeletable"] = Convert.ToBoolean(PAccess[4]);
                ViewBag.Title = "All Blotter Setup";
                return View(blotterTBO);
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
            SBP_BlotterTBO model = new SBP_BlotterTBO();
            try
            {
                if (ModelState.IsValid)
                {
                    model.TBO_Date = DateTime.Now.Date;
                    //ViewData["SysCurrentDt"] = GetCurrentDT().ToString("dd-MMM-yyyy");
                    // ViewData["BrCode"] = BrCode;
                    ViewBag.TBOTransactionTitles = GetAllTBOTransactionTitles();
                }
            }
            catch (Exception ex) { }
            return View(model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SBP_BlotterTBO BlotterTBO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    BlotterTBO.TBO_OutFLow = UC.CheckNegativeValue(BlotterTBO.TBO_OutFLow);
                    BlotterTBO.UserID = Convert.ToInt16(Session["UserID"].ToString());
                    BlotterTBO.BR = Convert.ToInt16(Session["BR"].ToString());
                    BlotterTBO.BID = Convert.ToInt16(Session["BranchID"].ToString());
                    BlotterTBO.CurID = Convert.ToInt16(Session["SelectedCurrency"].ToString());
                    BlotterTBO.CreateDate = DateTime.Now;
                    ServiceRepository serviceObj = new ServiceRepository();
                    HttpResponseMessage response = serviceObj.PostResponse("api/BlotterTBO/InsertTBO", BlotterTBO);
                    response.EnsureSuccessStatusCode();
                    //ViewData["SysCurrentDt"] = GetCurrentDT().ToString("dd-MMM-yyyy");
                    // ViewData["BrCode"] = BrCode;
                    UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(BlotterTBO), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
                    return RedirectToAction("BlotterTBO");
                }
                else {

                    ViewBag.TBOTransactionTitles = GetAllTBOTransactionTitles();
                }
            }
            catch (Exception ex) { }
            return View(BlotterTBO);
        }

        public ActionResult Edit(int id)
        {
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.GetResponse("/api/BlotterTBO/GetBlotterTBO?id=" + id.ToString());
            response.EnsureSuccessStatusCode();
            Models.SBP_BlotterTBO BlotterTBO = response.Content.ReadAsAsync<Models.SBP_BlotterTBO>().Result;
            var isDateChangable = Convert.ToBoolean(Session["CurrentPagesAccess"].ToString().Split('~')[2]);
            ViewData["isDateChangable"] = isDateChangable;
            ViewBag.TBOTransactionTitles = GetAllTBOTransactionTitles();
            UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(BlotterTBO), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
            return View(BlotterTBO);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(Models.SBP_BlotterTBO BlotterTBO)
        {
            BlotterTBO.TBO_OutFLow = UC.CheckNegativeValue(BlotterTBO.TBO_OutFLow);
            if (BlotterTBO.TBO_Date == null)
                BlotterTBO.TBO_Date = DateTime.Now;
            BlotterTBO.UpdateDate = DateTime.Now;
            UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(BlotterTBO), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.PutResponse("api/BlotterTBO/UpdateTBO", BlotterTBO);
            response.EnsureSuccessStatusCode();
            return RedirectToAction("BlotterTBO");
        }

        public ActionResult Delete(int id)
        {
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.DeleteResponse("api/BlotterTBO/DeleteTBO?id=" + id.ToString());
            response.EnsureSuccessStatusCode();
            UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(id), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
            return RedirectToAction("BlotterTBO");
        }
    }
}