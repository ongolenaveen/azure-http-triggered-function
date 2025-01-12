using System.ComponentModel.DataAnnotations;

namespace Api.Template.Domain.DataModels
{
    // User Detail Data Model
    public class UserDetail : IValidatableObject
    {
        public int UserId { get; set; }

        public string? UserNumber { get; set; }

        public string? Status { get; set; }

        public DateTime CreatedOn { get; set; }

        public string? LanguageCode { get; set; }

        public string? Source { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return new List<ValidationResult>();
        }
    }
}
