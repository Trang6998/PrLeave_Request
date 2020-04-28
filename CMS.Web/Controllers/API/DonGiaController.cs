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
    [RoutePrefix("api/dongia")]
    public class DonGiaController : BaseApiController
    {
        [HttpGet, Route("")]
        public async Task<IHttpActionResult> Search([FromUri]Pagination pagination, [FromUri]int? hinhThucGiatID = null, [FromUri]int? doGiatID = null, [FromUri]int? giaTu = null, [FromUri]int? giaDen = null)
        {
            using (var db = new ApplicationDbContext())
            {
                IQueryable<DonGia> results = db.DonGia;
                if (pagination == null)
                    pagination = new Pagination();
                if (pagination.includeEntities)
                {
                    results = results.Include(o => o.HinhThucGiat)
                                     .Include(o => o.DoGiat);
                }

                if (hinhThucGiatID.HasValue) results = results.Where(o => o.HinhThucGiatID == hinhThucGiatID);
                if (doGiatID.HasValue) results = results.Where(o => o.DoGiatID == doGiatID);
                if (giaTu.HasValue) results = results.Where(o => o.DonGiaGiat >= giaTu);
                if (giaDen.HasValue) results = results.Where(o => o.DonGiaGiat <= giaDen);

                results = results.OrderBy(o => o.DonGiaID);

                return Ok((await GetPaginatedResponseAsync(results, pagination)));
            }
        }

        [HttpGet, Route("{donGiaID:int}")]
        public async Task<IHttpActionResult> Get(int donGiaID)
        {
            using (var db = new ApplicationDbContext())
            {
                var donGia = await db.DonGia
                    .Include(o => o.HinhThucGiat)
                    .SingleOrDefaultAsync(o => o.DonGiaID == donGiaID);

                if (donGia == null)
                    return NotFound();

                return Ok(donGia);
            }
        }

        [HttpPost, Route("")]
        public async Task<IHttpActionResult> Insert([FromBody]DonGia donGia)
        {
            if (donGia.DonGiaID != 0) return BadRequest("Invalid DonGiaID");

            using (var db = new ApplicationDbContext())
            {
                var donGiaDaCo = db.DonGia.FirstOrDefault(x => x.HinhThucGiatID == donGia.HinhThucGiatID && x.DoGiatID == donGia.DoGiatID);
                if (donGiaDaCo != null)
                    return BadRequest("Đơn giá đã tồn tại");
                db.DonGia.Add(donGia);
                await db.SaveChangesAsync();
            }

            return Ok(donGia);
        }

        [HttpPut, Route("{donGiaID:int}")]
        public async Task<IHttpActionResult> Update(int donGiaID, [FromBody]DonGia donGia)
        {
            if (donGia.DonGiaID != donGiaID) return BadRequest("Id mismatch");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            using (var db = new ApplicationDbContext())
            {
                db.Entry(donGia).State = EntityState.Modified;

                try
                {
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ducEx)
                {
                    bool exists = db.DonGia.Count(o => o.DonGiaID == donGiaID) > 0;
                    if (!exists)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw ducEx;
                    }
                }

                return Ok(donGia);
            }
        }

        [HttpDelete, Route("{donGiaID:int}")]
        public async Task<IHttpActionResult> Delete(int donGiaID)
        {
            using (var db = new ApplicationDbContext())
            {
                var donGia = await db.DonGia.SingleOrDefaultAsync(o => o.DonGiaID == donGiaID);

                if (donGia == null)
                    return NotFound();

                db.Entry(donGia).State = EntityState.Deleted;

                await db.SaveChangesAsync();

                return Ok();
            }
        }

    }
}
