using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace CineTerra.Components.Common;

public partial class EmptyState : ComponentBase
{
    [Parameter] public string Title { get; set; } = "No Records Found";
    [Parameter] public string? Message { get; set; }
    [Parameter] public string? TelemetryCode { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public RenderFragment? Icon { get; set; }
    [Parameter] public string? ActionText { get; set; }
    [Parameter] public string? ActionHref { get; set; }
    [Parameter] public EventCallback<MouseEventArgs> OnAction { get; set; }
    [Parameter] public string? CssClass { get; set; }
    [Parameter(CaptureUnmatchedValues = true)] public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    protected bool HasAction => !string.IsNullOrEmpty(ActionText);

    protected async Task HandleAction(MouseEventArgs e)
    {
        if (OnAction.HasDelegate)
        {
            await OnAction.InvokeAsync(e);
        }
    }
}
