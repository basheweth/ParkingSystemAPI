namespace ParkingSystemAPI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Park
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Park()
        {
            HistoryResevings = new HashSet<HistoryReseving>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ParkId { get; set; }

        [Required]
        [StringLength(50)]
        public string ParkName { get; set; }

        public bool ParkStatus { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HistoryReseving> HistoryResevings { get; set; }
    }
}
