using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using udemy.Business.Services;
using udemy.Business.Services.IServices;
using Udemy.Models;

namespace Area.Customer.controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly IProductServices _productService;

        public HomeController(IProductServices productservice)
        {
            _productService = productservice;
        }
        public  async Task<IActionResult> Index()
        {
            var products = await _productService.GetAllProductAsync(includeCategory:true);
            return View(products);
        }
        public  async Task<IActionResult> ProductDetail(int productid)
        {
            var product =  await _productService.GetProductByIdAsync(productid ,includeCategory:true );
            return View(product);
        }

    }
}
