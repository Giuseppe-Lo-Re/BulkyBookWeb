using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace BulkyBook.Models
{
	public class ApplicationUser : IdentityUser
	{
		// Properties

		[Required]
		public string Name { get; set; }

        public string? StreeAddress { get; set; }

        public string? City { get; set; }

        public string? State { get; set; }

        public string? PostalCode { get; set; }

    }
}

