using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkolkaPerinka.Shared.Models
{
    public class ChildrenToSchool
    {
        public string Variant { get; set; } = "Day";
        public List<Children> ChildrenOfParent { get; set; } = new();
        public DateTime CurrentDay { get; set; }
    }
}
