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
    public class BlotterOpenBalController : Controller
    {
        UtilityClass UC = new UtilityClass();
        // GET: BlotterOpenBal

        public ActionResult OpeningBalance(FormCollection form)
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
                HttpResponseMessage response = serviceObj.GetResponse("/api/BlotterOpenBal/GetAllBlotterOpenBal?UserID=" + Session["UserID"].ToString() + "&BranchID=" + Session["BranchID"].ToString() + "&CurID=" + Session["SelectedCurrency"].ToString() + "&BR=" + Session["BR"].ToString() + "&DateVal=" + DateVal);
                response.EnsureSuccessStatusCode();
                //List<Models.SBP_BlotterOpeningBalance> BlotterOpenBal = response.Content.ReadAsAsync<List<Models.SBP_BlotterOpeningBalance>>().Result;
                List<Models.SBP_BlotterOpeningBalance> BlotterOpenBal = new List<Models.SBP_BlotterOpeningBalance>();

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
                        BlotterOpenBal = JsonConvert.DeserializeObject<List<Models.SBP_BlotterOpeningBalance>>(ResponseDD["Data"]);
                    }
                    else
                        TempData["DataStatus"] = "Data not available";
                }
                var PAccess = Session["CurrentPagesAccess"].ToString().Split('~');
                UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(BlotterOpenBal), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString() + "&DateVal=" + DateVal);
                ViewData["isDateChangable"] = Convert.ToBoolean(PAccess[2]);
                ViewData["isEditable"] = Convert.ToBoolean(PAccess[3]);
                ViewData["IsDeletable"] = Convert.ToBoolean(PAccess[4]);
                ViewBag.Title = "All Blotter Setup";
                return PartialView("_OpeningBalance", BlotterOpenBal);
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
            SBP_BlotterOpeningBalance model = new SBP_BlotterOpeningBalance();
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


                UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), "", this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
                SBP_BlotterOpeningBalance model = new SBP_BlotterOpeningBalance();
                return PartialView("_Create", model);
            }
            catch (Exception ex) { }

            return PartialView("_Create");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _Create(SBP_BlotterOpeningBalance BlotterOpenBal, FormCollection form)
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
                        BlotterOpenBal.DataType = "SBP";
                    }
                    BlotterOpenBal.BalDate = BlotterOpenBal.BalDate;
                    BlotterOpenBal.UserID = Convert.ToInt16(Session["UserID"].ToString());
                    BlotterOpenBal.BID = Convert.ToInt16(Session["BranchID"].ToString());
                    BlotterOpenBal.BR = Convert.ToInt16(Session["BR"].ToString());
                    BlotterOpenBal.CurID = Convert.ToInt16(Session["SelectedCurrency"].ToString());
                    BlotterOpenBal.CreateDate = DateTime.Now;
                    ServiceRepository serviceObj = new ServiceRepository();
                    HttpResponseMessage response = serviceObj.PostResponse("api/BlotterOpenBal/InsertOpenBal", BlotterOpenBal);
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
                    UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(BlotterOpenBal), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
                    return RedirectToAction("OpeningBalance");
                }
            }
            catch (Exception ex) { }
            return PartialView("_Create", BlotterOpenBal);

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
            HttpResponseMessage response = serviceObj.GetResponse("/api/BlotterOpenBal/GetBlotterOpenBalById?id=" + id.ToString());
            response.EnsureSuccessStatusCode();
            //Models.SBP_BlotterOpeningBalance BlotterOpenBal = response.Content.ReadAsAsync<Models.SBP_BlotterOpeningBalance>().Result;
            Models.SBP_BlotterOpeningBalance BlotterOpenBal = new Models.SBP_BlotterOpeningBalance();
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
                    BlotterOpenBal = JsonConvert.DeserializeObject<Models.SBP_BlotterOpeningBalance>(ResponseDD["Data"]);
                }
                else
                    TempData["DataStatus"] = getreponse.Message;
            }
            UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(BlotterOpenBal), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
            var isDateChangable = Convert.ToBoolean(Session["CurrentPagesAccess"].ToString().Split('~')[2]);
            ViewData["isDateChangable"] = isDateChangable;
            return PartialView("_Edit", BlotterOpenBal);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(Models.SBP_BlotterOpeningBalance BlotterOpenBal)
        {
            if (Session["BR"].ToString() == "01")
            {
                BlotterOpenBal.DataType = "SBP";
            }
            BlotterOpenBal.UpdateDate = DateTime.Now;
            if (BlotterOpenBal.BalDate == null)
                BlotterOpenBal.BalDate = DateTime.Now;
            BlotterOpenBal.UserID = Convert.ToInt16(Session["UserID"].ToString());
            BlotterOpenBal.CurID = Convert.ToInt16(Session["SelectedCurrency"].ToString());
            BlotterOpenBal.BID = Convert.ToInt16(Session["BranchID"].ToString());
            BlotterOpenBal.BR = Convert.ToInt16(Session["BR"].ToString());
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.PutResponse("api/BlotterOpenBal/UpdateOpenBal", BlotterOpenBal);
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
            UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(BlotterOpenBal), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
            return RedirectToAction("OpeningBalance");
        }

        public ActionResult Delete(int id)
        {
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.DeleteResponse("api/BlotterOpenBal/DeleteOpenBal?id=" + id.ToString());
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
            return RedirectToAction("OpeningBalance");
        }
    }
}