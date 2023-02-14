using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BulkyBook.Models
{
	public class Category
	{
        // Properties

        [Key] // DataAnnotations attribute for primary key
		public int Id { get; set; }

		[Required] // // DataAnnotations attribute for required property
        public string? Name { get; set; }

        [DisplayName("Display Order")] // Display different name from default property name
		[Range(1,100, ErrorMessage ="Display Order value must be between 1 and 100.")] // Define a range value
        public int DisplayOrder { get; set; }

		public DateTime CreatedDateTime { get; set; } = DateTime.Now;
	}
}

