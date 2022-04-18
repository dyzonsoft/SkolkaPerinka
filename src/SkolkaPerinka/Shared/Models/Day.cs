using System.ComponentModel.DataAnnotations;

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