using UKParliament.CodeTest.Data;

namespace UKParliament.CodeTest.Services.Departments;

public record DepartmentModel(
    int Id,
    string Name
)
{
    internal static DepartmentModel From(Department department) =>
        new(department.Id, department.Name);
}
