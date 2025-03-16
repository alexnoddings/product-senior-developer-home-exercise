using Microsoft.EntityFrameworkCore;

namespace UKParliament.CodeTest.Data;

// Repo doesn't run any validation - we presume an above layer has.
// If any data is invalid, the DB constraints will cause saving to change (at least it would for a real SQL database).
internal class PeopleRepository : IPeopleRepository
{
    private readonly PersonManagerContext _context;

    public PeopleRepository(PersonManagerContext context)
    {
        _context = context;
    }

    public async Task<Person?> GetByIdAsync(int id)
    {
        var person =
            await _context
                .People
                .AsNoTracking()
                .Include(p => p.Department)
                .FirstOrDefaultAsync(p => p.Id == id);

        return person;
    }

    public async Task<List<Person>> GetAllAsync()
    {
        var people =
            await _context
                .People
                .AsNoTracking()
                .Include(p => p.Department)
                .ToListAsync();

        return people;
    }

    public async Task<int> CreateAsync(Person person)
    {
        ArgumentNullException.ThrowIfNull(person);

        _context.People.Add(person);
        await _context.SaveChangesAsync();

        return person.Id;
    }

    public async Task<bool> UpdateAsync(Person person)
    {
        ArgumentNullException.ThrowIfNull(person);

        var dbPerson =
            await _context
                .People
                .FirstOrDefaultAsync(p => p.Id == person.Id);

        if (dbPerson is null)
            return false;

        _context.Entry(dbPerson).CurrentValues.SetValues(person);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var dbPerson =
            await _context
                .People
                .FirstOrDefaultAsync(p => p.Id == id);

        if (dbPerson is null)
            return false;

        // As an optimisation, we could use .AnyAsync() instead to determine if the person exists
        // without reading the model out of the database and materialising it in memory,
        // then manually set the EntityEntry as deleted.
        _context.People.Remove(dbPerson);
        await _context.SaveChangesAsync();

        return true;
    }
}
