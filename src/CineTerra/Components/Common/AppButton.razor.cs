using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace CineTerra.Components.Common;

public partial class AppButton : ComponentBase
{
    [Parameter] public string Variant { get; set; } = "primary"; // primary, secondary, ghost, subtle, danger
    [Parameter] public string Size { get; set; } = "md"; // sm, md, lg
    [Parameter] public string Type { get; set; } = "button";
    [Parameter] public bool Disabled { get; set; }
    [Parameter] public bool IsLoading { get; set; }
    [Parameter] public string? Href { get; set; }
    [Parameter] public EventCallback<MouseEventArgs> OnClick { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public RenderFragment? IconLeading { get; set; }
    [Parameter] public RenderFragment? IconTrailing { get; set; }
    [Parameter] public string? AriaLabel { get; set; }
    [Parameter] public string? CssClass { get; set; }
    [Parameter(CaptureUnmatchedValues = true)] public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    protected string ComputedClass =>
        $"cinema-btn cinema-btn--{Variant} cinema-btn--{Size} {(Disabled ? "cinema-btn--disabled" : "")} {(IsLoading ? "cinema-btn--loading" : "")} {CssClass}".Trim();

    protected async Task HandleClick(MouseEventArgs e)
    {
        if (Disabled || IsLoading) return;
        if (OnClick.HasDelegate)
        {
            await OnClick.InvokeAsync(e);
        }
    }
}
