using FluentValidation;
using FluentValidation.Validators;
using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Services.People;

namespace UKParliament.CodeTest.Web.Controllers.People;

public class CreatePersonRequest
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? DateOfBirth { get; set; }
    public int DepartmentId { get; set; }
    
    // The domain isn't complex enough to warrant an entire mapping service imo.
    // Instead, models in the services layer handle their own mapping.
    internal CreatePersonModel ToModel()
    {
        var dob = DateOnly.Parse(DateOfBirth!);
        return new(FirstName!, LastName!, Email!, dob, DepartmentId);
    }
}

public class CreatePersonRequestValidator : AbstractValidator<CreatePersonRequest>
{
    public CreatePersonRequestValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(PersonConstraints.FirstName_MaxLength);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(PersonConstraints.LastName_MaxLength);
        // Default FluentValidation email validation is *very* lax, and only cares about having an @
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.DepartmentId).NotEmpty();

        // This is complex enough that it should probably be pulled into a custom validation extension method:
        // https://docs.fluentvalidation.net/en/latest/custom-validators.html
        RuleFor(x => x.DateOfBirth)
            .Must(str =>
                DateOnly.TryParse(str, out var dob)
                && PersonConstraints.DateOfBirth_Minimum <= dob
                && dob <= PersonConstraints.DateOfBirth_Maximum
            )
            .WithMessage(_ =>
            {
                // Explicitly done as a delegate rather than a compute-once string so that the maximum in the message is updated every day
                var dob = nameof(CreatePersonRequest.DateOfBirth);
                var min = PersonConstraints.DateOfBirth_Minimum;
                var max = PersonConstraints.DateOfBirth_Maximum;

                return $"'{dob}' should be a valid date (yyyy/mm/dd), on or after '{min:yyyy/MM/dd}', and before '{max:yyyy/MM/dd}'.";
            });
    }
}
