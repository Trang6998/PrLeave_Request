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
    [RoutePrefix("api/chitietdogiat")]
    public class ChiTietDoGiatController : BaseApiController
    {
        [HttpGet, Route("")]
        public async Task<IHttpActionResult> Search([FromUri]Pagination pagination, [FromUri]int? khachDatHangID = null, [FromUri]int? doGiatID = null, [FromUri]int? hinhThucGiatID = null)
        {
            using (var db = new ApplicationDbContext())
            {
                IQueryable<ChiTietDoGiat> results = db.ChiTietDoGiat;
                if (pagination == null)
                    pagination = new Pagination();
                if (pagination.includeEntities)
                {
                    results = results.Include(o => o.DoGiat)
                                     .Include(o => o.KhachDatHang)
                                     .Include(o => o.HinhThucGiat);
                }

                if (khachDatHangID.HasValue) results = results.Include(o => o.HinhThucGiat)
                                                              .Include(o => o.DoGiat)
                                                              .Include(o => o.KhachDatHang)
                                                              .Where(o => o.KhachDatHangID == khachDatHangID);

                if (doGiatID.HasValue) results = results.Where(o => o.DoGiatID == doGiatID);
                if (hinhThucGiatID.HasValue) results = results.Where(o => o.HinhThucGiatID == hinhThucGiatID);

                results = results.OrderBy(o => o.ChiTietDoGiatID);

                return Ok((await GetPaginatedResponseAsync(results, pagination)));
            }
        }

        [HttpGet, Route("{chiTietDoGiatID:int}")]
        public async Task<IHttpActionResult> Get(int chiTietDoGiatID)
        {
            using (var db = new ApplicationDbContext())
            {
                var chiTietDoGiat = await db.ChiTietDoGiat
                    .SingleOrDefaultAsync(o => o.ChiTietDoGiatID == chiTietDoGiatID);

                if (chiTietDoGiat == null)
                    return NotFound();

                return Ok(chiTietDoGiat);
            }
        }

        [HttpPost, Route("")]
        public async Task<IHttpActionResult> Insert([FromBody]ChiTietDoGiat chiTietDoGiat)
        {
            if (chiTietDoGiat.ChiTietDoGiatID != 0) return BadRequest("Invalid ChiTietDoGiatID");

            using (var db = new ApplicationDbContext())
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    KhachDatHang kdh = await db.KhachDatHang.SingleOrDefaultAsync(x => x.KhachDatHangID == chiTietDoGiat.KhachDatHangID);
                    NguoiDung nguoiDung = await db.NguoiDung.SingleOrDefaultAsync(x => x.NguoiDungID == kdh.NguoiDungID);
                    var giaBan = await db.DonGia.FirstOrDefaultAsync(x => x.DoGiatID == chiTietDoGiat.DoGiatID && x.HinhThucGiatID == chiTietDoGiat.HinhThucGiatID);
                    if (giaBan != null)
                    {
                        int? donGia = giaBan.DonGiaGiat;
                        kdh.ThanhTien += donGia * chiTietDoGiat.SoLuong;
                        nguoiDung.DiemThuong = kdh.ThanhTien / 100;
                    }
                    var chiTietDoGiatDaCo = await db.ChiTietDoGiat.FirstOrDefaultAsync(x => x.KhachDatHangID == chiTietDoGiat.KhachDatHangID
                                                                                          && x.DoGiatID == chiTietDoGiat.DoGiatID
                                                                                          && x.HinhThucGiatID == chiTietDoGiat.HinhThucGiatID);
                    if (chiTietDoGiatDaCo != null)
                        chiTietDoGiatDaCo.SoLuong += chiTietDoGiat.SoLuong;
                    else
                        db.ChiTietDoGiat.Add(chiTietDoGiat);

                    await db.SaveChangesAsync();
                    transaction.Commit();
                }
            }

            return Ok(chiTietDoGiat);
        }

        [HttpPut, Route("{chiTietDoGiatID:int}")]
        public async Task<IHttpActionResult> Update(int chiTietDoGiatID, [FromBody]ChiTietDoGiat chiTietDoGiat)
        {
            if (chiTietDoGiat.ChiTietDoGiatID != chiTietDoGiatID) return BadRequest("Id mismatch");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            using (var db = new ApplicationDbContext())
            {
                ChiTietDoGiat chiTietDoGiatCu = await db.ChiTietDoGiat.SingleOrDefaultAsync(x => x.ChiTietDoGiatID == chiTietDoGiatID);
               
                try
                {
                    using (var transaction = db.Database.BeginTransaction())
                    {
                        KhachDatHang kdh = await db.KhachDatHang.SingleOrDefaultAsync(x => x.KhachDatHangID == chiTietDoGiat.KhachDatHangID);
                        NguoiDung nguoiDung = await db.NguoiDung.SingleOrDefaultAsync(x => x.NguoiDungID == kdh.NguoiDungID);
                        DoGiat doGiat = await db.DoGiat.SingleOrDefaultAsync(x => x.DoGiatID == chiTietDoGiat.DoGiatID);
                        HinhThucGiat hinhThucGiat = await db.HinhThucGiat.SingleOrDefaultAsync(x => x.HinhThucGiatID == chiTietDoGiat.HinhThucGiatID);

                        var giaBan = await db.DonGia.FirstOrDefaultAsync(x => x.DoGiatID == doGiat.DoGiatID && x.HinhThucGiatID == hinhThucGiat.HinhThucGiatID);
                        int? donGia = giaBan.DonGiaGiat;
                        kdh.ThanhTien = (kdh.ThanhTien - donGia * chiTietDoGiatCu.SoLuong + donGia * chiTietDoGiat.SoLuong);
                        nguoiDung.DiemThuong = nguoiDung.DiemThuong - (donGia * chiTietDoGiatCu.SoLuong) / 100 + (donGia * chiTietDoGiat.SoLuong) / 100;

                        chiTietDoGiatCu.KhachDatHangID = chiTietDoGiat.KhachDatHangID;
                        chiTietDoGiatCu.HinhThucGiatID = chiTietDoGiat.HinhThucGiatID;
                        chiTietDoGiatCu.DoGiatID = chiTietDoGiat.DoGiatID;
                        chiTietDoGiatCu.SoLuong = chiTietDoGiat.SoLuong;
                        await db.SaveChangesAsync();
                        transaction.Commit();
                    }
                }
                catch (DbUpdateConcurrencyException ducEx)
                {
                    bool exists = db.ChiTietDoGiat.Count(o => o.ChiTietDoGiatID == chiTietDoGiatID) > 0;
                    if (!exists)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw ducEx;
                    }
                }

                return Ok(chiTietDoGiat);
            }
        }

        [HttpDelete, Route("{chiTietDoGiatID:int}")]
        public async Task<IHttpActionResult> Delete(int chiTietDoGiatID)
        {
            using (var db = new ApplicationDbContext())
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    var chiTietDoGiat = await db.ChiTietDoGiat.SingleOrDefaultAsync(o => o.ChiTietDoGiatID == chiTietDoGiatID);

                    if (chiTietDoGiat == null)
                        return NotFound();

                    KhachDatHang kdh = await db.KhachDatHang.SingleOrDefaultAsync(x => x.KhachDatHangID == chiTietDoGiat.KhachDatHangID);
                    NguoiDung nguoiDung = await db.NguoiDung.SingleOrDefaultAsync(x => x.NguoiDungID == kdh.NguoiDungID);
                    DoGiat doGiat = await db.DoGiat.SingleOrDefaultAsync(x => x.DoGiatID == chiTietDoGiat.DoGiatID);
                    HinhThucGiat hinhThucGiat = await db.HinhThucGiat.SingleOrDefaultAsync(x => x.HinhThucGiatID == chiTietDoGiat.HinhThucGiatID);

                    var giaBan = await db.DonGia.SingleOrDefaultAsync(x => x.DoGiatID == doGiat.DoGiatID && x.HinhThucGiatID == hinhThucGiat.HinhThucGiatID);
                    int? donGia = giaBan.DonGiaGiat;

                    kdh.ThanhTien -= donGia * chiTietDoGiat.SoLuong;
                    nguoiDung.DiemThuong -= (donGia * chiTietDoGiat.SoLuong) / 100;

                    db.Entry(chiTietDoGiat).State = EntityState.Deleted;
                    await db.SaveChangesAsync();
                    transaction.Commit();
                    return Ok();
                }
            }
        }

    }
}
