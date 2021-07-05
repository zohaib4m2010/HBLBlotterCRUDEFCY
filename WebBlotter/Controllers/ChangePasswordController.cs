using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using WebBlotter.Classes;
using WebBlotter.Models;
using WebBlotter.Repository;

namespace WebBlotter.Controllers
{
    public class ChangePasswordController : Controller
    {
        // GET: ChangePassword

        [HttpGet]
        public ActionResult ChangePassword()
        {
            ChangePassword model = new ChangePassword();
            try
            {
                UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), "", this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());

                var ActiveAction = RouteData.Values["action"].ToString();
                var ActiveController = RouteData.Values["controller"].ToString();
                Session["ActiveAction"] = ActiveAction;
                Session["ActiveController"] = ActiveController;

                if (ModelState.IsValid)
                {
                    model.Password = "";
                    model.NewPassword = "";
                    model.ConfirmPassword = "";
                }
            }
            catch (Exception) { }
            return PartialView("_ChangePassword", model);
        }

        public ActionResult ChangePassword(FormCollection form)
        {
            ChangePassword model = new ChangePassword();
            try
            {
                UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), "", this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());

                var selectCurrency = (dynamic)null;
                if (form["selectCurrency"] != null)
                    selectCurrency = Convert.ToInt32(form["selectCurrency"].ToString());
                else
                    selectCurrency = Convert.ToInt32(Session["SelectedCurrency"].ToString());
                UtilityClass.GetSelectedCurrecy(selectCurrency);
                
                if (ModelState.IsValid)
                {
                   
                }
            }
            catch (Exception) { }
            return PartialView("_ChangePassword", model);
        }

        [HttpPost]
        public ActionResult _ChangePassword(ChangePassword BlotterCP, FormCollection form)
        {
            ChangePassword model = new ChangePassword();
            try
            {
                if (ModelState.IsValid)
                {
                    BlotterCP.Password = BlotterCP.Password;
                    BlotterCP.NewPassword = BlotterCP.NewPassword;
                    BlotterCP.ConfirmPassword = BlotterCP.ConfirmPassword;
                    BlotterCP.UserId = Convert.ToInt16(Session["UserID"].ToString());
                    if (BlotterCP.Password != BlotterCP.NewPassword )
                    {
                        if (BlotterCP.NewPassword == BlotterCP.ConfirmPassword)
                        {
                            ServiceRepository serviceObj = new ServiceRepository();
                            HttpResponseMessage response = serviceObj.PutResponse("/api/ChangePassword/UpdatePassword", BlotterCP);
                            response.EnsureSuccessStatusCode();
                            UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(BlotterCP), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());

                            if (response.Content.ReadAsAsync<bool>().Result)
                            {
                                return RedirectToAction("Logout", "BlotterLogin", new { area = "" });
                            }
                            else
                            {
                                ViewBag.ErrorPwd = "Something went wrong, please try again";
                            }
                        }
                        else
                        {
                            ViewBag.ErrorPwd = "Confirm Password Not Match";
                            return PartialView("_ChangePassword", model);
                        }
                    }
                    else
                    {
                        ViewBag.ErrorPwd = "Old Password & New Password must be different";
                        return PartialView("_ChangePassword", model);
                    }
                   
                }
            }
            catch (Exception ex) {

            }
            return PartialView("_ChangePassword", model);
        }
    }
}