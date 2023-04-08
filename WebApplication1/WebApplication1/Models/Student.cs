using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{



    public class Student
    {
        [Key]
        public int Id { get; set; }

        [Required] // Data Annotations
        [StringLength(80, ErrorMessage = "Name Should be Max 80 characters")]
        public string? Name { get; set; }

        [Required]
        public string? Class { get; set; }

        [Display(Name = "Department")]
        public virtual int DeptId { get; set; }

        [ForeignKey("DeptId")]

        public virtual Department? department { get; set; }

    }
}
