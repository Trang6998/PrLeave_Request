using System.Data.Entity;

namespace CMS.Models
{
    public partial class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() : base("name=MyConnection")
        {
            // Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationDbContext, HVITCore.Migrations.Configuration>());
            this.Configuration.LazyLoadingEnabled = false;
        }
       
        public DbSet<Users> Users { get; set; }
        public DbSet<AccessTokens> AccessTokens { get; set; }
        public DbSet<LuotTruyCap> LuotTruyCap { get; set; }
        public DbSet<HinhThucGiat> HinhThucGiat { get; set; }
        public DbSet<DonGia> DonGia { get; set; }
        public DbSet<ChiTietDoGiat> ChiTietDoGiat { get; set; }
        public DbSet<DoGiat> DoGiat { get; set; }
        public DbSet<LoaiDoGiat> LoaiDoGiat { get; set; }
        public DbSet<KhachDatHang> KhachDatHang { get; set; }
        public DbSet<NguoiDung> NguoiDung { get; set; }
        public DbSet<LoaiTaiKhoan> LoaiTaiKhoan { get; set; }
        public DbSet<LienHe> LienHe { get; set; }
        public DbSet<Leave_Request> Leave_Request { get; set; }


        public void ConfigureModelBuilder(DbModelBuilder modelBuilder)
        {
            
        }
    }
}
