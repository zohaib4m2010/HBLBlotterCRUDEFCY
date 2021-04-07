using WebAppBlotterSPA.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;


namespace WebAppBlotterSPA.Controllers
{
    public class BlotterController : Controller
    {
        // GET: Blotter
        // GET: Product  
        public ActionResult GetAllBlotter()
        {
            try
            {
                ServiceRepositoryBlotter  serviceObj = new ServiceRepositoryBlotter();
                HttpResponseMessage response = serviceObj.GetResponse("/api/Blotter/GetAllBlotterList");
                response.EnsureSuccessStatusCode();
                List<Models.SP_SBPBlotter_Result> blotter = response.Content.ReadAsAsync<List<Models.SP_SBPBlotter_Result>>().Result;
                ViewBag.Title = "All Blotter";
                return View(blotter);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}