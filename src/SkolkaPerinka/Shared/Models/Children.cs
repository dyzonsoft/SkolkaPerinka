﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkolkaPerinka.Shared.Models
{
    public class Children
    {
        [Key]
        public int Id { get; set; }
        [Required]
        //[StringLength(30, ErrorMessage = "Prosím zkraťte své vlastní jméno na {1} znaků. minimum jsou {2} znaky.", MinimumLength = 2)]
        public string FirstName { get; set; }
        [Required]
        //[StringLength(30, ErrorMessage = "Prosím zkraťte své  příjmení na {1} znaků. minimum jsou {2} znaky.", MinimumLength = 2)]
        public string LastName { get; set; }
        [Required]
        //[StringLength(60, ErrorMessage = "Prosím zkraťte svoji adresu na {1} znaků. minimum jsou {2} znaky.", MinimumLength = 6)]
        public string Address { get; set; }
        [Required]
        public DateTime BirthDate { get; set; }
        public string ParentEmail { get; set; }
        public string Gender { get; set; }

        [NotMapped]
        public bool Checked { get; set; } = false;
    }
}
