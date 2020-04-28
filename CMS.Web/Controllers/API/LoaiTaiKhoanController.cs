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
    [RoutePrefix("api/loaitaikhoan")]
    public class LoaiTaiKhoanController : BaseApiController
    {
        [HttpGet, Route("")]
        public async Task<IHttpActionResult> Search([FromUri]Pagination pagination, [FromUri]string keywords = null)
        {
            using (var db = new ApplicationDbContext())
            {
                IQueryable<LoaiTaiKhoan> results = db.LoaiTaiKhoan;
                if (!string.IsNullOrWhiteSpace(keywords))
                    results = results.Where(x => x.TenLoai.Contains(keywords));
                results = results.OrderBy(o => o.LoaiTaiKhoanID);

                return Ok((await GetPaginatedResponseAsync(results, pagination)));
            }
        }

        [HttpGet, Route("{loaiTaiKhoanID:int}")]
        public async Task<IHttpActionResult> Get(int loaiTaiKhoanID)
        {
            using (var db = new ApplicationDbContext())
            {
                var loaiTaiKhoan = await db.LoaiTaiKhoan
                    .SingleOrDefaultAsync(o => o.LoaiTaiKhoanID == loaiTaiKhoanID);

                if (loaiTaiKhoan == null)
                    return NotFound();

                return Ok(loaiTaiKhoan);
            }
        }

        [HttpPost, Route("")]
        public async Task<IHttpActionResult> Insert([FromBody]LoaiTaiKhoan loaiTaiKhoan)
        {
            if (loaiTaiKhoan.LoaiTaiKhoanID != 0) return BadRequest("Invalid LoaiTaiKhoanID");

            using (var db = new ApplicationDbContext())
            {
                db.LoaiTaiKhoan.Add(loaiTaiKhoan);
                await db.SaveChangesAsync();
            }

            return Ok(loaiTaiKhoan);
        }

        [HttpPut, Route("{loaiTaiKhoanID:int}")]
        public async Task<IHttpActionResult> Update(int loaiTaiKhoanID, [FromBody]LoaiTaiKhoan loaiTaiKhoan)
        {
            if (loaiTaiKhoan.LoaiTaiKhoanID != loaiTaiKhoanID) return BadRequest("Id mismatch");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            using (var db = new ApplicationDbContext())
            {
                db.Entry(loaiTaiKhoan).State = EntityState.Modified;

                try
                {
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ducEx)
                {
                    bool exists = db.LoaiTaiKhoan.Count(o => o.LoaiTaiKhoanID == loaiTaiKhoanID) > 0;
                    if (!exists)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw ducEx;
                    }
                }

                return Ok(loaiTaiKhoan);
            }
        }

        [HttpDelete, Route("{loaiTaiKhoanID:int}")]
        public async Task<IHttpActionResult> Delete(int loaiTaiKhoanID)
        {
            using (var db = new ApplicationDbContext())
            {
                var loaiTaiKhoan = await db.LoaiTaiKhoan.SingleOrDefaultAsync(o => o.LoaiTaiKhoanID == loaiTaiKhoanID);

                if (loaiTaiKhoan == null)
                    return NotFound();

                if (await db.LoaiTaiKhoan.AnyAsync(o => o.LoaiTaiKhoanID == loaiTaiKhoan.LoaiTaiKhoanID))
                    return BadRequest("Unable to delete the loaitaikhoan as it has related dogiat");

                db.Entry(loaiTaiKhoan).State = EntityState.Deleted;

                await db.SaveChangesAsync();

                return Ok();
            }
        }

    }
}
