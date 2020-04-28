using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using CMS.Models;
namespace HVITCore.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }
        protected override void Seed(ApplicationDbContext context)
        {
        }
    }
}
