using WebBlotter.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using WebBlotter.Classes;
//using Microsoft.VisualBasic;
namespace WebBlotter.Controllers
{
    [AuthAccess]
    public class BlotterDTLDBController : Controller
    {

        string BrCode = System.Configuration.ConfigurationManager.AppSettings["BranchCode"].ToString();
        [HttpGet]
        public ActionResult GetBlotterDTLDB()
        {
            try
            {
                //GetBlotterSum
                ServiceRepositoryBlotter serviceObj = new ServiceRepositoryBlotter();
                HttpResponseMessage response = serviceObj.GetResponse("/api/BlotterDTLDB/GetBlotterDBList?brcode=" + BrCode);
                response.EnsureSuccessStatusCode();
                Models.SBP_BlotterDTLDB  blotter = response.Content.ReadAsAsync<Models.SBP_BlotterDTLDB>().Result;
                
                    ViewBag.Title = "DTL Blotter";
                    ViewData["SysCurrentDt1"] = GetCurrentDT().ToString("dd-MMM-yyyy");
                    ViewData["BrCode"] = BrCode;
                    return View(blotter);
               
            }
            catch (Exception)
            {
                throw;
            }
         
        }


        public ActionResult UpdateCashFlow( )
        {

            try
            {
                //GetBlotterSum
                ServiceRepositoryBlotter serviceObj = new ServiceRepositoryBlotter();
                HttpResponseMessage response = serviceObj.GetResponse("/api/BlotterCashFlow/UpdateRunningBal?BranchCode=" + BrCode);
                response.EnsureSuccessStatusCode();
                //Models.SP_SBPBlotterRunningBal blotter = response.Content.ReadAsAsync<Models.SP_SBPBlotterRunningBal>().Result;
                

            }
            catch (Exception)
            {
                throw;
            }
            return RedirectToAction("GetBlotterDTLDB");


        }

        private int Update(Models.SBP_BlotterDTLDB DTLDeskBoard)
        {
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.PutResponse("/api/BlotterDTLDB/UpdateWiseBal", DTLDeskBoard);
            response.EnsureSuccessStatusCode();
            return 0; // RedirectToAction("GetBlotterDTLDB");
        }
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