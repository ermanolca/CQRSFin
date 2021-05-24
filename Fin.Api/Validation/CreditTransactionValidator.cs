using Fin.Api.Model;
using FluentValidation;

namespace Fin.Api.Validation
{
    public class CreateTransactionValidator : AbstractValidator<CreateTransaction>
	{
		public CreateTransactionValidator()
		{
			RuleFor(x => x.AccountId).NotNull();
			RuleFor(x => x.Amount).NotNull().GreaterThan(0);
			RuleFor(x => x.MessageType).IsEnumName(typeof(TransactionType));
			RuleFor(x => x.Origin).IsEnumName(typeof(TransactionOrigin));
		}
	}
}
