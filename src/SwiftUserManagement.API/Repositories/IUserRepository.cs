using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SwiftUserManagement.API.Entities;

namespace SwiftUserManagement.API.Repositories
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
