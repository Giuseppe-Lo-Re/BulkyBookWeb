using BulkyBook.DataAccess;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Controllers;
[Area("Admin")]
[Authorize(Roles = SD.Role_Admin)]

public class CategoryController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    // Constructor
    public CategoryController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    // -------------------- INDEX -------------------- //

    public IActionResult Index()
    {
        // Retrieve all categories
        IEnumerable<Category> objCategoryList = _unitOfWork.Category.GetAll();

        return View(objCategoryList);
    }

    // -------------------- CREATE -------------------- //

    // GET

    public IActionResult Create()
    {
            
        return View();
    }

    // POST

    [HttpPost] // Method attribute
    [ValidateAntiForgeryToken] // CSRF (Cross-Site Request Forgery) 

    public IActionResult Create(Category obj)
    {
        // Validation
        if (obj.Name == obj.DisplayOrder.ToString())
        {
            // Error message
            ModelState.AddModelError("name", "The DisplayOrder cannot exactly match the Name.");
        }

        // Validation
        if (ModelState.IsValid) 
        {
            // Add object to Categories
            _unitOfWork.Category.Add(obj);

            // Save on db
            _unitOfWork.Save();

            // Success message
            TempData["success"] = "Category created successfully";

            return RedirectToAction("Index");
        }
        return View(obj);
    }

    // -------------------- EDIT -------------------- //

    // GET

    public IActionResult Edit(int? id)
    {
        if(id == null || id == 0)
        {
            return NotFound();
        }

        //var categoryFromDb = _db.Categories.Find(id); // find category with specified id 

        var categoryFromDbFirst = _unitOfWork.Category.GetFirstOrDefault(u=>u.Id == id); // return first instance -> if no exists return NULL

        //var categoryFromDbSingle = _db.Categories.SingleOrDefault(u => u.Id == id); // return only one element -> If exists more than one or no exists return NULL 

        if (categoryFromDbFirst == null)
        {
            return NotFound();
        }

        return View(categoryFromDbFirst);
    }

    // POST

    [HttpPost] // Method attribute
    [ValidateAntiForgeryToken] // CSRF (Cross-Site Request Forgery) 

    public IActionResult Edit(Category obj)
    {
        // Validation
        if (obj.Name == obj.DisplayOrder.ToString())
        {
            // Error message
            ModelState.AddModelError("name", "The DisplayOrder cannot exactly match the Name.");
        }

        // Validation
        if (ModelState.IsValid) 
        {
            // Update object to Categories
            _unitOfWork.Category.Update(obj);

            // Save on db
            _unitOfWork.Save();

            // Success message
            TempData["success"] = "Category updated successfully";

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

        //var categoryFromDb = _db.Categories.Find(id); // find category with specified id 

        var categoryFromDbFirst = _unitOfWork.Category.GetFirstOrDefault(u=>u.Id == id); // return first instance -> if no exists return NULL

        //var categoryFromDbSingle = _db.Categories.SingleOrDefault(u => u.Id == id); // return only one element -> If exists more than one or no exists return NULL 

        if (categoryFromDbFirst == null)
        {
            return NotFound();
        }

        return View(categoryFromDbFirst);
    }

    // POST

    [HttpPost, ActionName("Delete")] // Method attribute + define action name("Category/Delete/id")
    [ValidateAntiForgeryToken] // CSRF (Cross-Site Request Forgery) 

    public IActionResult DeletePOST(int? id)
    {
        // Retrieve the first or default category where category Id = id variable
        var obj = _unitOfWork.Category.GetFirstOrDefault(u => u.Id == id);

        if (obj == null)
        {
            return NotFound();
        }

        // Remove category
        _unitOfWork.Category.Remove(obj);

        // Save on db
        _unitOfWork.Save();

        // Success message
        TempData["success"] = "Category deleted successfully";

        return RedirectToAction("Index");
    }
}
