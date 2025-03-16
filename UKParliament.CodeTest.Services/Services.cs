using Microsoft.Extensions.DependencyInjection;
using UKParliament.CodeTest.Services.Departments;
using UKParliament.CodeTest.Services.People;

namespace UKParliament.CodeTest.Services;

public static class AppServicesExtensions
{
    public static IServiceCollection AddPersonManagerServices(this IServiceCollection services)
    {
        services.AddScoped<IPeopleService, PeopleService>();
        services.AddScoped<IDepartmentService, DepartmentService>();
        
        return services;
    }
}
