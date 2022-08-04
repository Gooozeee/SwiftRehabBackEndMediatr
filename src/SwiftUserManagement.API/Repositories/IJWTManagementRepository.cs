using SwiftUserManagement.API.Entities;

namespace SwiftUserManagement.API.Repositories
{
    // Interface for managing JWT tokens
    public interface IJWTManagementRepository
    {
        Tokens Authenticate(string email, string password);
    }
}
