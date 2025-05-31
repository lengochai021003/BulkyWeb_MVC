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
            List<Product> objProduct = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();

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
                ProductVM.Product = _unitOfWork.Product.Get(u => u.productID == id, includeProperties: "ProductImages");
                return View(ProductVM);
            }
        }
        [HttpPost]
        public IActionResult Upsert(ProductVM ProductVM, List<IFormFile> files)
        {

            if (ModelState.IsValid)
            {

                if (ProductVM.Product.productID == 0)
                {
                    _unitOfWork.Product.Add(ProductVM.Product);
                }
                else
                {
                    _unitOfWork.Product.Update(ProductVM.Product);
                }
                _unitOfWork.Save();

                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (files != null)
                {

                    foreach (IFormFile file in files)
                    {
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        string productPath = @"images\products\product-" + ProductVM.Product.productID;
                        string finalPath = Path.Combine(wwwRootPath, productPath);

                        if (!Directory.Exists(finalPath))
                        {
                            Directory.CreateDirectory(finalPath);
                        }

                        using (var fileStream = new FileStream(Path.Combine(finalPath, fileName), FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                        }
                        ProductImage images = new()
                        {
                            ImageUrl = @"\" + productPath + @"\" + fileName,
                            ProductId = ProductVM.Product.productID,
                        };
                        if (ProductVM.Product.ProductImages == null)
                        {
                            ProductVM.Product.ProductImages = new List<ProductImage>();
                        }
                        ProductVM.Product.ProductImages.Add(images);
                    }
                    _unitOfWork.Product.Update(ProductVM.Product);
                    _unitOfWork.Save();

                }


                TempData["success"] = "Product created/updated successfully";
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

        public IActionResult DeleteImage(int imageId)
        {
            var imageToBeDeleted = _unitOfWork.ProductImage.Get(u => u.Id == imageId);
            int productId = imageToBeDeleted.ProductId;
            if (imageToBeDeleted != null)
            {
                if (!string.IsNullOrEmpty(imageToBeDeleted.ImageUrl))
                {
                    var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, imageToBeDeleted.ImageUrl.TrimStart('\\'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }
                _unitOfWork.ProductImage.Delete(imageToBeDeleted);
                _unitOfWork.Save();

                TempData["success"] = "Deleted successfully";
            }
            return RedirectToAction(nameof(Upsert), new { id = productId });

        }
        #region API CALL
        [HttpGet]
        public IActionResult GetAll() {
            List<Product> objProduct = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
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


            string productPath = @"images\products\product-" + id;
            string finalPath = Path.Combine(_webHostEnvironment.WebRootPath, productPath);

            if (!Directory.Exists(finalPath))
            {
                string[] filePaths = Directory.GetFiles(finalPath);
                foreach (string filePath in filePaths)
                {
                    System.IO.File.Delete(filePath);
                }
                Directory.Delete(finalPath);
            }

        //var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, ProductToBeDeleted.ImageUrl.TrimStart('\\'));
        //if (System.IO.File.Exists(oldImagePath))
        //{
        //    System.IO.File.Delete(oldImagePath);
        //}
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

