using WebBlotter.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using WebBlotter.Models;
using System.Configuration;
using WebBlotter.Classes;

namespace WebBlotter.Controllers
{
    [AuthAccess]
    public class BlotterManualDealsController : Controller
    {
        // GET: BlotterManualDeals
        #region Display...
        string BrCode = System.Configuration.ConfigurationManager.AppSettings["BranchCode"].ToString();
        //[HttpGet] 
        public ActionResult GetAllManualDeals()
        {
            try
            {
                ServiceRepository serviceObj = new ServiceRepository();
                HttpResponseMessage response = serviceObj.GetResponse("/api/BlotterManualDeals/GetAllManualDeals?brcode=" + BrCode);
                response.EnsureSuccessStatusCode();
                List<Models.SBP_BlotterManualDeals> blotterSetup = response.Content.ReadAsAsync<List<Models.SBP_BlotterManualDeals>>().Result;
                ViewBag.Title = "All Blotter Setup";
                ViewData["SysCurrentDt"] = GetCurrentDT().ToString("dd-MMM-yyyy");
                ViewData["BrCode"] = BrCode;
                return View(blotterSetup);
            }
            catch (Exception)
            {
                throw;
            }
        }
        //[HttpGet] 
        private void UpdateDTLBlotter01(decimal CashInflow,decimal CashOutFlow,DateTime Dt)
        {

            try
            {
                //GetBlotterSum
                ServiceRepositoryBlotter serviceObj = new ServiceRepositoryBlotter();
                HttpResponseMessage response = serviceObj.GetResponse("/api/BlotterDTLDB/GetBlotterDBList?brcode=" + BrCode);
                response.EnsureSuccessStatusCode();
                Models.SBP_BlotterDTLDB blotter = response.Content.ReadAsAsync<Models.SBP_BlotterDTLDB>().Result;

                    ViewBag.Title = "All Blotter";
                    var currentDT = Dt.ToString("dd-MMM-yyyy");
                    ViewData["SysCurrentDt1"] = currentDT;              
                    var upd = 0;
                    if (blotter.Date_01 .ToString("dd-MMM-yyyy") == Dt.ToString("dd-MMM-yyyy"))
                    {
                        blotter.CashFlow_01 = CashInflow;
                        blotter.CashOutFlow_01 = CashOutFlow;
                        upd = Update(blotter);
                    }
                    else
                    if (blotter.Date_02.ToString("dd-MMM-yyyy") == Dt.ToString("dd-MMM-yyyy"))
                    {
                        blotter.CashFlow_02 = CashInflow;
                        blotter.CashOutFlow_02 = CashOutFlow;
                        upd = Update(blotter); 
                    }
                    else
                    if (blotter.Date_03.ToString("dd-MMM-yyyy") == Dt.ToString("dd-MMM-yyyy"))
                    {
                        blotter.CashFlow_03 = CashInflow;
                        blotter.CashOutFlow_03 = CashOutFlow;
                        upd = Update(blotter);
                    }
                    else
                    if (blotter.Date_04.ToString("dd-MMM-yyyy") == Dt.ToString("dd-MMM-yyyy"))
                    {
                        blotter.CashFlow_04 = CashInflow;
                        blotter.CashOutFlow_04 = CashOutFlow;
                        upd = Update(blotter);
                    }
                    else
                    if (blotter.Date_05.ToString("dd-MMM-yyyy") == Dt.ToString("dd-MMM-yyyy"))
                    {
                        blotter.CashFlow_05 = CashInflow;
                        blotter.CashOutFlow_05 = CashOutFlow;
                        upd = Update(blotter);
                    }
                    else
                     if (blotter.Date_06.ToString("dd-MMM-yyyy") == Dt.ToString("dd-MMM-yyyy"))
                    {
                        blotter.CashFlow_06 = CashInflow;
                        blotter.CashOutFlow_06 = CashOutFlow;
                        upd = Update(blotter);
                    }
                    else
                     if (blotter.Date_07.ToString("dd-MMM-yyyy") == Dt.ToString("dd-MMM-yyyy"))
                    {
                        blotter.CashFlow_07 = CashInflow;
                        blotter.CashOutFlow_07 = CashOutFlow;
                        upd = Update(blotter);
                    }
                    else
                     if (blotter.Date_08.ToString("dd-MMM-yyyy") == Dt.ToString("dd-MMM-yyyy"))
                    {
                        blotter.CashFlow_08 = CashInflow;
                        blotter.CashOutFlow_08 = CashOutFlow;
                        upd = Update(blotter);
                    }
                    else
                    if (blotter.Date_09.ToString("dd-MMM-yyyy") == Dt.ToString("dd-MMM-yyyy"))
                    {
                        blotter.CashFlow_09 = CashInflow;
                        blotter.CashOutFlow_09 = CashOutFlow;
                        upd = Update(blotter);
                    }
                    else
                     if (blotter.Date_10.ToString("dd-MMM-yyyy") == Dt.ToString("dd-MMM-yyyy"))
                    {
                        blotter.CashFlow_10 = CashInflow;
                        blotter.CashOutFlow_10 = CashOutFlow;
                        upd = Update(blotter);
                    }
                    else
                    if (blotter.Date_11.ToString("dd-MMM-yyyy") == Dt.ToString("dd-MMM-yyyy"))
                    {
                        blotter.CashFlow_11 = CashInflow;
                        blotter.CashOutFlow_11 = CashOutFlow;
                        upd = Update(blotter);
                    }
                    else
                         if (blotter.Date_12.ToString("dd-MMM-yyyy") == Dt.ToString("dd-MMM-yyyy"))
                    {
                        blotter.CashFlow_12 = CashInflow;
                        blotter.CashOutFlow_12 = CashOutFlow;
                        upd = Update(blotter);
                    }
                    else
                    if (blotter.Date_13.ToString("dd-MMM-yyyy") == Dt.ToString("dd-MMM-yyyy"))
                    {
                        blotter.CashFlow_13 = CashInflow;
                        blotter.CashOutFlow_13 = CashOutFlow;
                        upd = Update(blotter);
                    }
                    else
                    if (blotter.Date_14.ToString("dd-MMM-yyyy") == Dt.ToString("dd-MMM-yyyy"))
                    {
                        blotter.CashFlow_14 = CashInflow;
                        blotter.CashOutFlow_14 = CashOutFlow;
                        upd = Update(blotter);
                    }
            }
            catch (Exception)
            {
                throw;
            }
           
        }
        //[HttpGet] 
        private int Update(Models.SBP_BlotterDTLDB DTLDeskBoard)
        {
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.PutResponse("/api/BlotterDTLDB/UpdateWiseBal", DTLDeskBoard);
            response.EnsureSuccessStatusCode();
            return 0; // RedirectToAction("GetBlotterDTLDB");
        }
        //[HttpGet] 
        public ActionResult Details(int id)
        {
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.GetResponse("api/BlotterManualDeals/GetManualDeal?id=" + id.ToString());
            response.EnsureSuccessStatusCode();
            Models.SBP_BlotterManualDeals blottersetup = response.Content.ReadAsAsync<Models.SBP_BlotterManualDeals>().Result;
            ViewBag.Title = "All Products";
            ViewData["BrCode"] = BrCode;
            ViewData["SysCurrentDt"] = GetCurrentDT().ToString("dd-MMM-yyyy");
            return View(blottersetup);
        }
        #endregion
        //[HttpGet]  
        public ActionResult Edit(int id)
        {
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.GetResponse("api/BlotterManualDeals/GetManualDeal?id=" + id.ToString());
            response.EnsureSuccessStatusCode();
            Models.SBP_BlotterManualDeals blottersetup = response.Content.ReadAsAsync<Models.SBP_BlotterManualDeals>().Result;
            blottersetup.DescriptionDD = GetDropDownList();
            ViewData["SysCurrentDt"] = GetCurrentDT().ToString("dd-MMM-yyyy");
            ViewData["BrCode"] = BrCode;
            ViewBag.Title = "All Products";
            return View(blottersetup);
           
        }
        //[HttpPost]  
        public ActionResult Update(Models.SBP_BlotterManualDeals setup)
        {
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.PutResponse("api/BlotterManualDeals/UpdateManualDeal", setup);
            response.EnsureSuccessStatusCode();
            ViewData["BrCode"] = BrCode;
            ViewData["SysCurrentDt"] = GetCurrentDT().ToString("dd-MMM-yyyy");
            return RedirectToAction("GetAllManualDeals");
          
        }
        //[HttpGet]
        public ActionResult Create()
        {
            SBP_BlotterManualDeals model = new SBP_BlotterManualDeals();
            model.DescriptionDD = GetDropDownList();
            var dt= GetCurrentDT();
            ViewData["SysCurrentDt"] = dt.ToString("dd-MMM-yyyy");
            model.DealDate = dt;
            ViewData["BrCode"] = BrCode;
            return View(model);
            
        }
        [HttpPost]
        public ActionResult Create(Models.SBP_BlotterManualDeals setup)
        {
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.PostResponse("api/BlotterManualDeals/InsertManualDeal", setup);
            response.EnsureSuccessStatusCode();
            ViewData["SysCurrentDt"] = GetCurrentDT().ToString("dd-MMM-yyyy");

            UpdateDTLBlotter01(setup.InFlow.Value, setup.OutFlow.Value, setup.DealDate.Value);
            ViewData["BrCode"] = BrCode;
            return RedirectToAction("GetAllManualDeals");
        }
        //[HttpPost]
        public ActionResult Delete(int id)
        {
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.DeleteResponse("api/BlotterManualDeals/DeleteManualDeal?id=" + id.ToString());
            response.EnsureSuccessStatusCode();
            return RedirectToAction("GetAllManualDeals");
        }
        //[HttpPost]
        public List<NameDropDown> GetDropDownList()

