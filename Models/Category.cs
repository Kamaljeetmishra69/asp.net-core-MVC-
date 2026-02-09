using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace udemy.Models
{
    public class Category
    {
       [Key]
       public int Id { get; set; }

       [Required]
       [MaxLength(30)]  //validation
       [DisplayName("Category Name")]// data antation for changing the label name in the view page
        public string CategoryName { get; set; }

       [Required]
       [Range(1,100)]   //validation
       [DisplayName("Display Order")]//data antation for changing the label name in the view page
        public string DisplayOrder { get; set; }
       
    }
}
