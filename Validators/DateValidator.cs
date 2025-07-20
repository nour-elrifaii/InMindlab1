using FluentValidation;
using WebApplication1.Models;

namespace WebApplication2.Validators;

public class DateValidator : AbstractValidator<Date>
{
    private static readonly string[] AvailableCultures
        = { "en-US", "es-ES", "fr-FR" };

    public DateValidator()
    {
        RuleFor(d => d.Language).NotEmpty().WithMessage("Language is required")
            .Must(d => AvailableCultures.Contains(d)).WithMessage("Language Unavailable");
    }
}