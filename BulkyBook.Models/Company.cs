using System;
using System.ComponentModel.DataAnnotations;

namespace BulkyBook.Models
{
    public class Company
    {
        // Properties

        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string? StreeAddress { get; set; }

        public string? City { get; set; }

        public string? State { get; set; }

        public string? PostalCode { get; set; }

        public string? PhoneNumber { get; set; }
    }
}
