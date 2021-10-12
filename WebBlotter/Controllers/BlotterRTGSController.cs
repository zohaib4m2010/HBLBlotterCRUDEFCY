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
using Newtonsoft.Json.Linq;
using System.Web.Script.Serialization;

namespace WebBlotter.Controllers
{
    [AuthAccess]
    public class BlotterRTGSController : Controller
    {
        UtilityClass UC = new UtilityClass();
        // GET: BlotterRTGS
       
        private List<Models.SP_GETAllTransactionTitles_Result> GetAllRTGSTransactionTitles()
        {
            try
            {
                ServiceRepository serviceObj = new ServiceRepository();
                HttpResponseMessage response = serviceObj.GetResponse("/api/BlotterRTGS/GetAllRTGSTransactionTitles");
                response.EnsureSuccessStatusCode();
                List<Models.SP_GETAllTransactionTitles_Result> blotterTTT = response.Content.ReadAsAsync<List<Models.SP_GETAllTransactionTitles_Result>>().Result;

                return blotterTTT;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult BlotterRTGS(FormCollection form)
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
                var DateVal = (dynamic)null;
                if (form["SearchByDate"] != null)
                {
                    DateVal = form["SearchByDate"].ToString();
                    ViewBag.DateVal = DateVal;
                }
                else
                {
                    ViewBag.DateVal = DateTime.Now.ToString("yyyy-MM-dd");
                }
                #endregion

                ServiceRepository serviceObj = new ServiceRepository();
                HttpResponseMessage response = serviceObj.GetResponse("/api/BlotterRTGS/GetAllBlotterRTGS?UserID=" + Session["UserID"].ToString() + "&BranchID=" + Session["BranchID"].ToString() + "&CurID=" + Session["SelectedCurrency"].ToString() + "&BR=" + Session["BR"].ToString() + "&DateVal=" + DateVal);
                response.EnsureSuccessStatusCode();
                //List<Models.SP_GetAll_SBPBlotterRTGS_Result> blotterRTGS = response.Content.ReadAsAsync<List<Models.SP_GetAll_SBPBlotterRTGS_Result>>().Result;
                List<Models.SP_GetAll_SBPBlotterRTGS_Result> blotterRTGS = new List<Models.SP_GetAll_SBPBlotterRTGS_Result>();

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = response.Content.ReadAsStringAsync().Result;
                    var JsonLinq = JObject.Parse(jsonResponse);
                    WebApiResponse getreponse = new WebApiResponse();
                    getreponse.Status = Convert.ToBoolean(JsonLinq["Status"]);
                    getreponse.Message = JsonLinq["Message"].ToString();
                    getreponse.Data = JsonLinq["Data"].ToString();
                    if (getreponse.Status == true)
                    {
                        JavaScriptSerializer ser = new JavaScriptSerializer();
                        Dictionary<string, dynamic> ResponseDD = ser.Deserialize<Dictionary<string, dynamic>>(JsonLinq.ToString());
                        blotterRTGS = JsonConvert.DeserializeObject<List<Models.SP_GetAll_SBPBlotterRTGS_Result>>(ResponseDD["Data"]);
                    }
                    else
                        TempData["DataStatus"] = "Data not available";
                }

                var PAccess = Session["CurrentPagesAccess"].ToString().Split('~');
                UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(blotterRTGS), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());

                ViewData["isDateChangable"] = Convert.ToBoolean(PAccess[2]);
                ViewData["isEditable"] = Convert.ToBoolean(PAccess[3]);
                ViewData["IsDeletable"] = Convert.ToBoolean(PAccess[4]);
                ViewBag.Title = "All Blotter Setup";
                return PartialView("_BlotterRTGS", blotterRTGS);
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
            SBP_BlotterRTGS model = new SBP_BlotterRTGS();
            try
            {
                if (ModelState.IsValid)
                {
                    model.RTGS_Date = DateTime.Now.Date;
                    ViewBag.RTGSTransactionTitles = GetAllRTGSTransactionTitles();
                }
                else
                {
                   
                    ViewBag.RTGSTransactionTitles = GetAllRTGSTransactionTitles();
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

                if (ModelState.IsValid)
                {
                    ViewBag.RTGSTransactionTitles = GetAllRTGSTransactionTitles();
                }
                else
                {
                    ViewBag.RTGSTransactionTitles = GetAllRTGSTransactionTitles();
                }
            }
            catch (Exception ex) { }

            return PartialView("_Create");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _Create(SBP_BlotterRTGS BlotterRTGS, FormCollection form)
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
                    BlotterRTGS.RTGS_OutFLow = UC.CheckNegativeValue(BlotterRTGS.RTGS_OutFLow);
                    BlotterRTGS.UserID = Convert.ToInt16(Session["UserID"].ToString());
                    BlotterRTGS.BID = Convert.ToInt16(Session["BranchID"].ToString());
                    BlotterRTGS.BR = Convert.ToInt16(Session["BR"].ToString());
                    BlotterRTGS.CurID = Convert.ToInt16(Session["SelectedCurrency"].ToString());
                    BlotterRTGS.CreateDate = DateTime.Now;
                    ServiceRepository serviceObj = new ServiceRepository();
                    HttpResponseMessage response = serviceObj.PostResponse("api/BlotterRTGS/InsertRTGS", BlotterRTGS);
                    response.EnsureSuccessStatusCode();
                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResponse = response.Content.ReadAsStringAsync().Result;
                        var JsonLinq = JObject.Parse(jsonResponse);
                        WebApiResponse getreponse = new WebApiResponse();
                        getreponse.Status = Convert.ToBoolean(JsonLinq["Status"]);
                        getreponse.Message = JsonLinq["Message"].ToString();
                        getreponse.Data = JsonLinq["Data"].ToString();
                        if (getreponse.Status == true)
                            TempData["DataStatus"] = getreponse.Message;
                        else
                            TempData["DataStatus"] = getreponse.Message;
                    }

