using Microsoft.EntityFrameworkCore;
using SharpIB.Domain.Entities;
using SharpIB.Domain.Interfaces;
using SharpIB.Infrastructure.Data;

namespace SharpIB.Infrastructure.Repositories;

public class ActivityRepository(SharpIBDbContext db) : IActivityRepository
{
    public async Task AddAsync(ActivityRecord record)
    {
        db.Activities.Add(record);
        await db.SaveChangesAsync();
    }

    public async Task<List<ActivityRecord>> GetByDateRangeAsync(DateTime start, DateTime end)
    {
        return await db.Activities
            .Include(a => a.Category)
            .Where(a => a.StartTime >= start && a.StartTime < end)
            .OrderByDescending(a => a.StartTime)
            .ToListAsync();
    }

    public async Task<List<ActivityRecord>> GetTodayAsync()
    {
        var today = DateTime.Today;
        return await GetByDateRangeAsync(today, today.AddDays(1));
    }

    public async Task UpdateAsync(ActivityRecord record)
    {
        db.Activities.Update(record);
        await db.SaveChangesAsync();
    }
}

public class GoalRepository(SharpIBDbContext db) : IGoalRepository
{
    public async Task AddAsync(Goal goal)
    {
        db.Goals.Add(goal);
        await db.SaveChangesAsync();
    }

    public async Task<List<Goal>> GetAllActiveAsync()
    {
        return await db.Goals
            .Include(g => g.TrackedCategory)
            .Where(g => g.Status == Domain.Enums.GoalStatus.Active)
            .ToListAsync();
    }

    public async Task<Goal?> GetByIdAsync(int id)
    {
        return await db.Goals.Include(g => g.TrackedCategory).FirstOrDefaultAsync(g => g.Id == id);
    }

    public async Task UpdateAsync(Goal goal)
    {
        db.Goals.Update(goal);
        await db.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var goal = await db.Goals.FindAsync(id);
        if (goal != null)
        {
            db.Goals.Remove(goal);
            await db.SaveChangesAsync();
        }
    }
}

public class DailySnapshotRepository(SharpIBDbContext db) : IDailySnapshotRepository
{
    public async Task AddOrUpdateAsync(DailySnapshot snapshot)
    {
        var existing = await db.DailySnapshots.FirstOrDefaultAsync(s => s.Date.Date == snapshot.Date.Date);
        if (existing != null)
        {
            existing.TotalActiveTime = snapshot.TotalActiveTime;
            existing.ProductiveTime = snapshot.ProductiveTime;
            existing.NeutralTime = snapshot.NeutralTime;
            existing.DistractingTime = snapshot.DistractingTime;
            existing.ProductivityScore = snapshot.ProductivityScore;
            existing.TopAppsJson = snapshot.TopAppsJson;
        }
        else
        {
            db.DailySnapshots.Add(snapshot);
        }
        await db.SaveChangesAsync();
    }

    public async Task<DailySnapshot?> GetByDateAsync(DateTime date)
    {
        return await db.DailySnapshots.FirstOrDefaultAsync(s => s.Date.Date == date.Date);
    }

    public async Task<List<DailySnapshot>> GetRangeAsync(DateTime start, DateTime end)
    {
        return await db.DailySnapshots
            .Where(s => s.Date >= start.Date && s.Date <= end.Date)
            .OrderBy(s => s.Date)
            .ToListAsync();
    }
}

public class CategoryRepository(SharpIBDbContext db) : ICategoryRepository
{
    public async Task AddAsync(Category category)
    {
        db.Categories.Add(category);
        await db.SaveChangesAsync();
    }

    public async Task<List<Category>> GetAllAsync()
    {
        return await db.Categories.Include(c => c.Mappings).ToListAsync();
    }

    public async Task<Category?> GetByIdAsync(int id)
    {
        return await db.Categories.Include(c => c.Mappings).FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task UpdateAsync(Category category)
    {
        db.Categories.Update(category);
        await db.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var category = await db.Categories.FindAsync(id);
        if (category != null)
        {
            db.Categories.Remove(category);
            await db.SaveChangesAsync();
        }
    }

    public async Task<AppCategoryMapping?> GetMappingByProcessNameAsync(string processName)
    {
        return await db.AppCategoryMappings
            .FirstOrDefaultAsync(m => m.ProcessName.ToLower() == processName.ToLower());
    }

    public async Task AddMappingAsync(AppCategoryMapping mapping)
    {
        db.AppCategoryMappings.Add(mapping);
        await db.SaveChangesAsync();
    }
}

