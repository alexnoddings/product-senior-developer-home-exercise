using Microsoft.AspNetCore.Mvc;
using UKParliament.CodeTest.Services.Departments;

namespace UKParliament.CodeTest.Web.Controllers.Departments;

[ApiController]
[Tags("departments")]
[Route("api/departments")]
public class DepartmentsController : ControllerBase
{
    private readonly IDepartmentService _departments;

    public DepartmentsController(IDepartmentService departments)
    {
        _departments = departments;
    }

    [HttpGet("")]
    public async Task<IResult> GetAllAsync()
    {
        var models = await _departments.GetAllAsync();
        var viewModels = models.Select(DepartmentViewModel.From).OrderBy(d => d.Name);
        return Results.Ok(viewModels);
    }
}