using UKParliament.CodeTest.Services.Departments;

namespace UKParliament.CodeTest.Web.Controllers.Departments;

public record DepartmentViewModel(
    int Id,
    string Name
)
{
    public static DepartmentViewModel From(DepartmentModel model) =>
        new(model.Id, model.Name);
}

