using WebBlotter.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using WebBlotter.Models;
using WebBlotter.Classes;
using Newtonsoft.Json;

namespace WebBlotter.Controllers
{
    [AuthAccess]
    public class NostroBanksController : Controller
    {
        // GET: NostroBank
        public ActionResult NostroBanks()
        {
            try
            {
                ServiceRepository serviceObj = new ServiceRepository();
                HttpResponseMessage response = serviceObj.GetResponse("/api/NostroBank/GetAllNostroBank");
                response.EnsureSuccessStatusCode();
                List<Models.NostroBank> NostroBank = response.Content.ReadAsAsync<List<Models.NostroBank>>().Result;
                UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(NostroBank), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());

                ViewBag.Title = "Nostro Bank";
                return View(NostroBank);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        [HttpGet]
        public ActionResult Create()
        {
            UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), "", this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NostroBank NostroBank)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    NostroBank.CreateDate = DateTime.Now;
                    ServiceRepository serviceObj = new ServiceRepository();
                    HttpResponseMessage response = serviceObj.PostResponse("api/NostroBank/InsertNostroBank", NostroBank);
                    response.EnsureSuccessStatusCode();
                    UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(NostroBank), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
                    return RedirectToAction("NostroBanks");
                }
            }
            catch (Exception ex) { }
            return View(NostroBank);
        }

        public ActionResult Edit(int id)
        {
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.GetResponse("/api/NostroBank/GetNostroBank?id=" + id.ToString());
            response.EnsureSuccessStatusCode();
            Models.NostroBank NostroBank = response.Content.ReadAsAsync<Models.NostroBank>().Result;
            UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(NostroBank), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
            return View(NostroBank);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(NostroBank NostroBank)
        {
            NostroBank.UpdateDate = DateTime.Now;
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.PutResponse("api/NostroBank/UpdateNostroBank", NostroBank);
            response.EnsureSuccessStatusCode();
            UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(NostroBank), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
            return RedirectToAction("NostroBanks");
        }

        public ActionResult Delete(int id)
        {
            UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(id), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.DeleteResponse("api/NostroBank/DeleteNostroBank?id=" + id.ToString());
            response.EnsureSuccessStatusCode();
            return RedirectToAction("NostroBanks");
        }
    }
}