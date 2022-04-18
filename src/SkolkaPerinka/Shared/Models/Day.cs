using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkolkaPerinka.Shared.Models
{
    public class Day
    {
        [Key]
        public int DayId { get; set; }
        public DateTime Date { get; set; }
        public bool CurrentMounth { get; set; }
        public int NumberOfChild { get; set; }
        [MaxLength(length: 50)]
        public string Background { get; set; }
    }
}
