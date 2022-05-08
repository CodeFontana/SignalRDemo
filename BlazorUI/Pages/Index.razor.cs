using BlazorUI.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace BlazorUI.Pages;

public partial class Index : IAsyncDisposable
{
    private HubConnection _chatHub;
    private List<string> _messages;
    private MessageModel _message;

    public Index()
    {
        _messages = new();
        _message = new();
    }

    [Inject]
    public NavigationManager NavigationManager { get; set; }

    protected override async Task OnInitializedAsync()
    {
        _chatHub = new HubConnectionBuilder()
            .WithUrl(NavigationManager.ToAbsoluteUri("/chathub"))
            .WithAutomaticReconnect()
            .Build();

        _chatHub.On<string, string>("ReceiveMessage", (user, message) => ReceiveMessage(user, message));
        await _chatHub.StartAsync();
    }

    private void ReceiveMessage(string user, string message)
    {
        string formattedMessage = $"{user}: {message}";
        _messages.Add(formattedMessage);
        StateHasChanged();
    }

    private async Task SendMessageAsync()
    {
        if (_chatHub is not null)
        {
            await _chatHub.SendAsync("SendMessage", _message.User, _message.Message);
            _message = new();
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_chatHub is not null)
        {
            await _chatHub.DisposeAsync();
        }
    }
}
