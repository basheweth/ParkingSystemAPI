using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ParkingSystemAPI.ViewModel
{
    public class ParkVM
    {
        

        public int ParkId { get; set; }

        [Required]
        [StringLength(50)]
        public string ParkName { get; set; }

        public bool ParkStatus { get; set; }

        
    }
}