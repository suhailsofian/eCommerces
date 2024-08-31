using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;

namespace ECommerce.API.Models
{
   public class UserLoginValidator : AbstractValidator<UserLoginDto>
{
    public UserLoginValidator()
    {
       
        // RuleFor(x => x.LastName).NotEmpty().MaximumLength(10);
        RuleFor(x => x.Email).EmailAddress().WithName("MailID").WithMessage("{PropertyName} is invalid! Please check!");
        RuleFor(x => x.Password).NotEmpty().MinimumLength(8);
      
    }

    private bool IsValidName(string name)
    {
        return name.All(Char.IsLetter);
    }
}
}