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
    public class BlotterFundingRepoController : Controller
    {
        UtilityClass UC = new UtilityClass();
        #region Latest Code
        //public ActionResult BlotterFundingRepo(FormCollection form)
        //{
        //    #region 
        //    var selectCurrency = (dynamic)null;
        //    if (form["selectCurrency"] != null)
        //        selectCurrency = Convert.ToInt32(form["selectCurrency"].ToString());
        //    else
        //        selectCurrency = Convert.ToInt32(Session["SelectedCurrency"].ToString());

        //    UtilityClass.GetSelectedCurrecy(selectCurrency);
        //    #endregion

        //    ServiceRepository serviceObj = new ServiceRepository();
        //    HttpResponseMessage response = serviceObj.GetResponse("/api/BlotterFundingRepo/GetAllblotterFundingRepo?UserID=" + Session["UserID"].ToString() + "&BranchID=" + Session["BranchID"].ToString() + "&CurID=" + selectCurrency + "&BR=" + Session["BR"].ToString());
        //    response.EnsureSuccessStatusCode();
        //    List<Models.SP_GetSBPBlotterFR_Result> blotterFR = new List<Models.SP_GetSBPBlotterFR_Result>();
        //    if (response.IsSuccessStatusCode)
        //    {
        //        string jsonResponse = response.Content.ReadAsStringAsync().Result;
        //        var JsonLinq = JObject.Parse(jsonResponse);
        //        WebApiResponse getreponse = new WebApiResponse();
        //        getreponse.Status = Convert.ToBoolean(JsonLinq["Status"]);
        //        getreponse.Message = JsonLinq["Message"].ToString();
        //        getreponse.Data = JsonLinq["Data"].ToString();

        //        if (getreponse.Message == "Success")
        //        {
        //            JavaScriptSerializer ser = new JavaScriptSerializer();
        //            Dictionary<string, dynamic> ResponseDD = ser.Deserialize<Dictionary<string, dynamic>>(JsonLinq.ToString());
        //            blotterFR = JsonConvert.DeserializeObject<List<Models.SP_GetSBPBlotterFR_Result>>(ResponseDD["Data"]);
        //        }
        //        else
        //        {
        //            TempData["DataStatus"] = "Data not available";
        //        }
        //    }

        //    ViewBag.Title = "All Blotter Setup";
        //    var PAccess = Session["CurrentPagesAccess"].ToString().Split('~');
        //    UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(blotterFR), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
        //    TempData["isDateChangable"] = Convert.ToBoolean(PAccess[2]);
        //    TempData["isEditable"] = Convert.ToBoolean(PAccess[3]);
        //    TempData["IsDeletable"] = Convert.ToBoolean(PAccess[4]);
        //    return PartialView("_BlotterFundingRepo", blotterFR);
        //}

        //[HttpGet]
        //public ActionResult Create()
        //{
        //    SBP_BlotterFundingRepo model = new SBP_BlotterFundingRepo();
        //    try
        //    {
        //        UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), "", this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());

        //        var ActiveAction = RouteData.Values["action"].ToString();
        //        var ActiveController = RouteData.Values["controller"].ToString();
        //        Session["ActiveAction"] = ActiveAction;
        //        Session["ActiveController"] = ActiveController;

        //        if (ModelState.IsValid)
        //        {
        //            model.CreateDate = DateTime.Now.Date;

        //            //ViewBag.FRBanks = GetAllNostroBanks();
        //        }
        //    }
        //    catch (Exception ex) { }

        //    return PartialView("_Create", model);

        //}
        //public ActionResult Create(FormCollection form)
        //{
        //    try
        //    {
        //        UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), "", this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());

        //        #region Added by shakir (Currency parameter)

        //        var selectCurrency = (dynamic)null;
        //        if (form["selectCurrency"] != null)
        //            selectCurrency = Convert.ToInt32(form["selectCurrency"].ToString());
        //        else
        //            selectCurrency = Convert.ToInt32(Session["SelectedCurrency"].ToString());
        //        UtilityClass.GetSelectedCurrecy(selectCurrency);

        //        #endregion

        //        //ViewBag.FRBanks = GetAllNostroBanks();
        //        return PartialView("_Create");
        //    }
        //    catch (Exception ex) { }

        //    return PartialView("_Create");
        //}

        //[HttpPost]
        //public ActionResult _Create(FormCollection form)
        //{
        //    #region Added by shakir (Currency parameter)
        //    var selectCurrency = (dynamic)null;
        //    if (form["selectCurrency"] != null)
        //        selectCurrency = Convert.ToInt32(form["selectCurrency"].ToString());
        //    else
        //        selectCurrency = Convert.ToInt32(Session["SelectedCurrency"].ToString());

        //    UtilityClass.GetSelectedCurrecy(selectCurrency);
        //    #endregion
        //    try
        //    {
        //        List<SBP_BlotterFundingRepo> BlotterFR = new List<SBP_BlotterFundingRepo>();
        //        for (int i = 0; i <= Request.Form.Count; i++)
        //        {
        //            if (Request.Form["DataType[" + i + "]"] != null)
        //            {
        //                var DataType_data = Request.Form["DataType[" + i + "]"];
        //                var Bank_data = Request.Form["Bank[" + i + "]"];
        //                var Days_data = Convert.ToInt32(Request.Form["Days[" + i + "]"]);
        //                var Rate_data = Convert.ToInt32(Request.Form["Rate[" + i + "]"]);
        //                var DealType_data = Request.Form["DealType[" + i + "]"];
        //                var Broker_data = Request.Form["Broker[" + i + "]"];
        //                var Issue_Date_data = Request.Form["Issue_Date[" + i + "]"];
        //                var IssueType_data = Request.Form["IssueType[" + i + "]"];
        //                var InFlow_data = Convert.ToDecimal(Request.Form["InFlow[" + i + "]"]);
        //                var OutFLow_data = Convert.ToDecimal(Request.Form["OutFLow[" + i + "]"]);
        //                var Note_data = Request.Form["Note[" + i + "]"];

        //                BlotterFR.Add(new SBP_BlotterFundingRepo
        //                {
        //                    DataType = DataType_data,
        //                    Bank = Bank_data,
        //                    Days = Days_data,
        //                    Rate = Rate_data,
        //                    DealType = DealType_data,
        //                    Broker = Broker_data,
        //                    Issue_Date = Convert.ToDateTime(Issue_Date_data.ToString()),
        //                    IssueType = IssueType_data,
        //                    InFlow = InFlow_data,
        //                    OutFLow = OutFLow_data,
        //                    Note = Note_data,
        //                    UserID = Convert.ToInt16(Session["UserID"].ToString()),
        //                    BID = Convert.ToInt16(Session["BranchID"].ToString()),
        //                    BR = Convert.ToInt16(Session["BR"].ToString()),
        //                    CurID = Convert.ToInt16(Session["SelectedCurrency"].ToString()),
        //                    CreateDate = DateTime.Now
        //                });
        //            }
        //        }

        //        if (ModelState.IsValid)
        //        {
        //            ServiceRepository serviceObj = new ServiceRepository();
        //            HttpResponseMessage response = serviceObj.PostResponse("api/BlotterFundingRepo/InsertFundingRepo", BlotterFR);
        //            response.EnsureSuccessStatusCode();

        //            if (response.IsSuccessStatusCode)
        //            {
        //                string jsonResponse = response.Content.ReadAsStringAsync().Result;
        //                var JsonLinq = JObject.Parse(jsonResponse);
        //                WebApiResponse getreponse = new WebApiResponse();
        //                getreponse.Status = Convert.ToBoolean(JsonLinq["Status"]);
        //                getreponse.Message = JsonLinq["Message"].ToString();
        //                getreponse.Data = JsonLinq["Data"].ToString();

        //                if (getreponse.Status == true)
        //                {
        //                    TempData["DataStatus"] = getreponse.Message;
        //                }
        //                else
        //                {
        //                    TempData["DataStatus"] = getreponse.Message;
        //                }
        //                //    }
        //                UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(BlotterFR), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
        //                return RedirectToAction("BlotterFundingRepo");
        //            }
        //        }
        //    }
        //    catch (Exception ex) { }
        //    return PartialView("_Create");
        //}


        //[HttpPost]
        //public ActionResult Delete(IEnumerable<int> employeeIdsToDelete)
        //{
        //    //SBP_BlotterFundingRepo BlotterFR = new SBP_BlotterFundingRepo();
        //    //int Ids = Convert.ToInt32(form["selectCurrency"]);
        //    //BlotterFR.Add(new SBP_BlotterFundingRepo
        //    //{
        //    //    SNo = Ids,

        //    //});

        //    ServiceRepository serviceObj = new ServiceRepository();
        //    HttpResponseMessage response = serviceObj.PostResponse("api/BlotterFundingRepo/DeleteFundingRepo", employeeIdsToDelete);
        //    response.EnsureSuccessStatusCode();

        //    if (response.IsSuccessStatusCode)
        //    {
        //        string jsonResponse = response.Content.ReadAsStringAsync().Result;
        //        var JsonLinq = JObject.Parse(jsonResponse);
        //        WebApiResponse getreponse = new WebApiResponse();
        //        getreponse.Status = Convert.ToBoolean(JsonLinq["Status"]);
        //        getreponse.Message = JsonLinq["Message"].ToString();
        //        getreponse.Data = JsonLinq["Data"].ToString();

        //        if (getreponse.Status == true)
        //        {
        //            TempData["DataStatus"] = getreponse.Message;
        //        }
        //        else
        //        {
        //            TempData["DataStatus"] = getreponse.Message;
        //        }
        //    }

        //    return RedirectToAction("BlotterFundingRepo");
        //}
        #endregion


        public ActionResult BlotterFundingRepo(FormCollection form)
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
            #endregion

            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.GetResponse("/api/BlotterFundingRepo/GetAllblotterFundingRepo?UserID=" + Session["UserID"].ToString() + "&BranchID=" + Session["BranchID"].ToString() + "&CurID=" + selectCurrency + "&BR=" + Session["BR"].ToString() + "&DateVal=" + DateVal);
            response.EnsureSuccessStatusCode();
            // List<Models.SP_GetSBPBlotterFR_Result> blotterFR = response.Content.ReadAsAsync<List<Models.SP_GetSBPBlotterFR_Result>>().Result;
            List<Models.SP_GetSBPBlotterFR_Result> blotterFR = new List<Models.SP_GetSBPBlotterFR_Result>();
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
                    blotterFR = JsonConvert.DeserializeObject<List<Models.SP_GetSBPBlotterFR_Result>>(ResponseDD["Data"]);
                }
                else
                {
                    TempData["DataStatus"] = "Data not available";
                }
            }
            ViewBag.Title = "All Blotter Setup";
            var PAccess = Session["CurrentPagesAccess"].ToString().Split('~');
            UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(blotterFR), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());

            ViewData["isDateChangable"] = Convert.ToBoolean(PAccess[2]);
            ViewData["isEditable"] = Convert.ToBoolean(PAccess[3]);
            ViewData["IsDeletable"] = Convert.ToBoolean(PAccess[4]);
            return PartialView("_BlotterFundingRepo", blotterFR);
        }

        [HttpGet]
        public ActionResult Create()
        {
            SBP_BlotterFundingRepo model = new SBP_BlotterFundingRepo();
            try
            {
                UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), "", this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());

                var ActiveAction = RouteData.Values["action"].ToString();
                var ActiveController = RouteData.Values["controller"].ToString();
                Session["ActiveAction"] = ActiveAction;
                Session["ActiveController"] = ActiveController;

                if (ModelState.IsValid)
                {
                    model.CreateDate = DateTime.Now.Date;

                    //ViewBag.FRBanks = GetAllNostroBanks();
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

                return PartialView("_Create");
            }
            catch (Exception ex) { }

            return PartialView("_Create");
        }


        [HttpPost]
        public ActionResult _Create(FormCollection form)
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
                List<SBP_BlotterFundingRepo> BlotterFR = new List<SBP_BlotterFundingRepo>();
                for (int i = 0; i <= Request.Form.Count; i++)
                {
                    if (Request.Form["DealType[" + i + "]"] != null)
                    {
                        var DataType_data = (Session["BR"].ToString() != "01") ? Request.Form["DataType[" + i + "]"] : "SBP";
                        var Bank_data = Request.Form["Bank[" + i + "]"];
                        var Days_data = Convert.ToInt32(Request.Form["Days[" + i + "]"]);
                        var Rate_data = Convert.ToDouble(Request.Form["Rate[" + i + "]"]);
                        var DealType_data = Request.Form["DealType[" + i + "]"];
                        var Broker_data = Request.Form["Broker[" + i + "]"];
                        var Issue_Date_data = Request.Form["Issue_Date[" + i + "]"];
                        var IssueType_data = Request.Form["IssueType[" + i + "]"];
                        var InFlow_data = Convert.ToDecimal(Request.Form["InFlow[" + i + "]"]);
                        var OutFLow_data = UC.CheckNegativeValue(Convert.ToDecimal(Request.Form["OutFLow[" + i + "]"]));
                        var Note_data = Request.Form["Note[" + i + "]"];

                        BlotterFR.Add(new SBP_BlotterFundingRepo
                        {
                            DataType = DataType_data,
                            Bank = Bank_data,
                            Days = Days_data,
                            Rate = Rate_data,
                            DealType = DealType_data,
                            Broker = Broker_data,
                            Issue_Date = Convert.ToDateTime(Issue_Date_data.ToString()),
                            IssueType = IssueType_data,
                            InFlow = InFlow_data,
                            OutFLow = OutFLow_data,
                            Note = Note_data,
                            UserID = Convert.ToInt16(Session["UserID"].ToString()),
                            BID = Convert.ToInt16(Session["BranchID"].ToString()),
                            BR = Convert.ToInt16(Session["BR"].ToString()),
                            CurID = Convert.ToInt16(Session["SelectedCurrency"].ToString()),
                            CreateDate = DateTime.Now
                        });
                    }
                }

                if (ModelState.IsValid)
                {
                    ServiceRepository serviceObj = new ServiceRepository();
                    HttpResponseMessage response = serviceObj.PostResponse("api/BlotterFundingRepo/InsertFundingRepo", BlotterFR);
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
                        UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(BlotterFR), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
                        return RedirectToAction("BlotterFundingRepo");
                    }
                }
            }
            catch (Exception) { }

            return PartialView("_Create");
        }

        [HttpPost]
        public ActionResult Delete(IEnumerable<int> employeeIdsToDelete)
        {
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.PostResponse("api/BlotterFundingRepo/DeleteFundingRepo", employeeIdsToDelete);
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
            UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(employeeIdsToDelete), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
            return RedirectToAction("BlotterFundingRepo");
        }


    }
}