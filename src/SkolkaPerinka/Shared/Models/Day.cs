using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkolkaPerinka.Shared.Models
{
    public class Day
    {
        [Key]
        public int DayId { get; set; }
        public DateTime Date { get; set; }
        public bool CurrentMounth { get; set; }
        public int NumberOfChild { get; set; } = 0;
        [MaxLength(length: 50)]
        public string Background { get; set; } = "";
        public string IdChildrensInSchool { get; set; } = "*|";
        [NotMapped]
        public List<ChildrenOfParentIcon> ChildrensOfParentIcon { get; set; } = new();
    }

    [NotMapped]
    public class ChildrenOfParentIcon
    {
        public string Gender { get; set; }
        public string Name { get; set; }
    }
}