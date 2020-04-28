using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models
{
    public class DoGiatDTO
    {
        public int DoGiatID { get; set; }

        public int? LoaiDoGiatID { get; set; }
        public string TenDoGiat { get; set; }
        public string PCS { get; set; }
        public string TenLoaiDoGiat { get; set; }
        public virtual LoaiDoGiat LoaiDoGiat { get; set; }
        public virtual ICollection<ChiTietDoGiat> ChiTietDoGiat { get; set; }
        public virtual ICollection<DonGia> DonGia { get; set; }
    }
}
