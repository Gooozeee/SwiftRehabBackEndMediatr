using Microsoft.AspNetCore.Http;
using SwiftUserManagement.Domain.Entities;

namespace SwiftUserManagement.Application.Contracts.Infrastructure
{
    public interface IRabbitMQRepository
    {

        bool EmitGameAnalysis(GameResults gameResults);
        string ReceiveGameAnalysis();
        Task<bool> EmitVideoAnalysis(IFormFile video);
        string ReceiveVideoAnalysis();
    }
}
