using AutoMapper;
using MediatR;
using SwiftUserManagement.Application.Contracts.Infrastructure;

namespace SwiftUserManagement.Application.Features.Commands.AuthenticateUser
{
    public class AuthenticateUserHandler : IRequestHandler<AuthenticateUserCommand, TokenVM>
    {
        private readonly IJWTManagementRepository _jwtManagementRepository;
        private readonly IMapper _mapper;

        public AuthenticateUserHandler(IJWTManagementRepository jwtManagementRepository, IMapper mapper)
        {
            _jwtManagementRepository = jwtManagementRepository ?? throw new ArgumentNullException(nameof(jwtManagementRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<TokenVM> Handle(AuthenticateUserCommand request, CancellationToken cancellationToken)
        {
            var token = await _jwtManagementRepository.Authenticate(request.Email, request.Password);
            if(token.Token == "Unauthorized")
            {
                return null;
            }

            return _mapper.Map<TokenVM>(token);
        }
    }
}
