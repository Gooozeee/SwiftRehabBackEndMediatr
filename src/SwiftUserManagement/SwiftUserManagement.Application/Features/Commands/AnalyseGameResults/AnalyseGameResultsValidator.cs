using FluentValidation;

namespace SwiftUserManagement.Application.Features.Commands.AnalyseGameResults
{
    public class AnalyseGameResultsValidator : AbstractValidator<AnalyseGameResultsCommand>
    {
        public AnalyseGameResultsValidator()
        {
            RuleFor(gameResults => gameResults.result1)
                .NotEmpty().WithMessage("Invalid game results")
                .NotNull().WithMessage("Invalid game results");

            RuleFor(gameResults => gameResults.result2)
                .NotEmpty().WithMessage("Invalid game results")
                .NotNull().WithMessage("Invalid game results");
        }
    }
}
