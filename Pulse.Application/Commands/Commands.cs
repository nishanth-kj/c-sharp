using MediatR;
using Pulse.Domain.Entities;
using Pulse.Domain.Interfaces;

namespace Pulse.Application.Commands;

// --- Record Activity ---
public record RecordActivityCommand(
    string ProcessName,
    string WindowTitle,
    string ExecutablePath,
    DateTime StartTime,
    DateTime EndTime) : IRequest<int>;

public class RecordActivityHandler(IActivityRepository activityRepo, ICategoryRepository categoryRepo)
    : IRequestHandler<RecordActivityCommand, int>
{
    public async Task<int> Handle(RecordActivityCommand request, CancellationToken cancellationToken)
    {
        var mapping = await categoryRepo.GetMappingByProcessNameAsync(request.ProcessName);

        var record = new ActivityRecord
        {
            ProcessName = request.ProcessName,
            WindowTitle = request.WindowTitle,
            ExecutablePath = request.ExecutablePath,
            StartTime = request.StartTime,
            EndTime = request.EndTime,
            CategoryId = mapping?.CategoryId
        };

        await activityRepo.AddAsync(record);
        return record.Id;
    }
}

// --- Create Goal ---
public record CreateGoalCommand(
    string Title,
    string Description,
    Domain.Enums.GoalType Type,
    TimeSpan TargetDuration,
    string TrackedProcesses,
    int? TrackedCategoryId) : IRequest<int>;

public class CreateGoalHandler(IGoalRepository goalRepo)
    : IRequestHandler<CreateGoalCommand, int>
{
    public async Task<int> Handle(CreateGoalCommand request, CancellationToken cancellationToken)
    {
        var goal = new Goal
        {
            Title = request.Title,
            Description = request.Description,
            Type = request.Type,
            TargetDuration = request.TargetDuration,
            TrackedProcesses = request.TrackedProcesses,
            TrackedCategoryId = request.TrackedCategoryId,
            CreatedAt = DateTime.Now
        };

        await goalRepo.AddAsync(goal);
        return goal.Id;
    }
}

// --- Delete Goal ---
public record DeleteGoalCommand(int GoalId) : IRequest;

public class DeleteGoalHandler(IGoalRepository goalRepo) : IRequestHandler<DeleteGoalCommand>
{
    public async Task Handle(DeleteGoalCommand request, CancellationToken cancellationToken)
    {
        await goalRepo.DeleteAsync(request.GoalId);
    }
}

// --- Assign Category ---
public record AssignCategoryCommand(string ProcessName, int CategoryId) : IRequest;

public class AssignCategoryHandler(ICategoryRepository categoryRepo) : IRequestHandler<AssignCategoryCommand>
{
    public async Task Handle(AssignCategoryCommand request, CancellationToken cancellationToken)
    {
        var existing = await categoryRepo.GetMappingByProcessNameAsync(request.ProcessName);
        if (existing != null)
        {
            existing.CategoryId = request.CategoryId;
        }
        else
        {
            await categoryRepo.AddMappingAsync(new AppCategoryMapping
            {
                ProcessName = request.ProcessName,
                CategoryId = request.CategoryId
            });
        }
    }
}
