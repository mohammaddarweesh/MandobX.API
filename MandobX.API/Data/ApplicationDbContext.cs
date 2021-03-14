using MandobX.API.Authentication;
using MandobX.API.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MandobX.API.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<OfferApplicationUser>().HasKey(auf => new { auf.ApplicationUserId, auf.OfferId });
        }

        public DbSet<ApplicationUser> ApplicationUsers { set; get; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Trader> Traders { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<PackageType> PackageTypes { get; set; }
        public DbSet<GoogleMap> GoogleMaps { get; set; }
        public DbSet<ShipmentOperation> ShipmentOperations { get; set; }
        public DbSet<TypeOfTrading> TypeOftradings { get; set; }
        public DbSet<CarBrand> CarBrands { get; set; }
        public DbSet<CarType> CarTypes { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Offer> Offers { get; set; }
        public DbSet<UploadedFile> UploadedFiles { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<OfferApplicationUser> OfferApplicationUsers { get; set; }

    }
}
