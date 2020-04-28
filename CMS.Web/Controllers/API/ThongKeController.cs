using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using System.Threading.Tasks;
using HVITCore.Controllers;
using System.Data.Entity.Infrastructure;
using CMS.Models;
using System.Collections.Generic;

namespace CMS.Controllers
{
    class ThongKe
    {
        public string GiaTri1;
        public double GiaTri2;
        public int GiaTri3;
        public static (DateTime, DateTime) GetNgayDauTuanCuoiTuan(int tuan, int thang, int nam)
        {
            int thuDauThang = (int)(new DateTime(nam, thang, 1)).DayOfWeek - 1;
            if (thuDauThang == -1) thuDauThang = 6;

            DateTime ngayDauTuan, ngayCuoiTuan;
            if(tuan == 1) 
                ngayDauTuan = new DateTime(nam, thang, 1);
            else 
                ngayDauTuan = new DateTime(nam, thang, (7 * (tuan - 1) + 1 - thuDauThang));

            int thuDauTuan = (int)ngayDauTuan.DayOfWeek;
            if (thuDauTuan == 0)
                ngayCuoiTuan = ngayDauTuan;
            else
                ngayCuoiTuan = ngayDauTuan.AddDays(7 - thuDauTuan);

            return (ngayDauTuan, ngayCuoiTuan);
        }
    }
   
    [RoutePrefix("api/thongke")]
    public class ThongKeController : BaseApiController
    {
        [HttpGet, Route("tuan")]
        public async Task<IHttpActionResult> ThongKeTheoTuan([FromUri]int tuan, [FromUri]int thang, [FromUri]int nam)
        {
            using (var db = new ApplicationDbContext())
            {
                (DateTime, DateTime) tpl = ThongKe.GetNgayDauTuanCuoiTuan(tuan, thang, nam);
                DateTime ngayBatDau = tpl.Item1;
                DateTime ngayKetThuc = tpl.Item2;

                List<ThongKe> res = new List<ThongKe>();
                string[] lstThu = new string[] { "Chủ nhật", "Thứ 2", "Thứ 3", "Thứ 4", "Thứ 5", "Thứ 6", "Thứ 7", "Chủ nhật" };
                int thuDauTuan = (int)ngayBatDau.DayOfWeek;
                if (thuDauTuan == 0)
                    thuDauTuan = 7;
                for (int i=1; i< (int)thuDauTuan; i++)
                {
                    res.Add(new ThongKe()
                    {
                        GiaTri1 = lstThu[i],
                        GiaTri2 = 0,
                        GiaTri3 = 0,
                    });
                }

                int soNgayTrongTuan = ((TimeSpan)(ngayKetThuc - ngayBatDau)).Days + 1;
                for(int i = 0; i < soNgayTrongTuan; i++)
                {
                    DateTime ngayThongKe = ngayBatDau.AddDays(i);
                    var lstDonDatHang = db.KhachDatHang.Where(x => x.NgayDat == ngayThongKe).ToList();
                    res.Add(new ThongKe()
                    {
                        GiaTri1 = lstThu[(int)ngayThongKe.DayOfWeek],
                        GiaTri2 = (lstDonDatHang.Sum(x => (int)x.ThanhTien))/100000.0,
                        GiaTri3 = lstDonDatHang.Count(),
                    });
                }

                return Ok(res);
            }
        }

        [HttpGet, Route("ngay")]
        public async Task<IHttpActionResult> ThongKeTheoNgay([FromUri]DateTime ngay)
        {
            using (var db = new ApplicationDbContext())
            {
                var lstDonDatHangHomNay = db.KhachDatHang.Where(x => x.NgayDat == ngay);
                DateTime ngayHomQua = ngay.AddDays(-1);
                var lstDonDatHangHomQua = db.KhachDatHang.Where(x => x.NgayDat == ngayHomQua);

                var res = new
                {
                    soDon = lstDonDatHangHomNay.Count(),
                    doanhThu = lstDonDatHangHomNay.Sum(x => x.ThanhTien),
                    soDonTang = lstDonDatHangHomNay.Count() - lstDonDatHangHomQua.Count(),
                    doanhThuTang = Math.Round((double)(lstDonDatHangHomNay.Sum(x => x.ThanhTien)*100.0/(lstDonDatHangHomQua.Sum(x => x.ThanhTien) != 0? lstDonDatHangHomQua.Sum(x => x.ThanhTien) : 1)),2)
                };
                return Ok(res);
            }
        }
    }
}
