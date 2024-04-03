
using Bulky.Models;
using Microsoft.AspNetCore.Mvc;
using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.Rendering;
using Bulky.Models.ViewModels;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            List<Product> objCategoryList = _unitOfWork.Product.GetAll().ToList();

            
             
            return View(objCategoryList);
        }

        public IActionResult Create()
        {
            

            ProductVM productVM = new()
            {
                CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                Product = new Product()
            };
            return View(productVM);
        }

        [HttpPost]
        public IActionResult Create(ProductVM productVM)
        {

            /*if(obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "The Name cannot exactly match the Display Order.");
            }*/


            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Add(productVM.Product);
                _unitOfWork.Save();
                TempData["success"] = "Product created successfully";
                return RedirectToAction("Index");

            }

            else
            {
                productVM.CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
                return View(productVM);
                
            }
           
        }

        public IActionResult Edit(int? id)
        {

            if (id == null || id == 0)
            {
                return NotFound();
            }

            Product? productFromDB = _unitOfWork.Product.Get(u => u.Id == id);

            if (productFromDB == null)
            {
                return NotFound();
            }
            return View(productFromDB);
        }

        [HttpPost]
        public IActionResult Edit(Product obj)
        {

            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "Product updated successfully";
                return RedirectToAction("Index");
            }
            return View();

        }

        public IActionResult Delete(int? id)
        {

            if (id == null || id == 0)
            {
                return NotFound();
            }

            Product productFromDB = _unitOfWork.Product.Get(u => u.Id == id);

            if (productFromDB == null)
            {
                return NotFound();
            }
            return View(productFromDB);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {

            Product? obj = _unitOfWork.Product.Get(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            _unitOfWork.Product.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "Product deleted successfully";
            return RedirectToAction("Index");

        }
    }

}
