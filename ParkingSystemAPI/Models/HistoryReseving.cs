namespace ParkingSystemAPI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HistoryReseving")]
    public partial class HistoryReseving
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long HistId { get; set; }

        [Column(TypeName = "date")]
        public DateTime HisTimeIn { get; set; }

        [Column(TypeName = "date")]
        public DateTime HisTimeOut { get; set; }

        [Required]
        [StringLength(50)]
        public string HisCarName { get; set; }

        [StringLength(50)]
        public string HisPlateNum { get; set; }

        public int HisParkId { get; set; }

        public double Cost { get; set; }

        public virtual Park Park { get; set; }
    }
}
