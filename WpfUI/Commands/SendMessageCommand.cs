using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using WpfUI.ViewModels;

namespace WpfUI.Commands;

public class SendMessageCommand : CommandBaseAsync
{
    private readonly ChatViewModel _chatViewModel;

    public SendMessageCommand(ChatViewModel chatViewModel)
    {
        _chatViewModel = chatViewModel;
        _chatViewModel.PropertyChanged += OnViewModelPropertyChanged;
    }

    public override bool CanExecute(object parameter)
    {
        return _chatViewModel.IsConnected;
    }

    public override async Task ExecuteAsync(object parameter)
    {
        try
        {
            await _chatViewModel.ChatHub.InvokeAsync("SendMessage", "WPF Client", _chatViewModel.Message);
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
