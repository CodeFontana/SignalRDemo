using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace BlazorUI.Pages;

public partial class Counter : IAsyncDisposable
{
    private int _currentCount = 0;
    private HubConnection _counterHub;

    [Inject]
    public NavigationManager NavigationManager { get; set; }

    protected override async Task OnInitializedAsync()
    {
        _counterHub = new HubConnectionBuilder()
            .WithUrl(NavigationManager.ToAbsoluteUri("/counterhub"))
            .WithAutomaticReconnect()
            .Build();

        _counterHub.On<string, int>("CounterIncrement", (user, value) => CounterIncrement(user, value));
        await _counterHub.StartAsync();
    }

    private void CounterIncrement(string user, int value)
    {
        _currentCount += value;
        InvokeAsync(StateHasChanged);
    }

    public async ValueTask DisposeAsync()
    {
        if (_counterHub is not null)
        {
            await _counterHub.DisposeAsync();
        }
    }
}
