using CMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Services
{
    public interface ILuotTruyCapService
    {
        IQueryable<LuotTruyCap> GetDanhSachLuotTruyCap(
            out int totalItems,
            string searchQuery,
            DateTime? thoiGianTruyCapTu = null,
            DateTime? thoiGianTruyCapDen = null,
            int page = 1,
            int rowsPerPage = 10,
            string sortBy = null,
            bool descending = false,
            bool includeEntities = false);
        LuotTruyCap GetLuotTruyCap(int luotTruyCapId);
        LuotTruyCap ThemLuotTruyCap(LuotTruyCap luotTruyCap);
        int SoLuotTruyCapTheoThoiGian(
            DateTime? thoiGianTruyCapTu = null,
            DateTime? thoiGianTruyCapDen = null);
    }
}
