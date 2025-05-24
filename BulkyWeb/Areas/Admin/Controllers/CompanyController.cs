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

    public class CompanyController : Controller
    {
        public readonly IUnitOfWork _unitOfWork;
        public CompanyController(IUnitOfWork unitOfWork) 
        { 
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            List<Company> objCompany = _unitOfWork.Company.GetAll().ToList();
           
            return View(objCompany);
        }
        public IActionResult Upsert(int? id)
        {
            
            if (id == null)
            {
                //create
                return View(new Company());
            }
            else
            {
                //update
                Company company = _unitOfWork.Company.Get(u=>u.Id==id);
                return View(company);
            }
        }
        [HttpPost]
        public IActionResult Upsert(Company companyObj)
        {
           
            if (ModelState.IsValid)
            {
                

                if (companyObj.Id == 0)
                {
                    _unitOfWork.Company.Add(companyObj);
                }
                else
                {
                    _unitOfWork.Company.Update(companyObj);
                }
                _unitOfWork.Save();
                TempData["success"] = "Company created successfully"; 
                return RedirectToAction("Index", "Company");
            }
            else
            {

               
                return View(companyObj);
            }
        }
     
        //public IActionResult Delete(int? id)
        //{
        //    if (id == null || id == 0)
        //    {
        //        return NotFound();
        //    }
        //    Company? Company = _unitOfWork.Company.Get(u => u.CompanyID == id);
        //    if (Company == null) 
        //    {
        //        return NotFound();
        //    }
        //    return View(Company);
        //}
        //[HttpPost,ActionName("Delete")]
        //public IActionResult DeletePost(int? id)
        //{
        //    Company? Company = _unitOfWork.Company.Get(u => u.CompanyID == id);
        //    if (Company == null)
        //    {
        //        return NotFound();
        //    }
        //    _unitOfWork.Company.Delete(Company);
        //    _unitOfWork.Save();
        //    TempData["success"] = "Company deleted  successfully";
        //    return RedirectToAction("Index", "Company");
        //}
        #region API CALL
        [HttpGet]
        public IActionResult GetAll() {
            List<Company> objCompany = _unitOfWork.Company.GetAll().ToList();
            return Json(new
            {
                data = objCompany
            });
        }
        [HttpDelete]    
        public IActionResult Delete(int? id) {
            Company CompanyToBeDeleted = _unitOfWork.Company.Get(u => u.Id == id);
            if (CompanyToBeDeleted == null)
            {
                return Json(new
                {
                    success = false,
                    message = "Error"
                });
            }
            _unitOfWork.Company.Delete(CompanyToBeDeleted);
            _unitOfWork.Save();
            return Json(new
            {
                success = true,
                message = "Company  deleted  successfully"
            });

        }

        #endregion
    }
}
