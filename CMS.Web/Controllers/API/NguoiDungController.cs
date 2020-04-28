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
    [RoutePrefix("api/nguoidung")]
    public class NguoiDungController : BaseApiController
    {
        [HttpGet, Route("")]
        public async Task<IHttpActionResult> Search([FromUri]Pagination pagination, [FromUri]string keywords = null)
        {
            using (var db = new ApplicationDbContext())
            {
                IQueryable<NguoiDung> results = db.NguoiDung;
                if (!string.IsNullOrWhiteSpace(keywords))
                    results = results.Where(x => x.TenNguoiDung.Contains(keywords) || x.DiaChi.Contains(keywords) || x.SoDienThoai.Contains(keywords) || x.SoKhac.Contains(keywords));
                results = results.OrderBy(o => o.NguoiDungID);

                return Ok((await GetPaginatedResponseAsync(results, pagination)));
            }
        }

        [HttpGet, Route("{nguoiDungID:int}")]
        public async Task<IHttpActionResult> Get(int nguoiDungID)
        {
            using (var db = new ApplicationDbContext())
            {
                var nguoiDung = await db.NguoiDung
                    .SingleOrDefaultAsync(o => o.NguoiDungID == nguoiDungID);

                if (nguoiDung == null)
                    return NotFound();

                return Ok(nguoiDung);
            }
        }

        [HttpPost, Route("")]
        public async Task<IHttpActionResult> Insert([FromBody]NguoiDung nguoiDung)
        {
            if (nguoiDung.NguoiDungID != 0) return BadRequest("Invalid NguoiDungID");

            using (var db = new ApplicationDbContext())
            {
                nguoiDung.DiemThuong = 0;
                db.NguoiDung.Add(nguoiDung);
                await db.SaveChangesAsync();
            }

            return Ok(nguoiDung);
        }

        [HttpPut, Route("{nguoiDungID:int}")]
        public async Task<IHttpActionResult> Update(int nguoiDungID, [FromBody]NguoiDung nguoiDung)
        {
            if (nguoiDung.NguoiDungID != nguoiDungID) return BadRequest("Id mismatch");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            using (var db = new ApplicationDbContext())
            {
                db.Entry(nguoiDung).State = EntityState.Modified;

                try
                {
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ducEx)
                {
                    bool exists = db.NguoiDung.Count(o => o.NguoiDungID == nguoiDungID) > 0;
                    if (!exists)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw ducEx;
                    }
                }

                return Ok(nguoiDung);
            }
        }

        [HttpDelete, Route("{nguoiDungID:int}")]
        public async Task<IHttpActionResult> Delete(int nguoiDungID)
        {
            using (var db = new ApplicationDbContext())
            {
                var nguoiDung = await db.NguoiDung.SingleOrDefaultAsync(o => o.NguoiDungID == nguoiDungID);

                if (nguoiDung == null)
                    return NotFound();

                if (await db.KhachDatHang.AnyAsync(o => o.NguoiDungID == nguoiDung.NguoiDungID))
                    return BadRequest("Unable to delete the nguoidung as it has related khachdathang");

                db.Entry(nguoiDung).State = EntityState.Deleted;

                await db.SaveChangesAsync();

                return Ok();
            }
        }

    }
}
