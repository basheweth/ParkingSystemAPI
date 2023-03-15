namespace ParkingSystemAPI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ReservedPark
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long ResId { get; set; }

        [Column(TypeName = "date")]
        public DateTime TimeIn { get; set; }

        [Required]
        [StringLength(50)]
        public string CarName { get; set; }

        [StringLength(50)]
        public string PlateNum { get; set; }

        [Required]
        [StringLength(50)]
        public string ParkName { get; set; }
    }
}
