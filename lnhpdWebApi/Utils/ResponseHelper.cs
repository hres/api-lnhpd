using lnhpdWebApi.Models.Request;
using lnhpdWebApi.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace lnhpdWebApi.Utils
{
    public static class ResponseHelper
    {

        const string basepath = "/api/natural-licences";

        public static Pagination PaginationFactory(RequestInfo requestInfo, int limit, int page, int count)
        {
            var pagination = new Pagination();
            pagination.limit = limit;
            pagination.page = page;
            pagination.total = count;
            pagination.next = getNextPage(requestInfo, limit, page, count);
            pagination.previous = getPreviousPage(requestInfo, limit, page, count);

            return pagination;
        }

        private static string getNextPage(RequestInfo requestInfo, int limit, int page, int count)
        {
            var next = buildBasePath(requestInfo);
            // page indexing starts at 1
            if ((page) * requestInfo.limit >= count) return null;
            next += $"?page={page + 1}&lang={requestInfo.languages}&type={requestInfo.type}";
            return next;
        }

        private static string getPreviousPage(RequestInfo requestInfo, int limit, int page, int count)
        {
            var previous = buildBasePath(requestInfo);
            if (page <= 1) return null;
            previous += $"?page={page - 1}&lang={requestInfo.languages}&type={requestInfo.type}";

            return previous;
        }

        private static string buildBasePath(RequestInfo requestInfo)
        {
            String requestPath = requestInfo.context.Request.Path;
            String applicationPath = requestInfo.context.Request.ApplicationPath;
            String endpoint = requestPath.Replace(applicationPath, "");
            // external url points to a reverse proxy for the IIS url, the two urls path don't match, so currently cannot get the orginal request base path from the IIS path, 
            // for now hardcode the basepath when it runns on servers
            return requestInfo.context.Request.IsLocal ? requestInfo.path : (basepath + endpoint);
        }
    }
}