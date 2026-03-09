using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using SharpIB.UI.ViewModels;

namespace SharpIB.UI;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = App.Services.GetRequiredService<MainViewModel>();
    }
}

