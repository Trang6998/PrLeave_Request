using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using System.Threading.Tasks;
using HVITCore.Controllers;
using System.Data.Entity.Infrastructure;
using CMS.Models;
using HVIT.Security;

namespace CMS.Controllers
{
    [RoutePrefix("api/luottruycap")]
    public class LuotTruyCapController : BaseApiController
    {
        [AuthorizeUser, HttpGet, Route("")]
        public async Task<IHttpActionResult> SearchAsync([FromUri]Pagination pagination, [FromUri]string q = null, [FromUri]DateTime? fromThoiGian = null, [FromUri]DateTime? toThoiGian = null)
        {
            using (var db = new ApplicationDbContext())
            {
                IQueryable<LuotTruyCap> results = db.LuotTruyCap;

                if (!string.IsNullOrWhiteSpace(q))
                    results = results.Where(o => o.IP.Contains(q) || o.ThietBi.Contains(q) || o.TrangTruyCapDauTien.Contains(q));

                if (fromThoiGian.HasValue)
                {
                    var fromThoiGianUtc = fromThoiGian.Value.ToUniversalTime(); results = results.Where(o => o.ThoiGian >= fromThoiGianUtc);
                }
                if (toThoiGian.HasValue)
                {
                    var toThoiGianUtc = toThoiGian.Value.ToUniversalTime(); results = results.Where(o => o.ThoiGian <= toThoiGianUtc);
                }
                if (fromThoiGian.HasValue)
                {
                    results = results.Where(o => o.ThoiGian >= fromThoiGian);
                }
                if (toThoiGian.HasValue)
                {
                    results = results.Where(o => o.ThoiGian <= toThoiGian);
                }

                results = results.OrderByDescending(o => o.LuotTruyCapId);

                var res = await GetPaginatedResponseAsync(results, pagination);
                return Ok(res);
            }
        }

        [AuthorizeUser, HttpGet, Route("{luotTruyCapId:int}")]
        public async Task<IHttpActionResult> GetAsync(int luotTruyCapId)
        {
            using (var db = new ApplicationDbContext())
            {
                var luotTruyCap = await db.LuotTruyCap
                    .SingleOrDefaultAsync(o => o.LuotTruyCapId == luotTruyCapId);

                if (luotTruyCap == null)
                    return NotFound();

                return Ok(luotTruyCap);
            }
        }

        [AuthorizeUser, HttpPost, Route("")]
        public async Task<IHttpActionResult> InsertAsync([FromBody]LuotTruyCap luotTruyCap)
        {
            if (luotTruyCap.LuotTruyCapId != 0)
                return BadRequest("Invalid LuotTruyCapId");

            using (var db = new ApplicationDbContext())
            {
                db.LuotTruyCap.Add(luotTruyCap);
                await db.SaveChangesAsync();
            }

            return Ok(luotTruyCap);
        }

        [AuthorizeUser, HttpPut, Route("{luotTruyCapId:int}")]
        public async Task<IHttpActionResult> UpdateAsync(int luotTruyCapId, [FromBody]LuotTruyCap luotTruyCap)
        {
            if (luotTruyCap.LuotTruyCapId != luotTruyCapId)
                return BadRequest("Id mismatch");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            using (var db = new ApplicationDbContext())
            {
                bool exists = db.LuotTruyCap.Count(o => o.LuotTruyCapId == luotTruyCapId) > 0;
                if (!exists)
                {
                    return NotFound();
                }
                db.Entry(luotTruyCap).State = EntityState.Modified;

                await db.SaveChangesAsync();

                return Ok(luotTruyCap);
            }
        }

        [AuthorizeUser, HttpDelete, Route("{luotTruyCapId:int}")]
        public async Task<IHttpActionResult> DeleteAsync(int luotTruyCapId)
        {
            using (var db = new ApplicationDbContext())
            {
                var luotTruyCap = await db.LuotTruyCap.SingleOrDefaultAsync(o => o.LuotTruyCapId == luotTruyCapId);

                if (luotTruyCap == null)
                    return NotFound();

                db.Entry(luotTruyCap).State = EntityState.Deleted;

                await db.SaveChangesAsync();

                return Ok();
            }
        }

    }
}
