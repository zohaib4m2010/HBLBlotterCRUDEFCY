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
using System.Web.Script.Serialization;
using Newtonsoft.Json.Linq;

namespace WebBlotter.Controllers
{
    [AuthAccess]
    public class NostroBanksController : Controller
    {
        // GET: NostroBank
        //public ActionResult NostroBanks()
        //{
        //    try
        //    {
        //        ServiceRepository serviceObj = new ServiceRepository();
        //        HttpResponseMessage response = serviceObj.GetResponse("/api/NostroBank/GetAllNostroBank");
        //        response.EnsureSuccessStatusCode();
        //        List<Models.NostroBank> NostroBank = response.Content.ReadAsAsync<List<Models.NostroBank>>().Result;
        //        UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(NostroBank), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());

        //        ViewBag.Title = "Nostro Bank";
        //        return View(NostroBank);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}
        // GET: NostroBank
        public ActionResult NostroBanks(FormCollection form)
        {
            try
            {
                var selectCurrency = (dynamic)null;
                if (form["selectCurrency"] != null)
                    selectCurrency = Convert.ToInt32(form["selectCurrency"].ToString());
                else
                    selectCurrency = Convert.ToInt32(Session["SelectedCurrency"].ToString());
                UtilityClass.GetSelectedCurrecy(selectCurrency);

                ServiceRepository serviceObj = new ServiceRepository();
                HttpResponseMessage response = serviceObj.GetResponse("/api/NostroBank/GetAllNostroBank?currId=" + selectCurrency);
                response.EnsureSuccessStatusCode();
                //List<Models.NostroBank> NostroBank = response.Content.ReadAsAsync<List<Models.NostroBank>>().Result;
                List<Models.SP_GetAllNostroBankList_Result> NostroBank = new List<Models.SP_GetAllNostroBankList_Result>();

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
                        NostroBank = JsonConvert.DeserializeObject<List<Models.SP_GetAllNostroBankList_Result>>(ResponseDD["Data"]);
                    }
                    else
                        TempData["DataStatus"] = "Data not available";
                }
                ViewBag.Title = "Nostro Bank";
                return PartialView("_NostroBanks", NostroBank);
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
            return PartialView("_Create");

        }

        public ActionResult Create(FormCollection form)
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

                UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), "", this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
                return PartialView("_Create");

            }
            catch (Exception)
            {

                throw;
            }

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _Create(NostroBank NostroBank, FormCollection form)
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
                    NostroBank.CreateDate = DateTime.Now;
                    NostroBank.CurId = Convert.ToInt32(Session["SelectedCurrency"].ToString());
                    ServiceRepository serviceObj = new ServiceRepository();
                    HttpResponseMessage response = serviceObj.PostResponse("api/NostroBank/InsertNostroBank", NostroBank);
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
                    UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(NostroBank), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
                    return RedirectToAction("NostroBanks");
                }
            }
            catch (Exception ex) { }
            return RedirectToAction("NostroBanks");
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
            HttpResponseMessage response = serviceObj.GetResponse("/api/NostroBank/GetNostroBank?id=" + id.ToString());
            response.EnsureSuccessStatusCode();
            //Models.NostroBank NostroBank = response.Content.ReadAsAsync<Models.NostroBank>().Result;
            Models.NostroBank NostroBank = new Models.NostroBank();

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
                NostroBank = JsonConvert.DeserializeObject<Models.NostroBank>(ResponseDD["Data"]);
            }
            else
                TempData["DataStatus"] = getreponse.Message;

            UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(NostroBank), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
            return PartialView("_Edit", NostroBank);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(NostroBank NostroBank)
        {
            NostroBank.UpdateDate = DateTime.Now;
            NostroBank.CurId = Convert.ToInt32(Session["SelectedCurrency"].ToString());
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.PutResponse("api/NostroBank/UpdateNostroBank", NostroBank);
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

            UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(NostroBank), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
            return RedirectToAction("NostroBanks");
        }

        public ActionResult Delete(int id)
        {
            UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(id), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.DeleteResponse("api/NostroBank/DeleteNostroBank?id=" + id.ToString());
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

            return RedirectToAction("NostroBanks");
        }
    }
}