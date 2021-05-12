using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
    }
}