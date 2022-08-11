using AutoMapper;
using MediatR;
using SwiftUserManagement.Application.Contracts.Infrastructure;
using SwiftUserManagement.Application.Contracts.Persistence;

namespace SwiftUserManagement.Application.Features.Commands.AnalyseGameResults
{
    public class AnalyseGameResultsHandler : IRequestHandler<AnalyseGameResultsCommand, string>
    {
        private readonly IRabbitMQRepository _rabbitMQRepository;
        private readonly IUserRepository _userRepository;

        public AnalyseGameResultsHandler(IRabbitMQRepository rabbitMQRepository, IUserRepository userRepository)
        {
            _rabbitMQRepository = rabbitMQRepository ?? throw new ArgumentNullException(nameof(rabbitMQRepository));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task<string> Handle(AnalyseGameResultsCommand request, CancellationToken cancellationToken)
        {
            var result = await _rabbitMQRepository.EmitGameAnalysis(request.result1, request.result2);
            if (!result)
                return "User not found";

            var receivedData = _rabbitMQRepository.ReceiveGameAnalysis();

            await _userRepository.AddGameAnalysisData(request.result1, request.result2, request.User_Id, request.level, receivedData);

            return receivedData;
        }
    }
}
