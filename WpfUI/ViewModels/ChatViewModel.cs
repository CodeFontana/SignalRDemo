using Microsoft.AspNetCore.SignalR.Client;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WpfUI.Commands;

namespace WpfUI.ViewModels;

public class ChatViewModel : ViewModelBase
{
    public ChatViewModel()
    {
        IsConnected = false;
        Messages = new();
        Message = "";

        _chatHub = new HubConnectionBuilder()
            .WithUrl(ServerAddress)
            .WithAutomaticReconnect()
            .Build();

        _chatHub.Reconnecting += (sender) =>
        {
            Messages.Add("Attempting to reconnect...");
            return Task.CompletedTask;
        };

        _chatHub.Reconnected += (sender) =>
        {
            Messages.Clear();
            Messages.Add("Reconnected to server");
            return Task.CompletedTask;
        };

        _chatHub.Closed += (sender) =>
        {
            Messages.Add("Server connection closed");
            return Task.CompletedTask;
        };

        ConnectChatHubCommand = new ConnectChatHubCommand(this);
        SendMessageCommand = new SendMessageCommand(this);
    }

    private HubConnection _chatHub;
    public HubConnection ChatHub
    {
        get
        {
            return _chatHub;
        }
        set
        {
            OnPropertyChanged(ref _chatHub, value);
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

    private string _serverAddress = "https://localhost:7078/chathub";
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

    private ObservableCollection<string> _list;
    public ObservableCollection<string> Messages
    {
        get
        {
            return _list;
        }
        set
        {
            OnPropertyChanged(ref _list, value);
        }
    }

    private string _message;
    public string Message
    {
        get
        {
            return _message;
        }
        set
        {
            OnPropertyChanged(ref _message, value);
        }
    }

    public ICommand ConnectChatHubCommand { get; set; }
    public ICommand SendMessageCommand { get; set; }

    public void ReceiveMessage(string user, string message)
    {
        string formattedMessage = $"{user}: {message}";
        Application.Current.Dispatcher.Invoke(() =>
        {
            Messages.Add(formattedMessage);
        });
    }
}
