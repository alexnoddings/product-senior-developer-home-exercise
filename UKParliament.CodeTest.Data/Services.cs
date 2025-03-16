using Microsoft.Extensions.DependencyInjection;

namespace UKParliament.CodeTest.Data;

public static class AppDataServicesExtensions
{
    public static IServiceCollection AddPersonManagerDataServices(this IServiceCollection services)
    {
        services.AddScoped<IPeopleRepository, PeopleRepository>();
        services.AddScoped<IDepartmentRepository, DepartmentRepository>();
        
        return services;
    }
}