        {
            
            List<NameDropDown> result = new List<NameDropDown>();
            try
            {
                ServiceRepository serviceObj = new ServiceRepository();
                HttpResponseMessage response = serviceObj.GetResponse("/api/BlotterSetup/GetAllSetupItems");
                response.EnsureSuccessStatusCode();
                List<Models.SBP_BlotterSetup> blotterSetup = response.Content.ReadAsAsync<List<Models.SBP_BlotterSetup>>().Result;
            
                foreach (var data in blotterSetup)
                {
                    NameDropDown model = new NameDropDown();
                    model.SNo = data.SNo;
                    model.Desc = data.Description;
                    result.Add(model);
                }

            }
            catch (Exception)
            {
                throw;
            }          
            return result;
        }
        //[HttpPost]
        public DateTime GetCurrentDT()
        {

            try
            {
                ServiceRepositoryBlotter serviceObj = new ServiceRepositoryBlotter();
                HttpResponseMessage response = serviceObj.GetResponse("/api/BlotterDT/GetBlotterSysDT?brcode=" + BrCode);
                response.EnsureSuccessStatusCode();
                List<Models.SP_SBPOpicsSystemDate_Result> blotterDT = response.Content.ReadAsAsync<List<Models.SP_SBPOpicsSystemDate_Result>>().Result;

                return Convert.ToDateTime(blotterDT[0].OpicsCurrentDate);


            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}