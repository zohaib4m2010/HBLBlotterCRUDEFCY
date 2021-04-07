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
    public class UserProfileController : Controller
    {
        // GET: UsersProfile
        public ActionResult UserProfile()
        {
            try
            {
                ServiceRepository serviceObj = new ServiceRepository();
                HttpResponseMessage response = serviceObj.GetResponse("/api/UsersProfile/GetAllUsers");
                response.EnsureSuccessStatusCode();
                List<Models.sp_GetAllUsers_Result> UsersProfile = response.Content.ReadAsAsync<List<Models.sp_GetAllUsers_Result>>().Result;
                
                return View(UsersProfile);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.AllBranchNames = GetBranchesNames();
            ViewBag.UserRoles = GetUserRoles();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SBP_LoginInfo SBP_LoginInfo)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    SBP_LoginInfo.Password = Utilities.EncryptPassword(SBP_LoginInfo.Password);
                    SBP_LoginInfo.CreateDate = DateTime.Now;
                    ServiceRepository serviceObj = new ServiceRepository();
                    HttpResponseMessage response = serviceObj.PostResponse("api/UsersProfile/InsertUser", SBP_LoginInfo);
                    response.EnsureSuccessStatusCode();
                    return RedirectToAction("UserProfile");
                }
            }
            catch (Exception ex) { }
            return View(SBP_LoginInfo);
        }

        public ActionResult Edit(int id)
        {
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.GetResponse("/api/UsersProfile/GetUser?id=" + id.ToString());
            response.EnsureSuccessStatusCode();
            Models.SBP_LoginInfo SBP_LoginInfo = response.Content.ReadAsAsync<Models.SBP_LoginInfo>().Result;
            SBP_LoginInfo.Password = Utilities.DecryptPassword(SBP_LoginInfo.Password);
            ViewBag.AllBranchNames = GetBranchesNames();
            ViewBag.UserRoles = GetUserRoles();
            return View(SBP_LoginInfo);

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
                    return RedirectToAction("UserProfile");
                }
            }
            catch (Exception ex) { }
            return View(SBP_LoginInfo);
        }

        public ActionResult Delete(int id)
        {
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