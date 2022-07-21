using System.ComponentModel.DataAnnotations;

namespace Flower.Models
{
    public class Table
    {
        [Key]
        public int Id { get; set; }

        [Required]        
        public string Name { get; set; }
    }
}
