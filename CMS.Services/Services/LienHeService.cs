using CMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Services
{
    public class LienHeService : ServiceBase<LienHe>, ILienHeService
    {
        public void ThemLienHe(LienHe lienHe)
        {
            DbContext.LienHe.Add(lienHe);
            DbContext.SaveChanges();
        }
    }
}
