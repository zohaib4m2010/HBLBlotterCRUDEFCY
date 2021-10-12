using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using WebBlotter.Models;
using WebBlotter.Repository;

namespace WebBlotter.Classes
{
    public class AuthAccessAttribute : AuthorizeAttribute
    {
        private readonly string[] allowedroles;
        private bool isLoggedIn;
        private bool CHangePassword;
        private bool SerssionExpired = false;
        public AuthAccessAttribute(params string[] roles)
        {
            this.allowedroles = roles;
        }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {

            bool authorize = false;
            string UserID = "";
            // Wrote If User Is Already Logged In --Arsal;
            if (httpContext.Session["UserID"] != null)
            {
                UserID = httpContext.Session["UserID"].ToString() ?? "";
                string ValidSession = System.Web.HttpContext.Current.Cache["_LoginUsersID" + UserID].ToString() ?? "";

                if ((ValidSession ?? "") != httpContext.Session.SessionID)
                {
                    SerssionExpired = true; isLoggedIn = true;
                    return authorize = false;
                }

            }

            if (httpContext.Session.Keys.Count > 0)
            {
                isLoggedIn = true;

                var routeData = ((MvcHandler)httpContext.CurrentHandler).RequestContext.RouteData;
                var actionName = routeData.Values["action"].ToString();
                var controllerName = routeData.Values["controller"].ToString();
                try
                {
                    ServiceRepository serviceObj = new ServiceRepository();
                    HttpResponseMessage response = serviceObj.PostResponse("/api/BlotterLogin/GetAllBlotterLoginById?id=" + UserID, null);
                    response.EnsureSuccessStatusCode();
                    List<Models.SP_SBPGetLoginInfo_Result> s = response.Content.ReadAsAsync<List<Models.SP_SBPGetLoginInfo_Result>>().Result;
                    foreach (var item in s)
                    {
                        if (item.UserExists == "Success")
                        {
                            httpContext.Session["UserID"] = item.ID;
                            httpContext.Session["UserName"] = item.UserName;
                            httpContext.Session["UserEmail"] = item.Email;
                            httpContext.Session["UserRole"] = item.RoleName;
                            httpContext.Session["BranchID"] = item.BranchID;
                            httpContext.Session["BranchName"] = item.BranchName;
                            httpContext.Session["Currencies"] = item.CurrencyID;
                            httpContext.Session["Pages"] = item.Pages;
                            httpContext.Session["ActiveController"] = controllerName;
                            if (item.ChangePassword)
                            {
                                CHangePassword = item.ChangePassword;
                                authorize = false;
                                return authorize;
                            }
                            List<UserPageAccess> UPA = new List<UserPageAccess>();
                            foreach (var pg in item.Pages.Split(','))
                            {
                                UserPageAccess upaobj = new UserPageAccess();
                                var val = pg.Split('~');
                                upaobj.DisplayName = val[0];
                                upaobj.PageName = val[1];
                                upaobj.ControllerName = val[2];
                                upaobj.DateChaneAccess = (val[3] == "1") ? true : false;
                                upaobj.EditAccess = (val[4] == "1") ? true : false;
                                upaobj.DeleteAccess = (val[5] == "1") ? true : false;
                                UPA.Add(upaobj);
                            }
                            httpContext.Session["PagesAccess"] = UPA;

                            #region Added By Shakir
                            if (httpContext.Session["Currencies"] != null)
                            {
                                Models.SP_GetAllBlotterCurrencyById_Result objc = new SP_GetAllBlotterCurrencyById_Result();
                                objc = UtilityClass.GetCurrencies(Convert.ToInt32(httpContext.Session["UserID"]));
                                httpContext.Session["Currencies"] = objc.Currencies;
                            }
                            #endregion
                        }
                    }
                }
                catch (Exception ex) { }

                List<UserPageAccess> permissionList = (List<UserPageAccess>)httpContext.Session["PagesAccess"];
                foreach (UserPageAccess item in permissionList)
                {
                    if (item.PageName == actionName)
                    {
                        httpContext.Session["CurrentPagesAccess"] = item.PageName + "~" + item.ControllerName + "~" + item.DateChaneAccess + "~" + item.EditAccess + "~" + item.DeleteAccess;
                        break;
                    }
                }
                if (actionName == "Edit" || actionName == "Create" || actionName == "_Create" || actionName == "Update" || actionName == "Delete" || actionName == "Reset" || actionName == "FillBlotterManualData" || actionName == "AddOpeningBalanceByBID" || actionName == "CreateOpnBal" || actionName == "EditOpeningBalance" || actionName == "UpdateOpeningBalance" || actionName == "UpdateUserPageRelation" || actionName == "GetOrgNostroBanks")
                {

                    authorize = true;
                }
                else
                {
                    authorize = permissionList.Any(item => item.PageName == actionName);
                }
            }
            else
            {
                isLoggedIn = false;
            }
            return authorize;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (isLoggedIn && !SerssionExpired)
            {
                if (CHangePassword)
                {
                    filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary
                    {
                        { "controller", "ChangePassword" },
                        { "action", "ChangePassword" }
                    });
                }
                else
                {
                    filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary
                    {
                    { "controller", "Home" },
                    { "action", "Unauthorize" }
                    });
                }
            }
            else
            {
                filterContext.Result = new RedirectToRouteResult(
                new RouteValueDictionary
                {
                    { "controller", "BlotterLogin" },
                    { "action", "logout" },
                    { "SessionExpired", (SerssionExpired ? "1" : "0") }
                });
            }
        }



        public void SetSessionStart(int UserID, string SessionID, string IP, string LoginGUID, DateTime LoginTime, DateTime Expires)
        {
            SP_ADD_SessionStart SS = new SP_ADD_SessionStart();
            SS.pUserID = UserID;
            SS.pSessionID = SessionID;
            SS.pIP = IP;
            SS.pLoginGUID = LoginGUID;
            SS.pLoginTime = LoginTime;
            SS.pExpires = Expires;
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.PostResponse("api/BlotterLogin/SessionStart", SS);
            response.EnsureSuccessStatusCode();
        }

        public void SetSessionStop(int UserID, string SessionID)
        {
            SP_ADD_SessionStart SS = new SP_ADD_SessionStart();
            SS.pUserID = UserID;
            SS.pSessionID = SessionID;
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.PostResponse("api/BlotterLogin/SessionStop", SS);
            response.EnsureSuccessStatusCode();
        }

    }
}