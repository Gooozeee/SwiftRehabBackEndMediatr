using AutoMapper;
using MediatR;
using SwiftUserManagement.Application.Contracts.Infrastructure;

namespace SwiftUserManagement.Application.Features.Commands.AnalyseGameResults
{
    public class AnalyseGameResultsHandler : IRequestHandler<AnalyseGameResultsCommand, string>
    {
        private readonly IRabbitMQRepository _rabbitMQRepository;
        private readonly IMapper _mapper;

        public AnalyseGameResultsHandler(IRabbitMQRepository rabbitMQRepository, IMapper mapper)
        {
            _rabbitMQRepository = rabbitMQRepository ?? throw new ArgumentNullException(nameof(rabbitMQRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<string> Handle(AnalyseGameResultsCommand request, CancellationToken cancellationToken)
        {
            var result = await _rabbitMQRepository.EmitGameAnalysis(request.result1, request.result2);
            if (!result)
                return "User not found";

            var receivedData = _rabbitMQRepository.ReceiveGameAnalysis();

            return receivedData;
        }
    }
}
