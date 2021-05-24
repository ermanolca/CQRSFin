namespace Fin.Core.Models
{
    public class FinResponse
    {
        public int StatusCode { get; set; }
        public string ErrorMessage { get; set; }
        public bool IsSuccess => StatusCode == 200;
        public bool IsBadRequest => StatusCode == 400;
        public bool IsUnsuccessful => !IsSuccess;
        public virtual bool HasData => false;
        public virtual object GetData() => null;

        public static FinResponse<T> Success<T>(T data) => new FinResponse<T> { StatusCode = 200, Data = data };
        public static FinResponse<T> NotFound<T>(string message) => new FinResponse<T> { StatusCode = 404, ErrorMessage = message };

        public void BadRequest(ValidationResult validationResult)
        {
            StatusCode = 400;
            ErrorMessage = validationResult.ErrorMessage;
        }

        public void ServerError()
        {
            StatusCode = 500;
            ErrorMessage = "Sorry, an unexpected error occurred.";
        }
    }

    public class FinResponse<T> : FinResponse
    {
        public T Data { get; set; }
        public override bool HasData => true;
        public override object GetData() => Data;
    }
}
