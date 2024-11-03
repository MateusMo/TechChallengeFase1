using FluentValidation;
using ContactZone.Api.Dtos;

public class PostContactDtoValidator : AbstractValidator<PostContactDto>
{
    public PostContactDtoValidator()
    {
        RuleFor(contact => contact.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");

        RuleFor(contact => contact.DDD)
            .NotEmpty().WithMessage("DDD is required.")
            .Matches(@"^\d{2}$").WithMessage("DDD must be exactly 2 digits.")
            .Must(ddd => int.TryParse(ddd, out int num) && num >= 11 && num <= 99)
            .WithMessage("DDD must be a valid Brazilian DDD between 11 and 99.");

        RuleFor(contact => contact.Phone)
            .NotEmpty().WithMessage("Phone is required.")
            .Matches(@"^\d{8,9}$").WithMessage("Phone must be 8 or 9 digits long.");

        RuleFor(contact => contact.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid Email Address.");
    }
}
