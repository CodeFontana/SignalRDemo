using Microsoft.AspNetCore.SignalR.Client;
using System.Windows.Input;
using WpfUI.Commands;

namespace WpfUI.ViewModels;
public class CounterViewModel : ViewModelBase
{
    public CounterViewModel()
    {
        IsConnected = false;
        CurrentCount = 0;
        ConnectCounterHubCommand = new ConnectCounterHubCommand(this);
        IncrementCountCommand = new IncrementCountCommand(this);
        ResetCountCommand = new ResetCountCommand(this);

        CounterHub = new HubConnectionBuilder()
            .WithUrl(ServerAddress)
            .WithAutomaticReconnect()
            .Build();
    }

    private HubConnection _counterHub;
    public HubConnection CounterHub
    {
        get
        {
            return _counterHub;
        }
        set
        {
            OnPropertyChanged(ref _counterHub, value);
        }
    }

    private bool _isConnected;
    public bool IsConnected
    {
        get
        {
            return _isConnected;
        }
        set
        {
            OnPropertyChanged(ref _isConnected, value);
        }
    }

    private string _serverAddress = "https://localhost:7078/counterhub";
    public string ServerAddress
    {
        get
        {
            return _serverAddress;
        }
        set
        {
            OnPropertyChanged(ref _serverAddress, value);
        }
    }

    private int _currentCount;
    public int CurrentCount
    {
        get
        {
            return _currentCount;
        }
        set
        {
            _currentCount = value;
            OnPropertyChanged(nameof(CurrentCount));
        }
    }

    public ICommand ConnectCounterHubCommand { get; set; }

    public ICommand IncrementCountCommand { get; }

    public ICommand ResetCountCommand { get; }
}
