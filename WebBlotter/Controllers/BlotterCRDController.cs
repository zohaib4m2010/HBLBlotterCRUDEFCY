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
    public class BlotterCRDController : Controller
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
        
        public ActionResult BlotterCRD(FormCollection form)
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
            HttpResponseMessage response = serviceObj.GetResponse("/api/BlotterCRD/GetAllBlotterCRD?UserID=" + Session["UserID"].ToString() + "&BranchID=" + Session["BranchID"].ToString() + "&CurID=" + selectCurrency + "&BR=" + Session["BR"].ToString());
            response.EnsureSuccessStatusCode();
            List<Models.SP_GetSBPBlotterCRD_Result> blotterCRD = response.Content.ReadAsAsync<List<Models.SP_GetSBPBlotterCRD_Result>>().Result;
            if (blotterCRD.Count < 1)
                ViewData["DataStatus"] = "Data Not Availavle";
            ViewBag.Title = "All Blotter Setup";
            var PAccess = Session["CurrentPagesAccess"].ToString().Split('~');
            UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(blotterCRD), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());

            ViewData["isDateChangable"] = Convert.ToBoolean(PAccess[2]);
            ViewData["isEditable"] = Convert.ToBoolean(PAccess[3]);
            ViewData["IsDeletable"] = Convert.ToBoolean(PAccess[4]);
            return PartialView("_BlotterCRD", blotterCRD);
        }

        [HttpGet]
        public ActionResult Create()
        {
            SBP_BlotterCRD model = new SBP_BlotterCRD();
            try
            {
                UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), "", this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());

                var ActiveAction = RouteData.Values["action"].ToString();
                var ActiveController = RouteData.Values["controller"].ToString();
                Session["ActiveAction"] = ActiveController;
                Session["ActiveController"] = ActiveAction;

                if (ModelState.IsValid)
                {
                    model.CreateDate = DateTime.Now.Date;

                    ViewBag.CRDNostroBanks = GetAllNostroBanks();
                }
            }
            catch (Exception ex) { }

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

                ViewBag.CRDNostroBanks = GetAllNostroBanks();
                return PartialView("_Create");
            }
            catch (Exception ex) { }

            return PartialView("_Create");
        }

        // POST: BlotterCRD/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _Create(SBP_BlotterCRD BlotterCRD, FormCollection form)
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
                    BlotterCRD.UserID = Convert.ToInt16(Session["UserID"].ToString());
                    BlotterCRD.BID = Convert.ToInt16(Session["BranchID"].ToString());
                    BlotterCRD.BR = Convert.ToInt16(Session["BR"].ToString());
                    BlotterCRD.CurID = Convert.ToInt16(Session["SelectedCurrency"].ToString());
                    BlotterCRD.CreateDate = DateTime.Now;
                    BlotterCRD.Nostro_Account = Convert.ToInt32(form["Nostro_AccountId"].ToString());
                    ServiceRepository serviceObj = new ServiceRepository();
                    HttpResponseMessage response = serviceObj.PostResponse("api/BlotterCRD/InsertCRD", BlotterCRD);
                    response.EnsureSuccessStatusCode();
                    UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(BlotterCRD), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
                    return RedirectToAction("BlotterCRD");
                }
                else
                {
                    ViewBag.CRDNostroBanks = GetAllNostroBanks();
                }
            }
            catch (Exception ex) { }

            return PartialView("_Create", BlotterCRD);
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
            HttpResponseMessage response = serviceObj.GetResponse("/api/BlotterCRD/GetBlotterCRD?id=" + id.ToString());
            response.EnsureSuccessStatusCode();
            Models.SBP_BlotterCRD BlotterCRD = response.Content.ReadAsAsync<Models.SBP_BlotterCRD>().Result;
            UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(BlotterCRD), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
            ViewBag.Nostro_Accountid = BlotterCRD.Nostro_Account;
            ViewBag.CRDNostroBanks = GetAllNostroBanks();
            return PartialView("_Edit", BlotterCRD);
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(Models.SBP_BlotterCRD BlotterCRD, FormCollection form)
        {
            BlotterCRD.UserID = Convert.ToInt16(Session["UserID"].ToString());
            BlotterCRD.BID = Convert.ToInt16(Session["BranchID"].ToString());
            BlotterCRD.BR = Convert.ToInt16(Session["BR"].ToString());
            BlotterCRD.CurID = Convert.ToInt16(Session["SelectedCurrency"].ToString());
            BlotterCRD.UpdateDate = DateTime.Now;
            BlotterCRD.Nostro_Account = Convert.ToInt32(form["Nostro_AccountId"].ToString());
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.PutResponse("api/BlotterCRD/UpdateCRD", BlotterCRD);
            response.EnsureSuccessStatusCode();
            UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(BlotterCRD), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
            return RedirectToAction("BlotterCRD");
        }

        public ActionResult Delete(int id)
        {
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.DeleteResponse("api/BlotterCRD/DeleteCRD?id=" + id.ToString());
            response.EnsureSuccessStatusCode();
            UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(id), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
            return RedirectToAction("BlotterCRD");
        }
    }
}
