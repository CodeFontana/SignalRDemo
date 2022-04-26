using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using WpfUI.ViewModels;

namespace WpfUI.Commands;

public class ConnectChatHubCommand : CommandBaseAsync
{
    private readonly ChatViewModel _chatViewModel;

    public ConnectChatHubCommand(ChatViewModel chatViewModel)
    {
        _chatViewModel = chatViewModel;
        _chatViewModel.PropertyChanged += OnViewModelPropertyChanged;
    }

    public override bool CanExecute(object parameter)
    {
        return !_chatViewModel.IsConnected;
    }

    public override async Task ExecuteAsync(object parameter)
    {
        _chatViewModel.ChatHub.On<string, string>(
            "ReceiveMessage", (user, message) => 
                _chatViewModel.ReceiveMessage(user, message));

        try
        {
            await _chatViewModel.ChatHub.StartAsync();
            _chatViewModel.Messages.Add("Connection established");
            _chatViewModel.IsConnected = true;
        }
        catch (Exception e)
        {
            _chatViewModel.Messages.Add(e.Message);
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
