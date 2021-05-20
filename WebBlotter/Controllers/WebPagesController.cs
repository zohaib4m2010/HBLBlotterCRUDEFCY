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
    public class WebPagesController : Controller
    {
        // GET: WebPages
        public ActionResult WebPages()
        {
            try
            {
                ServiceRepository serviceObj = new ServiceRepository();
                HttpResponseMessage response = serviceObj.GetResponse("/api/WebPages/GetAllWebPage");
                response.EnsureSuccessStatusCode();
                List<Models.WebPages> WebPages = response.Content.ReadAsAsync<List<Models.WebPages>>().Result;
                UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(WebPages), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());

                ViewBag.Title = "User Role";
                return View(WebPages);
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
        public ActionResult Create(WebPages WebPages)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    WebPages.CreateDate = DateTime.Now;
                    ServiceRepository serviceObj = new ServiceRepository();
                    HttpResponseMessage response = serviceObj.PostResponse("api/WebPages/InsertWebPage", WebPages);
                    response.EnsureSuccessStatusCode();
                    UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(WebPages), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
                    return RedirectToAction("WebPages");
                }
            }
            catch (Exception ex) { }
            return View(WebPages);
        }

        public ActionResult Edit(int id)
        {
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.GetResponse("/api/WebPages/GetWebPage?id=" + id.ToString());
            response.EnsureSuccessStatusCode();
            Models.WebPages WebPages = response.Content.ReadAsAsync<Models.WebPages>().Result;
            UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(WebPages), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
            return View(WebPages);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(WebPages WebPages)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    WebPages.UpdateDate = DateTime.Now;
                    ServiceRepository serviceObj = new ServiceRepository();
                    HttpResponseMessage response = serviceObj.PutResponse("api/WebPages/UpdateWebPage", WebPages);
                    response.EnsureSuccessStatusCode();
                    UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(WebPages), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
                    return RedirectToAction("WebPages");
                }
            }
            catch (Exception ex) { }
            return View(WebPages);
        }

        public ActionResult Delete(int id)
        {
            UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(id), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.DeleteResponse("api/WebPages/DeleteWebPage?id=" + id.ToString());
            response.EnsureSuccessStatusCode();
            return RedirectToAction("WebPages");
        }
    }
}