using FluentValidation;

namespace APIfornetapplication.Validations
{
    public class AddWalkDifficultyAsync:AbstractValidator<Models.DTO.AddWalkDiffucultyRequest>
    {
        public AddWalkDifficultyAsync()
        {
            RuleFor(x => x.Code).NotEmpty();
        }
    }
}
