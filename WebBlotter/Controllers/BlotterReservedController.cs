using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WebBlotter.Classes;
using WebBlotter.Models;
using WebBlotter.Repository;
namespace WebBlotter.Controllers
{
    public class BlotterReservedController : Controller
    {
        // GET: BlotterReserved
        public ActionResult BlotterReserved(FormCollection form)
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
            HttpResponseMessage response = serviceObj.GetResponse("/api/BlotterReserved/GetAllblotterReserved?UserID=" + Session["UserID"].ToString() + "&BranchID=" + Session["BranchID"].ToString() + "&CurID=" + selectCurrency + "&BR=" + Session["BR"].ToString() + "&DateVal=" + DateVal);
            response.EnsureSuccessStatusCode();
            List<Models.SP_GetSBP_Reserved_Result> blotterReserved = new List<Models.SP_GetSBP_Reserved_Result>();
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
                    blotterReserved = JsonConvert.DeserializeObject<List<Models.SP_GetSBP_Reserved_Result>>(ResponseDD["Data"]);
                }
                else
                {
                    TempData["DataStatus"] = "Data not available";
                }
            }
            if (blotterReserved.Count < 1)
                ViewData["DataStatus"] = "Data Not Availavle";
            ViewBag.Title = "All Blotter Setup";
            var PAccess = Session["CurrentPagesAccess"].ToString().Split('~');
            UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(blotterReserved), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());

            ViewData["isDateChangable"] = Convert.ToBoolean(PAccess[2]);
            ViewData["isEditable"] = Convert.ToBoolean(PAccess[3]);
            ViewData["IsDeletable"] = Convert.ToBoolean(PAccess[4]);
            return PartialView("_BlotterReserved", blotterReserved);
        }



        [System.Web.Http.HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Update(string sno, string Date, string ReservedBalance, string SBPBalanace, string BalanceDifference)
        {
            BlotterSBP_Reserved BlotterReserved = new BlotterSBP_Reserved();
            BlotterReserved.UserID = Convert.ToInt16(Session["UserID"].ToString());
            BlotterReserved.BID = Convert.ToInt16(Session["BranchID"].ToString());
            BlotterReserved.BR = Convert.ToInt16(Session["BR"].ToString());
            BlotterReserved.SNo = Convert.ToInt32(sno);
            BlotterReserved.Date = Convert.ToDateTime(Date);
            BlotterReserved.ReservedBalance = ReservedBalance == "" ? 0 : Convert.ToDecimal(ReservedBalance.ToString());
            BlotterReserved.SBPBalanace = Convert.ToDecimal(SBPBalanace.ToString());
            BlotterReserved.BalanceDifference = BalanceDifference == null ? 0 : Convert.ToDecimal(BalanceDifference.ToString());
            BlotterReserved.UpdateDate = DateTime.Now;

            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.PutResponse("api/BlotterReserved/UpdateReserved", BlotterReserved);
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
            UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(BlotterReserved), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
            return RedirectToAction("BlotterReserved");
        }

    }
}