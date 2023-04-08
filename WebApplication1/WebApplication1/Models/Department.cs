using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Department
    {

        [Key]
        public int ID { get; set; }
        [Required]
        public string? Name { get; set; }

    }
}
