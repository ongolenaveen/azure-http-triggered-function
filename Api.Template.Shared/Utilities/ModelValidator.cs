using System.ComponentModel.DataAnnotations;

namespace Api.Template.Shared.Utilities
{
    public static class ModelValidator
    {
        public static ValidationResponse Validate(object model)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(model);

            var isValid = Validator.TryValidateObject(model, context, results, true);

            return new ValidationResponse
            {
                IsValid = isValid,
                Results = results
            };
        }

        public static bool IsModelValid(object model)
        {
            var response = Validate(model);

            return response.IsValid;
        }
    }
}
