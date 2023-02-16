using System;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyBook.Models.ViewModels
{
	public class ProductVM
	{
        // Properties

        public Product Product { get; set; }

        [ValidateNever] // No validation
        public IEnumerable<SelectListItem> CategoryList { get; set; }

        [ValidateNever] // No validation
        public IEnumerable<SelectListItem> CoverTypeList { get; set; }
    }
}

