using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using udemy.Business.Services;
using udemy.Business.Services.IServices;
using udemy.Models.Models;
using Udemy.Models;

namespace Area.Customer.controllers
{
    [Area("Customer")]
    public class HomeController : Controller 
    {
        private readonly IProductServices _productService;
        private readonly IShoppingCardService _shoppingCardService;
        

        public HomeController(IProductServices productservice , IShoppingCardService ShoppingCardService)
        {
            _productService = productservice; 
            _shoppingCardService = ShoppingCardService;
        }
        public  async Task<IActionResult> Index() 
        {
            var products = await _productService.GetAllProductAsync(includeCategory:true); 
            return View(products);
        }
        public  async Task<IActionResult> ProductDetail(int productid)
        {
            var product =  await _productService.GetProductByIdAsync(productid ,includeCategory:true );
            if (product == null)
            {
                return NotFound();
            }
            ShoppingCard shoppingcard = new()
            {
               Product = product, 
               count = 1,
               ProductId = productid
            };    

            return View(shoppingcard);
        }
        [HttpPost]
        [Authorize]
        [ActionName("ProductDetail")]
        public async Task<IActionResult> Detail(ShoppingCard shoppingcard)
        {
            var claimsIdentiy = (ClaimsIdentity)User.Identity;
            var userid = claimsIdentiy?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userid == null)
            {
                return Unauthorized();
            }
            shoppingcard.ApplicationUserId = userid;
            await _shoppingCardService.AddToCardAsync(shoppingcard);


            return RedirectToAction("ProductDetail", new {productid=shoppingcard.ProductId});
        }
        public IActionResult Privacy()
        {
            return View();
        }

    }

}
