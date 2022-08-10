using Microsoft.AspNetCore.Http;
using SwiftUserManagement.Domain.Entities;

namespace SwiftUserManagement.Application.Contracts.Infrastructure
{
    public interface IRabbitMQRepository
    {

        Task<bool> EmitGameAnalysis(string result1, string result2);
        string ReceiveGameAnalysis();
        Task<bool> EmitVideoAnalysis(IFormFile video);
        string ReceiveVideoAnalysis();
    }
}
