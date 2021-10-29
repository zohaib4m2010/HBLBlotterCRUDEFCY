using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
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
    public class BlotterRECONController : Controller
    {

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

        private SP_Get_SBPBlotterConversionRate_Result GetConversionRate()
        {
            try
            {
                var currid = (dynamic)null;
                if (Session["SelectedCurrency"] != null)
                {
                    currid = Convert.ToInt32(Session["SelectedCurrency"].ToString());
                }
                ServiceRepository serviceObj = new ServiceRepository();
                HttpResponseMessage response = serviceObj.GetResponse("/api/BlotterRECON/GetConversionRateRECON?currID=" + currid + "&BR=" + Session["BR"].ToString());
                response.EnsureSuccessStatusCode();
                Models.SP_Get_SBPBlotterConversionRate_Result blotterCR = new Models.SP_Get_SBPBlotterConversionRate_Result();

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
                        blotterCR = JsonConvert.DeserializeObject<Models.SP_Get_SBPBlotterConversionRate_Result>(ResponseDD["Data"]);
                        if (blotterCR != null)
                        {
                            ViewBag.ConversionRate = blotterCR;
                        }
                        else
                        {
                            blotterCR.CurrencyID = 0;
                            blotterCR.SPOTRATE_8 = 0;
                            blotterCR.USDRate = 0;
                            ViewBag.ConversionRate = blotterCR;
                        }
                    }
                    else
                    {
                        ViewBag.ConversionRate = blotterCR;
                    }
                }
                return blotterCR;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public ActionResult BlotterRECON(FormCollection form)
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
            HttpResponseMessage response = serviceObj.GetResponse("/api/BlotterRECON/GetAllBlotterRECON?UserID=" + Session["UserID"].ToString() + "&BranchID=" + Session["BranchID"].ToString() + "&CurID=" + selectCurrency + "&BR=" + Session["BR"].ToString() + "&DateVal=" + DateVal);
            response.EnsureSuccessStatusCode();
            //List<Models.SP_GetSBPBlotterRECON_Result> BlotterRECON = response.Content.ReadAsAsync<List<Models.SP_GetSBPBlotterRECON_Result>>().Result;
            List<Models.SP_GetSBPBlotterRECON_Result> BlotterRECON = new List<Models.SP_GetSBPBlotterRECON_Result>();

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
                    BlotterRECON = JsonConvert.DeserializeObject<List<Models.SP_GetSBPBlotterRECON_Result>>(ResponseDD["Data"]);
                }
                else
                    TempData["DataStatus"] = "Data not available";
            }

            ViewBag.Title = "All Blotter Setup";
            var PAccess = Session["CurrentPagesAccess"].ToString().Split('~');
            UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(BlotterRECON), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());

            ViewData["isDateChangable"] = Convert.ToBoolean(PAccess[2]);
            ViewData["isEditable"] = Convert.ToBoolean(PAccess[3]);
            ViewData["IsDeletable"] = Convert.ToBoolean(PAccess[4]);
            return PartialView("_BlotterRECON", BlotterRECON);
        }

        [HttpGet]
        public ActionResult Create()
        {
            SBP_BlotterRECON model = new SBP_BlotterRECON();
            try
            {
                UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), "", this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());

                var ActiveAction = RouteData.Values["action"].ToString();
                var ActiveController = RouteData.Values["controller"].ToString();
                Session["ActiveAction"] = ActiveController;
                Session["ActiveController"] = ActiveAction;

                if (ModelState.IsValid)
                {
                    // model.CreateDate = DateTime.Now.Date;
                    ViewBag.RECONNostroBanks = GetAllNostroBanks();
                    GetConversionRate();
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

                ViewBag.RECONNostroBanks = GetAllNostroBanks();
                GetConversionRate();
                return PartialView("_Create");
            }
            catch (Exception ex) { }

            return PartialView("_Create");
        }

        // POST: BlotterRECON/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _Create(SBP_BlotterRECON BlotterRECON, FormCollection form)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    BlotterRECON.UserID = Convert.ToInt16(Session["UserID"].ToString());
                    BlotterRECON.BID = Convert.ToInt16(Session["BranchID"].ToString());
                    BlotterRECON.BR = Convert.ToInt16(Session["BR"].ToString());
                    BlotterRECON.CurID = Convert.ToInt16(Session["SelectedCurrency"].ToString());
                    BlotterRECON.CreateDate = DateTime.Now;
                    BlotterRECON.BankCode = form["BankCode"].ToString();
                    ServiceRepository serviceObj = new ServiceRepository();
                    HttpResponseMessage response = serviceObj.PostResponse("api/BlotterRECON/InsertRECON", BlotterRECON);
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
                    UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(BlotterRECON), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
                    return RedirectToAction("BlotterRECON");
                }
                else
                {
                    ViewBag.RECONNostroBanks = GetAllNostroBanks();
                    GetConversionRate();
                }
            }
            catch (Exception ex) { }

            return RedirectToAction("BlotterRECON", BlotterRECON);
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
            HttpResponseMessage response = serviceObj.GetResponse("/api/BlotterRECON/GetBlotterRECON?id=" + id.ToString());
            response.EnsureSuccessStatusCode();
            //Models.SBP_BlotterRECON BlotterRECON = response.Content.ReadAsAsync<Models.SBP_BlotterRECON>().Result;
            Models.SBP_BlotterRECON BlotterRECON = new Models.SBP_BlotterRECON();
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
                    BlotterRECON = JsonConvert.DeserializeObject<Models.SBP_BlotterRECON>(ResponseDD["Data"]);
                }
                else
                    TempData["DataStatus"] = getreponse.Message;
            }
            UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(BlotterRECON), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
            ViewBag.BankCode = BlotterRECON.BankCode;
            ViewBag.RECONNostroBanks = GetAllNostroBanks();
            GetConversionRate();
            return PartialView("_Edit", BlotterRECON);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(SBP_BlotterRECON BlotterRECON, FormCollection form)
        {
            BlotterRECON.UserID = Convert.ToInt16(Session["UserID"].ToString());
            BlotterRECON.BID = Convert.ToInt16(Session["BranchID"].ToString());
            BlotterRECON.BR = Convert.ToInt16(Session["BR"].ToString());
            BlotterRECON.CurID = Convert.ToInt16(Session["SelectedCurrency"].ToString());
            BlotterRECON.UpdateDate = DateTime.Now;
            BlotterRECON.BankCode = form["BankCode"].ToString();
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.PutResponse("api/BlotterRECON/UpdateRECON", BlotterRECON);
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
            UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(BlotterRECON), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
            return RedirectToAction("BlotterRECON");
        }

        public ActionResult Delete(int id)
        {
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.DeleteResponse("api/BlotterRECON/DeleteRECON?id=" + id.ToString());
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
            return RedirectToAction("BlotterRECON");
        }


        //public ActionResult UploadExcel(HttpPostedFileBase postedFile)
        //{
        //    List<SBP_BlotterRECON> reconList = new List<SBP_BlotterRECON>();
        //    if (Request != null)
        //    {
        //        HttpPostedFileBase file = Request.Files["postedFile"];
        //        if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
        //        {
        //            string fileName = file.FileName;
        //            string fileContentType = file.ContentType;
        //            byte[] fileBytes = new byte[file.ContentLength];
        //            var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));
        //            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.Commercial;
        //            using (var package = new ExcelPackage(file.InputStream))
        //            {
        //                var currentSheet = package.Workbook.Worksheets;
        //                var workSheet = currentSheet.First();
        //                var noOfCol = workSheet.Dimension.End.Column;
        //                var noOfRow = workSheet.Dimension.End.Row;
        //                for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
        //                {
        //                    var recon = new SBP_BlotterRECON();

        //                    recon.BankCode = workSheet.Cells[rowIterator, 1].Value.ToString();
        //                    recon.LastStatementDate = Convert.ToDateTime(workSheet.Cells[rowIterator, 2].Value);
        //                    recon.OurBooks = Convert.ToDecimal(workSheet.Cells[rowIterator, 3].Value);
        //                    recon.TheirBooks = Convert.ToDecimal(workSheet.Cells[rowIterator, 4].Value);
        //                    recon.ConversionRate = Convert.ToDecimal(workSheet.Cells[rowIterator, 5].Value);
        //                    recon.EquivalentUSD = Convert.ToDecimal(workSheet.Cells[rowIterator, 6].Value);
        //                    recon.LimitAvailable = Convert.ToDecimal(workSheet.Cells[rowIterator, 7].Value);
        //                    recon.EstimatedOpenBal = 0;
        //                    recon.UserID = Convert.ToInt16(Session["UserID"].ToString());
        //                    recon.BID = Convert.ToInt16(Session["BranchID"].ToString());
        //                    recon.BR = Convert.ToInt16(Session["BR"].ToString());
        //                    recon.CurID = Convert.ToInt16(Session["SelectedCurrency"].ToString());
        //                    recon.CreateDate = DateTime.Now;
        //                    reconList.Add(recon);
        //                }
        //                ServiceRepository serviceObj = new ServiceRepository();
        //                HttpResponseMessage response = serviceObj.PostResponse("api/BlotterRECON/BulkInsertRECON", reconList);
        //                response.EnsureSuccessStatusCode();

        //                if (response.IsSuccessStatusCode)
        //                {
        //                    string jsonResponse = response.Content.ReadAsStringAsync().Result;
        //                    var JsonLinq = JObject.Parse(jsonResponse);
        //                    WebApiResponse getreponse = new WebApiResponse();
        //                    getreponse.Status = Convert.ToBoolean(JsonLinq["Status"]);
        //                    getreponse.Message = JsonLinq["Message"].ToString();
        //                    getreponse.Data = JsonLinq["Data"].ToString();
        //                    if (getreponse.Status == true)
        //                        TempData["DataStatus"] = getreponse.Message;
        //                    else
        //                        TempData["DataStatus"] = getreponse.Message;
        //                }
        //                // UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(reconList), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());


        //            }
        //        }
        //    }

        //    return RedirectToAction("BlotterRECON");
        //}

        public ActionResult UploadExcel(HttpPostedFileBase postedFile)
        {
            try
            {
                if (postedFile != null)
                {
                    ViewBag.NostroBanksDDL = GetAllNostroBanks();
                    GetConversionRate();

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
                                SP_GetSBPBlotterRECON_Result blotterRECONNewItem = new SP_GetSBPBlotterRECON_Result();
                                ServiceRepository serviceObj = new ServiceRepository();
                                for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                                {
                                    SBP_BlotterRECON recon = new SBP_BlotterRECON();
                                    if (workSheet.Cells[rowIterator, 1].Value == null || Convert.ToInt64(workSheet.Cells[rowIterator, 1].Value) == 0)
                                    {
                                        recon.ID = 0;
                                    }
                                    else
                                    {
                                        recon.ID = Convert.ToInt64(workSheet.Cells[rowIterator, 1].Value);
                                    }

                                    if (workSheet.Cells[rowIterator, 7].Value.ToString() == "N")
                                    {
                                        recon.Flag = "N";
                                    }
                                    else if (workSheet.Cells[rowIterator, 7].Value.ToString() == "U")
                                    {
                                        recon.Flag = "U";
                                    }
                                    else if (workSheet.Cells[rowIterator, 7].Value.ToString() == "D")
                                    {
                                        recon.Flag = "D";
                                    }
                                    try
                                    {
                                        foreach (dynamic item in ViewBag.NostroBanksDDL)
                                        {
                                            if (item.BankName.Trim() == workSheet.Cells[rowIterator, 2].Value.ToString())
                                            {
                                                recon.BankCode = item.BankCode;
                                            }
                                        }
                                        recon.LastStatementDate = Convert.ToDateTime(workSheet.Cells[rowIterator, 3].Value);
                                        string ourbooks = workSheet.Cells[rowIterator, 4].Value.ToString();
                                        recon.OurBooks = ourbooks.Contains(",") ? Convert.ToDecimal(ourbooks.Replace(",", "")) : Convert.ToDecimal(workSheet.Cells[rowIterator, 4].Value);
                                        string theirbooks = workSheet.Cells[rowIterator, 5].Value.ToString();
                                        recon.TheirBooks = theirbooks.Contains(",") ? Convert.ToDecimal(theirbooks.Replace(",", "")) : Convert.ToDecimal(workSheet.Cells[rowIterator, 5].Value);
                                        string limitavailable = workSheet.Cells[rowIterator, 6].Value.ToString();
                                        if (!string.IsNullOrEmpty(limitavailable))
                                        {
                                            recon.LimitAvailable = limitavailable.Contains(",") ? Convert.ToDecimal(limitavailable.Replace(",", "")) : Convert.ToDecimal(workSheet.Cells[rowIterator, 6].Value);
                                        }
                                        else
                                        {
                                            recon.LimitAvailable = 0;
                                        }
                                        //---------------------------USD Conversion Rate-------------------------\\
                                        if (recon.TheirBooks > 0)
                                        {
                                            decimal SPOTRATE_8Val = Convert.ToDecimal(ViewBag.ConversionRate.SPOTRATE_8);
                                            decimal USDRateVal = Convert.ToDecimal(ViewBag.ConversionRate.USDRate);
                                            int CurrencyID = ViewBag.ConversionRate.CurrencyID;
                                            if (CurrencyID == 14)
                                            {
                                                recon.EquivalentUSD = (Convert.ToDecimal(recon.TheirBooks) / Convert.ToDecimal(SPOTRATE_8Val)) * Convert.ToDecimal(USDRateVal);
                                            }
                                            else
                                            {
                                                recon.EquivalentUSD = (SPOTRATE_8Val * Convert.ToDecimal(recon.TheirBooks)) / USDRateVal;
                                            }
                                            decimal ConversionRateVal = (decimal)(recon.TheirBooks / recon.EquivalentUSD);
                                            recon.ConversionRate = ConversionRateVal;
                                        }
                                        else
                                        {
                                            recon.EquivalentUSD = 0;
                                            recon.ConversionRate = 0;
                                        }
                                        //-------------------------------------------------------------------------------\\
                                        recon.UserID = Convert.ToInt16(Session["UserID"].ToString());
                                        recon.BID = Convert.ToInt16(Session["BranchID"].ToString());
                                        recon.BR = Convert.ToInt16(Session["BR"].ToString());
                                        recon.CurID = Convert.ToInt16(Session["SelectedCurrency"].ToString());
                                        recon.CreateDate = DateTime.Now;
                                        HttpResponseMessage response = serviceObj.PostResponse("api/BlotterRECON/BulkInsertRECON", recon);
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
                                                blotterRECONNewItem = JsonConvert.DeserializeObject<SP_GetSBPBlotterRECON_Result>(getreponse.Data);
                                                if (blotterRECONNewItem.Flag == "N")
                                                {
                                                    Remarks = "Data inserted";
                                                }
                                                else if (blotterRECONNewItem.Flag == "U")
                                                {
                                                    Remarks = "Data Updated";
                                                }
                                                else if (blotterRECONNewItem.Flag == "D")
                                                {
                                                    Remarks = "Data Deleted";
                                                }
                                                workSheet.Cells[rowIterator, 1].Value = blotterRECONNewItem.ID;
                                                workSheet.Cells[rowIterator, 8].Value = Remarks;
                                                package.Save();
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Remarks = ex.Message;
                                        workSheet.Cells[rowIterator, 1].Value = blotterRECONNewItem.ID;
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
                return RedirectToAction("BlotterRECON");
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}