using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace BulkyWebRazor_Temp.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "Bắt buộc nhập")]
        [DisplayName("Category Name")]
        public required string CategoryName { get; set; }
        [DisplayName("Display Order")]
        [Required(ErrorMessage = "Bắt buộc nhập")]
        [Range(1, 50, ErrorMessage = "Phải từ 1-50")]
        public int? DisplayOrder { get; set; }
    }
}
