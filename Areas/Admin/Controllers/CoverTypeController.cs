using BulkyBook.DataAccess;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Controllers;
[Area("Admin")]

    public class CoverTypeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        // Constructor
        public CoverTypeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // -------------------- INDEX -------------------- //

        public IActionResult Index()
        {
            // Retrieve all covertypes
            IEnumerable<CoverType> objCoverTypeList = _unitOfWork.CoverType.GetAll();

            return View(objCoverTypeList);
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

        public IActionResult Create(CoverType obj)
        {
            
            // Validation
            if (ModelState.IsValid) 
            {
                // Add object to Categories
                _unitOfWork.CoverType.Add(obj);

                // Save on db
                _unitOfWork.Save();

                // Success message
                TempData["success"] = "CoverType created successfully";

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

            // Retrieve the first or default covertype where category Id = id variable
            var CoverTypeFromDbFirst = _unitOfWork.CoverType.GetFirstOrDefaul(u=>u.Id == id); 

            if (CoverTypeFromDbFirst == null)
            {
                return NotFound();
            }

            return View(CoverTypeFromDbFirst);
        }

        // POST

        [HttpPost] // Method attribute
        [ValidateAntiForgeryToken] // CSRF (Cross-Site Request Forgery) 

        public IActionResult Edit(CoverType obj)
        {
            // Validation
            if (ModelState.IsValid) 
            {
                // Update object to CoverType
                _unitOfWork.CoverType.Update(obj);

                // Save on db
                _unitOfWork.Save();

                // Success message
                TempData["success"] = "CoverType updated successfully";

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

            // Retrieve the first or default covertype where category Id = id variable
            var CoverTypeFromDbFirst = _unitOfWork.CoverType.GetFirstOrDefaul(u => u.Id == id);

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
            var obj = _unitOfWork.CoverType.GetFirstOrDefaul(u => u.Id == id);

            if (obj == null)
            {
                return NotFound();
            }

            // Remove covertype
            _unitOfWork.CoverType.Remove(obj);

            // Save on db
            _unitOfWork.Save();

            // Success message
            TempData["success"] = "CoverType deleted successfully";

            return RedirectToAction("Index");
        }
    }
