using FakeItEasy;
using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Services.People;
using Xunit;

namespace UKParliament.CodeTest.Tests.Services;

// For time limitations, not every method has a test, and the department service isn't tested.
// Only UpdateAsync is tested as an example of testing the service (as it's the most complex method).
public class PeopleService_UpdateAsync_Tests
{
    private static DateOnly DateOfBirth = DateOnly.FromDateTime(DateTime.Parse("2010-02-03"));
    
    [Fact]
    public async Task UpdateAsync_NullPerson_Throws_ArgumentNullException()
    {
        // Arrange
        var personId = 1;

        var repo = A.Fake<IPeopleRepository>();
        var service = new PeopleService(repo);
        UpdatePersonModel? personModel = null;
        
        // Act
        Task<bool> Act() => service.UpdateAsync(personId, personModel!);
        
        // Assert
        await Assert.ThrowsAsync<ArgumentNullException>(Act);
    }

    [Fact]
    public async Task UpdateAsync_InvalidPerson_ReturnsFalse()
    {
        // Arrange
        var missingPersonId = 999_999_999;
        
        var repo = A.Fake<IPeopleRepository>();
        A.CallTo(() => repo.GetByIdAsync(missingPersonId))
            .Returns(Task.FromResult<Person?>(null));

        var service = new PeopleService(repo);
        var personModel = new UpdatePersonModel("First", "Last", DateOfBirth, 1);
        
        // Act
        var wasUpdated = await service.UpdateAsync(missingPersonId, personModel);
        
        // Assert
        Assert.False(wasUpdated);
    }

    [Fact]
    public async Task UpdateAsync_ValidPerson_IsUpdated()
    {
        // Arrange
        var personId = 1;

        var dbPerson = new Person
        {
            Id = personId,
            FirstName = "First",
            LastName = "Last",
            DateOfBirth = DateOfBirth,
            DepartmentId = 1
        };
        
        var repo = A.Fake<IPeopleRepository>();
        A.CallTo(() => repo.GetByIdAsync(personId))
            .Returns(Task.FromResult<Person?>(dbPerson));

        A.CallTo(() => repo.UpdateAsync(A<Person>._))
            .Invokes(call =>
            {
                // Pretty dumb logic for simplicity
                var item = call.GetArgument<Person>(0);
                Assert.NotNull(item);
                Assert.Equal(personId, item.Id);
                dbPerson.FirstName = item.FirstName;
                dbPerson.LastName = item.LastName;
                dbPerson.DateOfBirth = item.DateOfBirth;
                dbPerson.DepartmentId = item.DepartmentId;
            })
            .Returns(Task.FromResult(true));

        var service = new PeopleService(repo);
        
        // Act
        var person = new UpdatePersonModel("Updated First", "Updated Last", DateOfBirth.AddDays(5), 2);
        var wasUpdated = await service.UpdateAsync(personId, person);
        
        // Assert
        Assert.True(wasUpdated);
        Assert.Equal(dbPerson.FirstName, person.FirstName);
        Assert.Equal(dbPerson.LastName, person.LastName);
        Assert.Equal(dbPerson.DateOfBirth, person.DateOfBirth);
        Assert.Equal(dbPerson.DepartmentId, person.DepartmentId);
        A.CallTo(() => repo.UpdateAsync(A<Person>._))
            .MustHaveHappenedOnceExactly();
    }
}
