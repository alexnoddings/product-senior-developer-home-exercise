namespace UKParliament.CodeTest.Data;

/// <summary>
///     Interacts with persisted <see cref="Department"/>s. 
/// </summary>
public interface IDepartmentRepository
{
    public Task<List<Department>> GetAllAsync();

    public Task<bool> ExistsAsync(int id);
}
