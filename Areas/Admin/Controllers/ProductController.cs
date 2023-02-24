using BulkyBook.DataAccess;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using BulkyBook.Models.ViewModels;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;

namespace BulkyBookWeb.Controllers;
[Area("Admin")]
[Authorize(Roles = SD.Role_Admin)]

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

        // New instance of ProductVm (view model product)
        ProductVM productVM = new()
        {
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
            productVM.Product = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == id);

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
                var uploads = Path.Combine(wwwRootPath, @"images/products/");

                // Rename the uploaded image to a unique name
                var extension = Path.GetExtension(file.FileName);
                    
                if(obj.Product.ImageUrl != null)
                {
                    var oldImagePath = Path.Combine(wwwRootPath, obj.Product.ImageUrl.TrimStart('/'));

                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                // Create combined file path
                using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                {
                        // Copy file into file streams
                        file.CopyTo(fileStreams);
                    }

                    // Assign image file URL
                    obj.Product.ImageUrl = @"/images/products/" + fileName + extension;
                }

                if(obj.Product.Id == 0)
                {
                    // Add product to db
                    _unitOfWork.Product.Add(obj.Product);
                }
                else
                {
                    // Update product to db
                    _unitOfWork.Product.Update(obj.Product);
                }
                
                // Save on db
                _unitOfWork.Save();

                // Success message
                TempData["success"] = "Product created successfully";

                return RedirectToAction("Index");
            }

        return View(obj);
    }

    // -------------------- DELETE -------------------- //

#region API CALLS

[HttpGet]
public IActionResult GetAll()
{
    // Retrieve a list of all products and includes category and covertype
    var productList = _unitOfWork.Product.GetAll(includeProperties:"Category,CoverType");

    // Return a Json containting the list
    return Json(new { data = productList });
}

// POST

[HttpDelete] 
    
public IActionResult Delete(int? id)
{
    // Retrieve the first or default category where category Id = id variable
    var obj = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == id);

    if (obj == null)
    {
        return Json(new { success = false, message = "Error while deleting" });
    }

    var oldImagePath = Path.Combine(_hostEnvironment.WebRootPath, obj.ImageUrl.TrimStart('/'));

    if (System.IO.File.Exists(oldImagePath))
    {
        System.IO.File.Delete(oldImagePath);
    }

    // Remove object to Categories
    _unitOfWork.Product.Remove(obj);

    // Save on db
    _unitOfWork.Save();

    return Json(new { success = true, message = "Delete Successful" });
}

#endregion
}
