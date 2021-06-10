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
        public ActionResult UserRole(FormCollection form)
        {
            try
            {
                #region Added by shakir (Currency parameter)

                var selectCurrency = (dynamic)null;
                if (form["selectCurrency"] != null)
                    selectCurrency = Convert.ToInt32(form["selectCurrency"].ToString());
                else
                    selectCurrency = Convert.ToInt32(Session["SelectedCurrency"].ToString());
                UtilityClass.GetSelectedCurrecy(selectCurrency);

                #endregion

                ServiceRepository serviceObj = new ServiceRepository();
                HttpResponseMessage response = serviceObj.GetResponse("/api/UserRole/GetAllUserRole");
                response.EnsureSuccessStatusCode();
                List<Models.UserRole> UserRole = response.Content.ReadAsAsync<List<Models.UserRole>>().Result;
                UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(UserRole), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());

                ViewBag.Title = "User Role";
                return PartialView("_UserRole", UserRole);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        [HttpGet]
        public ActionResult Create()
        {
            var ActiveAction = RouteData.Values["action"].ToString();
            var ActiveController = RouteData.Values["controller"].ToString();
            Session["ActiveAction"] = ActiveController;
            Session["ActiveController"] = ActiveAction;

            UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), "", this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
            return PartialView("_Create");
        }

        public ActionResult Create(FormCollection form)
        {
            try
            {
                #region Added by shakir (Currency parameter)

                var selectCurrency = (dynamic)null;
                if (form["selectCurrency"] != null)
                    selectCurrency = Convert.ToInt32(form["selectCurrency"].ToString());
                else
                    selectCurrency = Convert.ToInt32(Session["SelectedCurrency"].ToString());
                UtilityClass.GetSelectedCurrecy(selectCurrency);

                #endregion

                UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), "", this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
                return PartialView("_Create");
            }
            catch (Exception)
            {
                throw;
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _Create(UserRole UserRole,FormCollection form)
        {
            try
            {
                #region Added by shakir (Currency parameter)

                var selectCurrency = (dynamic)null;
                if (form["selectCurrency"] != null)
                    selectCurrency = Convert.ToInt32(form["selectCurrency"].ToString());
                else
                    selectCurrency = Convert.ToInt32(Session["SelectedCurrency"].ToString());
                UtilityClass.GetSelectedCurrecy(selectCurrency);

                #endregion

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
            return PartialView("_Create", UserRole);
        }

        public ActionResult Edit(int id, FormCollection form)
        {
            #region Added by shakir (Currency parameter)

            var selectCurrency = (dynamic)null;
            if (form["selectCurrency"] != null)
                selectCurrency = Convert.ToInt32(form["selectCurrency"].ToString());
            else
                selectCurrency = Convert.ToInt32(Session["SelectedCurrency"].ToString());
            UtilityClass.GetSelectedCurrecy(selectCurrency);

            #endregion

            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.GetResponse("/api/UserRole/GetUserRole?id=" + id.ToString());
            response.EnsureSuccessStatusCode();
            Models.UserRole UserRole = response.Content.ReadAsAsync<Models.UserRole>().Result;
            UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(UserRole), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
            return PartialView("_Edit", UserRole);

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
            return PartialView("_Edit", UserRole);
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