using UKParliament.CodeTest.Data;

namespace UKParliament.CodeTest.Services.People;

// Implementation is identical to the creation model for now.
// They're left separate as the overhead from duplication is very low,
// and having them separate allows the models to diverge easily later if requirements change
// (eg can only set DateOfBirth/Department at creation)
public record UpdatePersonModel(
    string FirstName,
    string LastName,
    string Email,
    DateOnly DateOfBirth,
    int DepartmentId
)
{
    internal Person ToEntity() =>
        new()
        {
            FirstName = FirstName,
            LastName = LastName,
            Email = Email,
            DateOfBirth = DateOfBirth,
            DepartmentId = DepartmentId
        };
}
