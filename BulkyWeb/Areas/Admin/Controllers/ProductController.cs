using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]

    public class ProductController : Controller
    {
        public readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            List<Product> objProduct = _unitOfWork.Product.GetAll(includeProperties:"Category").ToList();
           
            return View(objProduct);
        }
        public IActionResult Upsert(int? id)
        {
            ProductVM ProductVM = new()
            {
                CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.CategoryName,
                    Value = u.CategoryId.ToString()
                }),
                Product = new Product()
            };
            if (id == null)
            {
                //create
                return View(ProductVM);
            }
            else
            {
                //update
                ProductVM.Product = _unitOfWork.Product.Get(u=>u.productID ==id);
                return View(ProductVM);
            }
        }
        [HttpPost]
        public IActionResult Upsert(ProductVM ProductVM, IFormFile? file)
        {
           
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if(file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string ProductPath = "";
                    ProductPath= Path.Combine(wwwRootPath, "images", "Product");
                    if (!string.IsNullOrEmpty(ProductVM.Product.ImageUrl))
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, ProductVM.Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    using (var fileStream = new FileStream(Path.Combine(ProductPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    ProductVM.Product.ImageUrl = @"\images\Product\" + fileName; 
                }

                if (ProductVM.Product.productID == 0)
                {
                    _unitOfWork.Product.Add(ProductVM.Product);
                }
                else
                {
                    _unitOfWork.Product.Update(ProductVM.Product);
                }
                _unitOfWork.Save();
                TempData["success"] = "Product created successfully"; 
                return RedirectToAction("Index", "Product");
            }
            else
            {

                ProductVM.CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.CategoryName,
                    Value = u.CategoryId.ToString()
                });
                return View(ProductVM);
            }
        }
     
        //public IActionResult Delete(int? id)
        //{
        //    if (id == null || id == 0)
        //    {
        //        return NotFound();
        //    }
        //    Product? Product = _unitOfWork.Product.Get(u => u.ProductID == id);
        //    if (Product == null) 
        //    {
        //        return NotFound();
        //    }
        //    return View(Product);
        //}
        //[HttpPost,ActionName("Delete")]
        //public IActionResult DeletePost(int? id)
        //{
        //    Product? Product = _unitOfWork.Product.Get(u => u.ProductID == id);
        //    if (Product == null)
        //    {
        //        return NotFound();
        //    }
        //    _unitOfWork.Product.Delete(Product);
        //    _unitOfWork.Save();
        //    TempData["success"] = "Product deleted  successfully";
        //    return RedirectToAction("Index", "Product");
        //}
        #region API CALL
        [HttpGet]
        public IActionResult GetAll() {
            List<Product> objProduct = _unitOfWork.Product.GetAll(includeProperties:"Category").ToList();
            return Json(new
            {
                data = objProduct
            });
        }
        [HttpDelete]    
        public IActionResult Delete(int? id) {
            Product ProductToBeDeleted = _unitOfWork.Product.Get(u => u.productID == id);
            if (ProductToBeDeleted == null)
            {
                return Json(new
                {
                    success = false,
                    message = "Error"
                });
            }
            var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, ProductToBeDeleted.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }
            _unitOfWork.Product.Delete(ProductToBeDeleted);
            _unitOfWork.Save();
            return Json(new
            {
                success = true,
                message = "Product  deleted  successfully"
            });

        }

        #endregion
    }
}
