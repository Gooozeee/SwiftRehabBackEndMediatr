using SwiftUserManagement.Domain.Entities;

namespace SwiftUserManagement.Application.Contracts.Persistence
{

    public interface IUserRepository 
    {
        Task<User> GetUser(string userName);
        Task<User> GetUserByEmail(string email);
        Task<bool> CreateUser(string email, string userName, string password, string role);
        Task<bool> UpdateUser(User user);
        Task<bool> AddVideoAnalysisData(string videoName, int userId, string weaknessPrediction);
    }
}
