using FluentValidation;
using WebApplication1.Models;

namespace WebApplication1.Validators;

public class StudentValidator : AbstractValidator<Student>
{
    public StudentValidator()
    {
        RuleFor(s => s.Id).NotNull().GreaterThan(0)
            .WithMessage("StudentId must be not null and greater than zero");
        
        RuleFor(s=>s.Name).NotNull().NotEmpty()
            .WithMessage("Name must be not null and not empty");
        
        RuleFor(s => s.Email).NotEmpty()
            .EmailAddress().WithMessage("A valid email is required.");
        
    }
}
