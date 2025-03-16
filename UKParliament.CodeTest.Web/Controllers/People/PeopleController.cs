using Microsoft.AspNetCore.Mvc;
using UKParliament.CodeTest.Services.Departments;
using UKParliament.CodeTest.Services.People;

namespace UKParliament.CodeTest.Web.Controllers.People;

[ApiController]
[Tags("people")]
[Route("api/people")]
public class PeopleController : ControllerBase
{
    private readonly IPeopleService _people;
    private readonly IDepartmentService _departments;

    public PeopleController(IPeopleService people, IDepartmentService departments)
    {
        _people = people;
        _departments = departments;
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType<PersonViewModel>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IResult> GetByIdAsync([FromRoute] int id)
    {
        var model = await _people.GetByIdAsync(id);
        if (model is null)
            return Results.NotFound();
        
        var viewModel = PersonViewModel.From(model);
        return Results.Ok(viewModel);
    }

    [HttpGet("")]
    [ProducesResponseType<List<PersonViewModel>>(StatusCodes.Status200OK)]
    public async Task<IResult> GetAllAsync()
    {
        var models = await _people.GetAllAsync();
        var viewModels = models.Select(PersonViewModel.From);
        return Results.Ok(viewModels);
    }
    
    [HttpPost("")]
    [Validate<CreatePersonRequest>(nameof(request))]
    [ProducesResponseType<Guid>(StatusCodes.Status201Created)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<IResult> CreateAsync([FromBody] CreatePersonRequest request)
    {
        // This could be moved into a validation layer/service.
        var departmentExists = await _departments.ExistsAsync(request.DepartmentId);
        if (!departmentExists)
        {
            var errors = new Dictionary<string, string[]>
            {
                { nameof(CreatePersonRequest.DepartmentId), ["Department not found."] }
            };
            return Results.ValidationProblem(errors);
        }
        
        var model = request.ToModel();
        var id = await _people.CreateAsync(model);
        return Results.Created($"/api/person/{id}", id);
    }

    [HttpPost("{id:int}")]
    [Validate<UpdatePersonRequest>(nameof(request))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IResult> UpdateAsync([FromRoute] int id, [FromBody] UpdatePersonRequest request)
    {
        var departmentExists = await _departments.ExistsAsync(request.DepartmentId);
        if (!departmentExists)
        {
            var errors = new Dictionary<string, string[]>
            {
                { nameof(UpdatePersonRequest.DepartmentId), ["Department not found."] }
            };
            return Results.ValidationProblem(errors);
        }
        
        var model = request.ToModel();
        var wasUpdated = await _people.UpdateAsync(id, model);
        if (wasUpdated)
            return Results.NoContent();

        return Results.NotFound();
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IResult> DeleteAsync([FromRoute] int id)
    {
        var wasDeleted = await _people.DeleteAsync(id);
        if (wasDeleted)
            return Results.NoContent();
        
        return Results.NotFound();
    }
}