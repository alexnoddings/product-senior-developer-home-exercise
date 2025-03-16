using UKParliament.CodeTest.Services.People;
using UKParliament.CodeTest.Web.Controllers.Departments;

namespace UKParliament.CodeTest.Web.Controllers.People;

public record PersonViewModel(
    int Id,
    string FirstName,
    string LastName,
    DateOnly DateOfBirth,
    DepartmentViewModel Department
)
{
    public static PersonViewModel From(PersonModel model)
    {
        var department = new DepartmentViewModel(model.Department.Id, model.Department.Name);
        return new(model.Id, model.FirstName, model.LastName, model.DateOfBirth, department);
    }
}
