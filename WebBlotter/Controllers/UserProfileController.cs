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
    public class UserProfileController : Controller
    {
        // GET: UsersProfile
        public ActionResult UserProfile(FormCollection form)
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
                HttpResponseMessage response = serviceObj.GetResponse("/api/UsersProfile/GetAllUsers");
                response.EnsureSuccessStatusCode();
                List<Models.sp_GetAllUsers_Result> UsersProfile = response.Content.ReadAsAsync<List<Models.sp_GetAllUsers_Result>>().Result;
                UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(UsersProfile), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());

                return PartialView("_UserProfile", UsersProfile);
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
            ViewBag.AllBranchNames = GetBranchesNames();
            ViewBag.UserRoles = GetUserRoles();
            return PartialView("_Create");
        }
        public ActionResult Create(FormCollection form)
        {
            try
            {
                UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), "", this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());

                #region Added by shakir (Currency parameter)

                var selectCurrency = (dynamic)null;
                if (form["selectCurrency"] != null)
                    selectCurrency = Convert.ToInt32(form["selectCurrency"].ToString());
                else
                    selectCurrency = Convert.ToInt32(Session["SelectedCurrency"].ToString());
                UtilityClass.GetSelectedCurrecy(selectCurrency);

                #endregion

                UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), "", this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
                ViewBag.AllBranchNames = GetBranchesNames();
                ViewBag.UserRoles = GetUserRoles();
                return PartialView();
            }
            catch (Exception ex) { }

            return PartialView("_Create");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _Create(SBP_LoginInfo SBP_LoginInfo, FormCollection form)
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
                    SBP_LoginInfo.Password = Utilities.EncryptPassword(SBP_LoginInfo.Password);
                    SBP_LoginInfo.CreateDate = DateTime.Now;
                    ServiceRepository serviceObj = new ServiceRepository();
                    HttpResponseMessage response = serviceObj.PostResponse("api/UsersProfile/InsertUser", SBP_LoginInfo);
                    response.EnsureSuccessStatusCode();
                    UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(SBP_LoginInfo), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
                    return RedirectToAction("UserProfile");
                }
            }
            catch (Exception ex) { }
            return PartialView("_Create", SBP_LoginInfo);
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
            HttpResponseMessage response = serviceObj.GetResponse("/api/UsersProfile/GetUser?id=" + id.ToString());
            response.EnsureSuccessStatusCode();
            Models.SBP_LoginInfo SBP_LoginInfo = response.Content.ReadAsAsync<Models.SBP_LoginInfo>().Result;
            SBP_LoginInfo.Password = Utilities.DecryptPassword(SBP_LoginInfo.Password);
            UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(SBP_LoginInfo), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
            ViewBag.AllBranchNames = GetBranchesNames();
            ViewBag.UserRoles = GetUserRoles();
            return PartialView("_Edit", SBP_LoginInfo);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(Models.SBP_LoginInfo SBP_LoginInfo)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    SBP_LoginInfo.Password = Utilities.EncryptPassword(SBP_LoginInfo.Password);
                    SBP_LoginInfo.UpdateDate = DateTime.Now;
                    ServiceRepository serviceObj = new ServiceRepository();
                    HttpResponseMessage response = serviceObj.PutResponse("api/UsersProfile/UpdateUser", SBP_LoginInfo);
                    response.EnsureSuccessStatusCode();
                    UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(SBP_LoginInfo), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
                    return RedirectToAction("UserProfile");
                }
            }
            catch (Exception ex) { }
            return PartialView("UserProfile", SBP_LoginInfo);
        }

        public ActionResult Delete(int id)
        {
            UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(id), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.DeleteResponse("api/UsersProfile/DeleteUser?id=" + id.ToString());
            response.EnsureSuccessStatusCode();
            return RedirectToAction("UserProfile");
        }

        private List<Models.Branches> GetBranchesNames() {

            try
            {
                ServiceRepository serviceObj = new ServiceRepository();
                HttpResponseMessage response = serviceObj.GetResponse("/api/Branches/GetAllBranches");
                response.EnsureSuccessStatusCode();
                List<Models.Branches> Branches = response.Content.ReadAsAsync<List<Models.Branches>>().Result;

                return Branches;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private List<Models.UserRole> GetUserRoles()
        {

            try
            {
                ServiceRepository serviceObj = new ServiceRepository();
                HttpResponseMessage response = serviceObj.GetResponse("/api/UsersProfile/GetUserRoles");
                response.EnsureSuccessStatusCode();
                List<Models.UserRole> UserRole = response.Content.ReadAsAsync<List<Models.UserRole>>().Result;

                return UserRole;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}