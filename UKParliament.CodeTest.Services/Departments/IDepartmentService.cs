using UKParliament.CodeTest.Data;

namespace UKParliament.CodeTest.Services.Departments;

/// <summary>
///     Performs business logic to interact with the <see cref="IDepartmentRepository"/>
/// </summary>
public interface IDepartmentService
{
    /// <summary>
    ///     Gets all <see cref="Department"/>s.
    /// </summary>
    public Task<List<DepartmentModel>> GetAllAsync();

    /// <summary>
    ///     Gets whether a <see cref="Department"/> exists.
    /// </summary>
    /// <param name="id">The <see cref="Department.Id"/>.</param>
    public Task<bool> ExistsAsync(int id);
}
