using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            IEnumerable<OrderHeader> orderHeaders;

            // Retrieve a list of all products and includes category and covertype
            orderHeaders = _unitOfWork.OrderHeader.GetAll(includeProperties: "ApllicationUser");

            // Return a Json containting the list
            return Json(new { data = orderHeaders });
        }

        #endregion
    }
}