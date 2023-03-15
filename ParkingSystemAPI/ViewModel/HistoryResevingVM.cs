using ParkingSystemAPI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ParkingSystemAPI.ViewModel
{
    public class HistoryResevingVM
    {
        [Key]
       
        public long HistId { get; set; }

        
        public DateTime HisTimeIn { get; set; }

    
        public DateTime HisTimeOut { get; set; }

        [Required]
        [StringLength(50)]
        public string HisCarName { get; set; }

        [StringLength(50)]
        public string HisPlateNum { get; set; }

        public int HisParkId { get; set; }

        public double Cost { get; set; }

        public virtual ParkVM Park { get; set; }
    }
}