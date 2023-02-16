using System;
using System.ComponentModel.DataAnnotations;

namespace BulkyBook.Models
{
	public class CoverType
	{
		// Properties

		[Key] // Primary key
		public int Id { get; set; }

		[Display(Name = "Cover Type")] // Display a different name from default property name
        [Required]
		[MaxLength(50)] 

		public string Name { get; set; }
    }
}

