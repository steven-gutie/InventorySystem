using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventorySystem.Models
{
    public class AppUser : IdentityUser
    {
        [Required(ErrorMessage="You must enter a value")]
        [MaxLength(80)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "You must enter a value")]
        [MaxLength(80)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "You must enter a value")]
        [MaxLength(200)]
        public string Address { get; set; }

        [Required(ErrorMessage = "You must enter a value")]
        [MaxLength(80)]
        public string City { get; set; }

        [Required(ErrorMessage = "You must enter a value")]
        [MaxLength(80)]
        public string Country { get; set; }

        [NotMapped]
        public string Role { get; set; }
    }
}
