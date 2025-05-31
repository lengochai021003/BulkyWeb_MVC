using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Reponsitory;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using Microsoft.EntityFrameworkCore;

namespace BulkyBook.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {

        private ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        

        public void Update(Product obj)
        {
            var objFromDb = _db.Products.FirstOrDefault(u => u.productID == obj.productID);
            if(objFromDb !=null)
            {
                objFromDb.titleProduct = obj.titleProduct;
                objFromDb.ISBN = obj.ISBN;
                objFromDb.Author = obj.Author;
                objFromDb.priceProduct = obj.priceProduct;
                objFromDb.listPrice= obj.listPrice;
                objFromDb.priceProductFor50 = obj.priceProductFor50;
                objFromDb.priceProductFor100 = obj.priceProductFor100;
                objFromDb.CategoryId = obj.CategoryId;
                objFromDb.productDecsciption = obj.productDecsciption;
                objFromDb.ProductImages = obj.ProductImages;
                //if(obj.ImageUrl != null)
                //{
                //    objFromDb.ImageUrl = obj.ImageUrl;

                //}

            }
        }
        

    }
}
