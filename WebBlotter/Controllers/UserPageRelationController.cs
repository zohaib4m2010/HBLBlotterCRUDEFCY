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
    public class UserPageRelationController : Controller
    {
        // GET: UserPageRelation
        [HttpGet]
        public ActionResult UserPageRelation()
        {
            try
            {
                var ActiveAction = RouteData.Values["action"].ToString();
                var ActiveController = RouteData.Values["controller"].ToString();
                Session["ActiveAction"] = ActiveController;
                Session["ActiveController"] = ActiveAction;

                UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), "", this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
                ViewBag.UserRoles = GetActiveUserRoles();
                return PartialView("_UserPageRelation");
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public ActionResult UserPageRelation(FormCollection form)
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
                ViewBag.UserRoles = GetActiveUserRoles();
                return PartialView("_UserPageRelation");
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        [HttpPost]
        public ActionResult UserPageRelation(FormCollection form, string a)
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

                    
                if (form["URID"] != null || form["WPID"] != null)
                {
                    var URID = Convert.ToInt32(form["URID"].ToString());
                    var WPID = Convert.ToInt32(form["WPID"].ToString());
                    ViewBag.UserRoles = GetActiveUserRoles();
                    ViewBag.SelectedURID = URID;
                    ViewBag.WebPages = GetActiveWebPages(URID);
                    ViewBag.SelectedWPID = WPID;

                    if (form.AllKeys.Contains("AddPage"))
                    {
                        if (URID != 0)
                        {
                            if (WPID != 0)
                            {

                                var DateChAcc = Convert.ToBoolean((form.AllKeys.Contains("DateChangeAcc")) ? form["DateChangeAcc"].ToString() : "false");
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
                                UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(UPR), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
                                ViewBag.UserRoles = GetActiveUserRoles();
                                ViewBag.WebPages = GetActiveWebPages(URID);
                                ViewBag.SelectedURID = URID;
                                ViewBag.SelectedWPID = 0;
                                return PartialView("_UserPageRelation", GetUserPageRaltions(URID));
                            }
                        }
                    }
                    return PartialView("_UserPageRelation", GetUserPageRaltions(URID));
                }
                else
                {
                    ViewBag.UserRoles = GetActiveUserRoles();
                }

                
            }
            catch (Exception ex)
            {
                throw;
            }
            return RedirectToAction("UserPageRelation");
        }




        public ActionResult UpdateUserPageRelation(UserPageRelation UPR)
        {

            try
            {
                ServiceRepository serviceObj = new ServiceRepository();
                HttpResponseMessage response = serviceObj.PutResponse("api/UserPageRelation/UpdateUserPageRelation", UPR);
                response.EnsureSuccessStatusCode();
                UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(UPR), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
                return RedirectToAction("UserPageRelation");
            }
            catch (Exception ex) { }

            return PartialView("_UserPageRelation");
        }

        public ActionResult Edit(int Id, FormCollection form)
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
                HttpResponseMessage response = serviceObj.GetResponse("/api/UserPageRelation/GetUserPageRaltion?UPRID=" + Id);
                response.EnsureSuccessStatusCode();
                Models.SP_GetAllUserPageRelations_Result UserPageRaltion = response.Content.ReadAsAsync<Models.SP_GetAllUserPageRelations_Result>().Result;
                UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(UserPageRaltion), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
                ViewBag.UserRoles = GetActiveUserRoles();
                ViewBag.SelectedURID = UserPageRaltion.URID;
                ViewBag.WebPages = GetWebPageByID(UserPageRaltion.WPID);
                ViewBag.SelectedWPID = UserPageRaltion.WPID;
                return PartialView("_Edit", UserPageRaltion);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public ActionResult Delete(int id)
        {
            UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(id), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
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
                UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(UserPageRaltions), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
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
                UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(WebPages), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
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
                UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(WebPages), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
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