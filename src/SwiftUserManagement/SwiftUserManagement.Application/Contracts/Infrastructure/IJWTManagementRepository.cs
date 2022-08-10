using SwiftUserManagement.Domain.Entities;

namespace SwiftUserManagement.Application.Contracts.Infrastructure
{
    // Interface for managing JWT tokens
    public interface IJWTManagementRepository
    {
        Task<Tokens> Authenticate(string email, string password);
    }
}
