using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Mvc;
using WebBlotter.Classes;
using WebBlotter.Models;
using WebBlotter.Repository;
using Newtonsoft.Json.Linq;
using System.Web.Script.Serialization;
using System.Linq;
using System.Web;
using OfficeOpenXml;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using System.Configuration;

namespace WebBlotter.Controllers
{
    [AuthAccess]
    public class BlotterTradeController : Controller
    {

        UtilityClass UC = new UtilityClass();
        // GET: BlotterTrade
        private List<SP_GetNostroBankFromOPICS_Result> GetAllNostroBanks()
        {
            try
            {
                var currid = (dynamic)null;
                if (Session["SelectedCurrency"] != null)
                {
                    currid = Convert.ToInt32(Session["SelectedCurrency"].ToString());
                }
                ServiceRepository serviceObj = new ServiceRepository();
                HttpResponseMessage response = serviceObj.GetResponse("/api/NostroBank/GetNostroBankFromOpicsDDL?currId=" + currid + "&BRCode=" + Session["BR"].ToString());
                response.EnsureSuccessStatusCode();
                List<SP_GetNostroBankFromOPICS_Result> blotterNB = new List<SP_GetNostroBankFromOPICS_Result>();

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = response.Content.ReadAsStringAsync().Result;
                    var JsonLinq = JObject.Parse(jsonResponse);
                    WebApiResponse getreponse = new WebApiResponse
                    {
                        Status = Convert.ToBoolean(JsonLinq["Status"]),
                        Message = JsonLinq["Message"].ToString(),
                        Data = JsonLinq["Data"].ToString()
                    };
                    if (getreponse.Message == "Success")
                    {
                        JavaScriptSerializer ser = new JavaScriptSerializer();
                        Dictionary<string, dynamic> ResponseDD = ser.Deserialize<Dictionary<string, dynamic>>(JsonLinq.ToString());
                        blotterNB = JsonConvert.DeserializeObject<List<SP_GetNostroBankFromOPICS_Result>>(ResponseDD["Data"]);
                        ViewBag.NostroBanksDDL = blotterNB;
                    }
                }
                return blotterNB;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private List<SP_GETAllTransactionTitles_Result> GetAllTradeTransactionTitles()
        {
            try
            {
                ServiceRepository serviceObj = new ServiceRepository();
                HttpResponseMessage response = serviceObj.GetResponse("/api/BlotterTrade/GetAllTradeTransactionTitles");
                response.EnsureSuccessStatusCode();
                //List<Models.SP_GETAllTransactionTitles_Result> blotterTTT = response.Content.ReadAsAsync<List<Models.SP_GETAllTransactionTitles_Result>>().Result;
                List<SP_GETAllTransactionTitles_Result> blotterTTT = new List<SP_GETAllTransactionTitles_Result>();

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = response.Content.ReadAsStringAsync().Result;
                    var JsonLinq = JObject.Parse(jsonResponse);
                    WebApiResponse getreponse = new WebApiResponse
                    {
                        Status = Convert.ToBoolean(JsonLinq["Status"]),
                        Message = JsonLinq["Message"].ToString(),
                        Data = JsonLinq["Data"].ToString()
                    };
                    if (getreponse.Message == "Success")
                    {
                        JavaScriptSerializer ser = new JavaScriptSerializer();
                        Dictionary<string, dynamic> ResponseDD = ser.Deserialize<Dictionary<string, dynamic>>(JsonLinq.ToString());
                        blotterTTT = JsonConvert.DeserializeObject<List<SP_GETAllTransactionTitles_Result>>(ResponseDD["Data"]);
                    }
                }
                return blotterTTT;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult BlotterTrade(FormCollection form)
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
                HttpResponseMessage response = serviceObj.GetResponse("/api/BlotterTrade/GetAllBlotterTrade?UserID=" + Session["UserID"].ToString() + "&BranchID=" + Session["BranchID"].ToString() + "&CurID=" + Session["SelectedCurrency"].ToString() + "&BR=" + Session["BR"].ToString() + "&DateVal=" + DateVal);
                response.EnsureSuccessStatusCode();
                // List<Models.SP_GetAll_SBPBlotterTrade_Result> blotterTrade = response.Content.ReadAsAsync<List<Models.SP_GetAll_SBPBlotterTrade_Result>>().Result;

                List<SP_GetAll_SBPBlotterTrade_Result> blotterTrade = new List<SP_GetAll_SBPBlotterTrade_Result>();

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = response.Content.ReadAsStringAsync().Result;
                    var JsonLinq = JObject.Parse(jsonResponse);
                    WebApiResponse getreponse = new WebApiResponse
                    {
                        Status = Convert.ToBoolean(JsonLinq["Status"]),
                        Message = JsonLinq["Message"].ToString(),
                        Data = JsonLinq["Data"].ToString()
                    };
                    if (getreponse.Status)
                    {
                        JavaScriptSerializer ser = new JavaScriptSerializer();
                        Dictionary<string, dynamic> ResponseDD = ser.Deserialize<Dictionary<string, dynamic>>(JsonLinq.ToString());
                        blotterTrade = JsonConvert.DeserializeObject<List<SP_GetAll_SBPBlotterTrade_Result>>(ResponseDD["Data"]);
                    }
                    else
                        TempData["DataStatus"] = "Data not available";
                }
                var PAccess = Session["CurrentPagesAccess"].ToString().Split('~');
                UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(blotterTrade), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());

