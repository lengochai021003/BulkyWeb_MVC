using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace BulkyBook.Models
{
    public class Product
    {
        [Key]
        public int productID { get; set; }
        [Required(ErrorMessage = "Bắt buộc nhập")]
        [DisplayName("Tên sách")]
        public string titleProduct { get; set; } = string.Empty;
        [DisplayName("Mô tả")]
        [Required(ErrorMessage = "Bắt buộc nhập")]
        public string productDecsciption { get; set; } = string.Empty;
        [DisplayName("ISBN")]
        [Required(ErrorMessage = "Bắt buộc nhập")]
        public string ISBN { get; set; } = string.Empty;
        [DisplayName("Tác giả")]
        [Required(ErrorMessage = "Bắt buộc nhập")]
        public string Author { get; set; } = string.Empty;
        [DisplayName("Giá niêm yết")]
        [Required(ErrorMessage = "Bắt buộc nhập")]
        public double listPrice {  get; set; }
        [DisplayName("Giá cho 1-50 quyển")]
        [Required(ErrorMessage = "Bắt buộc nhập")]
        public double priceProduct { get; set; }
        [DisplayName("Giá cho 50+ quyển")]
        [Required(ErrorMessage = "Bắt buộc nhập")]
        public double priceProductFor50 { get; set; }
        [DisplayName("Giá cho 100+ quyển")]
        [Required(ErrorMessage = "Bắt buộc nhập")]
        public double priceProductFor100 { get; set; }

        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        [ValidateNever]
        public Category? Category { get; set; }
        [ValidateNever]
        public string ImageUrl { get; set; }

    }
}
