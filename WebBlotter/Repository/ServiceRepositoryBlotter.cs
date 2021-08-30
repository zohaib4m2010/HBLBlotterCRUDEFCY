using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebBlotter.Models;

namespace WebBlotter.Repository
{
    public class ServiceRepositoryBlotter
    {
        public HttpClient Client { get; set; }
        public ServiceRepositoryBlotter()
        {
            Client = new HttpClient();
            Client.BaseAddress = new Uri(ConfigurationManager.AppSettings["ServiceUrl"].ToString());
        }

        public HttpResponseMessage GetResponse(string url)
        {
            return Client.GetAsync(url).Result;
        }

        internal HttpResponseMessage PostResponse(string v, List<SP_SBPBlotter_FCY_Result> blotterDataFCY)
        {
            throw new NotImplementedException();
        }

        internal HttpResponseMessage PutResponse(string v, List<SP_SBPBlotter_FCY_Result> blotterDataFCY)
        {
            throw new NotImplementedException();
        }

        //internal HttpResponseMessage PostResponse(string v, List<SP_SBPBlotter_Result> blotterDataLCY)
        //{
        //    throw new NotImplementedException();
        //}

        //internal HttpResponseMessage PutResponse(string v, List<SP_SBPBlotter_Result> blotterDataLCY)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
