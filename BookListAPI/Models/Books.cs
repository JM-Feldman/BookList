using System;
using System.ComponentModel.DataAnnotations;

namespace BookListAPI.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Author { get; set; }

        [Required]
        public string ISBN { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [CustomValidation(typeof(Book), nameof(ValidatePublishedDate))]
        public DateTime PublishedDate { get; set; }

        public static ValidationResult ValidatePublishedDate(DateTime date, ValidationContext context)
        {
            if (date > DateTime.Now)
            {
                return new ValidationResult("Published date cannot be in the future.");
            }
            return ValidationResult.Success;
        }
    }
}
