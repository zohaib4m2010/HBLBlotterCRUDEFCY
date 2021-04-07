using WebBlotter.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using WebBlotter.Classes;

namespace WebBlotter.Controllers
{
    [AuthAccess]
    public class BlotterSetupController : Controller
    {
        #region Display...
        string BrCode = System.Configuration.ConfigurationManager.AppSettings["BranchCode"].ToString();
        public ActionResult GetAllSetupItems()
        {
            try
            {
                ServiceRepository serviceObj = new ServiceRepository();
                HttpResponseMessage response = serviceObj.GetResponse("/api/BlotterSetup/GetAllSetupItems");
                response.EnsureSuccessStatusCode();
                List<Models.SBP_BlotterSetup> blotterSetup = response.Content.ReadAsAsync<List<Models.SBP_BlotterSetup>>().Result;
                ViewBag.Title = "All Blotter Setup";
                return View(blotterSetup);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ActionResult Details(int id)
        {
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.GetResponse("api/BlotterSetup/GetSetUpItem?id=" + id.ToString());
            response.EnsureSuccessStatusCode();
            Models.SBP_BlotterSetup blottersetup = response.Content.ReadAsAsync<Models.SBP_BlotterSetup>().Result;
            ViewBag.Title = "All Products";
            return View(blottersetup);
        }
        #endregion
        //[HttpGet]  
        public ActionResult EditSetupt(int id)
        {
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.GetResponse("api/BlotterSetup/GetSetUpItem?id=" + id.ToString());
            response.EnsureSuccessStatusCode();
            Models.SBP_BlotterSetup blottersetup = response.Content.ReadAsAsync<Models.SBP_BlotterSetup>().Result;
            ViewBag.Title = "All Products";
            return View(blottersetup);
        }
        //[HttpPost]  
        public ActionResult Update(Models.SBP_BlotterSetup setup)
        {
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.PutResponse("api/BlotterSetup/UpdateSetUp", setup);
            response.EnsureSuccessStatusCode();
            return RedirectToAction("GetAllSetupItems");
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Models.SBP_BlotterSetup setup)
        {
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.PostResponse("api/BlotterSetup/InsertSetup", setup);
            response.EnsureSuccessStatusCode();
            return RedirectToAction("GetAllSetupItems");
        }
        public ActionResult Delete(int id)
        {
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.DeleteResponse("api/BlotterSetup/DeleteSetUp?id=" + id.ToString());
            response.EnsureSuccessStatusCode();
            return RedirectToAction("GetAllSetupItems");
        }
        public DateTime GetCurrentDT()
        {

            try
            {
                ServiceRepositoryBlotter serviceObj = new ServiceRepositoryBlotter();
                HttpResponseMessage response = serviceObj.GetResponse("/api/BlotterDT/GetBlotterSysDT");
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