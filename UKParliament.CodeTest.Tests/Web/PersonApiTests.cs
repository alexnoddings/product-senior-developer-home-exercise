using System.Net;
using System.Net.Http.Json;
using UKParliament.CodeTest.Web.Controllers.People;
using Xunit;

namespace UKParliament.CodeTest.Tests.Web;

// For time limitations, not every endpoint has a test, and the department controller isn't tested.
// This is also more of an e2e test than a unit test.
// Ideally the tests project should be split into data/services/web tests for quicker running and logical separation,
// but for time/ease they're all left in the one project. 
[Collection(WebApiCollection.Name)]
public class PersonApiTests
{
    private readonly WebApiFixture _fixture;

    public PersonApiTests(WebApiFixture fixture)
    {
        _fixture = fixture;
    }
    
    [Fact]
    public async Task Create_NullBody_ReturnsBadRequest()
    {
        // Arrange
        var requestContent = JsonContent.Create<CreatePersonRequest?>(null);
        var httpClient = _fixture.CreateApiClient();
        
        // Act
        var response = await httpClient.PostAsync("/api/people/", requestContent);
        
        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        // should also ensure the body contains an error message which adequately describes the error
    }

    public static TheoryData<CreatePersonRequest> InvalidCreatePersonRequests =>
        new()
        {
            new CreatePersonRequest { FirstName = "", LastName = "", DateOfBirth = "", DepartmentId = 0 },
            new CreatePersonRequest { FirstName = "", LastName = "Jensen", DateOfBirth = "2020-01-01", DepartmentId = 0 },
            new CreatePersonRequest { FirstName = "Adam", LastName = "", DateOfBirth = "2020-01-01", DepartmentId = 0 },
            new CreatePersonRequest { FirstName = "Adam", LastName = "Jensen", DateOfBirth = "not a date", DepartmentId = 0 },
            new CreatePersonRequest { FirstName = "Adam", LastName = "Jensen", DateOfBirth = "1800-01-01", DepartmentId = 0 },
            new CreatePersonRequest { FirstName = "Adam", LastName = "Jensen", DateOfBirth = "2500-01-01", DepartmentId = 0 },
        };
    
    [Theory]
    [MemberData(nameof(InvalidCreatePersonRequests))]
    public async Task Create_InvalidRequest_ReturnsBadRequest(CreatePersonRequest request)
    {
        // Arrange
        var requestContent = JsonContent.Create(request);
        var httpClient = _fixture.CreateApiClient();
        
        // Act
        var response = await httpClient.PostAsync("/api/people/", requestContent);
        
        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        // should also ensure the body contains an error message which adequately describes the error
    }

    [Fact]
    public async Task Create_ValidPerson_IsCreated()
    {
        // Arrange
        var request = new CreatePersonRequest { FirstName = "Adam", LastName = "Jensen", DateOfBirth = "2023-04-05", DepartmentId = 1 };
        var requestContent = JsonContent.Create(request);
        var httpClient = _fixture.CreateApiClient();
        
        // Act
        var response = await httpClient.PostAsync("/api/people/", requestContent);
        
        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        
        // Ensure it returns a valid int
        var responseId = await response.Content.ReadFromJsonAsync<int?>();
        Assert.NotNull(responseId);
        Assert.NotEqual(0, responseId);
        
        // Ensure it returns a created at location
        var createdAtUri = response.Headers.Location;
        Assert.NotNull(createdAtUri);
    }
}
