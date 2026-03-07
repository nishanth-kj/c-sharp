using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MediatR;
using Pulse.Application.Commands;
using Pulse.Application.DTOs;
using Pulse.Application.Queries;
using Pulse.Domain.Interfaces;

namespace Pulse.UI.ViewModels;

public partial class ApplicationsViewModel : ObservableObject
{
    private readonly IMediator _mediator;
    private readonly ICategoryRepository _categoryRepo;
    private List<AppUsageSummaryDto> _allApps = [];

    [ObservableProperty] private string _searchText = string.Empty;

    public ObservableCollection<AppUsageSummaryDto> FilteredApps { get; } = [];

    public ApplicationsViewModel(IMediator mediator, ICategoryRepository categoryRepo)
    {
        _mediator = mediator;
        _categoryRepo = categoryRepo;
    }

    public async Task LoadDataAsync()
    {
        var today = DateTime.Today;
        _allApps = await _mediator.Send(new GetAllAppsSummaryQuery(today.AddDays(-7), today.AddDays(1)));
        ApplyFilter();
    }

    partial void OnSearchTextChanged(string value) => ApplyFilter();

    private void ApplyFilter()
    {
        FilteredApps.Clear();
        var filtered = string.IsNullOrWhiteSpace(SearchText)
            ? _allApps
            : _allApps.Where(a => a.ProcessName.Contains(SearchText, StringComparison.OrdinalIgnoreCase));

        foreach (var app in filtered)
            FilteredApps.Add(app);
    }

    [RelayCommand]
    private async Task AssignCategory(string processName)
    {
        // Default: cycle through categories 1→2→3
        var mapping = await _categoryRepo.GetMappingByProcessNameAsync(processName);
        var nextCategoryId = mapping == null ? 1 : (mapping.CategoryId % 3) + 1;
        await _mediator.Send(new AssignCategoryCommand(processName, nextCategoryId));
        await LoadDataAsync();
    }
}
