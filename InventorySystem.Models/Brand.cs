﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventorySystem.Models
{
    public class Brand
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "You must enter a value")]
        [MaxLength(50, ErrorMessage = "Brand cannot exceed 50 characters.")]
        public string BrandName { get; set; } = string.Empty;

        [Required(ErrorMessage = "You must enter a value")]
        [MaxLength(100, ErrorMessage = "Brand cannot exceed 100 characters.")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "You must select a value")]
        public bool Status { get; set; }
                
                

    }
}
