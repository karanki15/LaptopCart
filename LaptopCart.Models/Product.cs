using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LaptopCart.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage ="Product Name is Required")]
        public string Name { get; set; }
        [StringLength(200, ErrorMessage = "Description cannot be longer then 200 characters")]
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string? ImagePath { get; set; }
        public DateTime CreatedAt { get; set; }
        [NotMapped]
        public IFormFile? ImageFile { get; set; }
    }
}
