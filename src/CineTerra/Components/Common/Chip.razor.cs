using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace CineTerra.Components.Common;

public partial class Chip : ComponentBase
{
    [Parameter] public string? Label { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public string Variant { get; set; } = "default"; // default, accent, muted, outline, telemetry
    [Parameter] public bool IsActive { get; set; }
    [Parameter] public bool IsClickable { get; set; }
    [Parameter] public bool Removable { get; set; }
    [Parameter] public EventCallback<MouseEventArgs> OnClick { get; set; }
    [Parameter] public EventCallback<MouseEventArgs> OnRemove { get; set; }
    [Parameter] public string? AriaLabel { get; set; }
    [Parameter] public string? Value { get; set; }
    [Parameter] public string? CssClass { get; set; }
    [Parameter(CaptureUnmatchedValues = true)] public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    protected bool CanClick => IsClickable || OnClick.HasDelegate;
    protected bool CanRemove => Removable || OnRemove.HasDelegate;

    protected string ComputedClass =>
        $"cinema-chip cinema-chip--{Variant} {(IsActive ? "cinema-chip--active" : "")} {(CanClick ? "cinema-chip--clickable" : "")} {(CanRemove ? "cinema-chip--removable" : "")} {CssClass}".Trim();

    protected string? ComputedAriaLabel => AriaLabel ?? Label;

    protected async Task HandleClick(MouseEventArgs e)
    {
        if (CanClick && OnClick.HasDelegate)
        {
            await OnClick.InvokeAsync(e);
        }
    }

    protected async Task HandleRemove(MouseEventArgs e)
    {
        if (CanRemove && OnRemove.HasDelegate)
        {
            await OnRemove.InvokeAsync(e);
        }
    }
}
