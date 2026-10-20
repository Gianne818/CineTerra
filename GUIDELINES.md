# 🎬 CineTerra — Blazor Frontend Development Guidelines

> **CineTerra** is a set-jetting travel app. Users find real-world filming locations from movies, series, and anime, compare each scene with the real place, plan a "fandom pilgrimage" trip, and collect stamps in a digital film passport.

This document is the **source of truth** for structure, OOP modeling, and code standards. Every page, component, model, and service in the project must follow it.

---

## Table of Contents

1. [Scope & Constraints](#1-scope--constraints)
2. [Tech Stack](#2-tech-stack)
3. [Solution & Folder Structure](#3-solution--folder-structure)
4. [Naming Conventions](#4-naming-conventions)
5. [Models & OOP Principles](#5-models--oop-principles)
6. [Services & Dependency Injection](#6-services--dependency-injection)
7. [State Management](#7-state-management)
8. [Components](#8-components)
9. [Pages & Routing](#9-pages--routing)
10. [Static Data (JSON)](#10-static-data-json)
11. [Styling](#11-styling)
12. [C# Code Standards](#12-c-code-standards)
13. [UI States, Accessibility & Performance](#13-ui-states-accessibility--performance)
14. [Git Workflow](#14-git-workflow)
15. [Definition of Done Checklist](#15-definition-of-done-checklist)

---

## 1. Scope & Constraints

| Rule | Detail |
|---|---|
| **Frontend only** | No backend API, no database, no server project. |
| **Hosting model** | **Blazor WebAssembly Standalone**. The app runs fully in the browser. |
| **Data source** | Static JSON files in `wwwroot/data/`, loaded with `HttpClient`. |
| **Persistence** | User data (pilgrimage plan, passport stamps, favorites) is saved in the browser's **`localStorage`** through `IJSRuntime`. |
| **Auth** | None. If a "profile" is needed, simulate it locally. |

---

## 2. Tech Stack

| Concern | Choice |
|---|---|
| Framework | .NET 8 SDK (8.0.400 installed) |
| Project template | `dotnet new blazorwasm` |
| Language | C# 12, nullable reference types **enabled** |
| Styling | Plain CSS with design tokens + Blazor **CSS isolation** (`.razor.css`) |
| JSON | `System.Text.Json` |
| JS interop | Only for things Blazor can't do (localStorage, scroll, clipboard) |

**Scaffold command:**

```powershell
dotnet new sln -n CineTerra
dotnet new blazorwasm -n CineTerra -o src/CineTerra
dotnet sln add src/CineTerra/CineTerra.csproj
```

---

## 3. Solution & Folder Structure

```text
midterms/
├── CineTerra.sln
├── GUIDELINES.md
├── .editorconfig
└── src/
    └── CineTerra/
        ├── Program.cs                    # Entry point + DI registration
        ├── App.razor                     # Router
        ├── _Imports.razor                # Global @using directives
        │
        ├── Layout/                       # App shell
        │   ├── MainLayout.razor
        │   ├── MainLayout.razor.css
        │   └── NavBar.razor
        │
        ├── Pages/                        # Routable components only (@page)
        │   ├── Home.razor
        │   ├── Explore.razor             # /explore — all filming locations + filters
        │   ├── LocationDetail.razor      # /locations/{Id}
        │   ├── Productions.razor         # /productions
        │   ├── ProductionDetail.razor    # /productions/{Id}
        │   ├── PilgrimageBuilder.razor   # /pilgrimage
        │   ├── Passport.razor            # /passport
        │   └── NotFound.razor
        │
        ├── Components/                   # Non-routable reusable UI
        │   ├── Common/                   # Generic, theme-level building blocks
        │   │   ├── AppButton.razor
        │   │   ├── Chip.razor
        │   │   ├── Modal.razor
        │   │   ├── LoadingSpinner.razor
        │   │   └── EmptyState.razor
        │   └── Features/                 # CineTerra-specific components
        │       ├── Locations/
        │       │   ├── LocationCard.razor
        │       │   ├── LocationCard.razor.cs
        │       │   ├── LocationCard.razor.css
        │       │   └── LocationFilterBar.razor
        │       ├── Scenes/
        │       │   └── SceneComparisonSlider.razor   # "Scene vs Reality"
        │       ├── Productions/
        │       │   └── ProductionCard.razor
        │       ├── Pilgrimage/
        │       │   ├── RouteTimeline.razor
        │       │   └── StopItem.razor
        │       └── Passport/
        │           └── PassportStamp.razor
        │
        ├── Models/
        │   ├── Common/                   # Base classes & value objects
        │   │   ├── BaseEntity.cs
        │   │   └── Coordinates.cs
        │   ├── Productions/              # Inheritance hierarchy
        │   │   ├── Production.cs         # abstract
        │   │   ├── Film.cs
        │   │   ├── Series.cs
        │   │   └── Anime.cs
        │   ├── Locations/
        │   │   ├── FilmingLocation.cs
        │   │   └── Scene.cs
        │   ├── Pilgrimages/
        │   │   ├── Pilgrimage.cs
        │   │   └── PilgrimageStop.cs
        │   ├── Passport/
        │   │   └── Stamp.cs
        │   ├── Enums/
        │   │   ├── Genre.cs
        │   │   ├── ProductionType.cs
        │   │   ├── Region.cs
        │   │   └── Difficulty.cs
        │   ├── Filters/
        │   │   └── LocationFilter.cs
        │   └── Dtos/                     # Shapes used only for localStorage
        │       └── PilgrimageDto.cs
        │
        ├── Services/
        │   ├── Interfaces/
        │   │   ├── IProductionService.cs
        │   │   ├── ILocationService.cs
        │   │   ├── IPassportService.cs
        │   │   └── ILocalStorageService.cs
        │   └── Implementations/
        │       ├── JsonDataService.cs    # abstract generic base
        │       ├── ProductionService.cs
        │       ├── LocationService.cs
        │       ├── PassportService.cs
        │       └── LocalStorageService.cs
        │
        ├── State/                        # App-wide state containers
        │   └── PilgrimageState.cs
        │
        ├── Utilities/
        │   ├── AppRoutes.cs              # Route constants & link builders
        │   ├── StorageKeys.cs            # localStorage key constants
        │   ├── JsonDefaults.cs           # Shared JsonSerializerOptions
        │   └── Extensions/
        │       └── EnumExtensions.cs
        │
        └── wwwroot/
            ├── index.html
            ├── css/
            │   ├── tokens.css            # Design tokens (colors, spacing, fonts)
            │   └── app.css               # Global resets & base styles
            ├── data/
            │   ├── productions.json
            │   └── locations.json
            └── images/
                ├── productions/
                ├── scenes/
                └── locations/
```

### Folder rules

- **`Pages/`** contains only components with `@page`. Pages handle routing, load data, and put components together. They should not hold large chunks of markup.
- **`Components/Common/`** components must not know about CineTerra models. For example, `Chip` takes a `string`, not a `Genre`.
- **`Components/Features/`** components are domain-aware and grouped by feature.
- **Namespaces mirror folders.** For example, `Models/Productions/Film.cs` uses `CineTerra.Models.Productions`.
- **One public type per file.** The file name must match the type name.

---

## 4. Naming Conventions

| Element | Convention | Example |
|---|---|---|
| Namespace | PascalCase, mirrors folder | `CineTerra.Services.Interfaces` |
| Class / Record / Enum | PascalCase, noun | `FilmingLocation`, `Genre` |
| Interface | `I` + PascalCase | `ILocationService` |
| Abstract base | Plain noun (no `Base` suffix unless purely technical) | `Production`, `BaseEntity` |
| Method | PascalCase, verb | `GetByIdAsync`, `AddStop` |
| Async method | Ends in `Async` | `LoadLocationsAsync` |
| Property | PascalCase | `ReleaseYear` |
| Private field | `_camelCase` | `_locations` |
| Local / parameter | camelCase | `locationId` |
| Constant | PascalCase | `MaxStops` |
| Boolean | `Is` / `Has` / `Can` prefix | `IsVisited`, `HasScenes` |
| Component | PascalCase, noun | `SceneComparisonSlider.razor` |
| Event callback parameter | `On` + verb | `OnSelected`, `OnRemoved` |
| Event handler method | `Handle` + event | `HandleSelected` |
| CSS class | kebab-case, BEM-style | `location-card__title--active` |
| JSON property | camelCase | `"releaseYear": 2021` |
| Image files | kebab-case | `dune-wadi-rum-scene.jpg` |

---

## 5. Models & OOP Principles

The model layer must **visibly demonstrate the four OOP pillars**:

| Pillar | Where it shows up in CineTerra |
|---|---|
| **Abstraction** | `BaseEntity` and abstract `Production` define *what* every entity or production is, without specifying details. |
| **Inheritance** | `Film`, `Series`, `Anime` inherit from `Production`. All entities inherit from `BaseEntity`. |
| **Polymorphism** | `GetDurationLabel()` is overridden in each subtype. The UI calls it without knowing the concrete type. |
| **Encapsulation** | `Pilgrimage` keeps its stops in a private list. Callers can change it only through `AddStop` / `RemoveStop` / `MoveStop`. |

### 5.1 Base entity

```csharp
namespace CineTerra.Models.Common;

/// <summary>Root of every identifiable domain object.</summary>
public abstract class BaseEntity
{
    public string Id { get; init; } = string.Empty;

    public override bool Equals(object? obj) =>
        obj is BaseEntity other && GetType() == other.GetType() && Id == other.Id;

    public override int GetHashCode() => HashCode.Combine(GetType(), Id);
}
```

### 5.2 Value objects use `record`

Use `record` for small, immutable objects that are compared by value and have no identity.

```csharp
namespace CineTerra.Models.Common;

public sealed record Coordinates(double Latitude, double Longitude)
{
    public override string ToString() =>
        $"{Math.Abs(Latitude):F4}°{(Latitude >= 0 ? "N" : "S")}, " +
        $"{Math.Abs(Longitude):F4}°{(Longitude >= 0 ? "E" : "W")}";
}
```

### 5.3 Inheritance + polymorphism (`Production` hierarchy)

```csharp
using System.Text.Json.Serialization;
using CineTerra.Models.Common;
using CineTerra.Models.Enums;

namespace CineTerra.Models.Productions;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "kind")]
[JsonDerivedType(typeof(Film), "film")]
[JsonDerivedType(typeof(Series), "series")]
[JsonDerivedType(typeof(Anime), "anime")]
public abstract class Production : BaseEntity
{
    public string Title { get; init; } = string.Empty;
    public int ReleaseYear { get; init; }
    public string Director { get; init; } = string.Empty;
    public string Synopsis { get; init; } = string.Empty;
    public string PosterUrl { get; init; } = string.Empty;
    public IReadOnlyList<Genre> Genres { get; init; } = [];

    [JsonIgnore]
    public abstract ProductionType Type { get; }

    /// <summary>Each subtype describes its own length differently.</summary>
    public abstract string GetDurationLabel();

    public virtual string GetDisplayTitle() => $"{Title} ({ReleaseYear})";
}
```

```csharp
namespace CineTerra.Models.Productions;

public sealed class Film : Production
{
    public int RuntimeMinutes { get; init; }

    public override ProductionType Type => ProductionType.Film;

    public override string GetDurationLabel() =>
        $"{RuntimeMinutes / 60}h {RuntimeMinutes % 60}m";
}
```

```csharp
namespace CineTerra.Models.Productions;

public sealed class Series : Production
{
    public int Seasons { get; init; }

    public override ProductionType Type => ProductionType.Series;

    public override string GetDurationLabel() =>
        Seasons == 1 ? "1 season" : $"{Seasons} seasons";
}
```

```csharp
namespace CineTerra.Models.Productions;

public sealed class Anime : Production
{
    public string Studio { get; init; } = string.Empty;
    public int Episodes { get; init; }   // 0 = feature film

    public override ProductionType Type => ProductionType.Anime;

    public override string GetDurationLabel() =>
        Episodes == 0 ? "Feature film" : $"{Episodes} episodes";

    public override string GetDisplayTitle() => $"{base.GetDisplayTitle()} · {Studio}";
}
```

> ⚠️ **.NET 8 JSON note:** the `"kind"` discriminator **must be the first property** in each JSON object. Don't call it `"type"`, because that clashes with the `Type` property.

### 5.4 Locations & scenes

```csharp
namespace CineTerra.Models.Locations;

public sealed class FilmingLocation : BaseEntity
{
    public string Name { get; init; } = string.Empty;
    public string City { get; init; } = string.Empty;
    public string Country { get; init; } = string.Empty;
    public Region Region { get; init; }
    public Coordinates Coordinates { get; init; } = new(0, 0);
    public string Description { get; init; } = string.Empty;
    public string ImageUrl { get; init; } = string.Empty;
    public Difficulty Accessibility { get; init; }
    public IReadOnlyList<Scene> Scenes { get; init; } = [];

    public bool HasScenes => Scenes.Count > 0;
    public string DisplayPlace => $"{City}, {Country}";
}
```

```csharp
namespace CineTerra.Models.Locations;

public sealed record Scene(
    string ProductionId,
    string Title,
    TimeSpan Timestamp,      // JSON: "01:12:40"
    string StillImageUrl,    // frame from the film
    string RealImageUrl)     // real-world photo
{
    public string TimestampLabel => Timestamp.ToString(@"h\:mm\:ss");
}
```

### 5.5 Encapsulation (`Pilgrimage` aggregate)

```csharp
namespace CineTerra.Models.Pilgrimages;

public sealed class Pilgrimage : BaseEntity
{
    public const int MaxStops = 15;

    private readonly List<PilgrimageStop> _stops = [];

    public string Name { get; private set; } = "My Film Pilgrimage";
    public IReadOnlyList<PilgrimageStop> Stops => _stops.AsReadOnly();
    public int TotalDays => _stops.Sum(s => s.Days);
    public bool IsFull => _stops.Count >= MaxStops;

    public void Rename(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        Name = name.Trim();
    }

    public bool AddStop(FilmingLocation location, int days = 1)
    {
        ArgumentNullException.ThrowIfNull(location);
        if (IsFull || _stops.Any(s => s.LocationId == location.Id)) return false;

        _stops.Add(new PilgrimageStop(location.Id, location.Name, days));
        return true;
    }

    public bool RemoveStop(string locationId) =>
        _stops.RemoveAll(s => s.LocationId == locationId) > 0;

    public void MoveStop(int fromIndex, int toIndex)
    {
        if (fromIndex == toIndex) return;
        var stop = _stops[fromIndex];
        _stops.RemoveAt(fromIndex);
        _stops.Insert(toIndex, stop);
    }
}
```

### Model rules

1. Properties use `{ get; init; }` by default. Use `private set` only when a method controls the change.
2. **Never expose a mutable `List<T>` publicly.** Expose `IReadOnlyList<T>`.
3. Computed properties like `HasScenes` and `DisplayPlace` belong in the model, not in Razor markup.
4. Mark classes `sealed` unless they are designed to be inherited from.
5. Models **never** reference services, `HttpClient`, `IJSRuntime`, or UI types.
6. Domain objects with private state like `Pilgrimage` are saved through a **DTO** (`Models/Dtos/`) and mapped back. Don't serialize them directly.
7. Enums are stored as **strings** in JSON (see `JsonDefaults`).

---

## 6. Services & Dependency Injection

### 6.1 Always code against interfaces

```csharp
namespace CineTerra.Services.Interfaces;

public interface ILocationService
{
    Task<IReadOnlyList<FilmingLocation>> GetAllAsync();
    Task<FilmingLocation?> GetByIdAsync(string id);
    Task<IReadOnlyList<FilmingLocation>> GetByProductionAsync(string productionId);
    Task<IReadOnlyList<FilmingLocation>> SearchAsync(LocationFilter filter);
}
```

### 6.2 Reuse logic with an abstract generic base

```csharp
namespace CineTerra.Services.Implementations;

public abstract class JsonDataService<T> where T : BaseEntity
{
    private readonly HttpClient _http;
    private readonly string _dataPath;
    private IReadOnlyList<T>? _cache;

    protected JsonDataService(HttpClient http, string dataPath)
    {
        _http = http;
        _dataPath = dataPath;
    }

    protected async Task<IReadOnlyList<T>> LoadAsync()
    {
        _cache ??= await _http.GetFromJsonAsync<List<T>>(_dataPath, JsonDefaults.Options) ?? [];
        return _cache;
    }

    public async Task<T?> GetByIdAsync(string id) =>
        (await LoadAsync()).FirstOrDefault(e => e.Id == id);
}
```

```csharp
public sealed class LocationService(HttpClient http)
    : JsonDataService<FilmingLocation>(http, "data/locations.json"), ILocationService
{
    public Task<IReadOnlyList<FilmingLocation>> GetAllAsync() => LoadAsync();

    public async Task<IReadOnlyList<FilmingLocation>> GetByProductionAsync(string productionId) =>
        (await LoadAsync())
            .Where(l => l.Scenes.Any(s => s.ProductionId == productionId))
            .ToList();

    public async Task<IReadOnlyList<FilmingLocation>> SearchAsync(LocationFilter filter) =>
        (await LoadAsync()).Where(filter.Matches).ToList();
}
```

### 6.3 Shared JSON options

```csharp
namespace CineTerra.Utilities;

public static class JsonDefaults
{
    public static readonly JsonSerializerOptions Options = new(JsonSerializerDefaults.Web)
    {
        Converters = { new JsonStringEnumConverter() }
    };
}
```

### 6.4 Registration (`Program.cs`)

```csharp
builder.Services.AddScoped(_ => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Data services
builder.Services.AddScoped<IProductionService, ProductionService>();
builder.Services.AddScoped<ILocationService, LocationService>();
builder.Services.AddScoped<IPassportService, PassportService>();
builder.Services.AddScoped<ILocalStorageService, LocalStorageService>();

// State
builder.Services.AddScoped<PilgrimageState>();
```

> In Blazor WebAssembly, `Scoped` behaves like a singleton for the whole browser session. Use `Scoped` by default.

### Service rules

- Components inject **interfaces**, never concrete classes (`@inject ILocationService LocationService`). State containers are the one exception.
- Filtering, sorting, and searching logic lives in **services or filter models**, not in components.
- Every I/O method is `async` and returns `Task<T>`.
- Load each JSON file **once** per session and cache it.

---

## 7. State Management

Use the **state container pattern** for state shared across pages, such as the current pilgrimage plan.

```csharp
namespace CineTerra.State;

public sealed class PilgrimageState(ILocalStorageService storage)
{
    public Pilgrimage Current { get; private set; } = new();
    public event Action? OnChange;

    public async Task InitializeAsync()
    {
        var dto = await storage.GetAsync<PilgrimageDto>(StorageKeys.Pilgrimage);
        if (dto is not null) Current = dto.ToDomain();
        NotifyStateChanged();
    }

    public async Task<bool> AddStopAsync(FilmingLocation location)
    {
        var added = Current.AddStop(location);
        if (added) await PersistAsync();
        return added;
    }

    private async Task PersistAsync()
    {
        await storage.SetAsync(StorageKeys.Pilgrimage, PilgrimageDto.FromDomain(Current));
        NotifyStateChanged();
    }

    private void NotifyStateChanged() => OnChange?.Invoke();
}
```

**Components that subscribe must unsubscribe:**

```razor
@implements IDisposable
@inject PilgrimageState Pilgrimage

@code {
    protected override void OnInitialized() => Pilgrimage.OnChange += StateHasChanged;
    public void Dispose() => Pilgrimage.OnChange -= StateHasChanged;
}
```

| State type | Where it lives |
|---|---|
| UI-only (modal open, hover, slider position) | Private field in the component |
| Page-level (current filter, search text) | Page component, passed down through `[Parameter]` |
| Cross-page (pilgrimage, passport) | State container in `State/` |
| Must survive refresh | State container + `ILocalStorageService` |

---

## 8. Components

### 8.1 Split markup and logic

- **Under ~30 lines of `@code`:** an inline `@code { }` block is fine.
- **More than that:** move the logic into a **code-behind** `ComponentName.razor.cs` as a `partial class`.
- **Styles:** always go in `ComponentName.razor.css` (CSS isolation).

**`LocationCard.razor`**

```razor
<article class="location-card">
    <img class="location-card__image" src="@Location.ImageUrl" alt="@Location.Name" loading="lazy" />
    <div class="location-card__body">
        <h3 class="location-card__title">@Location.Name</h3>
        <p class="location-card__place">@Location.DisplayPlace</p>
        <button class="location-card__action" @onclick="HandleSelected">View scenes</button>
    </div>
</article>
```

**`LocationCard.razor.cs`**

```csharp
namespace CineTerra.Components.Features.Locations;

public partial class LocationCard
{
    [Parameter, EditorRequired] public FilmingLocation Location { get; set; } = default!;
    [Parameter] public EventCallback<FilmingLocation> OnSelected { get; set; }

    private Task HandleSelected() => OnSelected.InvokeAsync(Location);
}
```

### 8.2 Component rules

1. **Parameters:** mark required ones with `[Parameter, EditorRequired]`. **Never modify a `[Parameter]`** inside the child. Copy it into a private field if you need to change it.
2. **Child → parent communication:** use `EventCallback` / `EventCallback<T>`, never `Action`.
3. **Loops:** always add `@key` when rendering lists: `<LocationCard @key="loc.Id" Location="loc" />`.
4. **No business logic in markup.** Use `@location.DisplayPlace`, not `@($"{location.City}, {location.Country}")`.
5. **No `async void`.** Return `Task`.
6. **Content projection:** use `RenderFragment ChildContent` for wrapper components like `Modal` and `Chip`.
7. **Size limit:** if markup goes past ~150 lines, split it into child components.
8. **Dispose** event subscriptions, timers, and `DotNetObjectReference` by implementing `IDisposable` / `IAsyncDisposable`.
9. Use `@inject` (or `[Inject]` in code-behind), never `new` up a service.

### 8.3 Lifecycle cheat sheet

| Method | Use for |
|---|---|
| `OnInitializedAsync` | Loading data one time |
| `OnParametersSetAsync` | Reloading when a **route parameter** changes (e.g. `/locations/{Id}`) |
| `OnAfterRenderAsync(firstRender)` | JS interop such as localStorage reads and focus |

---

## 9. Pages & Routing

| Route | Page | Purpose |
|---|---|---|
| `/` | `Home` | Hero, featured productions, CTA |
| `/explore` | `Explore` | Grid of locations with filters (genre, region, type, difficulty) |
| `/locations/{Id}` | `LocationDetail` | Location info + **Scene vs Reality** slider + "Add to pilgrimage" |
| `/productions` | `Productions` | Browse films / series / anime |
| `/productions/{Id}` | `ProductionDetail` | Production info + every filming location |
| `/pilgrimage` | `PilgrimageBuilder` | Reorder stops, set days, view route timeline |
| `/passport` | `Passport` | Collected stamps & progress |

### Rules

- Keep route strings and link builders in one place:

```csharp
namespace CineTerra.Utilities;

public static class AppRoutes
{
    public const string Home = "/";
    public const string Explore = "/explore";
    public const string Productions = "/productions";
    public const string Pilgrimage = "/pilgrimage";
    public const string Passport = "/passport";

    public static string Location(string id) => $"/locations/{Uri.EscapeDataString(id)}";
    public static string Production(string id) => $"/productions/{Uri.EscapeDataString(id)}";
}
```

- Use `<NavLink href="@AppRoutes.Explore">` for links and `NavigationManager.NavigateTo(...)` for code-driven navigation. **Never hardcode route strings in markup.**
- Detail pages must handle a **missing ID** by rendering a "not found" state instead of crashing.
- Set a page title on every page with `<PageTitle>Explore · CineTerra</PageTitle>`.

---

## 10. Static Data (JSON)

- Files live in `wwwroot/data/` and use **camelCase** properties.
- IDs are **kebab-case slugs**, e.g. `"dune-2021"`, `"wadi-rum-jordan"`.
- Enums are written as strings, e.g. `"genres": ["SciFi", "Adventure"]`.
- Images are referenced by relative path, e.g. `"images/locations/wadi-rum.jpg"`.

**`productions.json`**

```json
[
  {
    "kind": "film",
    "id": "dune-2021",
    "title": "Dune",
    "releaseYear": 2021,
    "director": "Denis Villeneuve",
    "synopsis": "A noble family becomes embroiled in a war for the desert planet Arrakis.",
    "posterUrl": "images/productions/dune-2021.jpg",
    "genres": ["SciFi", "Adventure"],
    "runtimeMinutes": 155
  }
]
```

**`locations.json`**

```json
[
  {
    "id": "wadi-rum-jordan",
    "name": "Wadi Rum",
    "city": "Aqaba Governorate",
    "country": "Jordan",
    "region": "MiddleEast",
    "coordinates": { "latitude": 29.5764, "longitude": 35.4196 },
    "description": "A protected desert valley of sandstone and granite that stood in for Arrakis.",
    "imageUrl": "images/locations/wadi-rum.jpg",
    "accessibility": "Moderate",
    "scenes": [
      {
        "productionId": "dune-2021",
        "title": "Arrival on Arrakis",
        "timestamp": "00:41:10",
        "stillImageUrl": "images/scenes/dune-arrakis-still.jpg",
        "realImageUrl": "images/scenes/dune-arrakis-real.jpg"
      }
    ]
  }
]
```

---

## 11. Styling

### 11.1 Design tokens (`wwwroot/css/tokens.css`)

All colors, spacing, radii, and fonts come from CSS custom properties. **No raw hex values inside components.**

```css
:root {
  /* Cinematic dark palette */
  --color-bg:            #0b0b0f;
  --color-surface:       #16161d;
  --color-surface-raised:#1f1f29;
  --color-text:          #f2efe9;
  --color-text-muted:    #9a96a3;
  --color-accent:        #e8b04a;   /* subtitle gold */
  --color-accent-strong: #ffcc66;
  --color-danger:        #e5484d;

  /* Typography */
  --font-display: "Playfair Display", Georgia, serif;
  --font-body:    "Inter", system-ui, sans-serif;
  --font-mono:    "JetBrains Mono", monospace;  /* timestamps & coordinates */

  /* Spacing scale (4px base) */
  --space-1: 0.25rem; --space-2: 0.5rem; --space-3: 0.75rem;
  --space-4: 1rem;    --space-6: 1.5rem; --space-8: 2rem;

  --radius-sm: 6px; --radius-md: 12px; --radius-lg: 20px;
  --ease-out: cubic-bezier(0.22, 1, 0.36, 1);
  --duration-fast: 150ms; --duration-base: 250ms;
}
```

### 11.2 Styling rules

1. **CSS isolation first.** Component styles go in `.razor.css`. `app.css` is only for resets, typography, and utilities.
2. Use **BEM naming**: `block__element--modifier`.
3. **No inline `style=""`**, except for truly dynamic values such as the slider's `--split-position`.
4. Use `::deep` only when styling a child component's markup is unavoidable.
5. Design **mobile-first**: base styles target small screens, then `@media (min-width: 768px)` and `1200px`.
6. Respect motion preferences: wrap animations in `@media (prefers-reduced-motion: no-preference)`.
7. Remove the template's Bootstrap link from `index.html` unless it is actually used.

---

## 12. C# Code Standards

### 12.1 General

- **File-scoped namespaces** (`namespace X;`).
- `<Nullable>enable</Nullable>` and `<ImplicitUsings>enable</ImplicitUsings>` in the `.csproj`.
- Use `<TreatWarningsAsErrors>true</TreatWarningsAsErrors>` if possible. Fix nullable warnings; don't silence them with `!`.
- Use `var` when the type is obvious from the right side.
- Prefer **primary constructors** for services that only inject dependencies.
- Use guard clauses and early returns instead of deep nesting.
- **No magic strings or numbers.** Put them in `AppRoutes`, `StorageKeys`, or constants on the model.
- **Never** block on async code (`.Result`, `.Wait()`), because it deadlocks in WebAssembly.
- Write LINQ for readability. Break long chains so there is one operator per line.
- Use XML `///` doc comments on public models, interfaces, and non-obvious methods.
- Remove unused `using`s, commented-out code, and `Console.WriteLine` before committing.

### 12.2 `_Imports.razor`

Put shared namespaces here so components stay clean:

```razor
@using System.Net.Http.Json
@using Microsoft.AspNetCore.Components.Routing
@using Microsoft.AspNetCore.Components.Web
@using Microsoft.JSInterop
@using CineTerra.Components.Common
@using CineTerra.Models.Common
@using CineTerra.Models.Enums
@using CineTerra.Models.Locations
@using CineTerra.Models.Productions
@using CineTerra.Models.Pilgrimages
@using CineTerra.Services.Interfaces
@using CineTerra.State
@using CineTerra.Utilities
```

### 12.3 `.editorconfig` (root)

```ini
root = true

[*]
indent_style = space
indent_size = 4
end_of_line = crlf
charset = utf-8
trim_trailing_whitespace = true
insert_final_newline = true

[*.{json,css,html,razor}]
indent_size = 2

[*.cs]
csharp_style_namespace_declarations = file_scoped:warning
csharp_style_var_when_type_is_apparent = true:suggestion
dotnet_style_readonly_field = true:warning
dotnet_naming_rule.private_fields_underscore.symbols = private_fields
dotnet_naming_rule.private_fields_underscore.style = underscore_camel
dotnet_naming_rule.private_fields_underscore.severity = warning
dotnet_naming_symbols.private_fields.applicable_kinds = field
dotnet_naming_symbols.private_fields.applicable_accessibilities = private
dotnet_naming_style.underscore_camel.required_prefix = _
dotnet_naming_style.underscore_camel.capitalization = camel_case
```

---

## 13. UI States, Accessibility & Performance

### 13.1 Every data-driven view handles 4 states

```razor
@if (_isLoading)
{
    <LoadingSpinner />
}
else if (_error is not null)
{
    <EmptyState Title="Something went wrong" Message="@_error" />
}
else if (_locations.Count == 0)
{
    <EmptyState Title="No locations found" Message="Try removing a filter." />
}
else
{
    @foreach (var location in _locations)
    {
        <LocationCard @key="location.Id" Location="location" OnSelected="HandleSelected" />
    }
}
```

- Wrap `@Body` in `MainLayout` with an `<ErrorBoundary>` that shows a friendly fallback.
- Catch `HttpRequestException` / `JsonException` in services or pages and show an error state. Never let the app crash silently.

### 13.2 Accessibility

- Use semantic HTML: `<nav>`, `<main>`, `<article>`, `<button>`, plus one `<h1>` per page.
- Every `<img>` needs a meaningful `alt` (`alt=""` if purely decorative).
- Clickable elements must be `<button>` or `<a>`, not `<div @onclick>`.
- Make sure there is a visible `:focus-visible` style and text contrast of at least **4.5:1**.
- The Scene vs Reality slider must also work with the keyboard (arrow keys) and use `role="slider"` with `aria-valuenow`.

### 13.3 Performance

- Add `loading="lazy"` to images below the fold, and serve compressed `.webp` / `.jpg` (≤ 300 KB).
- Use `<Virtualize>` for any list that could exceed ~100 items.
- Avoid calling `StateHasChanged()` by hand unless the update comes from outside Blazor's event system (e.g. a state container event or timer).
- Debounce search input, e.g. 300 ms.

---

## 14. Git Workflow

- **Branches:** `main` (always builds) → `feature/<short-name>` (e.g. `feature/scene-slider`).
- **Commits** follow [Conventional Commits](https://www.conventionalcommits.org/):

| Prefix | Use |
|---|---|
| `feat:` | New feature, e.g. `feat: add scene comparison slider` |
| `fix:` | Bug fix |
| `style:` | CSS / formatting only |
| `refactor:` | Code change with no behavior change |
| `docs:` | README / guidelines |
| `chore:` | Config, packages, assets |

- `.gitignore`: use `dotnet new gitignore` (excludes `bin/`, `obj/`, `.vs/`).

---

## 15. Definition of Done Checklist

Before a feature counts as complete:

- [ ] `dotnet build` succeeds with **zero warnings**
- [ ] Files are in the correct folders, with namespaces matching the folders
- [ ] Models use proper OOP (`init`, encapsulated collections, no UI/service references)
- [ ] Components inject **interfaces**, not concrete services
- [ ] No business logic or string formatting inside Razor markup
- [ ] Loading, error, empty, and success states are all handled
- [ ] `@key` is used in every `@foreach` that renders components
- [ ] Event subscriptions are disposed
- [ ] Styles use design tokens with CSS isolation, and there are no inline styles
- [ ] Layout is responsive at 375px, 768px, and 1280px
- [ ] Keyboard navigable, images have `alt`, focus is visible
- [ ] Route strings come from `AppRoutes`, storage keys come from `StorageKeys`
- [ ] No `Console.WriteLine`, commented-out code, or unused `using`s
