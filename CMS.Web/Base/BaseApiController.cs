using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web;
using System.Net.Http;
using System.Threading;
using System.Net;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using System.Web.Http.Controllers;
using System.Security.Claims;

namespace HVITCore.Controllers
{
    public class BaseApiController : ApiController
    {
        protected async Task<PaginatedResponse<T>> GetPaginatedResponseAsync<T>(IQueryable<T> query, Pagination pagination)
        {
            if (pagination == null) pagination = new Pagination();

            var totalRecords = query.Count();
            var totalPages = (int)Math.Ceiling((double)totalRecords / pagination.rowsPerPage);

            pagination.page = pagination.page < 1 ? 0 : pagination.page - 1;

            var results = await (pagination.rowsPerPage <= 0
                ? query.ToListAsync()
                : query.Skip(pagination.rowsPerPage * pagination.page)
                    .Take(pagination.rowsPerPage)
                    .ToListAsync());

            var paginationHeader = new Pagination ()
            {
                page = pagination.page,
                rowsPerPage = pagination.rowsPerPage,
                records = results.Count,
                totalItems = totalRecords,
                totalPages = totalPages
            };

            HttpContext.Current.Response.Headers.Add("X-Pagination", Newtonsoft.Json.JsonConvert.SerializeObject(paginationHeader));

            return new PaginatedResponse<T>() {
                Pagination = paginationHeader,
                Data = results
            };
        }
    }
}