                    UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(BlotterRTGS), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
                    return RedirectToAction("BlotterRTGS");
                }
                else
                {
                    ViewBag.RTGSTransactionTitles = GetAllRTGSTransactionTitles();
                }
            }
            catch (Exception ex) { }
            return PartialView("_Create", BlotterRTGS);

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
            HttpResponseMessage response = serviceObj.GetResponse("/api/BlotterRTGS/GetBlotterRTGS?id=" + id.ToString());
            response.EnsureSuccessStatusCode();
            //Models.SBP_BlotterRTGS BlotterRTGS = response.Content.ReadAsAsync<Models.SBP_BlotterRTGS>().Result;
            Models.SBP_BlotterRTGS BlotterRTGS = new Models.SBP_BlotterRTGS();

            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = response.Content.ReadAsStringAsync().Result;
                var JsonLinq = JObject.Parse(jsonResponse);
                WebApiResponse getreponse = new WebApiResponse();
                getreponse.Status = Convert.ToBoolean(JsonLinq["Status"]);
                getreponse.Message = JsonLinq["Message"].ToString();
                getreponse.Data = JsonLinq["Data"].ToString();

                if (getreponse.Status == true)
                {
                    JavaScriptSerializer ser = new JavaScriptSerializer();
                    Dictionary<string, dynamic> ResponseDD = ser.Deserialize<Dictionary<string, dynamic>>(JsonLinq.ToString());
                    BlotterRTGS = JsonConvert.DeserializeObject<Models.SBP_BlotterRTGS>(ResponseDD["Data"]);
                }
                else
                    TempData["DataStatus"] = getreponse.Message;

            }
            UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(BlotterRTGS), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
            ViewBag.RTGSTransactionTitles = GetAllRTGSTransactionTitles();
            var isDateChangable = Convert.ToBoolean(Session["CurrentPagesAccess"].ToString().Split('~')[2]);
            ViewData["isDateChangable"] = isDateChangable;
            return PartialView("_Edit", BlotterRTGS);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(Models.SBP_BlotterRTGS BlotterRTGS)
        {
            BlotterRTGS.RTGS_OutFLow = UC.CheckNegativeValue(BlotterRTGS.RTGS_OutFLow);
            BlotterRTGS.UpdateDate = DateTime.Now;
            if (BlotterRTGS.RTGS_Date == null)
                BlotterRTGS.RTGS_Date = DateTime.Now;
            BlotterRTGS.CurID = Convert.ToInt16(Session["SelectedCurrency"].ToString());
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.PutResponse("api/BlotterRTGS/UpdateRTGS", BlotterRTGS);
            response.EnsureSuccessStatusCode();
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = response.Content.ReadAsStringAsync().Result;
                var JsonLinq = JObject.Parse(jsonResponse);
                WebApiResponse getreponse = new WebApiResponse();
                getreponse.Status = Convert.ToBoolean(JsonLinq["Status"]);
                getreponse.Message = JsonLinq["Message"].ToString();
                getreponse.Data = JsonLinq["Data"].ToString();
                if (getreponse.Status == true)
                    TempData["DataStatus"] = getreponse.Message;
                else
                    TempData["DataStatus"] = getreponse.Message;
            }

            UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(BlotterRTGS), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
            return RedirectToAction("BlotterRTGS");
        }

        public ActionResult Delete(int id)
        {
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.DeleteResponse("api/BlotterRTGS/DeleteRTGS?id=" + id.ToString());
            response.EnsureSuccessStatusCode();
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = response.Content.ReadAsStringAsync().Result;
                var JsonLinq = JObject.Parse(jsonResponse);
                WebApiResponse getreponse = new WebApiResponse();
                getreponse.Status = Convert.ToBoolean(JsonLinq["Status"]);
                getreponse.Message = JsonLinq["Message"].ToString();
                getreponse.Data = JsonLinq["Data"].ToString();

                if (getreponse.Status == true)
                {
                    TempData["DataStatus"] = getreponse.Message;
                }
                else
                    TempData["DataStatus"] = getreponse.Message;
            }
            UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(id), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
            return RedirectToAction("BlotterRTGS");
        }
    }
}