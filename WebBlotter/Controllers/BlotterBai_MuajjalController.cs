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
    public class BlotterBai_MuajjalController : Controller
    {
        UtilityClass UC = new UtilityClass();
        // GET: BlotterBai_Muajjal

        public ActionResult BlotterBai_Muajjal(FormCollection form)
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
                HttpResponseMessage response = serviceObj.GetResponse("/api/BlotterBai_Muajjal/GetAllBlotterBai_Muajjal?UserID=" + Session["UserID"].ToString() + "&BranchID=" + Session["BranchID"].ToString() + "&CurID=" + Session["SelectedCurrency"].ToString() + "&BR=" + Session["BR"].ToString());
                response.EnsureSuccessStatusCode();
                List<Models.SBP_BlotterBai_Muajjal> blotterBai_Muajjal = response.Content.ReadAsAsync<List<Models.SBP_BlotterBai_Muajjal>>().Result;
                var PAccess = Session["CurrentPagesAccess"].ToString().Split('~');
                UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(blotterBai_Muajjal), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());

                ViewData["isDateChangable"] = Convert.ToBoolean(PAccess[2]);
                ViewData["isEditable"] = Convert.ToBoolean(PAccess[3]);
                ViewData["IsDeletable"] = Convert.ToBoolean(PAccess[4]);
                ViewBag.Title = "All Blotter Setup";
                return PartialView("_BlotterBai_Muajjal", blotterBai_Muajjal);
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
            SBP_BlotterBai_Muajjal model = new SBP_BlotterBai_Muajjal();

            model.ValueDate = DateTime.Now.Date;



            return PartialView("_Create", model);
        }
        public ActionResult Create(FormCollection form)
        {
            #region Added by shakir (Currency parameter)

            var selectCurrency = (dynamic)null;
            if (form["selectCurrency"] != null)
                selectCurrency = Convert.ToInt32(form["selectCurrency"].ToString());
            else
                selectCurrency = Convert.ToInt32(Session["SelectedCurrency"].ToString());
            UtilityClass.GetSelectedCurrecy(selectCurrency);

            #endregion

            SBP_BlotterBai_Muajjal model = new SBP_BlotterBai_Muajjal();
            try
            {
                UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), "", this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
                model.ValueDate = DateTime.Now.Date;
            }
            catch (Exception ex) { }

            return PartialView("_Create", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _Create(SBP_BlotterBai_Muajjal BlotterBai_Muajjal, FormCollection form)
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
                    BlotterBai_Muajjal.BM_OutFLow = UC.CheckNegativeValue(BlotterBai_Muajjal.BM_OutFLow);
                    BlotterBai_Muajjal.UserID = Convert.ToInt16(Session["UserID"].ToString());
                    BlotterBai_Muajjal.BID = Convert.ToInt16(Session["BranchID"].ToString());
                    BlotterBai_Muajjal.BR = Convert.ToInt16(Session["BR"].ToString());
                    BlotterBai_Muajjal.CurID = Convert.ToInt16(Session["SelectedCurrency"].ToString());
                    BlotterBai_Muajjal.CreateDate = DateTime.Now;
                    ServiceRepository serviceObj = new ServiceRepository();
                    HttpResponseMessage response = serviceObj.PostResponse("api/BlotterBai_Muajjal/InsertBai_Muajjal", BlotterBai_Muajjal);
                    response.EnsureSuccessStatusCode();
                    UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(BlotterBai_Muajjal), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
                    return RedirectToAction("BlotterBai_Muajjal");
                }
            }
            catch (Exception ex) { }
            return PartialView("_Create", BlotterBai_Muajjal);
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
            HttpResponseMessage response = serviceObj.GetResponse("/api/BlotterBai_Muajjal/GetBlotterBai_Muajjal?id=" + id.ToString());
            response.EnsureSuccessStatusCode();
            Models.SBP_BlotterBai_Muajjal BlotterBai_Muajjal = response.Content.ReadAsAsync<Models.SBP_BlotterBai_Muajjal>().Result;
            UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(BlotterBai_Muajjal), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());

            var isDateChangable = Convert.ToBoolean(Session["CurrentPagesAccess"].ToString().Split('~')[2]);
            ViewData["isDateChangable"] = isDateChangable;
            return PartialView("_Edit", BlotterBai_Muajjal);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(Models.SBP_BlotterBai_Muajjal BlotterBai_Muajjal)
        {
            BlotterBai_Muajjal.BM_OutFLow = UC.CheckNegativeValue(BlotterBai_Muajjal.BM_OutFLow);
            BlotterBai_Muajjal.UpdateDate = DateTime.Now;
            if (BlotterBai_Muajjal.ValueDate == null)
                BlotterBai_Muajjal.ValueDate = DateTime.Now;
            BlotterBai_Muajjal.CurID = Convert.ToInt16(Session["SelectedCurrency"].ToString());
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.PutResponse("api/BlotterBai_Muajjal/UpdateBai_Muajjal", BlotterBai_Muajjal);
            response.EnsureSuccessStatusCode();
            UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(BlotterBai_Muajjal), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
            return RedirectToAction("BlotterBai_Muajjal");
        }

        public ActionResult Delete(int id)
        {
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.DeleteResponse("api/BlotterBai_Muajjal/DeleteBai_Muajjal?id=" + id.ToString());
            response.EnsureSuccessStatusCode();
            UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(id), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
            return RedirectToAction("BlotterBai_Muajjal");
        }
    }
}
