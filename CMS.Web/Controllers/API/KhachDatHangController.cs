using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using System.Threading.Tasks;
using HVITCore.Controllers;
using System.Data.Entity.Infrastructure;
using CMS.Models;
using CMS.Services;

namespace CMS.Controllers
{
    [RoutePrefix("api/khachdathang")]
    public class KhachDatHangController : BaseApiController
    {
        [HttpGet, Route("")]
        public async Task<IHttpActionResult> Search([FromUri]Pagination pagination, 
                                                    [FromUri]int? nguoiDungID = null, 
                                                    [FromUri]DateTime? tuNgay = null, 
                                                    [FromUri]DateTime? denNgay = null, 
                                                    [FromUri]string keywords = null,
                                                    [FromUri]int? trangThai = null,
                                                    [FromUri]int? gioLayDo = null,
                                                    [FromUri]int? thanhToan = null)
        {
            using (var db = new ApplicationDbContext())
            {
                IQueryable<KhachDatHang> results = db.KhachDatHang;
                if (pagination == null)
                    pagination = new Pagination();
                if (pagination.includeEntities)
                {
                    results = results.Include(o => o.NguoiDung);
                }

                if (nguoiDungID.HasValue) results = results.Where(o => o.NguoiDungID == nguoiDungID);
                if (tuNgay.HasValue) results = results.Where(o => o.NgayDat >= tuNgay);
                if (denNgay.HasValue) results = results.Where(o => o.NgayDat <= denNgay);
                if (!string.IsNullOrWhiteSpace(keywords)) results = results.Where(o => o.NguoiDung.TenNguoiDung.Contains(keywords));
                if (trangThai.HasValue) results = results.Where(o => o.TinhTrangXuLy == trangThai);
                if (gioLayDo.HasValue) results = results.Where(o => o.GioDat == gioLayDo);
                if (thanhToan.HasValue) results = results.Where(o => o.TinhTrangThanhToan == thanhToan);

                results = results.OrderByDescending(o => o.NgayDat);

                return Ok((await GetPaginatedResponseAsync(results, pagination)));
            }
        }

        //[HttpGet, Route("thongke")]
        //public async Task<IHttpActionResult> Get([FromUri]int? nam = null,
        //                                         [FromUri]int? thang = null)
        //{
        //    using (var db = new ApplicationDbContext())
        //    {
        //        var khachDatHang = await db.KhachDatHang
        //            .Include(o => o.NguoiDung)
        //            .SingleOrDefaultAsync(o => o.KhachDatHangID == khachDatHangID);

        //        if (khachDatHang == null)
        //            return NotFound();

        //        return Ok(khachDatHang);
        //    }
        //}
        [HttpGet, Route("{khachDatHangID:int}")]
        public async Task<IHttpActionResult> Get(int khachDatHangID)
        {
            using (var db = new ApplicationDbContext())
            {
                var khachDatHang = await db.KhachDatHang
                    .Include(o => o.NguoiDung)
                    .SingleOrDefaultAsync(o => o.KhachDatHangID == khachDatHangID);

                if (khachDatHang == null)
                    return NotFound();

                return Ok(khachDatHang);
            }
        }

        [HttpPost, Route("")]
        public async Task<IHttpActionResult> Insert([FromBody]KhachDatHang khachDatHang)
        {
            if (khachDatHang.KhachDatHangID != 0) return BadRequest("Invalid KhachDatHangID");

            using (var db = new ApplicationDbContext())
            {
                khachDatHang.ThanhTien = 0;
                khachDatHang.TinhTrangThanhToan = 2;
                khachDatHang.TinhTrangXuLy = 1;
                db.KhachDatHang.Add(khachDatHang);
                await db.SaveChangesAsync();
            }

            return Ok(khachDatHang);
        }

        [HttpPut, Route("{khachDatHangID:int}")]
        public async Task<IHttpActionResult> Update(int khachDatHangID, [FromBody]KhachDatHang khachDatHang)
        {
            if (khachDatHang.KhachDatHangID != khachDatHangID) return BadRequest("Id mismatch");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            using (var db = new ApplicationDbContext())
            {
                db.Entry(khachDatHang).State = EntityState.Modified;

                try
                {
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ducEx)
                {
                    bool exists = db.KhachDatHang.Count(o => o.KhachDatHangID == khachDatHangID) > 0;
                    if (!exists)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw ducEx;
                    }
                }

                return Ok(khachDatHang);
            }
        }
        
        [HttpDelete, Route("{khachDatHangID:int}")]
        public async Task<IHttpActionResult> Delete(int khachDatHangID)
        {
            using (var db = new ApplicationDbContext())
            {
                var khachDatHang = await db.KhachDatHang.SingleOrDefaultAsync(o => o.KhachDatHangID == khachDatHangID);

                if (khachDatHang == null)
                    return NotFound();

                var nguoiDung = await db.NguoiDung.SingleOrDefaultAsync(o => o.NguoiDungID == khachDatHang.NguoiDungID);
                var chiTietDoGiat = db.ChiTietDoGiat.Where(x => x.KhachDatHangID == khachDatHang.KhachDatHangID);

                if (chiTietDoGiat.Any())
                    db.ChiTietDoGiat.RemoveRange(chiTietDoGiat);

                db.Entry(khachDatHang).State = EntityState.Deleted;
                nguoiDung.DiemThuong -= khachDatHang.ThanhTien / 100;

                await db.SaveChangesAsync();

                return Ok();
            }
        }
    }
}
