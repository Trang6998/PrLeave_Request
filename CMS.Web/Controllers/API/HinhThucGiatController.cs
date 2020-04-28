using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using System.Threading.Tasks;
using HVITCore.Controllers;
using System.Data.Entity.Infrastructure;
using CMS.Models;

namespace CMS.Controllers
{
    [RoutePrefix("api/hinhthucgiat")]
    public class HinhThucGiatController : BaseApiController
    {
        [HttpGet, Route("")]
        public async Task<IHttpActionResult> Search([FromUri]Pagination pagination, [FromUri] string keywords = null)
        {
            using (var db = new ApplicationDbContext())
            {
                IQueryable<HinhThucGiat> results = db.HinhThucGiat;

                if (!string.IsNullOrWhiteSpace(keywords))
                    results = results.Where(x => x.TenHinhThuc.Contains(keywords));
                results = results.OrderBy(o => o.HinhThucGiatID);

                return Ok((await GetPaginatedResponseAsync(results, pagination)));
            }
        }

        [HttpGet, Route("{hinhThucGiatID:int}")]
        public async Task<IHttpActionResult> Get(int hinhThucGiatID)
        {
            using (var db = new ApplicationDbContext())
            {
                var hinhThucGiat = await db.HinhThucGiat
                    .SingleOrDefaultAsync(o => o.HinhThucGiatID == hinhThucGiatID);

                if (hinhThucGiat == null)
                    return NotFound();

                return Ok(hinhThucGiat);
            }
        }

        [HttpPost, Route("")]
        public async Task<IHttpActionResult> Insert([FromBody]HinhThucGiat hinhThucGiat)
        {
            if (hinhThucGiat.HinhThucGiatID != 0) return BadRequest("Invalid HinhThucGiatID");

            using (var db = new ApplicationDbContext())
            {
                db.HinhThucGiat.Add(hinhThucGiat);
                await db.SaveChangesAsync();
            }

            return Ok(hinhThucGiat);
        }

        [HttpPut, Route("{hinhThucGiatID:int}")]
        public async Task<IHttpActionResult> Update(int hinhThucGiatID, [FromBody]HinhThucGiat hinhThucGiat)
        {
            if (hinhThucGiat.HinhThucGiatID != hinhThucGiatID) return BadRequest("Id mismatch");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            using (var db = new ApplicationDbContext())
            {
                db.Entry(hinhThucGiat).State = EntityState.Modified;

                try
                {
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ducEx)
                {
                    bool exists = db.HinhThucGiat.Count(o => o.HinhThucGiatID == hinhThucGiatID) > 0;
                    if (!exists)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw ducEx;
                    }
                }

                return Ok(hinhThucGiat);
            }
        }

        [HttpDelete, Route("{hinhThucGiatID:int}")]
        public async Task<IHttpActionResult> Delete(int hinhThucGiatID)
        {
            using (var db = new ApplicationDbContext())
            {
                var hinhThucGiat = await db.HinhThucGiat.SingleOrDefaultAsync(o => o.HinhThucGiatID == hinhThucGiatID);

                if (hinhThucGiat == null)
                    return NotFound();

                if (await db.DonGia.AnyAsync(o => o.HinhThucGiatID == hinhThucGiat.HinhThucGiatID))
                    return BadRequest("Unable to delete the hinhthucgiat as it has related dongia");

                db.Entry(hinhThucGiat).State = EntityState.Deleted;

                await db.SaveChangesAsync();

                return Ok();
            }
        }

    }
}
