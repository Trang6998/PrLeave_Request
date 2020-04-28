using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models
{
    [Table("LienHe")]
    public class LienHe
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int LienHeID { get; set; }
        public string HoTen { get; set; }
        public string SoDienThoai { get; set; }
        public string NoiDung { get; set; }
    }
}
