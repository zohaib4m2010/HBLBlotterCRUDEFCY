using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBlotter.Classes
{
    public class WebApiResponse
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public string Data { get; set; }
    }
}