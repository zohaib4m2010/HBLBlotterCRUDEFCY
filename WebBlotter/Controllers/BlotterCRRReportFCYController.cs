using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WebBlotter.Classes;
using WebBlotter.Models;
using WebBlotter.Repository;

namespace WebBlotter.Controllers
{
    [AuthAccess]
    public class BlotterCRRReportFCYController : Controller
    {
        // GET: BlotterCRRReportFCY

        private Models.SP_Get_SBPBlotterConversionRate_Result GetConversionRate()
        {
            try
            {
                ServiceRepository serviceObj = new ServiceRepository();
                HttpResponseMessage response = serviceObj.GetResponse("/api/BlotterRECON/GetConversionRate?currID=0" + "&BR=" + Session["BR"].ToString());
                response.EnsureSuccessStatusCode();
                List<Models.SP_Get_SBPBlotterConversionRate_Result> blotterCR = new List<Models.SP_Get_SBPBlotterConversionRate_Result>();
                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = response.Content.ReadAsStringAsync().Result;
                    var JsonLinq = JObject.Parse(jsonResponse);
                    WebApiResponse getreponse = new WebApiResponse();
                    getreponse.Status = Convert.ToBoolean(JsonLinq["Status"]);
                    getreponse.Message = JsonLinq["Message"].ToString();
                    getreponse.Data = JsonLinq["Data"].ToString();
                    if (getreponse.Message == "Success")
                    {
                        JavaScriptSerializer ser = new JavaScriptSerializer();
                        Dictionary<string, dynamic> ResponseDD = ser.Deserialize<Dictionary<string, dynamic>>(JsonLinq.ToString());
                        blotterCR = JsonConvert.DeserializeObject<List<Models.SP_Get_SBPBlotterConversionRate_Result>>(ResponseDD["Data"]);
                        if (blotterCR != null)
                        {
                            ViewBag.ConversionRateCRR = blotterCR;
                            ViewBag.ConversionUSDRate = blotterCR[0].USDRate;
                        }
                        else
                        {
                            blotterCR[0].CurrencyID = 0;
                            blotterCR[0].SPOTRATE_8 = 0;
                            blotterCR[0].USDRate = 0;
                            ViewBag.ConversionRateCRR = blotterCR;
                            ViewBag.ConversionUSDRate = blotterCR[0].USDRate;
                        }
                    }
                    else
                    {
                        ViewBag.ConversionRateCRR = blotterCR;
                    }
                }
                return blotterCR[0];
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public ActionResult BlotterCRRReportFCY(FormCollection form)
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
            HttpResponseMessage response = serviceObj.GetResponse("/api/BlotterCRRReportFCY/GetAllblotterCRRReportFCY?UserID=" + Session["UserID"].ToString() + "&BranchID=" + Session["BranchID"].ToString() + "&BR=" + Session["BR"].ToString());
            response.EnsureSuccessStatusCode();
            List<Models.SP_GetSBPBlotterCRRReportFCY_Result> blotterCRRReportFCY = new List<Models.SP_GetSBPBlotterCRRReportFCY_Result>();
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = response.Content.ReadAsStringAsync().Result;
                var JsonLinq = JObject.Parse(jsonResponse);
                WebApiResponse getreponse = new WebApiResponse();
                getreponse.Status = Convert.ToBoolean(JsonLinq["Status"]);
                getreponse.Message = JsonLinq["Message"].ToString();
                getreponse.Data = JsonLinq["Data"].ToString();

                if (getreponse.Message == "Success")
                {
                    JavaScriptSerializer ser = new JavaScriptSerializer();
                    Dictionary<string, dynamic> ResponseDD = ser.Deserialize<Dictionary<string, dynamic>>(JsonLinq.ToString());
                    blotterCRRReportFCY = JsonConvert.DeserializeObject<List<Models.SP_GetSBPBlotterCRRReportFCY_Result>>(ResponseDD["Data"]);
                }
                else
                {
                    TempData["DataStatus"] = "Data not available";
                }
            }
            if (blotterCRRReportFCY.Count < 1)
                ViewData["DataStatus"] = "Data Not Availavle";
            ViewBag.Title = "All Blotter Setup";
            var PAccess = Session["CurrentPagesAccess"].ToString().Split('~');
            UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(blotterCRRReportFCY), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());

