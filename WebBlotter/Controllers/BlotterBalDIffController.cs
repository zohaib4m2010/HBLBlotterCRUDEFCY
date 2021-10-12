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
    [AuthAccess]
    public class BlotterBalanceDifferentialController : Controller
    {

        public ActionResult BlotterBalanceDifferential(FormCollection form)
        {
            #region Added by shakir (Currency parameter)
            var selectCurrency = (dynamic)null;
            if (form["selectCurrency"] != null)
                selectCurrency = Convert.ToInt32(form["selectCurrency"].ToString());
            else
                selectCurrency = Convert.ToInt32(Session["SelectedCurrency"].ToString());

            UtilityClass.GetSelectedCurrecy(selectCurrency);

            var DateVal = (dynamic)null;
            if (form["SearchByDate"] != null)
            {
                DateVal = form["SearchByDate"].ToString();
                ViewBag.DateVal = DateVal;
            }
            else
            {
                ViewBag.DateVal = DateTime.Now.ToString("yyyy-MM-dd");
            }

            #endregion

            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.GetResponse("/api/BlotterBalanceDIfferential/GetAllblotteropeningclosingbaldiff?BranchID=" + Session["BranchID"].ToString() + "&CurID=" + selectCurrency + "&BR=" + Session["BR"].ToString() + "&DateVal=" + DateVal);
            response.EnsureSuccessStatusCode();
            List<Models.SP_GetSBPBlotterOpeningClosingBalanceDIfferential_Result> blotterBD = response.Content.ReadAsAsync<List<Models.SP_GetSBPBlotterOpeningClosingBalanceDIfferential_Result>>().Result;
            if (blotterBD.Count < 1)
                ViewData["DataStatus"] = "Data Not Availavle";
            var PAccess = Session["CurrentPagesAccess"].ToString().Split('~');
            UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(blotterBD), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());

            return PartialView("_BlotterOpenCloseBalDiff", blotterBD);
        }

        [HttpPost]
        public ActionResult Update(IEnumerable<int> BalDiffIds)
        {
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.PostResponse("api/BlotterBalanceDIfferential/UpdateOpeningClosingBalanceDifferential", BalDiffIds);
            response.EnsureSuccessStatusCode();
            UtilityClass.ActivityMonitor(Convert.ToInt32(Session["UserID"]), Session.SessionID, Request.UserHostAddress.ToString(), new Guid().ToString(), JsonConvert.SerializeObject(BalDiffIds), this.RouteData.Values["action"].ToString(), Request.RawUrl.ToString());
            return RedirectToAction("BlotterBalanceDifferential");
        }

    }
}