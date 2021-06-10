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
    public class BlotterTradeController : Controller
    {

        UtilityClass UC = new UtilityClass();
        // GET: BlotterTrade
        private List<Models.SP_GETAllTransactionTitles_Result> GetAllTradeTransactionTitles()
        {
            try
            {
                ServiceRepository serviceObj = new ServiceRepository();
                HttpResponseMessage response = serviceObj.GetResponse("/api/BlotterTrade/GetAllTradeTransactionTitles");
                response.EnsureSuccessStatusCode();
                List<Models.SP_GETAllTransactionTitles_Result> blotterTTT = response.Content.ReadAsAsync<List<Models.SP_GETAllTransactionTitles_Result>>().Result;

                return blotterTTT;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult BlotterTrade(FormCollection form)
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
                HttpResponseMessage response = serviceObj.GetResponse("/api/BlotterTrade/GetAllBlotterTrade?UserID=" + Session["UserID"].ToString() + "&BranchID=" + Session["BranchID"].ToString() + "&CurID=" + Session["SelectedCurrency"].ToString() + "&BR=" + Session["BR"].ToString());
                response.EnsureSuccessStatusCode();
                List<Models.SP_GetAll_SBPBlotterTrade_Result> blotterTrade = response.Content.ReadAsAsync<List<Models.SP_GetAll_SBPBlotterTrade_Result>>().Result;
                var PAccess = Session["CurrentPagesAccess"].ToString().Split('~');
                UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(blotterTrade), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());

                ViewData["isDateChangable"] = Convert.ToBoolean(PAccess[2]);
                ViewData["isEditable"] = Convert.ToBoolean(PAccess[3]);
                ViewData["IsDeletable"] = Convert.ToBoolean(PAccess[4]);
                ViewBag.Title = "All Blotter Setup";
                return PartialView("_BlotterTrade", blotterTrade);
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
            SBP_BlotterTrade model = new SBP_BlotterTrade();
            try
            {
                if (ModelState.IsValid)
                {
                    model.Trade_Date = DateTime.Now.Date;
                    //ViewData["SysCurrentDt"] = GetCurrentDT().ToString("dd-MMM-yyyy");
                    // ViewData["BrCode"] = BrCode;
                    ViewBag.TradeTransactionTitles = GetAllTradeTransactionTitles();
                }
            }
            catch (Exception ex) { }
            return PartialView("_Create", model);

        }
        public ActionResult Create(FormCollection form)
        {
            #region Added by shakir (Currency parameter)

            var selectCurrency = (dynamic)null;
            if (form["selectCurrency"] != null)
                selectCurrency = Convert.ToInt32(form["selectCurrency"].ToString());
            else
                selectCurrency = Convert.ToInt32(Session["SelectedCurrency"].ToString());
            UtilityClass.GetSelectedCurrecy(selectCurrency);

            #endregion

            SBP_BlotterTrade model = new SBP_BlotterTrade();
            try
            {
                UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), "", this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
                if (ModelState.IsValid)
                {
                    model.Trade_Date = DateTime.Now.Date;
                    ViewBag.TradeTransactionTitles = GetAllTradeTransactionTitles();
                }
            }
            catch (Exception ex) { }

            return PartialView("_Create", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _Create(SBP_BlotterTrade BlotterTrade, FormCollection form)
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
                    BlotterTrade.Trade_OutFLow = UC.CheckNegativeValue(BlotterTrade.Trade_OutFLow);
                    BlotterTrade.UserID = Convert.ToInt16(Session["UserID"].ToString());
                    BlotterTrade.BID = Convert.ToInt16(Session["BranchID"].ToString());
                    BlotterTrade.BR = Convert.ToInt16(Session["BR"].ToString());
                    BlotterTrade.CurID = Convert.ToInt16(Session["SelectedCurrency"].ToString());
                    BlotterTrade.CreateDate = DateTime.Now;
                    ServiceRepository serviceObj = new ServiceRepository();
                    HttpResponseMessage response = serviceObj.PostResponse("api/BlotterTrade/InsertTrade", BlotterTrade);
                    response.EnsureSuccessStatusCode();
                    UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(BlotterTrade), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
                    return RedirectToAction("BlotterTrade");
                }
                else
                {

                    ViewBag.TradeTransactionTitles = GetAllTradeTransactionTitles();
                }
            }
            catch (Exception ex) { }
            return PartialView("_Create", BlotterTrade);
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
            HttpResponseMessage response = serviceObj.GetResponse("/api/BlotterTrade/GetBlotterTrade?id=" + id.ToString());
            response.EnsureSuccessStatusCode();
            Models.SBP_BlotterTrade BlotterTrade = response.Content.ReadAsAsync<Models.SBP_BlotterTrade>().Result;
            UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(BlotterTrade), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
            var isDateChangable = Convert.ToBoolean(Session["CurrentPagesAccess"].ToString().Split('~')[2]);
            ViewData["isDateChangable"] = isDateChangable;
            ViewBag.TradeTransactionTitles = GetAllTradeTransactionTitles();
            return PartialView("_Edit", BlotterTrade);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(Models.SBP_BlotterTrade BlotterTrade)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    BlotterTrade.Trade_OutFLow = UC.CheckNegativeValue(BlotterTrade.Trade_OutFLow);
                    if (BlotterTrade.Trade_Date == null)
                        BlotterTrade.Trade_Date = DateTime.Now;
                    BlotterTrade.CurID = Convert.ToInt16(Session["SelectedCurrency"].ToString());
                    BlotterTrade.UpdateDate = DateTime.Now;
                    UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(BlotterTrade), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
                    ServiceRepository serviceObj = new ServiceRepository();
                    HttpResponseMessage response = serviceObj.PutResponse("api/BlotterTrade/UpdateTrade", BlotterTrade);
                    response.EnsureSuccessStatusCode();
                    //ViewData["SysCurrentDt"] = GetCurrentDT().ToString("dd-MMM-yyyy");
                    //ViewData["BrCode"] = BrCode;
                    return RedirectToAction("BlotterTrade");
                }
                else
                {
                    ViewBag.TradeTransactionTitles = GetAllTradeTransactionTitles();
                }
            }
            catch (Exception ex) { }
            return PartialView("_Edit", BlotterTrade);
        }

        public ActionResult Delete(int id)
        {
            try
            {
                UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(id), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
                ServiceRepository serviceObj = new ServiceRepository();
                HttpResponseMessage response = serviceObj.DeleteResponse("api/BlotterTrade/DeleteTrade?id=" + id.ToString());
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex) { }

            return RedirectToAction("BlotterTrade");
        }
    }
}