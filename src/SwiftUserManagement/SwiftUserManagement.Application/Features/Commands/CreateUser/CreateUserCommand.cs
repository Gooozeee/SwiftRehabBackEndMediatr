using MediatR;

namespace SwiftUserManagement.Application.Features.Commands.CreateUser
{
    public class CreateUserCommand : IRequest<int>
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
