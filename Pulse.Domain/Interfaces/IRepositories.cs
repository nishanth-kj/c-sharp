using Pulse.Domain.Entities;

namespace Pulse.Domain.Interfaces;

public interface IActivityRepository
{
    Task AddAsync(ActivityRecord record);
    Task<List<ActivityRecord>> GetByDateRangeAsync(DateTime start, DateTime end);
    Task<List<ActivityRecord>> GetTodayAsync();
    Task UpdateAsync(ActivityRecord record);
}

public interface IGoalRepository
{
    Task AddAsync(Goal goal);
    Task<List<Goal>> GetAllActiveAsync();
    Task<Goal?> GetByIdAsync(int id);
    Task UpdateAsync(Goal goal);
    Task DeleteAsync(int id);
}

public interface IDailySnapshotRepository
{
    Task AddOrUpdateAsync(DailySnapshot snapshot);
    Task<DailySnapshot?> GetByDateAsync(DateTime date);
    Task<List<DailySnapshot>> GetRangeAsync(DateTime start, DateTime end);
}

public interface ICategoryRepository
{
    Task AddAsync(Category category);
    Task<List<Category>> GetAllAsync();
    Task<Category?> GetByIdAsync(int id);
    Task UpdateAsync(Category category);
    Task DeleteAsync(int id);
    Task<AppCategoryMapping?> GetMappingByProcessNameAsync(string processName);
    Task AddMappingAsync(AppCategoryMapping mapping);
}
