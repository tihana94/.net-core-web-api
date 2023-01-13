using FluentValidation;

namespace APIfornetapplication.Validations
{
    public class AddWalkAsyncValidator:AbstractValidator<Models.DTO.AddWalkRequest>
    {
        public AddWalkAsyncValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Length).GreaterThan(0);
        }
    }
}
