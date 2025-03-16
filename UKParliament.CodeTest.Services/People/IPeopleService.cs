using UKParliament.CodeTest.Data;

namespace UKParliament.CodeTest.Services.People;

/// <summary>
///     Performs business logic to interact with the <see cref="IPeopleRepository"/>
/// </summary>
public interface IPeopleService
{
    /// <summary>
    ///     Gets a <see cref="PersonModel"/> by it's <paramref name="id"/>.
    /// </summary>
    /// <param name="id">The <see cref="Person.Id"/>.</param>
    /// <returns>A <see cref="PersonModel"/> if found; otherwise, <see langword="null"/>.</returns>
    public Task<PersonModel?> GetByIdAsync(int id);
    
    /// <summary>
    ///     Gets all <see cref="Person"/>s.
    /// </summary>
    public Task<List<PersonModel>> GetAllAsync();

    /// <summary>
    ///     Creates a new <see cref="Person"/> from <paramref name="personModel"/>.
    /// </summary>
    /// <param name="personModel">The model to create the person with.</param>
    /// <returns>The <see cref="Person.Id"/> of the created person.</returns>
    public Task<int> CreateAsync(CreatePersonModel personModel);
    
    /// <summary>
    ///     Updates a <see cref="Person"/> using <paramref name="personModel"/>.
    /// </summary>
    /// <param name="id">The <see cref="Person.Id"/> of the person.</param>
    /// <param name="personModel">The model to update the person with.</param>
    /// <returns><see langword="true"/> if the person exists and was updated; otherwise, <see langword="false"/>.</returns>
    public Task<bool> UpdateAsync(int id, UpdatePersonModel personModel);
    
    /// <summary>
    ///     Deletes a <see cref="Person"/>.
    /// </summary>
    /// <param name="id">The <see cref="Person.Id"/> of the person.</param>
    /// <returns><see langword="true"/> if the person exists and was deleted; otherwise, <see langword="false"/>.</returns>
    public Task<bool> DeleteAsync(int id);
}
