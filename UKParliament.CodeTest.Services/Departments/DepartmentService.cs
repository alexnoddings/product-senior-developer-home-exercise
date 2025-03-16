using UKParliament.CodeTest.Data;

namespace UKParliament.CodeTest.Services.Departments;

internal class DepartmentService : IDepartmentService
{
    private readonly IDepartmentRepository _departmentRepository;

    public DepartmentService(IDepartmentRepository departmentRepository)
    {
        _departmentRepository = departmentRepository;
    }

    public async Task<List<DepartmentModel>> GetAllAsync()
    {
        var departments = await _departmentRepository.GetAllAsync();
        var departmentModels = departments.Select(DepartmentModel.From).ToList();
        return departmentModels;
    }

    public async Task<bool> ExistsAsync(int id) =>
        await _departmentRepository.ExistsAsync(id);
}
