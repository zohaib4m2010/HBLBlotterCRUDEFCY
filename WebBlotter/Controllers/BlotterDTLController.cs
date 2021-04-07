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
    public class BlotterDTLController : Controller
    {

        // GET: BlotterDTLDeals
        #region Display...
        string BrCode = System.Configuration.ConfigurationManager.AppSettings["BranchCode"].ToString();
        public ActionResult GetAllDTLDeals()
        {
            try
            {
                ServiceRepository serviceObj = new ServiceRepository();
                HttpResponseMessage response = serviceObj.GetResponse("/api/blotterdtl/GetAllDTLDeals?brcode=" + BrCode);
                response.EnsureSuccessStatusCode();
                List<Models.SBP_BlotterDTL> blotterDTL = response.Content.ReadAsAsync<List<Models.SBP_BlotterDTL>>().Result;
                ViewData["SysCurrentDt"] = GetCurrentDT().ToString("dd-MMM-yyyy");
                ViewData["BrCode"] = BrCode;
                ViewBag.Title = "All Blotter Setup";
                return View(blotterDTL);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ActionResult Details(int id)
        {
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.GetResponse("api/blotterdtl/GetDTLDeal?id=" + id.ToString());
            response.EnsureSuccessStatusCode();
            Models.SBP_BlotterDTL blottersetup = response.Content.ReadAsAsync<Models.SBP_BlotterDTL>().Result;
            ViewBag.Title = "All Products";
            ViewData["SysCurrentDt"] = GetCurrentDT().ToString("dd-MMM-yyyy");
            ViewData["BrCode"] = BrCode;
            return View(blottersetup);
        }
        #endregion
        //[HttpGet]  
        public ActionResult Edit(int id)
        {
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.GetResponse("/api/blotterdtl/GetDTLDeal?id=" + id.ToString());
            response.EnsureSuccessStatusCode();
            Models.SBP_BlotterDTL blotterdtl = response.Content.ReadAsAsync<Models.SBP_BlotterDTL>().Result;
            ViewData["SysCurrentDt"] = GetCurrentDT().ToString("dd-MMM-yyyy");
            ViewBag.Title = "All Products";
            ViewData["SysCurrentDt"] = GetCurrentDT().ToString("dd-MMM-yyyy");
            ViewData["BrCode"] = BrCode;
            return View(blotterdtl);
   
        }
        //[HttpPost]  
        public ActionResult Update(Models.SBP_BlotterDTL setup)
        {
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.PutResponse("api/blotterdtl/UpdateDTLDeal", setup);
            response.EnsureSuccessStatusCode();
            ViewData["SysCurrentDt"] = GetCurrentDT().ToString("dd-MMM-yyyy");
            ViewData["BrCode"] = BrCode;
            return RedirectToAction("GetAllDTLDeals");
        }
        [HttpGet]
        public ActionResult Create()
        {
            SBP_BlotterDTL model = new SBP_BlotterDTL();
            ViewData["SysCurrentDt"] = GetCurrentDT().ToString("dd-MMM-yyyy");
            ViewData["BrCode"] = BrCode;
            return View(model);

        }
        [HttpPost]
        public ActionResult Create(Models.SBP_BlotterDTL setup)
        {
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.PostResponse("api/blotterdtl/InsertDTLDeal", setup);
            response.EnsureSuccessStatusCode();
            ViewData["SysCurrentDt"] = GetCurrentDT().ToString("dd-MMM-yyyy");
            ViewData["BrCode"] = BrCode;
            return RedirectToAction("GetAllDTLDeals");
        }
        public ActionResult Delete(int id)
        {
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.DeleteResponse("api/blotterdtl/DeleteDTLDeal?id=" + id.ToString());
            response.EnsureSuccessStatusCode();
            return RedirectToAction("GetAllDTLDeals");
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
        public ActionResult DTLDeskBoard()
        {

            return View();
        }
    }
}