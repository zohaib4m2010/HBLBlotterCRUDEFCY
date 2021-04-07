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

namespace WebBlotter.Controllers
{
    public class BlotterLoginController : Controller
    {
        string BrCode = System.Configuration.ConfigurationManager.AppSettings["BranchCode"].ToString();



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
                    collection.Password = Utilities.EncryptPassword(collection.Password);
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
                            Session["Currencies"] = item.Currencies;
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
                                if (item.Currencies.Contains(','))
                                Session["SelectedCurrency"] = (item.Currencies.Split(',')[0]).Split('~')[0];
                            else
                                Session["SelectedCurrency"] = item.Currencies.Split('~')[0];

                            //(new AuthAccessAttribute()).SetSessionStart(oUser.UserID, Session.SessionID, Request.UserHostAddress, new Guid().ToString(), u.LoginTime, cookie.Expires);

                            HttpContext.Cache["_LoginUsersID" + item.ID] = Session.SessionID;
                            Response.Redirect(new Uri(Request.Url, Url.Action("Index", "Home")).ToString(), false);
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
                //if (Session["UserID"] != null)
                //{
                //    (new AuthAccessAttribute()).SetSessionStop((int)(Session["UserID"]), Session.SessionID);
                //}

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
                HttpResponseMessage response = serviceObj.GetResponse("/api/BlotterDT/GetBlotterSysDT?BrCode=" + BrCode);
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