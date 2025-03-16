namespace UKParliament.CodeTest.Data;

/// <summary>
///     Interacts with persisted <see cref="Person"/>s. 
/// </summary>
public interface IPeopleRepository
{
    public Task<Person?> GetByIdAsync(int id);
    public Task<List<Person>> GetAllAsync();
    public Task<int> CreateAsync(Person person);
    public Task<bool> UpdateAsync(Person person);
    public Task<bool> DeleteAsync(int id);
}
