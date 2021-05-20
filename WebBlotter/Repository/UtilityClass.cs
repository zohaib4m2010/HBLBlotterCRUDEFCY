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
    }
}