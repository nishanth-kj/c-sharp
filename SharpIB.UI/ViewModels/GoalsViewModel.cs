using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MediatR;
using SharpIB.Application.Commands;
using SharpIB.Application.DTOs;
using SharpIB.Application.Queries;
using SharpIB.Domain.Enums;

namespace SharpIB.UI.ViewModels;

public partial class GoalsViewModel : ObservableObject
{
    private readonly IMediator _mediator;

    [ObservableProperty] private string _newGoalTitle = string.Empty;
    [ObservableProperty] private string _newGoalDescription = string.Empty;
    [ObservableProperty] private string _newGoalProcesses = string.Empty;
    [ObservableProperty] private int _newGoalHours = 2;
    [ObservableProperty] private int _newGoalMinutes;
    [ObservableProperty] private GoalType _newGoalType = GoalType.Target;
    [ObservableProperty] private bool _isCreateDialogOpen;

    public ObservableCollection<GoalProgressDto> Goals { get; } = [];

    public GoalsViewModel(IMediator mediator) => _mediator = mediator;

    public async Task LoadDataAsync()
    {
        var progress = await _mediator.Send(new GetGoalProgressQuery());
        Goals.Clear();
        foreach (var g in progress) Goals.Add(g);
    }

    [RelayCommand]
    private void OpenCreateDialog() => IsCreateDialogOpen = true;

    [RelayCommand]
    private async Task CreateGoal()
    {
        if (string.IsNullOrWhiteSpace(NewGoalTitle)) return;

        await _mediator.Send(new CreateGoalCommand(
            NewGoalTitle,
            NewGoalDescription,
            NewGoalType,
            new TimeSpan(NewGoalHours, NewGoalMinutes, 0),
            NewGoalProcesses,
            null));

        // Reset form
        NewGoalTitle = string.Empty;
        NewGoalDescription = string.Empty;
        NewGoalProcesses = string.Empty;
        NewGoalHours = 2;
        NewGoalMinutes = 0;
        IsCreateDialogOpen = false;

        await LoadDataAsync();
    }

    [RelayCommand]
    private async Task DeleteGoal(int goalId)
    {
        await _mediator.Send(new DeleteGoalCommand(goalId));
        await LoadDataAsync();
    }
}

