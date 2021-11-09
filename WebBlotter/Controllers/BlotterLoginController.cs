using WebBlotter.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using WebBlotter.Models;
using WebBlotter.Classes;
using System.Web.Routing;
using System.Web.Security;

namespace WebBlotter.Controllers
{
    public class BlotterLoginController : Controller
    {
        

        public ActionResult Login()
        {
            if (Session["UserID"] != null)
            {
                Response.Redirect(new Uri(Request.Url, Url.Action("Index","Home")).ToString(), false);
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Classes.UserProfile collection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    
                    ServiceRepository serviceObj = new ServiceRepository();
                    HttpResponseMessage response = serviceObj.PostResponse("/api/BlotterLogin/GetAllBlotterLogin", collection);
                    response.EnsureSuccessStatusCode();
                    List<Models.SP_SBPGetLoginInfo_Result> s = response.Content.ReadAsAsync<List<Models.SP_SBPGetLoginInfo_Result>>().Result;
                    //var item = s.FirstOrDefault().ToString();
                    foreach (var item in s)
                    {
                        if (item.UserExists == "Success")
                        {
                            Session["UserID"] = item.ID;
                            Session["UserName"] = item.UserName;
                            Session["UserEmail"] = item.Email;
                            Session["UserRole"] = item.RoleName;
                            Session["BranchID"] = item.BranchID;
                            Session["BranchName"] = item.BranchName;
                            Session["BlotterType"] = item.BlotterType;
                            Session["Currencies"] = item.CurrencyID;
                            Session["BR"] = (item.isConventional)?"01":(item.isislamic)?"02":"00";
                            Session["Pages"] = item.Pages;
                            Session["ActiveController"] = "Login";
                            List<UserPageAccess> UPA = new List<UserPageAccess>();
                            foreach (var pg in item.Pages.Split(','))
                            {
                                UserPageAccess upaobj = new UserPageAccess();
                                var val = pg.Split('~');
                                upaobj.DisplayName = val[0];
                                upaobj.PageName = val[1];
                                upaobj.ControllerName = val[2];
                                upaobj.DateChaneAccess = (val[3]=="1")?true:false;
                                upaobj.EditAccess = (val[4] == "1") ? true : false;
                                upaobj.DeleteAccess = (val[5] == "1") ? true : false;
                                UPA.Add(upaobj);
                            }
                            Session["PagesAccess"] = UPA;
                           
                            #region Added By Shakir 
                            if (Session["Currencies"] != null)
                            {
                                if (Session["Currencies"].ToString().Contains(','))
                                    Session["SelectedCurrency"] = (Session["Currencies"].ToString().Split(',')[0]).Split('~')[0];
                                else
                                    Session["SelectedCurrency"] = Session["Currencies"].ToString().Split('~')[0];
                            }
                            #endregion

                            int timeout = 525600; //objLogin.RememberMe ? 525600 : 525600; // 525600 min = 1 year
                            var ticket = new FormsAuthenticationTicket(item.UserName, true, timeout);
                            string encrypted = FormsAuthentication.Encrypt(ticket);
                            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
                            cookie.Expires = DateTime.Now.AddMinutes(timeout);
                            cookie.HttpOnly = true;
                            Response.Cookies.Add(cookie);

                            (new AuthAccessAttribute()).SetSessionStart(item.ID, Session.SessionID, Request.UserHostAddress, new Guid().ToString(), DateTime.Now, cookie.Expires);

                            HttpContext.Cache["_LoginUsersID" + item.ID] = Session.SessionID;
                            if (item.ChangePassword)
                                Response.Redirect(new Uri(Request.Url, Url.Action("ChangePassword", "ChangePassword")).ToString(), false);
                            else
                            {
                                if (item.DefaultPage != null)
                                    Response.Redirect(new Uri(Request.Url, Url.Action(item.DefaultPage.Split('/')[1], item.DefaultPage.Split('/')[0])).ToString(), false);
                                else
                                    Response.Redirect(new Uri(Request.Url, Url.Action("Default", "Home")).ToString(), false);
                            }
                        }
                        else if (item.UserExists == "User Does not Exists")

                        {
                            ViewBag.ErrorMessage = item.UserExists;

                        }
                        else
                        {
                            ViewBag.ErrorMessage = item.UserExists;
                        }
                    }
                    ViewData["SysCurrentDt"] = GetCurrentDT().ToString("dd-MMM-yyyy");
                    return View("Login");
                }
            }
            catch (Exception ex)
            {
                //throw;
            }
            return View(collection);
        }

        public ActionResult Logout()
        {

            try
            {
                if (Session["UserID"] != null)
                {
                    (new AuthAccessAttribute()).SetSessionStop((int)(Session["UserID"]), Session.SessionID);
                }

                //SignInManager.AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                Session.Abandon();
                Session.RemoveAll();
            }

            catch (Exception ex)
            {
                //Logs.LogError("LoginControllerLogout", System.Reflection.MethodBase.GetCurrentMethod(), "Logging :: ", ex);
            }

            string SessionExpired = (Request.Params["SessionExpired"] ?? "0");

            return RedirectToAction("Login", new RouteValueDictionary(
                    new { controller = "BlotterLogin", action = "Login", SessionExpired = SessionExpired }));

            //return RedirectToAction("Index", "Login" , new { kick : "1" }); // + (kick.HasValue ? kick : "" ));
        }

        public DateTime GetCurrentDT()
        {

            try
            {
                ServiceRepositoryBlotter serviceObj = new ServiceRepositoryBlotter();
                HttpResponseMessage response = serviceObj.GetResponse("/api/BlotterDT/GetBlotterSysDT?BrCode=" + Session["BranchID"].ToString());
                response.EnsureSuccessStatusCode();
                List<Models.SP_SBPOpicsSystemDate_Result> blotterDT = response.Content.ReadAsAsync<List<Models.SP_SBPOpicsSystemDate_Result>>().Result;

                return Convert.ToDateTime(blotterDT[0].OpicsCurrentDate);


            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}