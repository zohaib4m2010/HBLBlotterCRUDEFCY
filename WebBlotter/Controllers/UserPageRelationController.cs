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
    public class UserPageRelationController : Controller
    {
        // GET: UserPageRelation

        public ActionResult UserPageRelation()
        {
            try
            {
                ViewBag.UserRoles = GetActiveUserRoles();
                return View();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpPost]
        public ActionResult UserPageRelation(FormCollection form)
        {
            try
            {
                var URID = Convert.ToInt32(form["URID"].ToString());
                var WPID = Convert.ToInt32(form["WPID"].ToString());
                ViewBag.UserRoles = GetActiveUserRoles();
                ViewBag.SelectedURID = URID;
                ViewBag.WebPages = GetActiveWebPages(URID);
                ViewBag.SelectedWPID = WPID;
                
                if (form.AllKeys.Contains("AddPage")) {
                    if (URID != 0) {
                        if (WPID != 0)
                        {

                            var DateChAcc = Convert.ToBoolean((form.AllKeys.Contains("DateChangeAcc"))?form["DateChangeAcc"].ToString():"false");
                            var EditAcc = Convert.ToBoolean((form.AllKeys.Contains("EditAcc")) ? form["EditAcc"].ToString() : "false");
                            var DelAcc = Convert.ToBoolean((form.AllKeys.Contains("DelAcc")) ? form["DelAcc"].ToString() : "false");
                            UserPageRelation UPR = new UserPageRelation();
                            UPR.URID = URID;
                            UPR.WPID = WPID;
                            UPR.DateChangeAccess = DateChAcc;
                            UPR.EditAccess = EditAcc;
                            UPR.DeleteAccess = DelAcc;
                            ServiceRepository serviceObj = new ServiceRepository();
                            HttpResponseMessage response = serviceObj.PostResponse("api/UserPageRelation/InsertUserPageRelation", UPR);
                            response.EnsureSuccessStatusCode();
                            ViewBag.UserRoles = GetActiveUserRoles();
                            ViewBag.WebPages = GetActiveWebPages(URID);
                            ViewBag.SelectedURID = URID;
                            ViewBag.SelectedWPID = 0;
                        }
                    }
                }
                return View(GetUserPageRaltions(URID));
            }
            catch (Exception ex)
            {
                throw;
            }
        }




        public ActionResult UpdateUserPageRelation(UserPageRelation UPR) {

            try
            {
                ServiceRepository serviceObj = new ServiceRepository();
                HttpResponseMessage response = serviceObj.PutResponse("api/UserPageRelation/UpdateUserPageRelation", UPR);
                response.EnsureSuccessStatusCode();
                return RedirectToAction("UserPageRelation");
            }
            catch (Exception ex) { }

            return View();
        }

        public ActionResult Edit(int Id)
        {
            try
            {
                ServiceRepository serviceObj = new ServiceRepository();
                HttpResponseMessage response = serviceObj.GetResponse("/api/UserPageRelation/GetUserPageRaltion?UPRID=" + Id);
                response.EnsureSuccessStatusCode();
                Models.SP_GetAllUserPageRelations_Result UserPageRaltion = response.Content.ReadAsAsync<Models.SP_GetAllUserPageRelations_Result>().Result;
                ViewBag.UserRoles = GetActiveUserRoles();
                ViewBag.SelectedURID = UserPageRaltion.URID;
                ViewBag.WebPages = GetWebPageByID(UserPageRaltion.WPID);
                ViewBag.SelectedWPID = UserPageRaltion.WPID;

                return View(UserPageRaltion);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public ActionResult Delete(int id)
        {
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.DeleteResponse("api/UserPageRelation/DeleteUserPageRelation?UPRID=" + id.ToString());
            response.EnsureSuccessStatusCode();
            return RedirectToAction("UserPageRelation");
        }
        public List<Models.SP_GetAllUserPageRelations_Result> GetUserPageRaltions(int URID)
        {
            try
            {
                ServiceRepository serviceObj = new ServiceRepository();
                HttpResponseMessage response = serviceObj.GetResponse("/api/UserPageRelation/GetUserPageRaltions?URID=" + URID);
                response.EnsureSuccessStatusCode();
                List<Models.SP_GetAllUserPageRelations_Result> UserPageRaltions = response.Content.ReadAsAsync<List<Models.SP_GetAllUserPageRelations_Result>>().Result;
                return UserPageRaltions;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        

            public Models.WebPages GetWebPageByID(int WPID)
        {
            try
            {
                ServiceRepository serviceObj = new ServiceRepository();
                HttpResponseMessage response = serviceObj.GetResponse("/api/UserPageRelation/GetWebPageById?WPID=" + WPID);
                response.EnsureSuccessStatusCode();
                Models.WebPages WebPages = response.Content.ReadAsAsync<Models.WebPages>().Result;
                return WebPages;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public List<Models.WebPages> GetActiveWebPages(int URID)
        {
            try
            {
                ServiceRepository serviceObj = new ServiceRepository();
                HttpResponseMessage response = serviceObj.GetResponse("/api/UserPageRelation/GetActiveWebPages?URID=" + URID);
                response.EnsureSuccessStatusCode();
                List<Models.WebPages> WebPages = response.Content.ReadAsAsync<List<Models.WebPages>>().Result;
                return WebPages;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public List<Models.UserRole> GetActiveUserRoles()
        {
            try
            {
                ServiceRepository serviceObj = new ServiceRepository();
                HttpResponseMessage response = serviceObj.GetResponse("/api/UserPageRelation/GetAllActiveUserRole");
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