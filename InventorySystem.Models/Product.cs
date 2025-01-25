using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventorySystem.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "You must enter a value")]
        [MaxLength(50, ErrorMessage= "Serial number cannot exceed 50 characters")]
        public string SerialNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "You must enter a value")]
        [MaxLength(100, ErrorMessage = "Description cannot exceed 100 characters")]
        public string ProdDescription { get; set; } = string.Empty;

        [Required(ErrorMessage = "You must enter a value")]
        public double Price { get; set; }

        [Required(ErrorMessage = "You must enter a value")]
        public double Cost { get; set; }

        [Required(ErrorMessage = "You must select a value")]
        public bool Status { get; set; }

        public string ImageUrl { get; set; } = string.Empty;

        [Required(ErrorMessage = "You must enter a value")]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public Category Category { get; set; }

        [Required(ErrorMessage = "You must enter a value")]
        public int BrandId { get; set; }

        [ForeignKey("BrandId")]
        public Brand Brand { get; set; }

        public int? ParentId { get; set; }

        public virtual Product Parent { get; set; }
    }
}
