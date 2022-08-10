using AutoMapper;
using MediatR;
using SwiftUserManagement.Application.Contracts.Persistence;
using SwiftUserManagement.Domain.Entities;

namespace SwiftUserManagement.Application.Features.Commands.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, int>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public CreateUserCommandHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<int> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = _mapper.Map<User>(request);
            var result = await _userRepository.CreateUser(user.Email, user.UserName, user.Password, user.Role);

            if (result == false)
                return -1;

            return 1;
        }
    }
}
