using FluentValidation;

namespace APIfornetapplication.Validations
{
    public class UpdateWalkDifficultyAsingValidator :AbstractValidator<Models.DTO.UpdateWalkDifficultyRequest>
    {
        public UpdateWalkDifficultyAsingValidator()
        {
            RuleFor(x => x.Code).NotEmpty();
        }
    }
}
