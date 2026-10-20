using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using CineTerra.Models.Locations;

namespace CineTerra.Components.Features.Scenes;

public partial class SceneComparisonSlider
{
    private ElementReference _sliderContainer;
    private bool _isDragging;

    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    [Parameter, EditorRequired]
    public Scene Scene { get; set; } = default!;

    [Parameter]
    public double InitialPosition { get; set; } = 50.0;

    public double Position { get; private set; } = 50.0;

    protected override void OnInitialized()
    {
        Position = Math.Clamp(InitialPosition, 0.0, 100.0);
    }

    public void SetPosition(double percentage)
    {
        Position = Math.Clamp(percentage, 0.0, 100.0);
        StateHasChanged();
    }

    public void HandleKeyDown(KeyboardEventArgs e)
    {
        switch (e.Key)
        {
            case "ArrowLeft":
            case "ArrowDown":
                SetPosition(Position - 5.0);
                break;
            case "ArrowRight":
            case "ArrowUp":
                SetPosition(Position + 5.0);
                break;
            case "Home":
                SetPosition(0.0);
                break;
            case "End":
                SetPosition(100.0);
                break;
        }
    }

    public void HandleRangeInput(ChangeEventArgs e)
    {
        if (e.Value is not null &&
            double.TryParse(e.Value.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out var parsed))
        {
            SetPosition(parsed);
        }
    }

    public async Task HandlePointerDown(PointerEventArgs e)
    {
        _isDragging = true;
        await UpdatePositionFromPointerAsync(e);
    }

    public async Task HandlePointerMove(PointerEventArgs e)
    {
        if (!_isDragging) return;
        await UpdatePositionFromPointerAsync(e);
    }

    public void HandlePointerUp(PointerEventArgs e)
    {
        _isDragging = false;
    }

    private async Task UpdatePositionFromPointerAsync(PointerEventArgs e)
    {
        try
        {
            var rect = await JSRuntime.InvokeAsync<BoundingRect?>("cineterra.getRect", _sliderContainer);
            if (rect is not null && rect.Width > 0)
            {
                var relativeX = e.ClientX - rect.Left;
                var pct = (relativeX / rect.Width) * 100.0;
                SetPosition(pct);
                return;
            }
        }
        catch
        {
            // Fallback for tests or prerendering without live JS DOM
            if (e.OffsetX > 0)
            {
                SetPosition(Math.Clamp(e.OffsetX, 0.0, 100.0));
            }
        }
    }

    public sealed record BoundingRect(double Left, double Width);
}
