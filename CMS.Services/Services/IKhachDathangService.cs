using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Models;

namespace CMS.Services
{
    public interface IKhachDathangService
    {
        int ThemKhachDatHang(KhachDatHang khachDatHang, NguoiDung nguoiDung);
    }
}
