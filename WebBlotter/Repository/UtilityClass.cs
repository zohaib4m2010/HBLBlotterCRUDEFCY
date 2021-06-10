using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using WebBlotter.Models;

namespace WebBlotter.Repository
{
    public class UtilityClass
    {
        public decimal? CheckNegativeValue(decimal? val)
        {

            if (val > 0)
            {
                val = val * -1;
            }
            return val;
        }


        public static void ActivityMonitor(int UserID, string SessionID, string IP, string LoginGUID, string Data, string Activity, string URL)
        {
            SP_ADD_SessionStart SS = new SP_ADD_SessionStart();
            SS.pUserID = UserID;
            SS.pSessionID = SessionID;
            SS.pIP = IP;
            SS.pLoginGUID = LoginGUID;
            SS.pData = Data;
            SS.pActivity = Activity;
            SS.pURL = URL;
            ServiceRepository serviceObj = new ServiceRepository();
            HttpResponseMessage response = serviceObj.PostResponse("api/BlotterLogin/ActivityMonitor", SS);
            response.EnsureSuccessStatusCode();
        }

        public static Models.SP_GetAllBlotterCurrencyById_Result GetCurrencies(int userid)
        {
            try
            {
                ServiceRepository serviceObj = new ServiceRepository();
                HttpResponseMessage response = serviceObj.GetResponse("/api/BlotterCurrency/GetAllCurrencies?userid=" + userid);
                response.EnsureSuccessStatusCode();
                Models.SP_GetAllBlotterCurrencyById_Result Currencies = response.Content.ReadAsAsync<Models.SP_GetAllBlotterCurrencyById_Result>().Result;
                return Currencies;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static void GetSelectedCurrecy(int curr)
        {

            var routeValues = HttpContext.Current.Request.RequestContext.RouteData.Values;
            var ActiveAction = routeValues["action"].ToString();
            var ActiveController = routeValues["controller"].ToString();
            HttpContext.Current.Session["ActiveAction"] = ActiveController;
            HttpContext.Current.Session["ActiveController"] = ActiveAction;

            int selectCurrency = curr;
            if (selectCurrency > 1)
                HttpContext.Current.Session["SelectedCurrency"] = selectCurrency;
            else
                selectCurrency = Convert.ToInt32(HttpContext.Current.Session["SelectedCurrency"]);

            //return selectCurrency;
        }
    }
}