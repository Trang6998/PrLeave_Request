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
    [RoutePrefix("api/loaidogiat")]
    public class LoaiDoGiatController : BaseApiController
    {
        [HttpGet, Route("")]
        public async Task<IHttpActionResult> Search([FromUri]Pagination pagination, [FromUri]string keywords = null)
        {
            using (var db = new ApplicationDbContext())
            {
                IQueryable<LoaiDoGiat> results = db.LoaiDoGiat;
                if (!string.IsNullOrWhiteSpace(keywords))
                    results = results.Where(x => x.TenLoaiDoGiat.Contains(keywords));
                results = results.OrderBy(o => o.LoaiDoGiatID);

                return Ok((await GetPaginatedResponseAsync(results, pagination)));
            }
        }

        [HttpGet, Route("{loaiDoGiatID:int}")]
        public async Task<IHttpActionResult> Get(int loaiDoGiatID)
        {
            using (var db = new ApplicationDbContext())
            {
                var loaiDoGiat = await db.LoaiDoGiat
                    .SingleOrDefaultAsync(o => o.LoaiDoGiatID == loaiDoGiatID);

                if (loaiDoGiat == null)
                    return NotFound();

                return Ok(loaiDoGiat);
            }
        }

        [HttpPost, Route("")]
        public async Task<IHttpActionResult> Insert([FromBody]LoaiDoGiat loaiDoGiat)
        {
            if (loaiDoGiat.LoaiDoGiatID != 0) return BadRequest("Invalid LoaiDoGiatID");

            using (var db = new ApplicationDbContext())
            {
                db.LoaiDoGiat.Add(loaiDoGiat);
                await db.SaveChangesAsync();
            }

            return Ok(loaiDoGiat);
        }

        [HttpPut, Route("{loaiDoGiatID:int}")]
        public async Task<IHttpActionResult> Update(int loaiDoGiatID, [FromBody]LoaiDoGiat loaiDoGiat)
        {
            if (loaiDoGiat.LoaiDoGiatID != loaiDoGiatID) return BadRequest("Id mismatch");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            using (var db = new ApplicationDbContext())
            {
                db.Entry(loaiDoGiat).State = EntityState.Modified;

                try
                {
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ducEx)
                {
                    bool exists = db.LoaiDoGiat.Count(o => o.LoaiDoGiatID == loaiDoGiatID) > 0;
                    if (!exists)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw ducEx;
                    }
                }

                return Ok(loaiDoGiat);
            }
        }

        [HttpDelete, Route("{loaiDoGiatID:int}")]
        public async Task<IHttpActionResult> Delete(int loaiDoGiatID)
        {
            using (var db = new ApplicationDbContext())
            {
                var loaiDoGiat = await db.LoaiDoGiat.SingleOrDefaultAsync(o => o.LoaiDoGiatID == loaiDoGiatID);

                if (loaiDoGiat == null)
                    return NotFound();

                if (await db.DoGiat.AnyAsync(o => o.LoaiDoGiatID == loaiDoGiat.LoaiDoGiatID))
                    return BadRequest("Unable to delete the loaidogiat as it has related dogiat");

                db.Entry(loaiDoGiat).State = EntityState.Deleted;

                await db.SaveChangesAsync();

                return Ok();
            }
        }

    }
}
