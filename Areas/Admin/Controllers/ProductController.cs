using BulkyBook.DataAccess;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using BulkyBook.Models.ViewModels;

namespace BulkyBookWeb.Controllers;
[Area("Admin")]

    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;

        // Constructor
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }

        // -------------------- INDEX -------------------- //

        public IActionResult Index()
        {
            return View();
        }

        // -------------------- EDIT -------------------- //

        // GET

        public IActionResult Upsert(int? id)
        {

            // new instance of ProductVm (view model product)
            ProductVM productVM = new()
            {
                // new 
                Product = new(),

                // Retrieve all categories
                CategoryList = _unitOfWork.Category.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),

                // Retrieve all covertypes
                CoverTypeList = _unitOfWork.CoverType.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
            };
            
            if (id == null || id == 0)
            {
                // Create product

                // Pass data to the Upsert View w/ Viewbag
                //ViewBag.CategoryList = CategoryList;

                // Pass data to the Upsert View w/ Viewdata
                //ViewData["CoverTypeList"] = CoverTypeList;

                return View(productVM);
            }
            else
            {
                // Retrieve the first or default product where category Id = id variable
                productVM.Product = _unitOfWork.Product.GetFirstOrDefaul(u => u.Id == id);

                return View(productVM);
            }
        }

        // POST

        [HttpPost] // Method attribute
        [ValidateAntiForgeryToken] // CSRF (Cross-Site Request Forgery) 

        public IActionResult Upsert(ProductVM obj, IFormFile file)
        {
            // Validation
            if (ModelState.IsValid) 
            {
                // Initialize wwwRootPath with the path of the web root folder in the web server's file system
                string wwwRootPath = _hostEnvironment.WebRootPath;

                // Upload image file
                if(file != null)
                {
                    // Generate a new unique identifier
                    string fileName = Guid.NewGuid().ToString();

                    // Set the path where the uploaded product image will be saved
                    var uploads = Path.Combine(wwwRootPath, @"images/products");

                    // Rename the uploaded image to a unique name
                    var extension = Path.GetExtension(file.FileName);

                    // Create combined file path
                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        //Copy file into file streams
                        file.CopyTo(fileStreams);
                    }

                    // Assign image file URL
                    obj.Product.ImageUrl = @"\images/products" + fileName + extension;
                }

                // Add product to db
                _unitOfWork.Product.Add(obj.Product);

                // Save on db
                _unitOfWork.Save();

                // Success message
                TempData["success"] = "Product created successfully";

                return RedirectToAction("Index");
            }

            return View(obj);
        }

        // -------------------- DELETE -------------------- //

        // GET

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            // Retrieve the first or default cavoertype where category Id = id variable
            var CoverTypeFromDbFirst = _unitOfWork.CoverType.GetFirstOrDefaul(u=>u.Id == id);

            if (CoverTypeFromDbFirst == null)
            {
                return NotFound();
            }

            return View(CoverTypeFromDbFirst);
        }

        // POST

        [HttpPost, ActionName("Delete")] // Method attribute + define action name("CoverType/Delete/id")
        [ValidateAntiForgeryToken] // CSRF (Cross-Site Request Forgery) 

        public IActionResult DeletePOST(int? id)
        {
            // Retrieve the first or default category where category Id = id variable
            var obj = _unitOfWork.CoverType.GetFirstOrDefaul(u => u.Id == id);

            if (obj == null)
            {
                return NotFound();
            }

            // Remove object to Categories
            _unitOfWork.CoverType.Remove(obj);

            // Save on db
            _unitOfWork.Save();

            // Success message
            TempData["success"] = "CoverType deleted successfully";

            return RedirectToAction("Index");
        }

    #region API CALLS

    [HttpGet]
    public IActionResult GetAll()
    {
        // Retrieve a list of all products and includes category and covertype
        var productList = _unitOfWork.Product.GetAll(includeProperties:"Category,CoverType");

        // Return a Json containting the list
        return Json(new { data = productList });
    }

    #endregion
    }
