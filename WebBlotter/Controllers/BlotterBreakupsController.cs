using WebBlotter.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using WebBlotter.Models;
using WebBlotter.Classes;

namespace WebBlotter.Controllers
{
    [AuthAccess]
    public class BlotterBreakupsController : Controller
    {

        UtilityClass UC = new UtilityClass();
        // GET: BlotterBreakups
        public ActionResult BlotterBreakups()
        {
            try
            {
                ServiceRepository serviceObj = new ServiceRepository();
                HttpResponseMessage response = serviceObj.GetResponse("/api/BlotterBreakups/GetAllBlotterBreakups?UserID="+ Session["UserID"].ToString() + "&BranchID="+ Session["BranchID"].ToString() + "&CurID="+ Session["SelectedCurrency"].ToString() + "&BR="+ Session["BR"].ToString());
                response.EnsureSuccessStatusCode();
                Models.SP_GetLatestBreakup_Result BlotterBreakups = response.Content.ReadAsAsync<Models.SP_GetLatestBreakup_Result>().Result;
                if(BlotterBreakups == null)
                    ViewData["DataStatus"] = "Data Not Available";
                var PAccess = Session["CurrentPagesAccess"].ToString().Split('~');

                ViewData["isDateChangable"] = Convert.ToBoolean(PAccess[2]);
                ViewData["isEditable"] = Convert.ToBoolean(PAccess[3]);
                ViewData["IsDeletable"] = Convert.ToBoolean(PAccess[4]);
                return View(BlotterBreakups);
               
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult AddOpeningBalance()
        {
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.GetResponse("/api/Branches/GetAllBranches");
            response.EnsureSuccessStatusCode();
            List<Models.Branches> Branches = response.Content.ReadAsAsync<List<Models.Branches>>().Result;

            ViewBag.AllBranchNames = Branches;

            

            ViewData["SysCurrentDt"] = DateTime.Now.ToString("dd-MMM-yyyy");
            ViewData["BranchName"] = Session["BranchName"];
            return View();

        }

        public ActionResult AddOpeningBalanceByBID(int BID)
        {
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.GetResponse("/api/BlotterBreakups/GetAllBlotterBreakups?UserID=0&BranchID="+BID+"&CurID=" + Session["SelectedCurrency"].ToString() + "&BR=" + Session["BR"].ToString());
            response.EnsureSuccessStatusCode();
            Models.SP_GetLatestBreakup_Result BlotterBreakups = response.Content.ReadAsAsync<Models.SP_GetLatestBreakup_Result>().Result;

            BreakupOpeningBalance BOB = new BreakupOpeningBalance();
            if (BlotterBreakups != null)
            {
                BOB.BranchName = BlotterBreakups.BranchName;
                BOB.SNo = BlotterBreakups.SNo;
                BOB.OpeningBalActual = BlotterBreakups.OpeningBalActual;
            }
            var PAccess = Session["CurrentPagesAccess"].ToString().Split('~');

            ViewData["isDateChangable"] = Convert.ToBoolean(PAccess[2]);
            ViewData["isEditable"] = Convert.ToBoolean(PAccess[3]);
            ViewData["IsDeletable"] = Convert.ToBoolean(PAccess[4]);
            return PartialView("_OpeningBalanceTable", BOB);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateOpnBal(BreakupOpeningBalance BreakupOpnBal)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    SBP_BlotterBreakups BlotterBreakups = new SBP_BlotterBreakups();
                    BlotterBreakups.OpeningBalActual = BreakupOpnBal.OpeningBalActual;
                    BlotterBreakups.FoodPayment_inFlow = 0;
                    BlotterBreakups.HOKRemittance_inFlow = 0;
                    BlotterBreakups.ERF_inflow = 0;
                    BlotterBreakups.SBPChequeDeposite_inflow = 0;
                    BlotterBreakups.Miscellaneous_inflow = 0;
                    BlotterBreakups.CashWithdrawbySBPCheques_outFlow = 0;
                    BlotterBreakups.ERF_outflow = 0;
                    BlotterBreakups.DSC_outFlow = 0;
                    BlotterBreakups.RemitanceToHOK_outFlow = 0;
                    BlotterBreakups.SBPCheqGivenToOtherBank_outFlow = 0;
                    BlotterBreakups.Miscellaneous_outflow = 0;
                    BlotterBreakups.EstimatedCLossingBal = 0;
                    BlotterBreakups.UserID = Convert.ToInt16(Session["UserID"].ToString());
                    BlotterBreakups.BR = Convert.ToInt16(Session["BR"].ToString());
                    BlotterBreakups.BID = BreakupOpnBal.BID;
                    BlotterBreakups.CurID = Convert.ToInt16(Session["SelectedCurrency"].ToString());
                    BlotterBreakups.BreakupDate = DateTime.Now;
                    BlotterBreakups.CreateDate = DateTime.Now;
                    ServiceRepository serviceObj = new ServiceRepository();
                    HttpResponseMessage response = serviceObj.PostResponse("api/BlotterBreakups/InsertBlotterBreakups", BlotterBreakups);
                    response.EnsureSuccessStatusCode();
                    return RedirectToAction("AddOpeningBalance");
                }
            }
            catch (Exception ex) { }
            return View(BreakupOpnBal);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SBP_BlotterBreakups BlotterBreakups)
        {
            try
            {
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
                    ServiceRepository serviceObj = new ServiceRepository();
                    HttpResponseMessage response = serviceObj.PostResponse("api/BlotterBreakups/InsertBlotterBreakups", BlotterBreakups);
                    response.EnsureSuccessStatusCode();
                    return RedirectToAction("AddOpeningBalance");
                }
            }
            catch (Exception ex) { }
            return View(BlotterBreakups);
        }
        public ActionResult EditOpeningBalance(int id)
        {
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.GetResponse("/api/BlotterBreakups/GetBlotterBreakups?id=" + id.ToString());
            response.EnsureSuccessStatusCode();
            Models.SBP_BlotterBreakups BlotterBreakups = response.Content.ReadAsAsync<Models.SBP_BlotterBreakups>().Result;

            BreakupOpeningBalance BreakupOpnBal = new BreakupOpeningBalance();
            BreakupOpnBal.SNo = BlotterBreakups.SNo;
            BreakupOpnBal.OpeningBalActual = BlotterBreakups.OpeningBalActual;
            BreakupOpnBal.BID = BlotterBreakups.BID;

            return View(BreakupOpnBal);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateOpeningBalance(SBP_BlotterBreakups BlotterBreakups)
        {
            BlotterBreakups.UpdateDate = DateTime.Now;
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.PutResponse("api/BlotterBreakups/UpdateBreakupsOpngBal", BlotterBreakups);
            response.EnsureSuccessStatusCode();
            return RedirectToAction("AddOpeningBalance");
        }

        public ActionResult Edit(int id)
        {
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.GetResponse("/api/BlotterBreakups/GetBlotterBreakups?id=" + id.ToString());
            response.EnsureSuccessStatusCode();
            Models.SBP_BlotterBreakups BlotterBreakups = response.Content.ReadAsAsync<Models.SBP_BlotterBreakups>().Result;
            return View(BlotterBreakups);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(SBP_BlotterBreakups BlotterBreakups)
        {
            BlotterBreakups.CashWithdrawbySBPCheques_outFlow = UC.CheckNegativeValue(BlotterBreakups.CashWithdrawbySBPCheques_outFlow);
            BlotterBreakups.ERF_outflow = UC.CheckNegativeValue(BlotterBreakups.ERF_outflow);
            BlotterBreakups.DSC_outFlow = UC.CheckNegativeValue(BlotterBreakups.DSC_outFlow);
            BlotterBreakups.RemitanceToHOK_outFlow = UC.CheckNegativeValue(BlotterBreakups.RemitanceToHOK_outFlow);
            BlotterBreakups.SBPCheqGivenToOtherBank_outFlow = UC.CheckNegativeValue(BlotterBreakups.SBPCheqGivenToOtherBank_outFlow);
            BlotterBreakups.Miscellaneous_outflow = UC.CheckNegativeValue(BlotterBreakups.Miscellaneous_outflow);
            BlotterBreakups.UpdateDate = DateTime.Now;
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.PutResponse("api/BlotterBreakups/UpdateBlotterBreakups", BlotterBreakups);
            response.EnsureSuccessStatusCode();
            return RedirectToAction("BlotterBreakups");
        }

        public ActionResult Delete(int id)
        {
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.DeleteResponse("api/BlotterBreakups/DeleteBlotterBreakups?id=" + id.ToString());
            response.EnsureSuccessStatusCode();
            return RedirectToAction("BlotterBreakups");
        }


       
    }
}