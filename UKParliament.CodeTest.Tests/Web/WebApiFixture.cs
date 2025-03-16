using Microsoft.AspNetCore.Mvc.Testing;
using UKParliament.CodeTest.Web;
using Xunit;

namespace UKParliament.CodeTest.Tests.Web;

[CollectionDefinition(Name)]
public class WebApiCollection : ICollectionFixture<WebApiFixture>
{
    public const string Name = "WebAPI collection";
}

// Lets us share one instance of the WebAPI (which is expensive to construct/run, relative to a test) across test classes
public sealed class WebApiFixture : IDisposable
{
    private readonly WebApplicationFactory<Program> _webApplicationFactory = new();
    
    public HttpClient CreateApiClient() => _webApplicationFactory.CreateClient();

    public void Dispose() => _webApplicationFactory.Dispose();
}