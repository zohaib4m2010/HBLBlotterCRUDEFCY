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
                            httpContext.Session["Currencies"] = item.Currencies;
                            httpContext.Session["Pages"] = item.Pages;
                            httpContext.Session["ActiveController"] = controllerName;
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
                            if (item.Currencies.Contains(','))
                                httpContext.Session["SelectedCurrency"] = (item.Currencies.Split(',')[0]).Split('~')[0];
                            else
                                httpContext.Session["SelectedCurrency"] = item.Currencies.Split('~')[0];
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
                foreach (var item in permissionList)
                {
                    if (item.PageName != actionName) {
                        authorize = true;
                        break;
                    }
                }

                //authorize = permissionList.Any(item => item.FormsName == actionName);
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
                filterContext.Result = new RedirectToRouteResult(
                new RouteValueDictionary
                {
                    { "controller", "Home" },
                    { "action", "Unauthorize" }
                });
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
        


        //public void SetSessionStart(int UserID, string SessionID, string IP, string LoginGUID, DateTime LoginTime, DateTime Expires)
        //{
        //    List<ParameterInfo> parameters = new List<ParameterInfo>();

        //    parameters.Add(new ParameterInfo() { ParameterName = "pSessionID", ParameterValue = SessionID });
        //    parameters.Add(new ParameterInfo() { ParameterName = "pUserID", ParameterValue = UserID });
        //    parameters.Add(new ParameterInfo() { ParameterName = "pIP", ParameterValue = IP });
        //    parameters.Add(new ParameterInfo() { ParameterName = "pLoginGUID", ParameterValue = LoginGUID });
        //    parameters.Add(new ParameterInfo() { ParameterName = "pLoginTime", ParameterValue = LoginTime });
        //    parameters.Add(new ParameterInfo() { ParameterName = "pExpires", ParameterValue = Expires });

        //    int success = SqlHelper.ExecuteQuery("SP_ADD_SessionStart", parameters);
        //}

        //public void SetSessionStop(int UserID, string SessionID)
        //{
        //    List<ParameterInfo> parameters = new List<ParameterInfo>();
        //    parameters.Add(new ParameterInfo() { ParameterName = "pUserID", ParameterValue = UserID });
        //    parameters.Add(new ParameterInfo() { ParameterName = "pSessionID", ParameterValue = SessionID });

        //    int success = SqlHelper.ExecuteQuery("SP_SessionStop", parameters);
        //}
    }
}