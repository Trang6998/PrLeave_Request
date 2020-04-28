using CMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Services
{
    public class KhachDatHangService : ServiceBase<KhachDatHang>, IKhachDathangService
    {
        public int ThemKhachDatHang(KhachDatHang khachDatHang, NguoiDung nguoiDung)
        {
            int ID = 0;
            NguoiDung checkNguoiDung = DbContext.NguoiDung.FirstOrDefault(x => (x.SoDienThoai == nguoiDung.SoDienThoai) ||
                                                                                (x.SoKhac == nguoiDung.SoDienThoai));
            if (checkNguoiDung == null)
            {
                DbContext.NguoiDung.Add(nguoiDung);
                DbContext.SaveChanges();

                khachDatHang.ThanhTien = 0;
                khachDatHang.TinhTrangThanhToan = 2;
                khachDatHang.TinhTrangXuLy = 1;
                khachDatHang.NguoiDungID = nguoiDung.NguoiDungID;
                DbContext.KhachDatHang.Add(khachDatHang);
                DbContext.SaveChanges();
                return 1;
            }
            else
            {
                ID = checkNguoiDung.NguoiDungID;
                KhachDatHang checkKhachDatHang = DbContext.KhachDatHang.FirstOrDefault(x => x.NguoiDungID == ID &&
                                                                                          x.NgayDat == khachDatHang.NgayDat &&
                                                                                          x.GioDat == khachDatHang.GioDat);
                if (checkNguoiDung != null && checkKhachDatHang == null)
                {
                    khachDatHang.ThanhTien = 0;
                    khachDatHang.TinhTrangThanhToan = 2;
                    khachDatHang.TinhTrangXuLy = 1;
                    khachDatHang.NguoiDungID = checkNguoiDung.NguoiDungID;
                    DbContext.KhachDatHang.Add(khachDatHang);
                    DbContext.SaveChanges();
                    return 2;
                }
                else return 3;
            }
            
        }
    }
}
