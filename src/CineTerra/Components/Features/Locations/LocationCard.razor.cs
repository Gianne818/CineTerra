using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using CineTerra.Models.Locations;

namespace CineTerra.Components.Features.Locations;

public partial class LocationCard
{
    [Parameter, EditorRequired]
    public FilmingLocation Location { get; set; } = default!;

    [Parameter]
    public EventCallback<FilmingLocation> OnSelected { get; set; }

    [Parameter]
    public EventCallback<FilmingLocation> OnAddToPilgrimage { get; set; }

    private Task HandleCardClick() => OnSelected.InvokeAsync(Location);

    private Task HandleSelectedClick(MouseEventArgs _) => OnSelected.InvokeAsync(Location);

    private Task HandleAddToPilgrimageClick(MouseEventArgs _) => OnAddToPilgrimage.InvokeAsync(Location);
}
