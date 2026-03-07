using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Pulse.UI.ViewModels;

namespace Pulse.UI;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = App.Services.GetRequiredService<MainViewModel>();
    }
}
