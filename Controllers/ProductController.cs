using Microsoft.AspNetCore.Mvc;
using udemy.DataAccess.Repository.IRepository;
using udemy.Models.Models;

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
            return View();
        }
        [HttpPost]
        public IActionResult Create(Product obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Add(obj);
                _unitOfWork.Save();
                TempData["sucess"] = "product created successfully";
                return RedirectToAction("index");
            }
            return View();
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