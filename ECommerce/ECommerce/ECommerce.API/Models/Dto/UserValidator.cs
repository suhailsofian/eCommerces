using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;

namespace ECommerce.API.Models
{
   public class UserValidator : AbstractValidator<UserDto>
{
    public UserValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .MinimumLength(4)
            .Must(IsValidName).WithMessage("{PropertyName} should be all letters.");
        RuleFor(x => x.LastName).NotEmpty();
        // RuleFor(x => x.LastName).NotEmpty().MaximumLength(10);
        RuleFor(x => x.Email).EmailAddress().WithName("MailID").WithMessage("{PropertyName} is invalid! Please check!");
        RuleFor(x => x.Password).NotEmpty();
       RuleFor(x => x.Mobile).Matches("^\\d+$").WithMessage("only numbers");
    }

    private bool IsValidName(string name)
    {
        return name.All(Char.IsLetter);
    }
}
}