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
    public class UserRoleController : Controller
    {
        // GET: UserRole
        public ActionResult UserRole()
        {
            try
            {
                ServiceRepository serviceObj = new ServiceRepository();
                HttpResponseMessage response = serviceObj.GetResponse("/api/UserRole/GetAllUserRole");
                response.EnsureSuccessStatusCode();
                List<Models.UserRole> UserRole = response.Content.ReadAsAsync<List<Models.UserRole>>().Result;
                UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(UserRole), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());

                ViewBag.Title = "User Role";
                return View(UserRole);
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
        public ActionResult Create(UserRole UserRole)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    UserRole.CreateDate = DateTime.Now;
                    ServiceRepository serviceObj = new ServiceRepository();
                    HttpResponseMessage response = serviceObj.PostResponse("api/UserRole/InsertUserRole", UserRole);
                    response.EnsureSuccessStatusCode();
                    UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(UserRole), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
                    return RedirectToAction("UserRole");
                }
            }
            catch (Exception ex) { }
            return View(UserRole);
        }

        public ActionResult Edit(int id)
        {
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.GetResponse("/api/UserRole/GetUserRole?id=" + id.ToString());
            response.EnsureSuccessStatusCode();
            Models.UserRole UserRole = response.Content.ReadAsAsync<Models.UserRole>().Result;
            UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(UserRole), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
            return View(UserRole);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(UserRole UserRole)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    UserRole.UpdateDate = DateTime.Now;
                    ServiceRepository serviceObj = new ServiceRepository();
                    HttpResponseMessage response = serviceObj.PutResponse("api/UserRole/UpdateUserRole", UserRole);
                    response.EnsureSuccessStatusCode();
                    UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(UserRole), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
                    return RedirectToAction("UserRole");
                }
            }
            catch (Exception ex) { }
            return View(UserRole);
        }

        public ActionResult Delete(int id)
        {
            UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(id), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.DeleteResponse("api/UserRole/DeleteUserRole?id=" + id.ToString());
            response.EnsureSuccessStatusCode();
            return RedirectToAction("UserRole");
        }
    }
}