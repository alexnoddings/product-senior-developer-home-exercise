using Microsoft.EntityFrameworkCore;
using UKParliament.CodeTest.Data;
using Xunit;

namespace UKParliament.CodeTest.Tests.Data;

// For time limitations, not every method has a test, and the department repo isn't tested.
// Only UpdateAsync is tested as an example of testing the repo (as it's the most complex method).
public class PeopleRepository_UpdateAsync_Tests
{
    [Fact]
    public async Task Update_NullPerson_Throws_ArgumentNullException()
    {
        await using var dbContext = CreateDbContext();
        var repo = new PeopleRepository(dbContext);
        
        // Arrange
        Person? item = null;

        // Act
        Task<bool> Act() => repo.UpdateAsync(item!);

        // Assert
        await Assert.ThrowsAsync<ArgumentNullException>(Act);
    }
    
    [Fact]
    public async Task Update_InvalidPerson_ReturnsFalse()
    {
        // Arrange
        var missingPersonId = 999_999_999;
        
        await using var dbContext = CreateDbContext();
        var repo = new PeopleRepository(dbContext);

        // Act
        var person = new Person
        {
            Id = missingPersonId,
            FirstName = "first",
            LastName = "last",
            DateOfBirth = DateOnly.FromDateTime(DateTime.Parse("2021-02-03")),
            DepartmentId = 1
        };
        var updated = await repo.UpdateAsync(person);
        
        // Assert
        Assert.False(updated);
    }
    
    [Fact]
    public async Task Update_ExistingPerson_IsUpdated()
    {
        // Arrange
        var personId = 1;
        var newTodo = new Person
        {
            Id = personId,
            FirstName = "old first",
            LastName = "old last",
            DateOfBirth = DateOnly.FromDateTime(DateTime.Parse("2001-02-03")),
            DepartmentId = 1
        };
        await CreatePersonAsync(newTodo);
        
        await using var dbContext = CreateDbContext();
        var repo = new PeopleRepository(dbContext);

        // Act
        var person = new Person
        {
            Id = personId,
            FirstName = "updated first",
            LastName = "updated last",
            DateOfBirth = DateOnly.FromDateTime(DateTime.Parse("2021-02-03")),
            DepartmentId = 3
        };
        var updated = await repo.UpdateAsync(person);
        
        // Assert
        Assert.True(updated);
        var dbPerson = await GetPersonAsync(personId);
        Assert.NotNull(dbPerson);
        Assert.Equal(person.FirstName, dbPerson.FirstName);
        Assert.Equal(person.LastName, dbPerson.LastName);
        Assert.Equal(person.DateOfBirth, dbPerson.DateOfBirth);
        Assert.Equal(person.DepartmentId, dbPerson.DepartmentId);
    }

    // Helper methods, these should be pulled out into a common class if more tests are added
    private static PersonManagerContext CreateDbContext()
    {
        // If this was strictly a unit test, we'd mock the DbContext rather than new-ing one up.
        // But that hides a lot of hidden complexity with EF, and is quite time-consuming to mock for this exercise.
        
        // Ideally for an integration test, this would use something like test containers
        // to make sure we're testing all the way down to the database level.
        var optionsBuilder = new DbContextOptionsBuilder<PersonManagerContext>();
        optionsBuilder.UseInMemoryDatabase("tests");
        var options = optionsBuilder.Options;
        var dbContext = new PersonManagerContext(options); 
        return dbContext;
    }
    
    private static async Task CreatePersonAsync(Person person)
    {
        await using var dbContext = CreateDbContext();
        dbContext.People.Add(person);
        await dbContext.SaveChangesAsync();
    }

    private static async Task<Person?> GetPersonAsync(int id)
    {
        await using var dbContext = CreateDbContext();
        return await dbContext.People.AsNoTracking().FirstOrDefaultAsync(person => person.Id == id);
    }
}
