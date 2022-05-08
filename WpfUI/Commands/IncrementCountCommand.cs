using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Windows;
using WpfUI.ViewModels;

namespace WpfUI.Commands;
public class IncrementCountCommand : CommandBase
{
    private readonly CounterViewModel _counterViewModel;

    public IncrementCountCommand(CounterViewModel counterViewModel)
    {
        _counterViewModel = counterViewModel;
    }

    public override void Execute(object parameter)
    {
        _counterViewModel.CurrentCount += 1;

        if (_counterViewModel.IsConnected)
        {
            try
            {
                _counterViewModel.CounterHub.InvokeAsync("CounterIncrement", "Wpf Client", 1);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
