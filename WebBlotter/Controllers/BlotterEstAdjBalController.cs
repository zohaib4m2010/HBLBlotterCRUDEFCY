using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using WebBlotter.Classes;
using WebBlotter.Models;
using WebBlotter.Repository;

namespace WebBlotter.Controllers
{
    [AuthAccess]
    public class BlotterEstAdjBalController : Controller
    {

        UtilityClass UC = new UtilityClass();
        // GET: BlotterEstAdjBal
        public ActionResult EstimatedAdjustedBalance(FormCollection form)
        {
            try
            {
                #region Added by shakir (Currency parameter)
                var selectCurrency = (dynamic)null;
                if (form["selectCurrency"] != null)
                    selectCurrency = Convert.ToInt32(form["selectCurrency"].ToString());
                else
                    selectCurrency = Convert.ToInt32(Session["SelectedCurrency"].ToString());

                UtilityClass.GetSelectedCurrecy(selectCurrency);
                #endregion

                ServiceRepository serviceObj = new ServiceRepository();
                HttpResponseMessage response = serviceObj.GetResponse("/api/BlotterManualDeals/GetAllBlotterEstAdjBal?UserID=" + Session["UserID"].ToString() + "&BranchID=" + Session["BranchID"].ToString() + "&CurID=" + Session["SelectedCurrency"].ToString() + "&BR=" + Session["BR"].ToString());
                response.EnsureSuccessStatusCode();
                List<Models.SBP_BlotterManualEstBalance> BlotterEstAdjBal = response.Content.ReadAsAsync<List<Models.SBP_BlotterManualEstBalance>>().Result;
                var PAccess = Session["CurrentPagesAccess"].ToString().Split('~');
                UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(BlotterEstAdjBal), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
                ViewData["BR"] = Session["BR"].ToString();
                ViewData["isDateChangable"] = Convert.ToBoolean(PAccess[2]);
                ViewData["isEditable"] = Convert.ToBoolean(PAccess[3]);
                ViewData["IsDeletable"] = Convert.ToBoolean(PAccess[4]);
                ViewBag.Title = "All Blotter Setup";
                return PartialView("_EstimatedAdjustedBalance", BlotterEstAdjBal);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        [HttpGet]
        public ActionResult Create()
        {

            var ActiveAction = RouteData.Values["action"].ToString();
            var ActiveController = RouteData.Values["controller"].ToString();
            Session["ActiveAction"] = ActiveController;
            Session["ActiveController"] = ActiveAction;

            UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), "", this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
            SBP_BlotterManualEstBalance model = new SBP_BlotterManualEstBalance();
            ViewData["BR"] = Session["BR"].ToString();
            return PartialView("_Create", model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _Create(SBP_BlotterManualEstBalance BlotterEstAdjBal, FormCollection form)
        {
            try
            {
                #region Added by shakir (Currency parameter)
                var selectCurrency = (dynamic)null;
                if (form["selectCurrency"] != null)
                    selectCurrency = Convert.ToInt32(form["selectCurrency"].ToString());
                else
                    selectCurrency = Convert.ToInt32(Session["SelectedCurrency"].ToString());

                UtilityClass.GetSelectedCurrecy(selectCurrency);
                #endregion

                if (ModelState.IsValid)
                {
                    if (Session["BR"].ToString() == "01")
                    {
                        BlotterEstAdjBal.DataType = "SBP";
                    }
                    BlotterEstAdjBal.AdjDate = BlotterEstAdjBal.AdjDate;
                    BlotterEstAdjBal.isAdjusted = true;
                    BlotterEstAdjBal.UserID = Convert.ToInt16(Session["UserID"].ToString());
                    BlotterEstAdjBal.BID = Convert.ToInt16(Session["BranchID"].ToString());
                    BlotterEstAdjBal.BR = Convert.ToInt16(Session["BR"].ToString());
                    BlotterEstAdjBal.CurID = Convert.ToInt16(Session["SelectedCurrency"].ToString());
                    BlotterEstAdjBal.CreateDate = DateTime.Now;
                    ServiceRepository serviceObj = new ServiceRepository();
                    HttpResponseMessage response = serviceObj.PostResponse("api/BlotterManualDeals/InsertEstAdjBal", BlotterEstAdjBal);
                    response.EnsureSuccessStatusCode();
                    UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(BlotterEstAdjBal), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
                    return RedirectToAction("EstimatedAdjustedBalance");
                }
            }
            catch (Exception ex) { }
            return PartialView("_Create", BlotterEstAdjBal);

        }



        public ActionResult Edit(int id, FormCollection form)
        {
            #region Added by shakir (Currency parameter)
            var selectCurrency = (dynamic)null;
            if (form["selectCurrency"] != null)
                selectCurrency = Convert.ToInt32(form["selectCurrency"].ToString());
            else
                selectCurrency = Convert.ToInt32(Session["SelectedCurrency"].ToString());

            UtilityClass.GetSelectedCurrecy(selectCurrency);
            #endregion

            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.GetResponse("/api/BlotterManualDeals/GetBlotterEstAdjBalById?id=" + id.ToString());
            response.EnsureSuccessStatusCode();
            Models.SBP_BlotterManualEstBalance BlotterEstAdjBal = response.Content.ReadAsAsync<Models.SBP_BlotterManualEstBalance>().Result;
            UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(BlotterEstAdjBal), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
            var isDateChangable = Convert.ToBoolean(Session["CurrentPagesAccess"].ToString().Split('~')[2]);
            ViewData["isDateChangable"] = isDateChangable;
            ViewData["BR"] = Session["BR"].ToString();
            return PartialView("_Edit", BlotterEstAdjBal);

        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(Models.SBP_BlotterManualEstBalance BlotterEstAdjBal)
        {
            if (Session["BR"].ToString() == "01")
            {
                BlotterEstAdjBal.DataType = "SBP";
            }
            BlotterEstAdjBal.UpdateDate = DateTime.Now;
            if (BlotterEstAdjBal.AdjDate == null)
                BlotterEstAdjBal.AdjDate = DateTime.Now;
            BlotterEstAdjBal.isAdjusted = true;
            BlotterEstAdjBal.CurID = Convert.ToInt16(Session["SelectedCurrency"].ToString());
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.PutResponse("api/BlotterManualDeals/UpdateEstAdjBal", BlotterEstAdjBal);
            response.EnsureSuccessStatusCode();
            UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(BlotterEstAdjBal), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
            return RedirectToAction("EstimatedAdjustedBalance");
        }

        public ActionResult Delete(int id)
        {
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.DeleteResponse("api/BlotterManualDeals/DeleteEstAdjBal?id=" + id.ToString());
            response.EnsureSuccessStatusCode();
            UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(id), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
            return RedirectToAction("EstimatedAdjustedBalance");
        }


        public ActionResult Reset(int id)
        {
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.DeleteResponse("api/BlotterManualDeals/ResetEstAdjBal?id=" + id.ToString());
            response.EnsureSuccessStatusCode();
            UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(id), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
            return RedirectToAction("EstimatedAdjustedBalance");
        }
    }
}