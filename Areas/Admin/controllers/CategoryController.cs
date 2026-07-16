using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using System.Collections.Generic;
using System.Web.WebPages.Html;
using udemy.Business.Services;
using udemy.Business.Services.IServices;
using Udemy.DataAccess.data;

using Udemy.Models;
using Udemy.Models.Models;

namespace Areas.Admin.controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        //here we are injecting the ApplicationDbContext class to access the database
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {

            var Categories = await _categoryService.GetAllCategoryAsync();
            return View(Categories);

        }
        //this Method is for creating a new category page 
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category obj1) 
        {
            // Required validation
            if (string.IsNullOrWhiteSpace(obj1.CategoryName))
            {
                ModelState.AddModelError("CategoryName", "Category Name is required");
            }
            else
            {
                // Duplicate validation
                bool isAvailable = await _categoryService.IsCategoryNameAsync(obj1.CategoryName);

                if (isAvailable)
                {
                    ModelState.AddModelError("CategoryName", "Category Name already exists.");
                }
            }

            if (ModelState.IsValid)
            {
                await _categoryService.CreateCategoryAsync(obj1);

                TempData["success"] = "Category created successfully";

                return RedirectToAction(nameof(Index));
            }

            return View(obj1);
        }
        // this is for edit the category
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var categoryFromDb = await _categoryService.GetCategoryByIdAsync(id.Value);

            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Category obj)
        {
            // Required validation
            if (string.IsNullOrWhiteSpace(obj.CategoryName))
            {
                ModelState.AddModelError("CategoryName", "Category Name is required");
            }
            else
            {
                // Duplicate validation (excluding current category)
                bool isExists = await _categoryService.IsCategoryNameAsync(obj.CategoryName, obj.Id);

                if (isExists)
                {
                    ModelState.AddModelError("CategoryName", "Category Name already exists");
                }
            }

            if (ModelState.IsValid)
            {
                await _categoryService.UpdateCategoryAsync(obj);
                TempData["success"] = "Data updated successfully";
                return RedirectToAction("Index");
            }

            return View(obj);
        }
        //delete category logic
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
           var categoryfromdb =  await _categoryService .GetCategoryByIdAsync(id.Value);
            if (categoryfromdb == null)
            {
                return NotFound();
            }
            return View(categoryfromdb);
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            
           bool IsDeleted =  await _categoryService.DeleteCategoryAsync(id);
            if(!IsDeleted)
            {
                return NotFound();
            }
            TempData["success"] = "Data Deleted successfully";

            return RedirectToAction("Index");
        }
    }







}
