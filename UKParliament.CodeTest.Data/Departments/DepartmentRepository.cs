using Microsoft.EntityFrameworkCore;

namespace UKParliament.CodeTest.Data;

internal class DepartmentRepository : IDepartmentRepository
{
    private readonly PersonManagerContext _dbContext;

    public DepartmentRepository(PersonManagerContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Department>> GetAllAsync()
    {
        var departments =
            await _dbContext
                .Departments
                .AsNoTracking()
                .ToListAsync();
        
        return departments;
    }

    public async Task<bool> ExistsAsync(int id)
    {
        var departmentExists =
            await _dbContext
                .Departments
                .AnyAsync(d => d.Id == id);
        
        return departmentExists;
    }
}
