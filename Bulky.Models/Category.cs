using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BulkyBook.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        [Required(ErrorMessage ="Bắt buộc nhập")]
        [DisplayName("Tên danh mục")]
        public string CategoryName { get; set; }=string.Empty;
        [DisplayName("Thứ tự hiển thị")]
        [Required(ErrorMessage = "Bắt buộc nhập")]
        [Range(1,50,ErrorMessage ="Phải từ 1-50")]
        public int? DisplayOrder { get; set; }

    }
}
