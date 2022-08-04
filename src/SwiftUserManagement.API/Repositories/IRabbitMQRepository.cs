using SwiftUserManagement.API.Entities;

namespace SwiftUserManagement.API.Repositories
{
    public interface IRabbitMQRepository
    {

        bool EmitGameAnalysis(GameResults gameResults);
        string ReceiveGameAnalysis();
        Task<bool> EmitVideoAnalysis(IFormFile video);
        string ReceiveVideoAnalysis();
    }
}
