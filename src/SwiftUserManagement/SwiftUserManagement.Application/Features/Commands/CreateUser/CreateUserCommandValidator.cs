using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwiftUserManagement.Application.Features.Commands.CreateUser
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(user => user.UserName)
                .NotEmpty().WithMessage("Username is required")
                .NotNull().WithMessage("Username is required");

            RuleFor(user => user.Password)
                .NotEmpty().WithMessage("Passowrd is required")
                .NotNull().WithMessage("Password is required");

        }
    }
}
