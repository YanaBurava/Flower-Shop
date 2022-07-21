using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Flower.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [DisplayName("Display Opder")]
        [Required]
        [Range(1,int.MaxValue, ErrorMessage ="Display Order must be greater than 0")]
        public int Order { get; set; }
    }
}