            ViewData["isDateChangable"] = Convert.ToBoolean(PAccess[2]);
            ViewData["isEditable"] = Convert.ToBoolean(PAccess[3]);
            ViewData["IsDeletable"] = Convert.ToBoolean(PAccess[4]);
            return PartialView("_BlotterCRRReportFCY", blotterCRRReportFCY);
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewModel.BlotterCRRFCYModel model = new ViewModel.BlotterCRRFCYModel();
            try
            {
                UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), "", this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());

                var ActiveAction = RouteData.Values["action"].ToString();
                var ActiveController = RouteData.Values["controller"].ToString();
                Session["ActiveAction"] = ActiveController;
                Session["ActiveController"] = ActiveAction;

                if (ModelState.IsValid)
                {
                    GetConversionRate();
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
                GetConversionRate();
                return PartialView("_Create");
            }
            catch (Exception ex) { }

            return PartialView("_Create");
        }

        // POST: BlotterCRRReportFCY/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _Create(ViewModel.BlotterCRRFCYModel BlotterCRRReportFCY, FormCollection form)
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
                    BlotterCRRReportFCY.CRRReportFCY.UserID = Convert.ToInt16(Session["UserID"].ToString());
                    BlotterCRRReportFCY.CRRReportFCY.BID = Convert.ToInt16(Session["BranchID"].ToString());
                    BlotterCRRReportFCY.CRRReportFCY.BR = Convert.ToInt16(Session["BR"].ToString());
                    BlotterCRRReportFCY.CRRReportFCY.CreateDate = DateTime.Now;
                    for (int i = 0; i < BlotterCRRReportFCY.CRRReportingCurrencyWise.Count; i++)
                    {
                        BlotterCRRReportFCY.CRRReportingCurrencyWise[i].UserID = Convert.ToInt16(Session["UserID"].ToString());
                        BlotterCRRReportFCY.CRRReportingCurrencyWise[i].BID = Convert.ToInt16(Session["BranchID"].ToString());
                        BlotterCRRReportFCY.CRRReportingCurrencyWise[i].BR = Convert.ToInt16(Session["BR"].ToString());
                        BlotterCRRReportFCY.CRRReportingCurrencyWise[i].CreateDate = DateTime.Now;
                    }
                   
                    ServiceRepository serviceObj = new ServiceRepository();
                    HttpResponseMessage response = serviceObj.PostResponse("api/BlotterCRRReportFCY/InsertCRRReportFCY", BlotterCRRReportFCY);
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
                        {
                            TempData["DataStatus"] = getreponse.Message;
                        }
                    }
                    UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(BlotterCRRReportFCY.CRRReportFCY), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
                    return RedirectToAction("BlotterCRRReportFCY");
                }
                else
                {
                    GetConversionRate();
                }
            }
            catch (Exception ex) { }

            return PartialView("_Create", BlotterCRRReportFCY);
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
            HttpResponseMessage response = serviceObj.GetResponse("/api/BlotterCRRReportFCY/GetBlotterCRRReportFCY?id=" + id.ToString());
            response.EnsureSuccessStatusCode();
            ViewModel.BlotterCRRFCYModel BlotterCRRReportFCY = new ViewModel.BlotterCRRFCYModel();
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = response.Content.ReadAsStringAsync().Result;
                var JsonLinq = JObject.Parse(jsonResponse);
                WebApiResponse getreponse = new WebApiResponse();
                getreponse.Status = Convert.ToBoolean(JsonLinq["Status"]);
                getreponse.Message = JsonLinq["Message"].ToString();
                getreponse.Data = JsonLinq["Data"].ToString();

                if (getreponse.Message == "Success")
                {
                    JavaScriptSerializer ser = new JavaScriptSerializer();
                    Dictionary<string, dynamic> ResponseDD = ser.Deserialize<Dictionary<string, dynamic>>(JsonLinq.ToString());
                    BlotterCRRReportFCY.CRRReportFCY = JsonConvert.DeserializeObject<Models.SBP_BlotterCRRReportFCY>(ResponseDD["Data"]);
                }
            }
            UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(BlotterCRRReportFCY), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
            GetConversionRate();
            return PartialView("_Edit", BlotterCRRReportFCY);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(ViewModel.BlotterCRRFCYModel BlotterCRRReportFCY, FormCollection form)
        {
            BlotterCRRReportFCY.CRRReportFCY.UserID = Convert.ToInt16(Session["UserID"].ToString());
            BlotterCRRReportFCY.CRRReportFCY.BID = Convert.ToInt16(Session["BranchID"].ToString());
            BlotterCRRReportFCY.CRRReportFCY.BR = Convert.ToInt16(Session["BR"].ToString());
            BlotterCRRReportFCY.CRRReportFCY.UpdateDate = DateTime.Now;
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.PutResponse("api/BlotterCRRReportFCY/UpdateCRRReportFCY", BlotterCRRReportFCY.CRRReportFCY);
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
                {
                    TempData["DataStatus"] = getreponse.Message;
                }
            }
            UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(BlotterCRRReportFCY), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
            return RedirectToAction("BlotterCRRReportFCY");
        }

        public ActionResult Delete(int id)
        {
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.DeleteResponse("api/BlotterCRRReportFCY/DeleteCRRReportFCY?id=" + id.ToString());
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
                {
                    TempData["DataStatus"] = getreponse.Message;
                }
            }
            UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(id), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
            return RedirectToAction("BlotterCRRReportFCY");
        }
    }
}