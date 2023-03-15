using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ParkingSystemAPI.ViewModel
{
    public class ReservedParkVM
    {
        [Key]
        public long ResId { get; set; }

        
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