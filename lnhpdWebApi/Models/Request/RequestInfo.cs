using System;
using System.Web;

namespace lnhpdWebApi.Models.Request
{
    public class RequestInfo
    {

        public RequestInfo() { }

        public int? limit = 100;

        public Nullable<int> page { get; set; }

        public string sort { get; set; }


        public string languages { get; set; }

        public string type { get; set; }

        public string path
        {
            get { return context.Request.Path; }
        }

        public HttpContext context { get; set; }
    }
}