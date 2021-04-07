using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WebBlotter.Classes;
using WebBlotter.Repository;

namespace WebBlotter.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [AuthAccess]
    public class HomeController : Controller
    {
        string BrCode = ConfigurationManager.AppSettings["BranchCode"].ToString();
        public ActionResult index()
        {
            //var currentDT = GetCurrentDT();
            //ViewData["SysCurrentDt"] = currentDT.ToString("dd-MMM-yyyy"); 

            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.GetResponse("/api/Blotter/GetLatestBlotterDTLReportDayWise?&BR=" + Session["BR"].ToString());
            response.EnsureSuccessStatusCode();
            List<Models.SBP_BlotterCRRReportDaysWiseBal> BlotterCRRReportsDayWiseBal = response.Content.ReadAsAsync<List<Models.SBP_BlotterCRRReportDaysWiseBal>>().Result;

            ViewBag.SBP_BlotterCRRReportDaysWiseBal = BlotterCRRReportsDayWiseBal;


            return View();
        }
        
       


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
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

        public void SessionIsAlive()
        {

            if (Session["UserID"] == null)
            {
                Response.Redirect(new Uri(Request.Url, Url.Action("Login")).ToString(), false);
            }
        }

    }
}