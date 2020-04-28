using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using System.Threading.Tasks;
using HVITCore.Controllers;
using System.Data.Entity.Infrastructure;
using CMS.Models;

namespace CMS.Controllers
{
    [RoutePrefix("api/dogiat")]
    public class DoGiatController : BaseApiController
    {
        [HttpGet, Route("")]
        public async Task<IHttpActionResult> Search([FromUri]Pagination pagination, [FromUri]int? loaiDoGiatID = null, [FromUri]string keywords = null)
        {
            using (var db = new ApplicationDbContext())
            {
                IQueryable<DoGiat> results = db.DoGiat;
                if (pagination == null)
                    pagination = new Pagination();
                if (pagination.includeEntities)
                {
                    results = results.Include(o => o.LoaiDoGiat);
                }
                if (!string.IsNullOrWhiteSpace(keywords)) results = results.Where(o => o.TenDoGiat.Contains(keywords));
                if (loaiDoGiatID.HasValue) results = results.Where(o => o.LoaiDoGiatID == loaiDoGiatID);

                results = results.OrderBy(o => o.DoGiatID);
                return Ok((await GetPaginatedResponseAsync(results, pagination)));
            }
        }

        [HttpGet, Route("{doGiatID:int}")]
        public async Task<IHttpActionResult> Get(int doGiatID)
        {
            using (var db = new ApplicationDbContext())
            {
                var doGiat = await db.DoGiat
                    .Include(o => o.LoaiDoGiat)
                    .SingleOrDefaultAsync(o => o.DoGiatID == doGiatID);

                if (doGiat == null)
                    return NotFound();

                return Ok(doGiat);
            }
        }

        [HttpPost, Route("")]
        public async Task<IHttpActionResult> Insert([FromBody]DoGiat doGiat)
        {
            if (doGiat.DoGiatID != 0) return BadRequest("Invalid DoGiatID");

            using (var db = new ApplicationDbContext())
            {
                db.DoGiat.Add(doGiat);
                await db.SaveChangesAsync();
            }

            return Ok(doGiat);
        }

        [HttpPut, Route("{doGiatID:int}")]
        public async Task<IHttpActionResult> Update(int doGiatID, [FromBody]DoGiat doGiat)
        {
            if (doGiat.DoGiatID != doGiatID) return BadRequest("Id mismatch");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            using (var db = new ApplicationDbContext())
            {
                db.Entry(doGiat).State = EntityState.Modified;

                try
                {
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ducEx)
                {
                    bool exists = db.DoGiat.Count(o => o.DoGiatID == doGiatID) > 0;
                    if (!exists)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw ducEx;
                    }
                }

                return Ok(doGiat);
            }
        }

        [HttpDelete, Route("{doGiatID:int}")]
        public async Task<IHttpActionResult> Delete(int doGiatID)
        {
            using (var db = new ApplicationDbContext())
            {
                var doGiat = await db.DoGiat.SingleOrDefaultAsync(o => o.DoGiatID == doGiatID);

                if (doGiat == null)
                    return NotFound();

                if (await db.ChiTietDoGiat.AnyAsync(o => o.DoGiatID == doGiat.DoGiatID))
                    return BadRequest("Unable to delete the dogiat as it has related chitietdogiat");

                if (await db.DonGia.AnyAsync(o => o.DoGiatID == doGiat.DoGiatID))
                    return BadRequest("Unable to delete the dogiat as it has related dongia");

                db.Entry(doGiat).State = EntityState.Deleted;

                await db.SaveChangesAsync();

                return Ok();
            }
        }

    }
}
