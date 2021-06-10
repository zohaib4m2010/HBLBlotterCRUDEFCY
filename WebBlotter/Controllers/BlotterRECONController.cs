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
    public class BlotterRECONController : Controller
    { 

        private List<Models.NostroBank> GetAllNostroBanks()
        {
            try
            {
                var currid = (dynamic)null;
                if (Session["SelectedCurrency"] != null)
                {
                    currid = Convert.ToInt32(Session["SelectedCurrency"].ToString());
                }
                ServiceRepository serviceObj = new ServiceRepository();
                HttpResponseMessage response = serviceObj.GetResponse("/api/NostroBank/GetAllNostroBank?currId=" + currid);
                response.EnsureSuccessStatusCode();
                List<Models.NostroBank> blotterNB = response.Content.ReadAsAsync<List<Models.NostroBank>>().Result;

                return blotterNB;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult BlotterRECON(FormCollection form)
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
            HttpResponseMessage response = serviceObj.GetResponse("/api/BlotterRECON/GetAllBlotterRECON?UserID=" + Session["UserID"].ToString() + "&BranchID=" + Session["BranchID"].ToString() + "&CurID=" + selectCurrency + "&BR=" + Session["BR"].ToString());
            response.EnsureSuccessStatusCode();
            List<Models.SP_GetSBPBlotterRECON_Result> BlotterRECON = response.Content.ReadAsAsync<List<Models.SP_GetSBPBlotterRECON_Result>>().Result;
            if (BlotterRECON.Count < 1)
                ViewData["DataStatus"] = "Data Not Availavle";
            ViewBag.Title = "All Blotter Setup";
            var PAccess = Session["CurrentPagesAccess"].ToString().Split('~');
            UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(BlotterRECON), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());

            ViewData["isDateChangable"] = Convert.ToBoolean(PAccess[2]);
            ViewData["isEditable"] = Convert.ToBoolean(PAccess[3]);
            ViewData["IsDeletable"] = Convert.ToBoolean(PAccess[4]);
            return PartialView("_BlotterRECON", BlotterRECON);
        }

        [HttpGet]
        public ActionResult Create()
        {
            SBP_BlotterRECON model = new SBP_BlotterRECON();
            try
            {
                UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), "", this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());

                var ActiveAction = RouteData.Values["action"].ToString();
                var ActiveController = RouteData.Values["controller"].ToString();
                Session["ActiveAction"] = ActiveController;
                Session["ActiveController"] = ActiveAction;

                if (ModelState.IsValid)
                {
                    // model.CreateDate = DateTime.Now.Date;
                    ViewBag.RECONNostroBanks = GetAllNostroBanks();
                }
            }
            catch (Exception ex)
            {

            }
            return PartialView("_Create", model);

        }


        public ActionResult Create(FormCollection form)
        {
            try
            {
                UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), "", this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());

                #region Added by shakir (Currency parameter)

                var selectCurrency = (dynamic)null;
                if (form["selectCurrency"] != null)
                    selectCurrency = Convert.ToInt32(form["selectCurrency"].ToString());
                else
                    selectCurrency = Convert.ToInt32(Session["SelectedCurrency"].ToString());
                UtilityClass.GetSelectedCurrecy(selectCurrency);

                #endregion

                ViewBag.RECONNostroBanks = GetAllNostroBanks();
                return PartialView("_Create");
            }
            catch (Exception ex) { }

            return PartialView("_Create");
        }

        // POST: BlotterRECON/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _Create(SBP_BlotterRECON BlotterRECON,FormCollection form)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    BlotterRECON.UserID = Convert.ToInt16(Session["UserID"].ToString());
                    BlotterRECON.BID = Convert.ToInt16(Session["BranchID"].ToString());
                    BlotterRECON.BR = Convert.ToInt16(Session["BR"].ToString());
                    BlotterRECON.CurID = Convert.ToInt16(Session["SelectedCurrency"].ToString());
                    BlotterRECON.CreateDate = DateTime.Now;
                    BlotterRECON.NostroBankId = Convert.ToInt32(form["NostroBankId"].ToString());
                    ServiceRepository serviceObj = new ServiceRepository();
                    HttpResponseMessage response = serviceObj.PostResponse("api/BlotterRECON/InsertRECON", BlotterRECON);
                    response.EnsureSuccessStatusCode();
                    UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(BlotterRECON), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
                    return RedirectToAction("BlotterRECON");
                }
                else
                {
                    ViewBag.RECONNostroBanks = GetAllNostroBanks();
                }
            }
            catch (Exception ex) { }

            return RedirectToAction("BlotterRECON", BlotterRECON);
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
            HttpResponseMessage response = serviceObj.GetResponse("/api/BlotterRECON/GetBlotterRECON?id=" + id.ToString());
            response.EnsureSuccessStatusCode();
            Models.SBP_BlotterRECON BlotterRECON = response.Content.ReadAsAsync<Models.SBP_BlotterRECON>().Result;
            UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(BlotterRECON), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
            ViewBag.nostobankid = BlotterRECON.NostroBankId;
            ViewBag.RECONNostroBanks = GetAllNostroBanks();
            return PartialView("_Edit", BlotterRECON);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(Models.SBP_BlotterRECON BlotterRECON, FormCollection form)
        {
            BlotterRECON.UserID = Convert.ToInt16(Session["UserID"].ToString());
            BlotterRECON.BID = Convert.ToInt16(Session["BranchID"].ToString());
            BlotterRECON.BR = Convert.ToInt16(Session["BR"].ToString());
            BlotterRECON.CurID = Convert.ToInt16(Session["SelectedCurrency"].ToString());
            BlotterRECON.UpdateDate = DateTime.Now;
            BlotterRECON.NostroBankId = Convert.ToInt32(form["NostroBankId"].ToString());
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.PutResponse("api/BlotterRECON/UpdateRECON", BlotterRECON);
            response.EnsureSuccessStatusCode();
            UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(BlotterRECON), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
            return RedirectToAction("BlotterRECON");
        }

        public ActionResult Delete(int id)
        {
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.DeleteResponse("api/BlotterRECON/DeleteRECON?id=" + id.ToString());
            response.EnsureSuccessStatusCode();
            UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(id), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
            return RedirectToAction("BlotterRECON");
        }
    }
}