using System.ComponentModel.DataAnnotations;

namespace Api.Template.Shared.Utilities
{
    public class ValidationResponse
    {
        public List<ValidationResult> Results { get; set; } = new();
        public bool IsValid { get; set; } = false;
    }
}
