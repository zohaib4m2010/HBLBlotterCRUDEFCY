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

        public ActionResult BlotterTrade()
        {
            try
            {
                ServiceRepository serviceObj = new ServiceRepository();
                HttpResponseMessage response = serviceObj.GetResponse("/api/BlotterTrade/GetAllBlotterTrade?UserID="+ Session["UserID"].ToString()+"&BranchID=" + Session["BranchID"].ToString() + "&CurID=" + Session["SelectedCurrency"].ToString() + "&BR=" + Session["BR"].ToString());
                response.EnsureSuccessStatusCode();
                List<Models.SP_GetAll_SBPBlotterTrade_Result> blotterTrade = response.Content.ReadAsAsync<List<Models.SP_GetAll_SBPBlotterTrade_Result>>().Result;
                var PAccess = Session["CurrentPagesAccess"].ToString().Split('~');

                ViewData["isDateChangable"] = Convert.ToBoolean(PAccess[2]);
                ViewData["isEditable"] = Convert.ToBoolean(PAccess[3]);
                ViewData["IsDeletable"] = Convert.ToBoolean(PAccess[4]);
                ViewBag.Title = "All Blotter Setup";
                return View(blotterTrade);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        [HttpGet]
        public ActionResult Create()
        {
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
            return View(model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SBP_BlotterTrade BlotterTrade)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    BlotterTrade.UserID = Convert.ToInt16(Session["UserID"].ToString());
                    BlotterTrade.BID = Convert.ToInt16(Session["BranchID"].ToString());
                    BlotterTrade.BR = Convert.ToInt16(Session["BR"].ToString());
                    BlotterTrade.CurID = Convert.ToInt16(Session["SelectedCurrency"].ToString());
                    BlotterTrade.CreateDate = DateTime.Now;
                    ServiceRepository serviceObj = new ServiceRepository();
                    HttpResponseMessage response = serviceObj.PostResponse("api/BlotterTrade/InsertTrade", BlotterTrade);
                    response.EnsureSuccessStatusCode();
                    return RedirectToAction("BlotterTrade");
                }
                else
                {

                    ViewBag.TradeTransactionTitles = GetAllTradeTransactionTitles();
                }
            }
            catch (Exception ex) { }
            return View(BlotterTrade);
        }

        public ActionResult Edit(int id)
        { 
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.GetResponse("/api/BlotterTrade/GetBlotterTrade?id=" + id.ToString());
            response.EnsureSuccessStatusCode();
            Models.SBP_BlotterTrade BlotterTrade = response.Content.ReadAsAsync<Models.SBP_BlotterTrade>().Result;
            var isDateChangable = Convert.ToBoolean(Session["CurrentPagesAccess"].ToString().Split('~')[2]);
            ViewData["isDateChangable"] = isDateChangable;
            ViewBag.TradeTransactionTitles = GetAllTradeTransactionTitles();
            return View(BlotterTrade);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(Models.SBP_BlotterTrade BlotterTrade)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (BlotterTrade.Trade_Date == null)
                        BlotterTrade.Trade_Date = DateTime.Now;
                    BlotterTrade.UpdateDate = DateTime.Now;
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
            return View(BlotterTrade);
        }

        public ActionResult Delete(int id)
        {
            try
            {
                ServiceRepository serviceObj = new ServiceRepository();
                HttpResponseMessage response = serviceObj.DeleteResponse("api/BlotterTrade/DeleteTrade?id=" + id.ToString());
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex) { }

            return RedirectToAction("BlotterTrade");
        }
    }
}