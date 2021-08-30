using WebBlotter.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Http;
using WebBlotter.ViewModel;
using System.Net.Mail;
using System.Configuration;
using WebBlotter.Classes;
using Newtonsoft.Json.Linq;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using WebBlotter.Models;

namespace WebBlotter.Controllers
{
    [AuthAccess]
    public class BlotterController : Controller
    {
        // GET: Blotter
        // GET: Product  
        Boolean SendE = false;
        //string BrCode = ConfigurationManager.AppSettings["BranchCode"].ToString();

        private List<Models.SP_GetNostroBankFromOPICS_Result> GetAllNostroBanks()
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
        public ActionResult GetAllBlotter(FormCollection form)
        {
            try
            {
                #region Added by shakir (Currency parameter)

                var selectCurrency = (dynamic)null;
                if (form["selectCurrency"] != null)
                    selectCurrency = Convert.ToInt32(form["selectCurrency"].ToString());
                else
                    selectCurrency = Convert.ToInt32(Session["SelectedCurrency"].ToString());

                var BlotterCurrentDate = (dynamic)null;
                string bankname = string.Empty;
                if (form["BlotterCurrentDate"] != null)
                {
                    BlotterCurrentDate = form["BlotterCurrentDate"].ToString();
                    ViewBag.CurrentDt = BlotterCurrentDate;
                }
                else
                {
                    ViewBag.CurrentDt = DateTime.Now.ToString("yyyy-MM-dd");
                    BlotterCurrentDate = ViewBag.CurrentDt;
                }
                if (form["Nostro_Bank"] != null)
                {
                    bankname = form["Nostro_Bank"].ToString();
                    ViewBag.BankCode = bankname;
                }


                UtilityClass.GetSelectedCurrecy(selectCurrency);
                #endregion

                ViewBag.NostroBanksDDL = GetAllNostroBanks();
                BlotterMultiModel blotterMulit = new BlotterMultiModel();
                ServiceRepositoryBlotter serviceObj = new ServiceRepositoryBlotter();
                if (Convert.ToInt32(selectCurrency) == 1)
                {
                    HttpResponseMessage response = serviceObj.GetResponse("/api/Blotter/GetAllBlotterList?brcode=" + Session["BR"].ToString() + "&DataType=SBP" + "&CurrentDate=" + BlotterCurrentDate);
                    response.EnsureSuccessStatusCode();
                    //List<Models.SP_SBPBlotter_Result> blotter = response.Content.ReadAsAsync<List<Models.SP_SBPBlotter_Result>>().Result;
                    // List<blotterMulit.GetAllBlotter01> blotter = response.Content.ReadAsAsync<List<Models.SP_SBPBlotter_Result>>().Result;
                    List<Models.SP_SBPBlotter_Result> blotter = new List<Models.SP_SBPBlotter_Result>();

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
                            blotter = JsonConvert.DeserializeObject<List<Models.SP_SBPBlotter_Result>>(ResponseDD["Data"]);
                        }
                        else
                            TempData["DataStatus"] = getreponse.Message;
                    }
                    blotterMulit.GetAllBlotter01 = blotter;
                }
                else
                {
                    HttpResponseMessage response = serviceObj.GetResponse("/api/Blotter/GetAllBlotterFCYList?brcode=" + Session["BR"].ToString() + "&CurrId=" + selectCurrency + "&CurrentDate=" + BlotterCurrentDate + "&NostroBank=" + bankname);
                    response.EnsureSuccessStatusCode();
                    //List<Models.SP_SBPBlotter_FCY_Result> blotter = response.Content.ReadAsAsync<List<Models.SP_SBPBlotter_FCY_Result>>().Result;
                    List<Models.SP_SBPBlotter_FCY_Result> blotter = new List<Models.SP_SBPBlotter_FCY_Result>();
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
                            blotter = JsonConvert.DeserializeObject<List<Models.SP_SBPBlotter_FCY_Result>>(ResponseDD["Data"]);
                        }
                        else
                            TempData["DataStatus"] = getreponse.Message;
                    }
                    blotterMulit.GetAllBlotterFCY01 = blotter;
                }
                HttpResponseMessage response2 = serviceObj.GetResponse("/api/Blotter/GetLatestOpeningBalaceForToday?&BR=" + Session["BR"].ToString() + "&Date=" + BlotterCurrentDate);
                response2.EnsureSuccessStatusCode();
                Models.SBP_BlotterOpeningBalance BlotterOpeningBalaceForToday = response2.Content.ReadAsAsync<Models.SBP_BlotterOpeningBalance>().Result;

                ViewBag.SBP_BlotterOpeningBalaceForToday = BlotterOpeningBalaceForToday;
                ViewBag.Title = "All Blotter";
                ViewData["SysCurrentDt"] = BlotterCurrentDate;
                return PartialView("_GetAllBlotter", blotterMulit);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult _Create(FormCollection form)
        {
            ServiceRepository serviceObj = new ServiceRepository();
            List<Models.BlotterDataColor_Result> BlotterDataLCY = new List<BlotterDataColor_Result>();
            List<Models.BlotterDataColor_Result> BlotterDataFCY = new List<BlotterDataColor_Result>();

            #region Currency parameter
            var selectCurrency = (dynamic)null;
            if (form["selectCurrency"] != null)
                selectCurrency = Convert.ToInt32(form["selectCurrency"].ToString());
            else
                selectCurrency = Convert.ToInt32(Session["SelectedCurrency"].ToString());

            UtilityClass.GetSelectedCurrecy(selectCurrency);
            #endregion
            try
            {
                bool statuschk = false;
                if (Convert.ToInt32(selectCurrency) == 1)
                {
                    for (int i = 0; i <= Request.Form.Count; i++)
                    {
                        if (Request.Form["GetAllBlotter01[" + i + "].DealNo"] != null)
                        {
                            var DealNo = Request.Form["GetAllBlotter01[" + i + "].DealNo"];
                            var Description = Request.Form["GetAllBlotter01[" + i + "].Description"];
                            var Status = Request.Form["GetAllBlotter01[" + i + "].Status"];
                            decimal Inflow = Convert.ToDecimal(Request.Form["GetAllBlotter01[" + i + "].Inflow"]);
                            decimal Outflow = Convert.ToDecimal(Request.Form["GetAllBlotter01[" + i + "].Outflow"]);
                            decimal OpeningBalance = Convert.ToDecimal(Request.Form["OpeningBalance[" + i + "]"]);
                            var chk = Request.Form["CheckLCY[" + i + "]"];
                            if (chk == "on")
                            {
                                statuschk = true;
                            }
                            else
                            {
                                statuschk = false;
                            }

                            BlotterDataLCY.Add(new BlotterDataColor_Result
                            {
                                DealNo = Convert.ToInt32(DealNo),
                                Description = Description,
                                Status = Status,
                                Inflow = Inflow,
                                Outflow = Outflow,
                                Balance = OpeningBalance,
                                Recon_isActive = statuschk
                            });
                        }
                    }
                }
                else
                {
                    for (int i = 0; i <= Request.Form.Count; i++)
                    {
                        if (Request.Form["CheckFCY[" + i + "]"] != null)
                        {
                            var DealNo = Request.Form["GetAllBlotterFCY01[" + i + "].DealNo"];
                            var Description = Request.Form["GetAllBlotterFCY01[" + i + "].Description"];
                            var Status = Request.Form["GetAllBlotterFCY01[" + i + "].Status"];
                            decimal Inflow = Convert.ToDecimal(Request.Form["GetAllBlotterFCY01[" + i + "].Inflow"]);
                            decimal Outflow = Convert.ToDecimal(Request.Form["GetAllBlotterFCY01[" + i + "].Outflow"]);
                            decimal OpeningBalance = Convert.ToDecimal(Request.Form["OpeningBalance[" + i + "]"]);
                            var chk = Request.Form["CheckFCY[" + i + "]"];
                            if (chk == "On")
                            {
                                statuschk = true;
                            }
                            else
                            {
                                statuschk = false;
                            }
                            BlotterDataFCY.Add(new BlotterDataColor_Result
                            {
                                DealNo = Convert.ToInt32(DealNo),
                                Description = Description,
                                Status = Status,
                                Inflow = Inflow,
                                Outflow = Outflow,
                                Balance = OpeningBalance,
                                Recon_isActive = statuschk
                            });
                        }
                    }
                }
                if (Request.Form["Save"] != null && (BlotterDataLCY.Count > 0 || BlotterDataFCY.Count > 0)) // For Insert
                {
                    if (Convert.ToInt32(selectCurrency) == 1)
                    {
                        HttpResponseMessage response = serviceObj.PostResponse("/api/Blotter/InsertBlotterListLCY", BlotterDataLCY);
                        response.EnsureSuccessStatusCode();
                        return RedirectToAction("GetAllBlotter");
                    }
                    else
                    {
                        HttpResponseMessage response = serviceObj.PostResponse("/api/Blotter/InsertBlotterListFCY", BlotterDataFCY);
                        response.EnsureSuccessStatusCode();
                        return RedirectToAction("GetAllBlotter");
                    }
                }
                else
                {
                    return RedirectToAction("GetAllBlotter");
                }
            }
            catch (Exception ex) { }
            return RedirectToAction("GetAllBlotter");
        }
        public ActionResult GetAllBlotterInternal(FormCollection form)
        {
            try
            {
                #region Added by shakir (Currency parameter)
                var selectCurrency = (dynamic)null;
                var BlotterCurrentDate = (dynamic)null;

                if (form["selectCurrency"] != null)
                    selectCurrency = Convert.ToInt32(form["selectCurrency"].ToString());
                else
                    selectCurrency = Convert.ToInt32(Session["SelectedCurrency"].ToString());

                if (form["BlotterCurrentDate"] != null)
                {
                    BlotterCurrentDate = form["BlotterCurrentDate"].ToString();
                    ViewBag.CurrentDt = BlotterCurrentDate;
                }
                else
                {
                    ViewBag.CurrentDt = DateTime.Now.ToString("yyyy-MM-dd");
                    BlotterCurrentDate = ViewBag.CurrentDt;
                }

                UtilityClass.GetSelectedCurrecy(selectCurrency);
                #endregion
                BlotterMultiModel blotterMulit = new BlotterMultiModel();
                ServiceRepositoryBlotter serviceObj = new ServiceRepositoryBlotter();
                HttpResponseMessage response = serviceObj.GetResponse("/api/Blotter/GetAllBlotterList?brcode=" + Session["BR"].ToString() + "&DataType=HBLC" + "&CurrentDate=" + BlotterCurrentDate);
                response.EnsureSuccessStatusCode();
                List<Models.SP_SBPBlotter_Result> blotter = response.Content.ReadAsAsync<List<Models.SP_SBPBlotter_Result>>().Result;
                // List<blotterMulit.GetAllBlotter01> blotter = response.Content.ReadAsAsync<List<Models.SP_SBPBlotter_Result>>().Result;
                blotterMulit.GetAllBlotter01 = blotter;
                ViewBag.Title = "All Blotter Internal";
                ViewData["SysCurrentDt"] = BlotterCurrentDate;
                return PartialView("_GetAllBlotterInternal", blotterMulit);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public DateTime GetCurrentDT()
        {

            try
            {
                ServiceRepositoryBlotter serviceObj = new ServiceRepositoryBlotter();
                HttpResponseMessage response = serviceObj.GetResponse("/api/BlotterDT/GetBlotterSysDT?BrCode=" + Session["BR"].ToString());
                response.EnsureSuccessStatusCode();
                List<Models.SP_SBPOpicsSystemDate_Result> blotterDT = response.Content.ReadAsAsync<List<Models.SP_SBPOpicsSystemDate_Result>>().Result;

                return Convert.ToDateTime(blotterDT[0].OpicsCurrentDate);


            }
            catch (Exception)
            {
                throw;
            }
        }
        public ActionResult SendEmail(ViewModel.BlotterMultiModel setup)
        {

            try
            {
                //GetBlotterSum
                ServiceRepositoryBlotter serviceObj = new ServiceRepositoryBlotter();
                HttpResponseMessage response = serviceObj.GetResponse("/api/BlotterDTLDB/GetBlotterDBList?brcode=" + Session["BR"].ToString());
                response.EnsureSuccessStatusCode();
                Models.SBP_BlotterDTLDB blotter = response.Content.ReadAsAsync<Models.SBP_BlotterDTLDB>().Result;
                if (blotter != null)
                {
                    DateTime DTLSartDate = blotter.DTL_Date;
                    int DTLDays = blotter.DTL_Days;
                    decimal DTLAmt = Convert.ToDecimal(blotter.DTL_Amount / 1000000);
                    decimal DTLAmt3Per = (DTLAmt * 3 / 100);
                    decimal DTLAmt5Per = (DTLAmt * 5 / 100);

                    Models.BlottterEmails TotalSum = BlotterSum();
                    decimal DTLAmt3Actual = (Convert.ToDecimal(0.03) / Convert.ToDecimal(DTLAmt3Per)) * (Convert.ToDecimal(TotalSum.Balance) * 100);
                    decimal DTLAmt5Actual = (Convert.ToDecimal(0.05) / Convert.ToDecimal(DTLAmt5Per)) * (Convert.ToDecimal(TotalSum.Balance) * 100);
                    //decimal DTLAmt3Actual = (Convert.ToDecimal(0.03) / Convert.ToDecimal(DTLAmt)) * (Convert.ToDecimal(TotalSum.Balance) );
                    //decimal DTLAmt5Actual = (Convert.ToDecimal(0.05) / Convert.ToDecimal(DTLAmt)) * (Convert.ToDecimal(TotalSum.Balance) );

                    decimal DTLAmtOpeningBalance = (Convert.ToDecimal((TotalSum.Balance - (TotalSum.Inflow + TotalSum.Outflow))));
                    ViewBag.Title = "All Blotter";
                    var currentDT = GetCurrentDT();
                    ViewData["SysCurrentDt1"] = currentDT.ToString("dd-MMM-yyyy");
                    var DatCount = (currentDT - DTLSartDate).TotalDays;
                    var DatCount1 = (DTLSartDate.AddDays(DTLDays));
                    if (currentDT >= DTLSartDate && DTLSartDate <= DatCount1)
                    {
                        var upd = 0;
                        if (DatCount == 0) //&& String.IsNullOrEmpty(blotter.Friday_01.ToString()))
                        {

                            blotter.Friday_01 = TotalSum.Balance * 1000000;
                            blotter.Saturday_02 = TotalSum.Balance * 1000000;
                            blotter.Sunday_03 = TotalSum.Balance * 1000000;
                            SendE = true;
                            upd = Update(blotter);

                        }
                        else
                        if (DatCount == 3) // && String.IsNullOrEmpty(blotter.Monday_04.ToString()))
                        {
                            blotter.Monday_04 = TotalSum.Balance * 1000000;
                            SendE = true;
                            upd = Update(blotter);

                        }
                        else
                        if (DatCount == 4) //&& String.IsNullOrEmpty(blotter.Tuesday_05.ToString()))
                        {
                            blotter.Tuesday_05 = TotalSum.Balance * 1000000;
                            SendE = true;
                            upd = Update(blotter);

                        }
                        else
                        if (DatCount == 5) // && String.IsNullOrEmpty(blotter.Wednesday_06.ToString()))
                        {
                            blotter.Wednesday_06 = TotalSum.Balance * 1000000;
                            SendE = true;
                            upd = Update(blotter);
                        }
                        else
                        if (DatCount == 6) //&& String.IsNullOrEmpty(blotter.Thursday_07.ToString()))
                        {
                            blotter.Thursday_07 = TotalSum.Balance * 1000000;
                            SendE = true;
                            upd = Update(blotter);
                        }
                        else
                        if (DatCount == 7) // && String.IsNullOrEmpty(blotter.Friday_08.ToString()))
                        {
                            blotter.Friday_08 = TotalSum.Balance * 1000000;
                            blotter.Saturday_09 = TotalSum.Balance * 1000000;
                            blotter.Sunday_10 = TotalSum.Balance * 1000000;
                            SendE = true;
                            upd = Update(blotter);
                        }
                        else
                        if (DatCount == 10) //&& String.IsNullOrEmpty(blotter.Monday_11.ToString()))
                        {
                            blotter.Monday_11 = TotalSum.Balance * 1000000;
                            SendE = true;
                            upd = Update(blotter);
                        }
                        else
                        if (DatCount == 11) //&& String.IsNullOrEmpty(blotter.Tuesday_12.ToString()))
                        {
                            blotter.Tuesday_12 = TotalSum.Balance * 1000000;
                            SendE = true;
                            upd = Update(blotter);
                        }
                        else
                        if (DatCount == 12) // && String.IsNullOrEmpty(blotter.Wednesday_13.ToString()))
                        {
                            blotter.Wednesday_13 = TotalSum.Balance * 1000000;
                            SendE = true;
                            upd = Update(blotter);
                        }
                        else
                        if (DatCount == 13) // && String.IsNullOrEmpty(blotter.Thursday_14.ToString()))
                        {
                            blotter.Thursday_14 = TotalSum.Balance * 1000000;
                            SendE = true;
                            upd = Update(blotter);
                        }
                        //var upd=Update(blotter);

                    }

                    if (SendE == true)
                    {
                        string htmlString = @"<table border = " + Convert.ToChar(34) + "2" + Convert.ToChar(34) + " width = " + Convert.ToChar(34) + "50%" + Convert.ToChar(34) + ">";
                        htmlString = htmlString + "<tr><td>  </td><td align=" + Convert.ToChar(34) + "center" + Convert.ToChar(34) + "> " + "Amount in millions </td></tr>";
                        htmlString = htmlString + "<tr><td> Opening Balance </td><td align=" + Convert.ToChar(34) + "right" + Convert.ToChar(34) + "> " + String.Format("{0:0,0}", DTLAmtOpeningBalance) + " </td></tr>";
                        htmlString = htmlString + "<tr><td> InFlow </td><td align=" + Convert.ToChar(34) + "right" + Convert.ToChar(34) + "> " + String.Format("{0:0,0}", TotalSum.Inflow) + " </td></tr>";
                        htmlString = htmlString + "<tr><td> OutFlow </td><td align=" + Convert.ToChar(34) + "right" + Convert.ToChar(34) + "> " + String.Format("{0:0,0}", TotalSum.Outflow) + " </td></tr>";
                        htmlString = htmlString + "<tr><td> Balance maintained </td><td align=" + Convert.ToChar(34) + "right" + Convert.ToChar(34) + "> " + String.Format("{0:0,0}", TotalSum.Balance) + "</td></tr>";
                        htmlString = htmlString + "<tr  bgcolor = " + Convert.ToChar(34) + "#5DADE2" + Convert.ToChar(34) + " ><td> Working </td></tr>";
                        htmlString = htmlString + "<tr bgcolor = " + Convert.ToChar(34) + "#76D8A0" + Convert.ToChar(34) + "><td> 5% Requirement </td><td align=" + Convert.ToChar(34) + "right" + Convert.ToChar(34) + "> " + String.Format("{0:0,0}", DTLAmt5Per) + " </td></tr>";
                        htmlString = htmlString + "<tr  bgcolor = " + Convert.ToChar(34) + "#76D8A0" + Convert.ToChar(34) + " ><td> 3% Amount To Maintain </td><td align=" + Convert.ToChar(34) + "right" + Convert.ToChar(34) + "> " + String.Format("{0:0,0}", DTLAmt3Per) + " </td></tr>";

                        // if (Convert.ToDecimal(DTLAmt3Actual) < Convert.ToDecimal(3.0))
                        // { 
                        //   htmlString = htmlString + "<tr bgcolor = " + Convert.ToChar(34) + "#DE1C51" + Convert.ToChar(34) + "><td> 3% Actual % </td><td align=" + Convert.ToChar(34) + "right" + Convert.ToChar(34) + "> " + String.Format("{0:0,0.00}", DTLAmt3Actual) + " </td></tr>";
                        // }
                        // else
                        //{
                        //     htmlString = htmlString + "<tr bgcolor = " + Convert.ToChar(34) + "#00FF00" + Convert.ToChar(34) + "><td> 3% Actual % </td><td align=" + Convert.ToChar(34) + "right" + Convert.ToChar(34) + "> " + String.Format("{0:0,0.00}", DTLAmt3Actual) + " </td></tr>";
                        // }

                        htmlString = htmlString + "<tr bgcolor = " + Convert.ToChar(34) + "#00FF00" + Convert.ToChar(34) + "><td>Balance Maintained in % age </td><td align=" + Convert.ToChar(34) + "right" + Convert.ToChar(34) + "> " + String.Format("{0:0.00}", DTLAmt5Actual) + " </td></tr>";

                        htmlString = htmlString + "</table>";

                        string EmailFrom = ConfigurationManager.AppSettings["EmailSender"].ToString();
                        string EmailTo = ConfigurationManager.AppSettings["EmailReceviers"].ToString();
                        SEND_EMAIL(EmailFrom, EmailTo, "", "", "SBP DTL Summary", htmlString, "");

                        SendE = false;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }


            return RedirectToAction("GetAllBlotter");
        }
        private Models.BlottterEmails BlotterSum()
        {

            try
            {
                ServiceRepositoryBlotter serviceObj = new ServiceRepositoryBlotter();
                HttpResponseMessage response = serviceObj.GetResponse("/api/Blotter/GetBlotterSum?brcode=" + Session["BR"].ToString());
                response.EnsureSuccessStatusCode();
                Models.BlottterEmails BlotterSum = response.Content.ReadAsAsync<Models.BlottterEmails>().Result;
                return BlotterSum;
            }

            catch (Exception)
            {
                throw;
            }
        }
        private int Update(Models.SBP_BlotterDTLDB DTLDeskBoard)
        {
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.PutResponse("/api/BlotterDTLDB/UpdateWiseBal", DTLDeskBoard);
            response.EnsureSuccessStatusCode();
            return 0; // RedirectToAction("GetBlotterDTLDB");
        }
        private bool SEND_EMAIL(string _from, string _to, string _cc, string _bcc, string _subject, string _message, string _path)
        {

            try
            {
                string exch_ip = ConfigurationManager.AppSettings["Exchange_Server"].ToString();
                if (_from != string.Empty)
                {

                    SmtpClient scEmail = new SmtpClient(exch_ip);
                    MailMessage mmEmail = new MailMessage();


                    // MailFrom
                    mmEmail.From = new MailAddress(_from);


                    // MailTo
                    string[] mailTo = _to.Split(';');
                    foreach (string var in mailTo)
                    {
                        mmEmail.To.Add(var);
                    }

                    // CC
                    if (_cc != "")
                    {
                        string[] mailCC = _cc.Split(';');
                        foreach (string var in mailCC)
                        {
                            mmEmail.CC.Add(var);
                        }

                    }

                    // BCC                    
                    if (_bcc != "")
                    {
                        string[] mailBCC = _bcc.Split(';');
                        foreach (string var in mailBCC)
                        {
                            mmEmail.Bcc.Add(var);
                        }

                    }

                    // Subject
                    mmEmail.Subject = _subject;
                    // Body
                    mmEmail.Body = _message;
                    // Body Format
                    mmEmail.IsBodyHtml = true;

                    // IF ATTACHMENT
                    if (_path != "")
                    {
                        // Add the file attachment to this e-mail message.
                        mmEmail.Attachments.Add(new Attachment(_path));
                    }



                    //Sending the email
                    //this.write_logs("------ Emailed-------------------------------------");
                    scEmail.Send(mmEmail);
                    scEmail.Dispose();
                    return true;
                }
                else return false;
            }

            catch (Exception ex)
            {

                return false;
            }


        }

    }
}