using UKParliament.CodeTest.Data;

namespace UKParliament.CodeTest.Services.People;

public record CreatePersonModel(
    string FirstName,
    string LastName,
    DateOnly DateOfBirth,
    int DepartmentId
)
{
    internal Person ToEntity() =>
        new()
        {
            FirstName = FirstName,
            LastName = LastName,
            DateOfBirth = DateOfBirth,
            DepartmentId = DepartmentId
        };
}
