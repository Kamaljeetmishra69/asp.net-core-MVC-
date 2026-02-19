using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using System.Collections.Generic;
using udemy.data;
using udemy.DataAccess.Repository;
using udemy.DataAccess.Repository.IRepository;
using udemy.Models;

namespace commerce.Controllers
{
    public class CategoryController : Controller
    {
        //here we are injecting the ApplicationDbContext class to access the database
        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            //retrieve all the category records from the database?

            List<udemy.Models.Category> objCategoryList = _unitOfWork.Category.GetAll().ToList();
            return View(objCategoryList);

        }
        //this Method is for creating a new category page 
        public IActionResult Create()
        {
            return View();
        }
        //this method is for show the entry in to the table which are created 
        [HttpPost]
        public IActionResult Create(udemy.Models.Category obj1)
        {
            //if(obj1.CategoryName == obj1.DisplayOrder.ToString())
            //{
            //    ModelState.AddModelError("Categoryname","The DispalyOrder can not exatly match the CatetoryName");
            //}

            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Add(obj1);
                _unitOfWork.Save();

                TempData["success"] = "category Created successfully";

                return RedirectToAction("Index");
            }
            return View();
        }
        // this is for edit the category
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? categoryFromDb = _unitOfWork.Category.Get(u=>u.Id==id);
            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);

        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult Edit(Category obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Update(obj);   // update record
                _unitOfWork.Save();                    // save to database
                TempData["success"] = "Data updated successfully";
                return RedirectToAction("Index");
            }
            return View(obj);
        }
        //delete category logic
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category categoryfromdb = _unitOfWork.Category.Get(u => u.Id == id);
            if (categoryfromdb == null)
            {
                return NotFound();
            }
            return View(categoryfromdb);
        }
        [HttpPost, ActionName("Delete")]

        public IActionResult DeleteConfirmed(int id)
        {
            var categoryFromDb = _unitOfWork.Category.Get(u => u.Id == id);

            if (categoryFromDb == null)
            {
                return NotFound();
            }

            _unitOfWork.Category.Remove(categoryFromDb);
            _unitOfWork.Save();

            TempData["success"] = "Data Deleted successfully";

            return RedirectToAction("Index");
        }
    }







}
