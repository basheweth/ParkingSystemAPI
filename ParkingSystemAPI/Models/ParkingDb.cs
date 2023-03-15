using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace ParkingSystemAPI.Models
{
    public partial class ParkingDb : DbContext
    {
        public ParkingDb()
            : base("name=ParkingDb")
        {
        }

        public virtual DbSet<HistoryReseving> HistoryResevings { get; set; }
        public virtual DbSet<Park> Parks { get; set; }
        public virtual DbSet<ReservedPark> ReservedParks { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Park>()
                .HasMany(e => e.HistoryResevings)
                .WithRequired(e => e.Park)
                .HasForeignKey(e => e.HisParkId)
                .WillCascadeOnDelete(false);
        }
    }
}
