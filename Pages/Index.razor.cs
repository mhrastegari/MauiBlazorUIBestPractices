using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace MauiBlazorUIBestPractices.Pages;

public partial class Index : ComponentBase
{
    public int WindowWidth { get; set; }
    private string _resizeEventListenerId = string.Empty;
    private DotNetObjectReference<Index>? _dotnetObjectReference;

    [Inject] private IJSRuntime JSRuntime { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        _dotnetObjectReference = DotNetObjectReference.Create(this);
        await base.OnInitializedAsync();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await JSRuntime.InvokeVoidAsync("UpdateWindowWidth", _dotnetObjectReference);
            await InitWindowWidthListener();
        }

        await base.OnAfterRenderAsync(firstRender);
    }

    [JSInvokable]
    public void UpdateWindowWidth(int windowWidth)
    {
        WindowWidth = windowWidth;
        StateHasChanged();
    }

    private async Task InitWindowWidthListener()
    {
        _resizeEventListenerId = Guid.NewGuid().ToString();
        await JSRuntime.InvokeVoidAsync("AddWindowWidthListener", _dotnetObjectReference, _resizeEventListenerId);
    }
}
