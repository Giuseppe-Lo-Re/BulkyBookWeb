using BulkyBook.DataAccess;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using BulkyBook.Models.ViewModels;

namespace BulkyBookWeb.Controllers;
[Area("Admin")]

public class CompanyController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    // Constructor
    public CompanyController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
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
        Company company = new();


        if (id == null || id == 0)
        {
            return View(company);
        }
        else
        {
            // Retrieve the first or default product where category Id = id variable
            company = _unitOfWork.Company.GetFirstOrDefault(u => u.Id == id);

            return View(company);
        }

    }
    // POST

    [HttpPost] // Method attribute
    [ValidateAntiForgeryToken] // CSRF (Cross-Site Request Forgery) 

    public IActionResult Upsert(Company obj, IFormFile? file)
    {
        // Validation
        if (ModelState.IsValid)
        {

            if (obj.Id == 0)
            {
                // Add product to db
                _unitOfWork.Company.Add(obj);

                // Success message
                TempData["success"] = "Company created successfully";
            }
            else
            {
                // Update product to db
                _unitOfWork.Company.Update(obj);
                TempData["success"] = "Company updated successfully";
            }

            // Save on db
            _unitOfWork.Save();

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
        var companyList = _unitOfWork.Company.GetAll();

        // Return a Json containting the list
        return Json(new { data = companyList });
    }

    // POST

    [HttpDelete]

    public IActionResult Delete(int? id)
    {
        // Retrieve the first or default category where category Id = id variable
        var obj = _unitOfWork.Company.GetFirstOrDefault(u => u.Id == id);

        if (obj == null)
        {
            return Json(new { success = false, message = "Error while deleting" });
        }

        // Remove object to Categories
        _unitOfWork.Company.Remove(obj);

        // Save on db
        _unitOfWork.Save();

        return Json(new { success = true, message = "Delete Successful" });
    }

    #endregion
}