                ViewData["isDateChangable"] = Convert.ToBoolean(PAccess[2]);
                ViewData["isEditable"] = Convert.ToBoolean(PAccess[3]);
                ViewData["IsDeletable"] = Convert.ToBoolean(PAccess[4]);
                ViewBag.Title = "All Blotter Setup";
                return PartialView("_BlotterTrade", blotterTrade);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        [HttpGet]
        public ActionResult Create()
        {
            string ActiveAction = RouteData.Values["action"].ToString();
            string ActiveController = RouteData.Values["controller"].ToString();
            Session["ActiveAction"] = ActiveController;
            Session["ActiveController"] = ActiveAction;

            UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), "", this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
            SBP_BlotterTrade model = new SBP_BlotterTrade();
            try
            {
                if (ModelState.IsValid)
                {
                    model.Trade_Date = DateTime.Now.Date;
                    //ViewData["SysCurrentDt"] = GetCurrentDT().ToString("dd-MMM-yyyy");
                    // ViewData["BrCode"] = BrCode;
                    ViewBag.NostroBanksDDL = GetAllNostroBanks();
                    ViewBag.TradeTransactionTitles = GetAllTradeTransactionTitles();
                }
            }
            catch (Exception) { }
            return PartialView("_Create", model);

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

            SBP_BlotterTrade model = new SBP_BlotterTrade();
            try
            {
                UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), "", this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
                if (ModelState.IsValid)
                {
                    model.Trade_Date = DateTime.Now.Date;
                    ViewBag.NostroBanksDDL = GetAllNostroBanks();
                    ViewBag.TradeTransactionTitles = GetAllTradeTransactionTitles();
                }
            }
            catch (Exception ex) { }

