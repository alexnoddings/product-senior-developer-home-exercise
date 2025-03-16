using UKParliament.CodeTest.Data;

namespace UKParliament.CodeTest.Services.People;

public record PersonModel(
    int Id,
    string FirstName,
    string LastName,
    string Email,
    DateOnly DateOfBirth,
    PersonDepartmentModel Department
)
{
    // The domain isn't complex enough to warrant an entire mapping service imo.
    // Instead, models in the services layer handle their own mapping.
    // I also tend to stay away from mapping libraries;
    // if mapping is complex enough that models can't handle it themselves,
    // then hiding said complexity away in a 3rd party mapping library usually makes it even more complex/harder to work with.
    internal static PersonModel From(Person person)
    {
        var department = PersonDepartmentModel.From(person.Department);
        return new(person.Id, person.FirstName, person.LastName, person.Email, person.DateOfBirth, department);
    }
}

public record PersonDepartmentModel(
    int Id,
    string Name
)
{
    internal static PersonDepartmentModel From(Department department) =>
        new(department.Id, department.Name);
}