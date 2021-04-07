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
        // GET: BlotterBreakups
        public ActionResult BlotterBreakups()
        {
            try
            {
                ServiceRepository serviceObj = new ServiceRepository();
                HttpResponseMessage response = serviceObj.GetResponse("/api/BlotterBreakups/GetAllBlotterBreakups?UserID="+ Session["UserID"].ToString() + "&BranchID="+ Session["BranchID"].ToString() + "&CurID="+ Session["SelectedCurrency"].ToString() + "&BR="+ Session["BR"].ToString());
                response.EnsureSuccessStatusCode();
                List<Models.SP_GetLatestBreakup_Result> BlotterBreakups = response.Content.ReadAsAsync<List<Models.SP_GetLatestBreakup_Result>>().Result;
                if(BlotterBreakups.Count < 1)
                    ViewData["DataStatus"] = "Data Not Availavle";
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
        public ActionResult Create()
        {
            ViewData["SysCurrentDt"] = DateTime.Now.ToString("dd-MMM-yyyy");
            ViewData["BranchName"] = Session["BranchName"];
            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SBP_BlotterBreakups BlotterBreakups)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    BlotterBreakups.UserID = Convert.ToInt16(Session["UserID"].ToString());
                    BlotterBreakups.BR = Convert.ToInt16(Session["BR"].ToString());
                    BlotterBreakups.BID = Convert.ToInt16(Session["BranchID"].ToString());
                    BlotterBreakups.CurID = Convert.ToInt16(Session["SelectedCurrency"].ToString());
                    BlotterBreakups.BreakupDate = DateTime.Now;
                    BlotterBreakups.CreateDate = DateTime.Now;
                    ServiceRepository serviceObj = new ServiceRepository();
                    HttpResponseMessage response = serviceObj.PostResponse("api/BlotterBreakups/InsertBlotterBreakups", BlotterBreakups);
                    response.EnsureSuccessStatusCode();
                    return RedirectToAction("BlotterBreakups");
                }
            }
            catch (Exception ex) { }
            return View(BlotterBreakups);
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