using CMS.Models;
using HVITCore.Controllers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http; 

namespace CMS.Controllers
{
    [RoutePrefix("api/lienhe")]
    public class LienHeController : BaseApiController
    {
        [HttpGet, Route("")]
        public async Task<IHttpActionResult> Search([FromUri]Pagination pagination, [FromUri]int? lienHeID = null, [FromUri]string keywords = null)
        {
            using (var db = new ApplicationDbContext())
            {
                IQueryable<LienHe> results = db.LienHe;
                if (pagination == null)
                    pagination = new Pagination();
                if (pagination.includeEntities)
                {
                }

                if (lienHeID.HasValue) results = results.Where(o => o.LienHeID == lienHeID);
                if (!string.IsNullOrWhiteSpace(keywords))
                    results = results.Where(x => x.HoTen.Contains(keywords) || x.SoDienThoai.Contains(keywords));

                results = results.OrderBy(o => o.LienHeID);

                return Ok((await GetPaginatedResponseAsync(results, pagination)));
            }
        }

        [HttpGet, Route("{lienHeID:int}")]
        public async Task<IHttpActionResult> Get(int lienHeID)
        {
            using (var db = new ApplicationDbContext())
            {
                var lienHe = await db.LienHe
                    .SingleOrDefaultAsync(o => o.LienHeID == lienHeID);

                if (lienHe == null)
                    return NotFound();

                return Ok(lienHe);
            }
        }

        [HttpPost, Route("")]
        public async Task<IHttpActionResult> Insert([FromBody]LienHe lienHe)
        {
            if (lienHe.LienHeID != 0) return BadRequest("Invalid LienHeID");

            using (var db = new ApplicationDbContext())
            {
                db.LienHe.Add(lienHe);
                await db.SaveChangesAsync();
            }

            return Ok(lienHe);
        }

        [HttpPut, Route("{lienHeID:int}")]
        public async Task<IHttpActionResult> Update(int lienHeID, [FromBody]LienHe lienHe)
        {
            if (lienHe.LienHeID != lienHeID) return BadRequest("Id mismatch");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            using (var db = new ApplicationDbContext())
            {
                db.Entry(lienHe).State = EntityState.Modified;

                try
                {
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ducEx)
                {
                    bool exists = db.LienHe.Count(o => o.LienHeID == lienHeID) > 0;
                    if (!exists)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw ducEx;
                    }
                }

                return Ok(lienHe);
            }
        }

        [HttpDelete, Route("{lienHeID:int}")]
        public async Task<IHttpActionResult> Delete(int lienHeID)
        {
            using (var db = new ApplicationDbContext())
            {
                var lienHe = await db.LienHe.SingleOrDefaultAsync(o => o.LienHeID == lienHeID);

                if (lienHe == null)
                    return NotFound();

                db.Entry(lienHe).State = EntityState.Deleted;

                await db.SaveChangesAsync();

                return Ok();
            }
        }
    }
}