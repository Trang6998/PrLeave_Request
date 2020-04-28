using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using System.Threading.Tasks;
using HVITCore.Controllers;
using System.Data.Entity.Infrastructure;
using CMS.Models;

namespace CMS.Controllers
{
    [RoutePrefix("api/leave_request")]
    public class Leave_RequestController : BaseApiController
    {
        [HttpGet, Route("")]
        public async Task<IHttpActionResult> Search([FromUri]Pagination pagination, [FromUri]string keywords = null)
        {
            using (var db = new ApplicationDbContext())
            {
                IQueryable<Leave_Request> results = db.Leave_Request;
                if (pagination == null)
                    pagination = new Pagination();
                if (pagination.includeEntities)
                {
                    results = results.Include(o => o.User_Leave).Include(o => o.User_Approve);
                }
                if (!string.IsNullOrWhiteSpace(keywords)) results = results.Where(o => o.User_Leave.UserName.Contains(keywords));

                results = results.OrderBy(o => o.Id);
                return Ok((await GetPaginatedResponseAsync(results, pagination)));
            }
        }

        [HttpGet, Route("{leave_RequestID:int}")]
        public async Task<IHttpActionResult> Get(int leave_RequestID)
        {
            using (var db = new ApplicationDbContext())
            {
                var leave_Request = await db.Leave_Request
                    .SingleOrDefaultAsync(o => o.Id == leave_RequestID);

                if (leave_Request == null)
                    return NotFound();

                return Ok(leave_Request);
            }
        }

        [HttpPost, Route("")]
        public async Task<IHttpActionResult> Insert([FromBody]Leave_Request leave_Request)
        {
            if (leave_Request.Id != 0) return BadRequest("Invalid Id");

            using (var db = new ApplicationDbContext())
            {
                db.Leave_Request.Add(leave_Request);
                await db.SaveChangesAsync();
            }

            return Ok(leave_Request);
        }

        [HttpPut, Route("{leave_RequestID:int}")]
        public async Task<IHttpActionResult> Update(int leave_RequestID, [FromBody]Leave_Request leave_Request)
        {
            if (leave_Request.Id != leave_RequestID) return BadRequest("Id mismatch");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            using (var db = new ApplicationDbContext())
            {
                db.Entry(leave_Request).State = EntityState.Modified;

                try
                {
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ducEx)
                {
                    bool exists = db.Leave_Request.Count(o => o.Id == leave_RequestID) > 0;
                    if (!exists)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw ducEx;
                    }
                }

                return Ok(leave_Request);
            }
        }

        [HttpDelete, Route("{leave_RequestID:int}")]
        public async Task<IHttpActionResult> Delete(int leave_RequestID)
        {
            using (var db = new ApplicationDbContext())
            {
                var leave_Request = await db.Leave_Request.SingleOrDefaultAsync(o => o.Id == leave_RequestID);

                if (leave_Request == null)
                    return NotFound();

                db.Entry(leave_Request).State = EntityState.Deleted;

                await db.SaveChangesAsync();

                return Ok();
            }
        }

    }
}
