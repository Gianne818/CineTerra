using Microsoft.AspNetCore.Components;

namespace CineTerra.Components.Common;

public partial class LoadingSpinner : ComponentBase
{
    [Parameter] public string Size { get; set; } = "md"; // sm, md, lg
    [Parameter] public string? Label { get; set; } = "Loading Archive...";
    [Parameter] public bool ShowLabel { get; set; } = true;
    [Parameter] public bool FullScreen { get; set; }
    [Parameter] public string? TelemetryText { get; set; }
    [Parameter] public string? CssClass { get; set; }
    [Parameter(CaptureUnmatchedValues = true)] public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    protected string ComputedClass =>
        $"cinema-loader cinema-loader--{Size} {(FullScreen ? "cinema-loader--fullscreen" : "")} {CssClass}".Trim();
}
