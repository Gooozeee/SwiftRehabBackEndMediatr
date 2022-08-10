using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwiftUserManagement.Application.Features.Commands.AuthenticateUser
{
    public class AuthenticateUserValidator : AbstractValidator<AuthenticateUserCommand>
    {
        public AuthenticateUserValidator()
        {
            RuleFor(user => user.Email)
                .NotEmpty().WithMessage("Username is required")
                .NotNull().WithMessage("Username is required");

            RuleFor(user => user.Password)
                .NotEmpty().WithMessage("Passowrd is required")
                .NotNull().WithMessage("Password is required");
        }
    }
}
