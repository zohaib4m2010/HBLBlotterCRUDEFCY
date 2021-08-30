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
    public class BlotterBreakupsController : Controller
    {

        UtilityClass UC = new UtilityClass();
        // GET: BlotterBreakups
        public ActionResult BlotterBreakups(FormCollection form)
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
                HttpResponseMessage response = serviceObj.GetResponse("/api/BlotterBreakups/GetAllBlotterBreakups?UserID=" + Session["UserID"].ToString() + "&BranchID=" + Session["BranchID"].ToString() + "&CurID=" + Session["SelectedCurrency"].ToString() + "&BR=" + Session["BR"].ToString());
                response.EnsureSuccessStatusCode();
                //Models.SP_GetLatestBreakup_Result BlotterBreakups = response.Content.ReadAsAsync<Models.SP_GetLatestBreakup_Result>().Result;
                Models.SP_GetLatestBreakup_Result BlotterBreakups = new Models.SP_GetLatestBreakup_Result();

                if (response.IsSuccessStatusCode)
                {
                    string jsonResult = response.Content.ReadAsStringAsync().Result;
                    var jsonLinq = JObject.Parse(jsonResult);

                    WebApiResponse getreponse = new WebApiResponse();
                    getreponse.Status = Convert.ToBoolean(jsonLinq["Status"]);
                    getreponse.Message = jsonLinq["Message"].ToString();
                    getreponse.Data = jsonLinq["Data"].ToString();
                    if (getreponse.Status == true)
                    {
                        JavaScriptSerializer ser = new JavaScriptSerializer();
                        Dictionary<string, dynamic> ResponseDD = ser.Deserialize<Dictionary<string, dynamic>>(jsonLinq.ToString());
                        BlotterBreakups = JsonConvert.DeserializeObject<List<Models.SP_GetLatestBreakup_Result>>(ResponseDD["Data"]);
                    }
                    else
                        TempData["DataStatus"] = "Data not available";
                }

                var PAccess = Session["CurrentPagesAccess"].ToString().Split('~');
                UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(BlotterBreakups), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
                ViewData["BranchName"] = Session["BranchName"].ToString();
                ViewData["isDateChangable"] = Convert.ToBoolean(PAccess[2]);
                ViewData["isEditable"] = Convert.ToBoolean(PAccess[3]);
                ViewData["IsDeletable"] = Convert.ToBoolean(PAccess[4]);
                return PartialView("_BlotterBreakups", BlotterBreakups);

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
            ViewData["BranchName"] = Session["BranchName"].ToString();
            SBP_BlotterBreakups BlotterBreakups = new SBP_BlotterBreakups();
            BlotterBreakups.FoodPayment_inFlow = 0;
            BlotterBreakups.HOKRemittance_inFlow = 0;
            BlotterBreakups.Miscellaneous_inflow = 0;
            BlotterBreakups.SBPChequeDeposite_inflow = 0;
            BlotterBreakups.ERF_inflow = 0;
            BlotterBreakups.CashWithdrawbySBPCheques_outFlow = 0;
            BlotterBreakups.DSC_outFlow = 0;
            BlotterBreakups.ERF_outflow = 0;
            BlotterBreakups.Miscellaneous_outflow = 0;
            BlotterBreakups.RemitanceToHOK_outFlow = 0;
            BlotterBreakups.SBPCheqGivenToOtherBank_outFlow = 0;
            return PartialView("_Create", BlotterBreakups);
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
            ViewData["BranchName"] = Session["BranchName"].ToString();
            SBP_BlotterBreakups BlotterBreakups = new SBP_BlotterBreakups();
            BlotterBreakups.FoodPayment_inFlow = 0;
            BlotterBreakups.HOKRemittance_inFlow = 0;
            BlotterBreakups.Miscellaneous_inflow = 0;
            BlotterBreakups.SBPChequeDeposite_inflow = 0;
            BlotterBreakups.ERF_inflow = 0;
            BlotterBreakups.CashWithdrawbySBPCheques_outFlow = 0;
            BlotterBreakups.DSC_outFlow = 0;
            BlotterBreakups.ERF_outflow = 0;
            BlotterBreakups.Miscellaneous_outflow = 0;
            BlotterBreakups.RemitanceToHOK_outFlow = 0;
            BlotterBreakups.SBPCheqGivenToOtherBank_outFlow = 0;
            return PartialView("_Create", BlotterBreakups);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _Create(SBP_BlotterBreakups BlotterBreakups, FormCollection form)
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
                    BlotterBreakups.CashWithdrawbySBPCheques_outFlow = UC.CheckNegativeValue(BlotterBreakups.CashWithdrawbySBPCheques_outFlow);
                    BlotterBreakups.ERF_outflow = UC.CheckNegativeValue(BlotterBreakups.ERF_outflow);
                    BlotterBreakups.DSC_outFlow = UC.CheckNegativeValue(BlotterBreakups.DSC_outFlow);
                    BlotterBreakups.RemitanceToHOK_outFlow = UC.CheckNegativeValue(BlotterBreakups.RemitanceToHOK_outFlow);
                    BlotterBreakups.SBPCheqGivenToOtherBank_outFlow = UC.CheckNegativeValue(BlotterBreakups.SBPCheqGivenToOtherBank_outFlow);
                    BlotterBreakups.Miscellaneous_outflow = UC.CheckNegativeValue(BlotterBreakups.Miscellaneous_outflow);
                    BlotterBreakups.UserID = Convert.ToInt16(Session["UserID"].ToString());
                    BlotterBreakups.BR = Convert.ToInt16(Session["BR"].ToString());
                    BlotterBreakups.BID = Convert.ToInt16(Session["BranchID"].ToString());
                    BlotterBreakups.CurID = Convert.ToInt16(Session["SelectedCurrency"].ToString());
                    BlotterBreakups.BreakupDate = DateTime.Now;
                    BlotterBreakups.CreateDate = DateTime.Now;
                    UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(BlotterBreakups), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
                    ServiceRepository serviceObj = new ServiceRepository();
                    HttpResponseMessage response = serviceObj.PostResponse("api/BlotterBreakups/InsertBlotterBreakups", BlotterBreakups);
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
                    return RedirectToAction("BlotterBreakups");
                }
            }
            catch (Exception ex) { }

            return PartialView("_Create", BlotterBreakups);
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
            HttpResponseMessage response = serviceObj.GetResponse("/api/BlotterBreakups/GetBlotterBreakups?id=" + id.ToString());
            response.EnsureSuccessStatusCode();
            //Models.SBP_BlotterBreakups BlotterBreakups = response.Content.ReadAsAsync<Models.SBP_BlotterBreakups>().Result;
            Models.SBP_BlotterBreakups BlotterBreakups = new Models.SBP_BlotterBreakups();

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
                    BlotterBreakups = JsonConvert.DeserializeObject<Models.SBP_BlotterBreakups>(ResponseDD["Data"]);
                }
                else
                    TempData["DataStatus"] = getreponse.Message;
            }
            UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(BlotterBreakups), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
            return PartialView("_Edit", BlotterBreakups);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(SBP_BlotterBreakups BlotterBreakups)
        {
            BlotterBreakups.CashWithdrawbySBPCheques_outFlow = UC.CheckNegativeValue(BlotterBreakups.CashWithdrawbySBPCheques_outFlow);
            BlotterBreakups.ERF_outflow = UC.CheckNegativeValue(BlotterBreakups.ERF_outflow);
            BlotterBreakups.DSC_outFlow = UC.CheckNegativeValue(BlotterBreakups.DSC_outFlow);
            BlotterBreakups.CurID = Convert.ToInt16(Session["SelectedCurrency"].ToString());
            BlotterBreakups.RemitanceToHOK_outFlow = UC.CheckNegativeValue(BlotterBreakups.RemitanceToHOK_outFlow);
            BlotterBreakups.SBPCheqGivenToOtherBank_outFlow = UC.CheckNegativeValue(BlotterBreakups.SBPCheqGivenToOtherBank_outFlow);
            BlotterBreakups.Miscellaneous_outflow = UC.CheckNegativeValue(BlotterBreakups.Miscellaneous_outflow);
            BlotterBreakups.UpdateDate = DateTime.Now;
            UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(BlotterBreakups), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.PutResponse("api/BlotterBreakups/UpdateBlotterBreakups", BlotterBreakups);
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
            return RedirectToAction("BlotterBreakups");
        }

        public ActionResult Delete(int id)
        {
            UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(id), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.DeleteResponse("api/BlotterBreakups/DeleteBlotterBreakups?id=" + id.ToString());
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
            return RedirectToAction("BlotterBreakups");
        }




    }
}