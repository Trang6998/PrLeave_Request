using CMS.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Reflection;

namespace CMS.Services
{
    public class LuotTruyCapService : ServiceBase<LuotTruyCap>, ILuotTruyCapService
    {
        public IQueryable<LuotTruyCap> GetDanhSachLuotTruyCap(out int totalItems, string searchQuery, DateTime? thoiGianTruyCapTu = null, DateTime? thoiGianTruyCapDen = null, int page = 1, int rowsPerPage = 10, string sortBy = null, bool descending = false, bool includeEntities = false)
        {
            var results = DbContext.LuotTruyCap.AsQueryable();
            if (!string.IsNullOrWhiteSpace(searchQuery))
                results = results.Where(o => o.IP.Contains(searchQuery)
                || o.ThietBi.Contains(searchQuery)
                || o.TrangTruyCapDauTien.Contains(searchQuery));
            if (thoiGianTruyCapTu.HasValue)
                results = results.Where(x => x.ThoiGian >= thoiGianTruyCapTu.Value);
            if (thoiGianTruyCapDen.HasValue)
                results = results.Where(x => x.ThoiGian <= thoiGianTruyCapDen.Value);
            switch (sortBy)
            {
                case "IP":
                    if (descending)
                        results = results.OrderByDescending(o => o.IP);
                    else
                        results = results.OrderBy(o => o.IP);
                    break;
                case "ThietBi":
                    if (descending)
                        results = results.OrderByDescending(o => o.ThietBi);
                    else
                        results = results.OrderBy(o => o.ThietBi);
                    break;
                case "TrangTruyCapDauTien":
                    if (descending)
                        results = results.OrderByDescending(o => o.TrangTruyCapDauTien);
                    else
                        results = results.OrderBy(o => o.TrangTruyCapDauTien);
                    break;
                default:
                    if (descending)
                        results = results.OrderByDescending(o => o.ThoiGian);
                    else
                        results = results.OrderBy(o => o.ThoiGian);
                    break;
            }
            page = page < 1 ? 0 : page - 1;
            if (rowsPerPage > 0)
                results = results.Skip(rowsPerPage * page).Take(rowsPerPage);
            totalItems = results.Count();
            return results;
        }

        public LuotTruyCap GetLuotTruyCap(int luotTruyCapId)
        {
            return DbContext.LuotTruyCap.Find(luotTruyCapId);
        }

        public int SoLuotTruyCapTheoThoiGian(DateTime? thoiGianTruyCapTu = null, DateTime? thoiGianTruyCapDen = null)
        {
            var result = DbContext.LuotTruyCap.AsQueryable();
            if (thoiGianTruyCapTu.HasValue)
                result = result.Where(x => x.ThoiGian >= thoiGianTruyCapTu.Value);
            if (thoiGianTruyCapDen.HasValue)
                result = result.Where(x => x.ThoiGian <= thoiGianTruyCapDen.Value);
            return result.Count();
        }

        public LuotTruyCap ThemLuotTruyCap(LuotTruyCap luotTruyCap)
        {
            DbContext.LuotTruyCap.Add(luotTruyCap);
            DbContext.SaveChanges();
            return luotTruyCap;
        }
    }
}