            return PartialView("_Create", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _Create(SBP_BlotterTrade BlotterTrade, FormCollection form)
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
                    BlotterTrade.Trade_OutFLow = UC.CheckNegativeValue(BlotterTrade.Trade_OutFLow);
                    BlotterTrade.UserID = Convert.ToInt16(Session["UserID"].ToString());
                    BlotterTrade.BID = Convert.ToInt16(Session["BranchID"].ToString());
                    BlotterTrade.BR = Convert.ToInt16(Session["BR"].ToString());
                    BlotterTrade.CurID = Convert.ToInt16(Session["SelectedCurrency"].ToString());
                    BlotterTrade.BankCode = form["BankCode"].ToString();
                    BlotterTrade.CreateDate = DateTime.Now;
                    ServiceRepository serviceObj = new ServiceRepository();
                    HttpResponseMessage response = serviceObj.PostResponse("api/BlotterTrade/InsertTrade", BlotterTrade);
                    response.EnsureSuccessStatusCode();

                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResponse = response.Content.ReadAsStringAsync().Result;
                        var JsonLinq = JObject.Parse(jsonResponse);
                        WebApiResponse getreponse = new WebApiResponse
                        {
                            Status = Convert.ToBoolean(JsonLinq["Status"]),
                            Message = JsonLinq["Message"].ToString(),
                            Data = JsonLinq["Data"].ToString()
                        };
                        if (getreponse.Status == true)
                            TempData["DataStatus"] = getreponse.Message;
                        else
                            TempData["DataStatus"] = getreponse.Message;
                    }
                    UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(BlotterTrade), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
                    return RedirectToAction("BlotterTrade");
                }
                else
                {
                    ViewBag.NostroBanksDDL = GetAllNostroBanks();
                    ViewBag.TradeTransactionTitles = GetAllTradeTransactionTitles();
                }
            }
            catch (Exception ex) { }
            return PartialView("_Create", BlotterTrade);
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
            HttpResponseMessage response = serviceObj.GetResponse("/api/BlotterTrade/GetBlotterTrade?id=" + id.ToString());
            response.EnsureSuccessStatusCode();
            //Models.SBP_BlotterTrade BlotterTrade = response.Content.ReadAsAsync<Models.SBP_BlotterTrade>().Result;
            Models.SBP_BlotterTrade BlotterTrade = new Models.SBP_BlotterTrade();
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
                    BlotterTrade = JsonConvert.DeserializeObject<Models.SBP_BlotterTrade>(ResponseDD["Data"]);
                }
                else
                    TempData["DataStatus"] = getreponse.Message;
            }

            UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(BlotterTrade), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
            var isDateChangable = Convert.ToBoolean(Session["CurrentPagesAccess"].ToString().Split('~')[2]);
            ViewData["isDateChangable"] = isDateChangable;
            ViewBag.BankCode = BlotterTrade.BankCode;
            ViewBag.NostroBanksDDL = GetAllNostroBanks();
            ViewBag.TradeTransactionTitles = GetAllTradeTransactionTitles();
            return PartialView("_Edit", BlotterTrade);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(SBP_BlotterTrade BlotterTrade, FormCollection form)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    BlotterTrade.Trade_OutFLow = UC.CheckNegativeValue(BlotterTrade.Trade_OutFLow);
                    if (BlotterTrade.Trade_Date == null)
                        BlotterTrade.Trade_Date = DateTime.Now;
                    BlotterTrade.CurID = Convert.ToInt16(Session["SelectedCurrency"].ToString());
                    BlotterTrade.UpdateDate = DateTime.Now;
                    BlotterTrade.BankCode = form["BankCode"].ToString();
                    UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(BlotterTrade), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
                    ServiceRepository serviceObj = new ServiceRepository();
                    HttpResponseMessage response = serviceObj.PutResponse("api/BlotterTrade/UpdateTrade", BlotterTrade);
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

                    //ViewData["SysCurrentDt"] = GetCurrentDT().ToString("dd-MMM-yyyy");
                    //ViewData["BrCode"] = BrCode;
                    return RedirectToAction("BlotterTrade");
                }
                else
                {
                    ViewBag.NostroBanksDDL = GetAllNostroBanks();
                    ViewBag.TradeTransactionTitles = GetAllTradeTransactionTitles();
                }
            }
            catch (Exception ex) { }
            return PartialView("_Edit", BlotterTrade);
        }

        public ActionResult Delete(int id)
        {
            try
            {
                UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(id), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
                ServiceRepository serviceObj = new ServiceRepository();
                HttpResponseMessage response = serviceObj.DeleteResponse("api/BlotterTrade/DeleteTrade?id=" + id.ToString());
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = response.Content.ReadAsStringAsync().Result;
                    var JsonLinq = JObject.Parse(jsonResponse);
                    WebApiResponse getreponse = new WebApiResponse
                    {
                        Status = Convert.ToBoolean(JsonLinq["Status"]),
                        Message = JsonLinq["Message"].ToString(),
                        Data = JsonLinq["Data"].ToString()
                    };

                    if (getreponse.Status)
                    {
                        TempData["DataStatus"] = getreponse.Message;
                    }
                    else
                        TempData["DataStatus"] = getreponse.Message;
                }
            }
            catch (Exception ex) { }

            return RedirectToAction("BlotterTrade");
        }


        public ActionResult UploadExcel(HttpPostedFileBase postedFile)
        {
            try
            {
                if (postedFile != null)
                {
                    ViewBag.NostroBanksDDL = GetAllNostroBanks();
                    ViewBag.TradeTransactionTitles = GetAllTradeTransactionTitles();
                    HttpPostedFileBase file = Request.Files["postedFile"];
                    string Remarks = string.Empty;

                    if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                    {
                        string fileContentType = file.ContentType;
                        byte[] fileBytes = new byte[file.ContentLength];
                        string str = Path.GetExtension(file.FileName);

                        FileInfo fi = new FileInfo(file.FileName);
                        string justFileName = fi.Name.Substring(0, file.FileName.Length - str.Length);
                        string NewFileName = justFileName + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
                        string filePath = Server.MapPath(ConfigurationManager.AppSettings["ExcelFile"]);

                        ExcelPackage.LicenseContext = LicenseContext.Commercial;
                        FileInfo newfile = new FileInfo(filePath + NewFileName);
                        using (ExcelPackage excel = new ExcelPackage(file.InputStream))
                        {
                            excel.SaveAs(newfile);
                        }

                        if (str == ".xlsx")
                        {
                            using (ExcelPackage package = new ExcelPackage(newfile))
                            {
                                ExcelWorksheets currentSheet = package.Workbook.Worksheets;
                                ExcelWorksheet workSheet = currentSheet.First();

                                int noOfCol = workSheet.Dimension.End.Column;
                                int noOfRow = workSheet.Dimension.End.Row;
                                SP_GetAll_SBPBlotterTrade_Result blotterTradeNewItem = new SP_GetAll_SBPBlotterTrade_Result();
                                ServiceRepository serviceObj = new ServiceRepository();
                                for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                                {
                                    SBP_BlotterTrade trade = new SBP_BlotterTrade();
                                    if (workSheet.Cells[rowIterator, 1].Value == null || Convert.ToInt64(workSheet.Cells[rowIterator, 1].Value) == 0)
                                    {
                                        trade.SNo = 0;
                                    }
                                    else
                                    {
                                        trade.SNo = Convert.ToInt64(workSheet.Cells[rowIterator, 1].Value);
                                    }

                                    if (workSheet.Cells[rowIterator, 7].Value.ToString() == "N")
                                    {
                                        trade.Flag = "N";
                                    }
                                    else if (workSheet.Cells[rowIterator, 7].Value.ToString() == "U")
                                    {
                                        trade.Flag = "U";
                                    }
                                    else if (workSheet.Cells[rowIterator, 7].Value.ToString() == "D")
                                    {
                                        trade.Flag = "D";
                                    }
                                    try
                                    {
                                        foreach (dynamic item in ViewBag.NostroBanksDDL)
                                        {
                                            if (item.BankName.Trim() == workSheet.Cells[rowIterator, 2].Value.ToString())
                                            {
                                                trade.BankCode = item.BankCode;
                                            }
                                        }
                                        foreach (dynamic item in ViewBag.TradeTransactionTitles)
                                        {
                                            if (item.TranctionTitle.Trim() == workSheet.Cells[rowIterator, 3].Value.ToString())
                                            {
                                                trade.TTID = item.TTID;
                                            }
                                        }

                                        trade.Trade_Date = Convert.ToDateTime(workSheet.Cells[rowIterator, 4].Value);
                                        string inflow = workSheet.Cells[rowIterator, 5].Value.ToString();
                                        trade.Trade_InFlow = inflow.Contains(",") ? Convert.ToDecimal(inflow.Replace(",", "")) : Convert.ToDecimal(workSheet.Cells[rowIterator, 5].Value);
                                        string OutFLow = workSheet.Cells[rowIterator, 6].Value.ToString();
                                        trade.Trade_OutFLow = OutFLow.Contains(",") ? Convert.ToDecimal(OutFLow.Replace(",", "")) : Convert.ToDecimal(workSheet.Cells[rowIterator, 6].Value);

                                        trade.UserID = Convert.ToInt16(Session["UserID"].ToString());
                                        trade.BID = Convert.ToInt16(Session["BranchID"].ToString());
                                        trade.BR = Convert.ToInt16(Session["BR"].ToString());
                                        trade.CurID = Convert.ToInt16(Session["SelectedCurrency"].ToString());
                                        trade.CreateDate = DateTime.Now;
                                        HttpResponseMessage response = serviceObj.PostResponse("api/BlotterTrade/BulkInsertTrade", trade);
                                        response.EnsureSuccessStatusCode();
                                        if (response.IsSuccessStatusCode)
                                        {
                                            string jsonResponse = response.Content.ReadAsStringAsync().Result;
                                            JObject JsonLinq = JObject.Parse(jsonResponse);
                                            WebApiResponse getreponse = new WebApiResponse
                                            {
                                                Status = Convert.ToBoolean(JsonLinq["Status"]),
                                                Message = JsonLinq["Message"].ToString(),
                                                Data = JsonLinq["Data"].ToString()
                                            };
                                            if (getreponse.Status)
                                            {
                                                JavaScriptSerializer ser = new JavaScriptSerializer();
                                                blotterTradeNewItem = JsonConvert.DeserializeObject<SP_GetAll_SBPBlotterTrade_Result>(getreponse.Data);
                                                if (blotterTradeNewItem.Flag == "N" && (trade.SNo == null || trade.SNo == 0))
                                                {
                                                    Remarks = "Data inserted";
                                                }
                                                else if ((blotterTradeNewItem.Flag == "U" || blotterTradeNewItem.Flag == "N") && blotterTradeNewItem.SNo > 0)
                                                {
                                                    if (blotterTradeNewItem.Flag == "U" && blotterTradeNewItem.SNo > 0)
                                                    {
                                                        Remarks = "Data Updated";
                                                    }
                                                    else
                                                    {
                                                        Remarks = "Data Inserted";
                                                    }
                                                }                                               
                                                else if (blotterTradeNewItem.Flag == "D")
                                                {
                                                    Remarks = "Data Deleted";
                                                }
                                                workSheet.Cells[rowIterator, 1].Value = blotterTradeNewItem.SNo;
                                                workSheet.Cells[rowIterator, 8].Value = Remarks;
                                                package.Save();
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Remarks = ex.Message;
                                        workSheet.Cells[rowIterator, 1].Value = blotterTradeNewItem.SNo;
                                        workSheet.Cells[rowIterator, 8].Value = Remarks;
                                        package.Save();
                                    }
                                }
                                //End Loop
                                workSheet.Cells["A:AZ"].AutoFitColumns();
                                Response.Clear();
                                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                                Response.AddHeader("content-disposition", "attachment: filename=" + newfile);
                                Response.BinaryWrite(package.GetAsByteArray());
                                Response.End();
                            }
                            // UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(tradeList), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
                        }
                    }
                }
                return RedirectToAction("BlotterTrade");
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}