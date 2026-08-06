using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using udemy.Business.Services;
using udemy.Business.Services.IServices;
using udemy.Models.Models;
using udemy.Models.ViewModels;
using Udemy.Models;

namespace Area.Customer.controllers
{
    [Area("Customer")]
    public class CartController : Controller 
    {
        private readonly IProductServices _productService;
        private readonly IShoppingCardService _shoppingCardService;
        

        public CartController(IProductServices productservice , IShoppingCardService ShoppingCardService)
        {
            _productService = productservice; 
            _shoppingCardService = ShoppingCardService;
        }
        public  async Task<IActionResult> Index() 
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userid = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userid))
            {
                return Unauthorized();
            }
            var cardItem = await _shoppingCardService.GetUserCardItemsAsync(userid);
            ShoppingCartVM shoppingcartvm = new()
            {
                ShoppingCartList = cardItem,
                OrderHeader = new()

            };
            foreach (var item in shoppingcartvm.ShoppingCartList)
            {
                shoppingcartvm.OrderHeader.OrderTotal += (item.price * item.count); 

            }
            return View(shoppingcartvm);
        }
       

    }

}
