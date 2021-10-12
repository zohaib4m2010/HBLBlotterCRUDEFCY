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
    public class BlotterImportExportController : Controller
    {
        // GET: BlotterImportExport

        public ActionResult GetOrgNostroBanks(string OrgCurId, string Againstcurid)
        {
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = new HttpResponseMessage();
            if (Convert.ToInt32(OrgCurId) > 0 && Convert.ToInt32(Againstcurid) == 0)
            {
                response = serviceObj.GetResponse("/api/NostroBank/GetNostroBankFromOpicsDDL?currId=" + OrgCurId + "&BRCode=" + Session["BR"].ToString());
            }
            else if (Convert.ToInt32(OrgCurId) == 0 && Convert.ToInt32(Againstcurid) > 0)
            {
                response = serviceObj.GetResponse("/api/NostroBank/GetNostroBankFromOpicsDDL?currId=" + Againstcurid + "&BRCode=" + Session["BR"].ToString());
            }
            //response = serviceObj.GetResponse("/api/NostroBank/GetAllNostroBank?currId=" + OrgCurId);
            response.EnsureSuccessStatusCode();
            List<Models.SP_GetNostroBankFromOPICS_Result> blotterNB = new List<Models.SP_GetNostroBankFromOPICS_Result>();

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
                    blotterNB = JsonConvert.DeserializeObject<List<Models.SP_GetNostroBankFromOPICS_Result>>(ResponseDD["Data"]);
                }
            }
            //UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(OPICSManualData), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());

            if (Convert.ToInt32(OrgCurId) > 0 && Convert.ToInt32(Againstcurid) == 0)
            {
                ViewBag.ImportExportNostroBanks = blotterNB;
            }
            else if (Convert.ToInt32(OrgCurId) == 0 && Convert.ToInt32(Againstcurid) > 0)
            {
                ViewBag.ImportExportAgainstNostroBanks = blotterNB;
            }

            return PartialView("_NostroBankDDL");
        }
        public ActionResult BlotterImportExport(FormCollection form)
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
            HttpResponseMessage response = serviceObj.GetResponse("/api/BlotterImportExport/GetAllBlotterImportExport?UserID=" + Session["UserID"].ToString() + "&BranchID=" + Session["BranchID"].ToString() + "&BR=" + Session["BR"].ToString() + "&DateVal=" + DateVal);
            response.EnsureSuccessStatusCode();
            List<Models.SP_GetSBP_BlotterImportExport_Result> BlotterImportExport = new List<Models.SP_GetSBP_BlotterImportExport_Result>();

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
                    BlotterImportExport = JsonConvert.DeserializeObject<List<Models.SP_GetSBP_BlotterImportExport_Result>>(ResponseDD["Data"]);
                }
                else
                    TempData["DataStatus"] = "Data not available";
            }

            ViewBag.Title = "All Blotter Setup";
            var PAccess = Session["CurrentPagesAccess"].ToString().Split('~');
            UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(BlotterImportExport), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());

            ViewData["isDateChangable"] = Convert.ToBoolean(PAccess[2]);
            ViewData["isEditable"] = Convert.ToBoolean(PAccess[3]);
            ViewData["IsDeletable"] = Convert.ToBoolean(PAccess[4]);
            return PartialView("_BlotterImportExport", BlotterImportExport);
        }

        [HttpGet]
        public ActionResult Create()
        {
            SBP_BlotterImportExport model = new SBP_BlotterImportExport();
            try
            {
                UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), "", this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());

                var ActiveAction = RouteData.Values["action"].ToString();
                var ActiveController = RouteData.Values["controller"].ToString();
                Session["ActiveAction"] = ActiveController;
                Session["ActiveController"] = ActiveAction;

                if (ModelState.IsValid)
                {
                    //ViewBag.ImportExportNostroBanks = GetAllNostroBanks();
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

                //ViewBag.ImportExportNostroBanks = GetAllNostroBanks();
                return PartialView("_Create");
            }
            catch (Exception) { }

            return PartialView("_Create");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _Create(SBP_BlotterImportExport BlotterImportExport, FormCollection form)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    BlotterImportExport.UserId = Convert.ToInt16(Session["UserID"].ToString());
                    BlotterImportExport.BID = Convert.ToInt16(Session["BranchID"].ToString());
                    BlotterImportExport.BR = Convert.ToInt16(Session["BR"].ToString());
                    BlotterImportExport.BlotterType = form["BlotterType"].ToString();
                    BlotterImportExport.CurId = Convert.ToInt32(form["OrigCurrency"].ToString());
                    BlotterImportExport.BankCode = form["BankCode"].ToString();
                    BlotterImportExport.AgainstCurId = Convert.ToInt32(form["AgainstCurrency"].ToString());
                    BlotterImportExport.AgainstBankCode = form["AgainstBankCode"].ToString();
                    BlotterImportExport.Createdate = DateTime.Now;
                    ServiceRepository serviceObj = new ServiceRepository();
                    HttpResponseMessage response = serviceObj.PostResponse("api/BlotterImportExport/InsertImportExport", BlotterImportExport);
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
                    UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(BlotterImportExport), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
                    return RedirectToAction("BlotterImportExport");
                }
                else
                {
                    //ViewBag.ImportExportNostroBanks = GetAllNostroBanks();
                }
            }
            catch (Exception ex) { }

            return RedirectToAction("BlotterImportExport", BlotterImportExport);
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
            HttpResponseMessage response = serviceObj.GetResponse("/api/BlotterImportExport/GetBlotterImportExport?id=" + id.ToString());
            response.EnsureSuccessStatusCode();
            Models.SBP_BlotterImportExport BlotterImportExport = new Models.SBP_BlotterImportExport();
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
                    BlotterImportExport = JsonConvert.DeserializeObject<Models.SBP_BlotterImportExport>(ResponseDD["Data"]);
                }
                else
                    TempData["DataStatus"] = getreponse.Message;
            }
            UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(BlotterImportExport), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
            ViewBag.OrgCurId = BlotterImportExport.CurId;
            ViewBag.OrgBankCode = BlotterImportExport.BankCode;
            ViewBag.AgainstCurId = BlotterImportExport.AgainstCurId;
            ViewBag.AgainstBankCode = BlotterImportExport.AgainstBankCode;

            //ViewBag.ImportExportNostroBanks = GetAllNostroBanks();
            return PartialView("_Edit", BlotterImportExport);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(Models.SBP_BlotterImportExport BlotterImportExport, FormCollection form)
        {
            BlotterImportExport.UserId = Convert.ToInt16(Session["UserID"].ToString());
            BlotterImportExport.BID = Convert.ToInt16(Session["BranchID"].ToString());
            BlotterImportExport.BR = Convert.ToInt16(Session["BR"].ToString());
            BlotterImportExport.BlotterType = form["BlotterType"].ToString();
            BlotterImportExport.CurId = Convert.ToInt32(form["OrigCurrency"].ToString());
            BlotterImportExport.BankCode = form["BankCode"].ToString();
            BlotterImportExport.AgainstCurId = Convert.ToInt32(form["AgainstCurrency"].ToString());
            BlotterImportExport.AgainstBankCode = form["AgainstBankCode"].ToString();
            BlotterImportExport.UpdateDate = DateTime.Now;
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.PutResponse("api/BlotterImportExport/UpdateImportExport", BlotterImportExport);
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
            UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(BlotterImportExport), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
            return RedirectToAction("BlotterImportExport");
        }

        public ActionResult Delete(int id)
        {
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.DeleteResponse("api/BlotterImportExport/DeleteImportExport?id=" + id.ToString());
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
            return RedirectToAction("BlotterImportExport");
        }
    }
}