using UKParliament.CodeTest.Data;

namespace UKParliament.CodeTest.Services.People;

public record CreatePersonModel(
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
