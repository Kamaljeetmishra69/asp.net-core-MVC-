using udemy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using udemy.Models.ViewModel;
using Udemy.DataAccess.Repository.IRepository;
using Udemy.Models.Models;
//using Udemy.Models.ViewModel;

namespace ecommerce.Controllers
{
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        //private int id;

        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            //retrieve all the product record from the data -base ans passing it to the coresseponding view
            List<Product> productobj = _unitOfWork.Product.GetAll().ToList();

            return View(productobj);

        }

        public IActionResult Create()
        {
            // HERE we are retrieving all the category records from the database and projecting them into a list of SelectListItem objects, which can be used to populate a dropdown list in the view. Each SelectListItem contains the category name as the text and the category ID as the value
            //ViewBag.Categorylist = Categorylist;
            //ViewData["Categorylist"] = Categorylist;
           
            //ViewBag.CategoryList = CategoryList;
           
            ProductVM productVM = new ProductVM()
            {
                CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.CategoryName,
                    Value = u.Id.ToString()      // projection in EF core to convert the category records into a list of SelectListItem objects, which can be used to populate a dropdown list in the view. Each SelectListItem contains the category name as the text and the category ID as the value

                }),
                product = new Product()

            };
            return View(productVM);
        }
        [HttpPost]
        public IActionResult Create(ProductVM obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Add(obj.product);
                _unitOfWork.Save();
                TempData["sucess"] = "product created successfully";
                return RedirectToAction("index");
            }
            return View();
            //else
            //{
            //    obj.CategoryList = _unitOfWork.Category
            //     .GetAll().Select(u => new SelectListItem
            //     {
            //         Text = u.CategoryName,
            //         Value = u.Id.ToString()
            //     });
            //     return View(productVM);

            //}
               
        }
        public IActionResult Edit(int? id = 0)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Product? productfromdb = _unitOfWork.Product.GetAll().Where(x => x.Id == id).FirstOrDefault();

            if (productfromdb == null)
            {
                return NotFound();

            }
            return View(productfromdb);

           
        }
        [HttpPost]
        public IActionResult Edit(Product obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "product updated successfully";
                return RedirectToAction("index");

            }
            return View();
        }
        public IActionResult Delete(int? id)
        {
            if(id==null || id==0)
            {
                return NotFound();
            }
            Product? productfromdb = (Product?)_unitOfWork.Product.GetAll().Where(x => x.Id == id).FirstOrDefault();
            if (productfromdb==null)
            {
                return NotFound(); 
            }
            return View(productfromdb);
            
        }
        [HttpPost ,ActionName("Delete")]
        public IActionResult DeleteConfirm(Product obj)
        {
           var productfromdb = _unitOfWork.Product.GetAll().Where(x => x.Id == obj.Id).FirstOrDefault();
            if (productfromdb==null)
            {
                return NotFound();    
            }
            _unitOfWork.Product.Remove(productfromdb);
            _unitOfWork.Save();
            TempData["success"] = "Data Deleted successfully";

            return RedirectToAction("index");
        }
    }
}