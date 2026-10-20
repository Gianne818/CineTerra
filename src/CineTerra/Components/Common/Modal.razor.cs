using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace CineTerra.Components.Common;

public partial class Modal : ComponentBase
{
    private ElementReference _modalContainer;
    private bool _wasOpen;

    [Parameter] public bool IsOpen { get; set; }
    [Parameter] public EventCallback<bool> IsOpenChanged { get; set; }
    [Parameter] public EventCallback OnClose { get; set; }
    [Parameter] public string? Title { get; set; }
    [Parameter] public string? TelemetryStamp { get; set; }
    [Parameter] public RenderFragment? TitleContent { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public RenderFragment? FooterContent { get; set; }
    [Parameter] public string Size { get; set; } = "md"; // sm, md, lg, xl
    [Parameter] public bool CloseOnBackdropClick { get; set; } = true;
    [Parameter] public bool CloseOnEscape { get; set; } = true;
    [Parameter] public bool ShowCloseButton { get; set; } = true;
    [Parameter] public string? AriaLabel { get; set; }
    [Parameter] public string? CssClass { get; set; }
    [Parameter(CaptureUnmatchedValues = true)] public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (IsOpen && !_wasOpen)
        {
            _wasOpen = true;
            try
            {
                await _modalContainer.FocusAsync();
            }
            catch
            {
                // Container focus fallback
            }
        }
        else if (!IsOpen && _wasOpen)
        {
            _wasOpen = false;
        }
    }

    protected async Task HandleKeyDown(KeyboardEventArgs e)
    {
        if (CloseOnEscape && e.Key is "Escape" or "Esc")
        {
            await CloseAsync();
        }
    }

    protected async Task HandleBackdropClick()
    {
        if (CloseOnBackdropClick)
        {
            await CloseAsync();
        }
    }

    public async Task CloseAsync()
    {
        IsOpen = false;
        _wasOpen = false;
        await IsOpenChanged.InvokeAsync(false);
        if (OnClose.HasDelegate)
        {
            await OnClose.InvokeAsync();
        }
    }
}
