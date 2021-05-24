namespace Fin.Core.Models
{
    public class ValidationResult
    {
        public string ErrorMessage { get; set; }

        public bool IsSuccessfull => string.IsNullOrWhiteSpace(ErrorMessage);

        public static ValidationResult Success() => new ValidationResult();
        public static ValidationResult Fail(string errorMessage) => new ValidationResult(errorMessage);

        public ValidationResult()
        {

        }

        public ValidationResult(string errorMessage)
        {
            this.ErrorMessage = errorMessage;
        }
    }
}
