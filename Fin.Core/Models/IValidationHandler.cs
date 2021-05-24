using System.Threading.Tasks;

namespace Fin.Core.Models
{
    public interface IValidationHandler<TRequest>
    {
        Task<ValidationResult> Validate(TRequest request);
    }
}
