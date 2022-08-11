using Microsoft.AspNetCore.Http;
using SwiftUserManagement.Domain.Entities;

namespace SwiftUserManagement.Application.Contracts.Infrastructure
{
    public interface IRabbitMQRepository
    {

        Task<bool> EmitGameAnalysis(int result1, int result2);
        string ReceiveGameAnalysis();
        Task<bool> EmitVideoAnalysis(IFormFile video);
        string ReceiveVideoAnalysis();
    }
}
