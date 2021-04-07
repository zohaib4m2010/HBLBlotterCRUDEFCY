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
    public class BlotterOpeningController : Controller
    {
        #region Display...
        string BrCode = System.Configuration.ConfigurationManager.AppSettings["BranchCode"].ToString();
        public ActionResult GetAllOpeningAmt()
        {
            try
            {
                ServiceRepository serviceObj = new ServiceRepository();
                HttpResponseMessage response = serviceObj.GetResponse("/api/BlotterOpening/GetAllOpeningDeals?brcode=" + BrCode);
                response.EnsureSuccessStatusCode();
                List<Models.SBP_BlotterOpening> blotterOpening = response.Content.ReadAsAsync<List<Models.SBP_BlotterOpening>>().Result;
                ViewBag.Title = "All Blotter Setup";
                ViewData["SysCurrentDt"] = GetCurrentDT().ToString("dd-MMM-yyyy");
                ViewData["BrCode"] = BrCode;
                return View(blotterOpening);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ActionResult Details(int id)
        {
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.GetResponse("/api/BlotterOpening/GetOpeningDeal?id=" + id.ToString());
            response.EnsureSuccessStatusCode();
            Models.SBP_BlotterOpening blotterOpening = response.Content.ReadAsAsync<Models.SBP_BlotterOpening>().Result;
            ViewBag.Title = "All Products";
            ViewData["SysCurrentDt"] = GetCurrentDT().ToString("dd-MMM-yyyy");
            ViewData["BrCode"] = BrCode;
            return View(blotterOpening);
        }
        #endregion
        //[HttpGet]  
        public ActionResult Edit(int id)
        {
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.GetResponse("/api/BlotterOpening/GetOpeningDeal?id=" + id.ToString());
            response.EnsureSuccessStatusCode();
            Models.SBP_BlotterOpening blotterOpening = response.Content.ReadAsAsync<Models.SBP_BlotterOpening>().Result;
            ViewBag.Title = "All Products";
            ViewData["SysCurrentDt"] = GetCurrentDT().ToString("dd-MMM-yyyy");
            ViewData["BrCode"] = BrCode;
            return View(blotterOpening);
        }
        //[HttpPost]  
        public ActionResult Update(Models.SBP_BlotterOpening setup)
        {
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.PutResponse("/api/BlotterOpening/Update", setup);
            response.EnsureSuccessStatusCode();
            ViewData["SysCurrentDt"] = GetCurrentDT().ToString("dd-MMM-yyyy");
            ViewData["BrCode"] = BrCode;
            return RedirectToAction("GetAllOpeningAmt");
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewData["SysCurrentDt"] = GetCurrentDT().ToString("dd-MMM-yyyy");
            ViewData["BrCode"] = BrCode;
            return View();
        }
        [HttpPost]
        public ActionResult Create(Models.SBP_BlotterOpening setup)
        {
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.PostResponse("/api/BlotterOpening/Insert", setup);
            response.EnsureSuccessStatusCode();
            ViewData["SysCurrentDt"] = GetCurrentDT().ToString("dd-MMM-yyyy");
            ViewData["BrCode"] = BrCode;
            return RedirectToAction("GetAllOpeningAmt");
        }
        public ActionResult Delete(int id)
        {
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.DeleteResponse("api/BlotterOpening/Delete?id=" + id.ToString());
            response.EnsureSuccessStatusCode();
            return RedirectToAction("GetAllOpeningAmt");
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