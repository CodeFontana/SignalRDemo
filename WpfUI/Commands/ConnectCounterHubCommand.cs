using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using WpfUI.ViewModels;

namespace WpfUI.Commands;

public class ConnectCounterHubCommand : CommandBaseAsync
{
    private readonly CounterViewModel _counterViewModel;

    public ConnectCounterHubCommand(CounterViewModel counterViewModel)
    {
        _counterViewModel = counterViewModel;
        _counterViewModel.PropertyChanged += OnViewModelPropertyChanged;
    }

    public override bool CanExecute(object parameter)
    {
        return !_counterViewModel.IsConnected;
    }

    public override async Task ExecuteAsync(object parameter)
    {
        try
        {
            await _counterViewModel.CounterHub.StartAsync();
            _counterViewModel.IsConnected = true;
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(ChatViewModel.IsConnected))
        {
            OnCanExecutedChanged();
        }
    }
}
