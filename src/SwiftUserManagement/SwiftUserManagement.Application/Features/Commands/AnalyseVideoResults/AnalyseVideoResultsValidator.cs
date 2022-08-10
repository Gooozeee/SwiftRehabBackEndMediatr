using FluentValidation;

namespace SwiftUserManagement.Application.Features.Commands.AnalyseVideoResults
{
    public class AnalyseVideoResultsValidator : AbstractValidator<AnalyseVideoResultsCommand>
    {
        public AnalyseVideoResultsValidator()
        {
            RuleFor(video => video.VideoData.Count)
                .Equal(1).WithMessage("Please only send one video file");

            RuleFor(video => video.VideoData[0].ContentType)
                .Must(contentType => contentType.Contains("video"));
        }
    }
}
