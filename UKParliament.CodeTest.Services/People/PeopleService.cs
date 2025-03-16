using UKParliament.CodeTest.Data;

namespace UKParliament.CodeTest.Services.People;

internal class PeopleService : IPeopleService
{
    private readonly IPeopleRepository _repository;

    public PeopleService(IPeopleRepository repository)
    {
        _repository = repository;
    }

    public async Task<PersonModel?> GetByIdAsync(int id)
    {
        var person = await _repository.GetByIdAsync(id);
        if (person is null)
            return null;
        
        var personModel = PersonModel.From(person);
        return personModel;
    }

    public async Task<List<PersonModel>> GetAllAsync()
    {
        var people = await _repository.GetAllAsync();
        var peopleModels = people.Select(PersonModel.From).ToList();
        
        return peopleModels;
    }

    public async Task<int> CreateAsync(CreatePersonModel personModel)
    {
        ArgumentNullException.ThrowIfNull(personModel);

        var person = personModel.ToEntity();
        var createdId = await _repository.CreateAsync(person);
        return createdId;
    }

    public async Task<bool> UpdateAsync(int id, UpdatePersonModel personModel)
    {
        ArgumentNullException.ThrowIfNull(personModel);

        var person = personModel.ToEntity();
        person.Id = id;
        var wasUpdated = await _repository.UpdateAsync(person);
        return wasUpdated;
    }

    public async Task<bool> DeleteAsync(int id) =>
        await _repository.DeleteAsync(id);
}
