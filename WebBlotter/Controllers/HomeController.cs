using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WebBlotter.Classes;
using WebBlotter.Repository;

namespace WebBlotter.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [AuthAccess]
    public class HomeController : Controller
    {

        public ActionResult index(FormCollection form)
        {
            //var currentDT = GetCurrentDT();
            //ViewData["SysCurrentDt"] = currentDT.ToString("dd-MMM-yyyy"); 

            #region Added by shakir (Currency parameter)
            var selectCurrency = (dynamic)null;
            string StartDate = string.Empty;
            string EndDate = string.Empty;

            if (form["selectCurrency"] != null)
                selectCurrency = Convert.ToInt32(form["selectCurrency"].ToString());
            else
                selectCurrency = Convert.ToInt32(Session["SelectedCurrency"].ToString());

            UtilityClass.GetSelectedCurrecy(selectCurrency);

            // Date Parameters
            if (form["startdate"] != null)
            {
                StartDate = form["startdate"].ToString();
                ViewBag.SetStartDate = StartDate;
            }
            else
            {
                ViewBag.SetStartDate = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
            }
            if (form["enddate"] != null)
            {
                EndDate = form["enddate"].ToString();
                ViewBag.SetEndDate = EndDate;
            }
            else
            {
                ViewBag.SetEndDate = DateTime.Now.ToString("yyyy-MM-dd");
            }

            #endregion

            UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), "", this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.GetResponse("/api/Blotter/GetLatestBlotterDTLReportDayWise?&BR=" + Session["BR"].ToString() + "&StartDate=" + StartDate + "&EndDate=" + EndDate);
            response.EnsureSuccessStatusCode();
            List<Models.SBP_BlotterCRRReportDaysWiseBal> BlotterCRRReportsDayWiseBal = response.Content.ReadAsAsync<List<Models.SBP_BlotterCRRReportDaysWiseBal>>().Result;

            ViewBag.SBP_BlotterCRRReportDaysWiseBal = BlotterCRRReportsDayWiseBal;

            //HttpResponseMessage response1 = serviceObj.GetResponse("/api/Blotter/GetLatestBlotterDTLReportForToday?&BR=" + Session["BR"].ToString());
            //response.EnsureSuccessStatusCode();
            //Models.SBP_BlotterCRRReportDaysWiseBal BlotterCRRReportForTodayBal = response.Content.ReadAsAsync<Models.SBP_BlotterCRRReportDaysWiseBal>().Result;

            //ViewBag.SBP_BlotterCRRReportForTodayBal = BlotterCRRReportForTodayBal;

            HttpResponseMessage response2 = serviceObj.GetResponse("/api/Blotter/GetLatestOpeningBalaceForToday?&BR=" + Session["BR"].ToString() + "&Date=" + DateTime.Now.ToString("yyyy-MM-dd"));
            response2.EnsureSuccessStatusCode();
            Models.SBP_BlotterOpeningBalance BlotterOpeningBalaceForToday = response2.Content.ReadAsAsync<Models.SBP_BlotterOpeningBalance>().Result;


            ViewBag.SBP_BlotterOpeningBalaceForToday = BlotterOpeningBalaceForToday;
            return View();
        }


        public ActionResult FillBlotterManualData(string Date, string flag, int Recon)
        {
            Models.GetBlotterMnualDataParam BMDP = new Models.GetBlotterMnualDataParam();
            BMDP.Recon = (Recon == 1) ? true : false;
            BMDP.DateFor = Convert.ToDateTime(Date);
            BMDP.flag = flag;
            BMDP.BR = Convert.ToInt32(Session["BR"].ToString());
            BMDP.CurId = Convert.ToInt32(Session["SelectedCurrency"].ToString());

            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.PostResponse("/api/Blotter/GetOPICSManualData", BMDP);
            response.EnsureSuccessStatusCode();
            List<Models.SP_GetOPICSManualData_Result> OPICSManualData = response.Content.ReadAsAsync<List<Models.SP_GetOPICSManualData_Result>>().Result;

            UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(OPICSManualData), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
            ViewBag.FillBlotterManualData = OPICSManualData;


            return PartialView("_FillManualData");

        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public DateTime GetCurrentDT()
        {

            try
            {
                ServiceRepositoryBlotter serviceObj = new ServiceRepositoryBlotter();
                HttpResponseMessage response = serviceObj.GetResponse("/api/BlotterDT/GetBlotterSysDT?brcode=" + Session["BR"].ToString());
                response.EnsureSuccessStatusCode();
                //List<Models.SP_SBPOpicsSystemDate_Result> blotterDT = response.Content.ReadAsAsync<List<Models.SP_SBPOpicsSystemDate_Result>>().Result;
                List<Models.SP_SBPOpicsSystemDate_Result> blotterDT = new List<Models.SP_SBPOpicsSystemDate_Result>();

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
                        blotterDT = JsonConvert.DeserializeObject<List<Models.SP_SBPOpicsSystemDate_Result>>(ResponseDD["Data"]);
                    }
                    else
                        TempData["DataStatus"] = "Data not available";
                }
                return Convert.ToDateTime(blotterDT[0].OpicsCurrentDate);

            }
            catch (Exception)
            {
                throw;
            }
        }

        public void SessionIsAlive()
        {

            if (Session["UserID"] == null)
            {
                Response.Redirect(new Uri(Request.Url, Url.Action("Login")).ToString(), false);
            }
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Update(string Id, string BlotterBalance)
        {
            Models.SBP_BlotterCRRReportDaysWiseBal BlotterCRRdayWise = new Models.SBP_BlotterCRRReportDaysWiseBal();

            BlotterCRRdayWise.BR = Convert.ToInt16(Session["BR"].ToString());
            BlotterCRRdayWise.Id = Convert.ToInt32(Id);
            BlotterCRRdayWise.BlotterBalance = Convert.ToDecimal(BlotterBalance.ToString());
            BlotterCRRdayWise.UpdateDate = DateTime.Now;
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.PutResponse("api/Blotter/Update", BlotterCRRdayWise);
            response.EnsureSuccessStatusCode();
            UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(BlotterCRRdayWise), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
            return RedirectToAction("index", "Home");
        }

    
        public ActionResult CRRFCYReportingCurrencyWise(FormCollection form)
        {
            #region Added by shakir (Currency parameter)
            var selectCurrency = (dynamic)null;
            string StartDate = string.Empty;
            string EndDate = string.Empty;

            if (form["selectCurrency"] != null)
                selectCurrency = Convert.ToInt32(form["selectCurrency"].ToString());
            else
                selectCurrency = Convert.ToInt32(Session["SelectedCurrency"].ToString());

            UtilityClass.GetSelectedCurrecy(selectCurrency);
            #endregion

            if (form["startdate"] != null)
            {
                StartDate = form["startdate"].ToString();
                ViewBag.SetStartDate = StartDate;
            }
            else
            {
                ViewBag.SetStartDate = DateTime.Now.ToString("yyyy-MM-dd");
            }
            if (form["enddate"] != null)
            {
                EndDate = form["enddate"].ToString();
                ViewBag.SetEndDate = EndDate;
            }
            else
            {
                ViewBag.SetEndDate = DateTime.Now.ToString("yyyy-MM-dd");
            }
            //UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), "", this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.GetResponse("/api/Blotter/GetLatestBlotterCRRFCYReportCurrencyWise?&BR=" + Session["BR"].ToString() + "&BID=" + Session["BranchID"].ToString() + "&UserID=" + Session["UserID"].ToString() + "&StartDate=" + StartDate + "&EndDate=" + EndDate);
            response.EnsureSuccessStatusCode();
            List<Models.SP_GetSBP_CRRReportingFCYCurrWise_Result> SBP_BlotterCRRFCYReportCurrencyWise = response.Content.ReadAsAsync<List<Models.SP_GetSBP_CRRReportingFCYCurrWise_Result>>().Result;
            if (SBP_BlotterCRRFCYReportCurrencyWise.Count > 0)
            {
                ViewBag.SBP_BlotterCRRFCYReportCurrencyWise = SBP_BlotterCRRFCYReportCurrencyWise;
                GetConversionRate();
            }
            else
            {

                ViewBag.SBP_BlotterCRRFCYReportCurrencyWise = SBP_BlotterCRRFCYReportCurrencyWise;
                ViewBag.ConversionRateCRR = null;
                ViewBag.ConversionUSDRate = null;
                //SBP_BlotterCRRFCYReportCurrencyWise[0].CRRBal5PcrReq = 0;
                //SBP_BlotterCRRFCYReportCurrencyWise[0].CRRBal10PcrReq = 0;
                //SBP_BlotterCRRFCYReportCurrencyWise[0].Deposit = 0;
                //ViewBag.ConversionRateCRR = SBP_BlotterCRRFCYReportCurrencyWise;
                //ViewBag.ConversionUSDRate = SBP_BlotterCRRFCYReportCurrencyWise[0].EquivalentUSD;

            }

            return PartialView("_HomeFCY", SBP_BlotterCRRFCYReportCurrencyWise);
        }

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
    }
}