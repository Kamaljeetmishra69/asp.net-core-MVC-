
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Identity.Client;
using System.Collections.Generic;
using System.Web.WebPages.Html;
using udemy.Business.Services;
using udemy.Business.Services.IServices;
using udemy.Models.ViewModel;
using Udemy.DataAccess.data;


using Udemy.Models;
using Udemy.Models.Models;

namespace Areas.Admin.controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    
    public class ProductController : Controller
    {
        private readonly IProductServices _productService;
        private readonly ICategoryService _categoryService;
        private readonly IWebHostEnvironment _webHostEnviornment;
        public ProductController(IProductServices productService,ICategoryService categoryService , IWebHostEnvironment webHostEnviornment)
        {
            _productService = productService;
            _categoryService = categoryService;
            _webHostEnviornment = webHostEnviornment;
        }
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
          return View();
        }
        
        public async Task<IActionResult> Upsert(int? id)
        {
            // Load Category Dropdown
            var categories = await _categoryService.GetAllCategoryAsync();
            ProductVM productvm = new ProductVM()
            {
                Product = new Product(),
                CategoryList = categories.Select(i => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Text = i.CategoryName,
                    Value = i.Id.ToString()
                })
            };
            if (id == null || id == 0)
            {
                return View(productvm);
            } 
            else
            {
                productvm.Product = await _productService.GetProductByIdAsync(id.Value);

                if (productvm == null)
                {
                    return NotFound();
                }

                return View(productvm);
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> Upsert(ProductVM vm)
        {
            if (string.IsNullOrWhiteSpace(vm.Product.Title)) 
            {
                ModelState.AddModelError("Product.Title", "Product Title is required");
            }

            if (ModelState.IsValid)
            {
                var wwwRoootPath = _webHostEnviornment.WebRootPath;
                if(vm.File!= null)
                {
                    string FileName = Guid.NewGuid() + Path.GetExtension(vm.File.FileName);
                    string ProductPath = Path.Combine("img", "product");
                    string FinalPath = Path.Combine(wwwRoootPath, ProductPath);
                    if (!Directory.Exists(FinalPath))
                    {
                        Directory.CreateDirectory(FinalPath);
                    }
                    using (var fileStreame = new FileStream(Path.Combine(FinalPath, FileName), FileMode.Create))   
                    {
                        vm.File.CopyTo(fileStreame);
                    }
                    vm.Product.ImageUrl = Path.Combine(@"\", ProductPath, FileName).Replace("\\", "/");
                }
                if(vm.Product.Id==null ||  vm.Product.Id == 0)
                {
                    await _productService.CreateProductAsync(vm.Product);

                    TempData["success"] = "Product created successfully";

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    await _productService.UpdateProductAsync(vm.Product);

                    TempData["success"] = "Product updated successfully";

                    return RedirectToAction(nameof(Index));

                }

                
            }

            // Reload dropdown before returning the view
            vm.CategoryList = (await _categoryService.GetAllCategoryAsync())
                .Select(c => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Text = c.CategoryName,
                    Value = c.Id.ToString()
                });

            return View(vm);
        }


        #region API Endpoint
        [AllowAnonymous]

        public async Task<IActionResult> GetAll()
        {

            var product = await _productService.GetAllProductAsync(true);
            return Json(new { data = product });

        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
            {
                return Json(new { success = false, message = "Invalid Id" });
            }

            var productTobeDeleted = await _productService.GetProductByIdAsync(id);

            if (productTobeDeleted == null)
            {
                return Json(new { success = false, message = "Id not found" });
            }

            // Delete image from wwwroot
            if (!string.IsNullOrEmpty(productTobeDeleted.ImageUrl))
            {
                var imagePath = Path.Combine(
                    _webHostEnviornment.WebRootPath,
                    productTobeDeleted.ImageUrl.TrimStart('\\', '/')
                );

                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            await _productService.DeleteProductAsync(id);

            return Json(new { success = true, message = "Delete successful" });
        }
        #endregion
    }


}
