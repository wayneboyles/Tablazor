# Tablazor

[![NuGet](https://img.shields.io/nuget/v/Tablazor.svg)](https://www.nuget.org/packages/Tablazor/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Tablazor.svg)](https://www.nuget.org/packages/Tablazor/)
[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)

A Blazor component library built on the [Tabler](https://tabler.io/) UI kit — providing production-ready, themeable UI components for Blazor Server and WebAssembly applications.

## Goals

- Provide a comprehensive set of Blazor components that wrap the Tabler design system, so developers can build beautiful dashboards and web apps without writing raw HTML/CSS.
- Support .NET 8.0, 9.0, and 10.0 with a single package.
- Keep JavaScript to an absolute minimum — favour pure Blazor rendering and state management wherever possible.
- Ship accessible components with ARIA attributes and keyboard support out of the box.
- Offer a simple, consistent API across all components via a shared base class and fluent builders.

## Features

- **Multi-Target Support** — .NET 8.0, 9.0, and 10.0
- **40+ Components** — Buttons, Badges, Avatars, Cards, Modals, Dialogs, Toasts, Carousels, Dropdowns, Breadcrumbs, Icons, and more
- **4,000+ Icons** — The complete Tabler Icons set included as strongly-typed constants
- **Minimal JavaScript** — Pure Blazor state management; JS interop only where the DOM API requires it
- **Accessible** — ARIA roles, labels, and keyboard navigation
- **Themeable** — Built on Tabler CSS variables for easy colour and style customisation

## Installation

### 1. Install the NuGet package

```bash
dotnet add package Tablazor
```

### 2. Register services

In your `Program.cs`, register the Tablazor services:

```csharp
builder.Services.AddTablazor();
```

This registers services required by components such as `TabToastContainer` and `TabPageHeader`.

### 3. Add the Tabler stylesheet and script

In your `App.razor` (or `index.html` for WASM), add the Tabler CSS and JS references.  Note that I'm  woorking hard to
completely remove the dependency on the `tabler.js` file and create everything in pure Blazor.

```html
<!-- In <head> -->
<link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/@tabler/core@latest/dist/css/tabler.min.css">

<!-- Before closing </body> -->
<script src="https://cdn.jsdelivr.net/npm/@tabler/core@latest/dist/js/tabler.min.js"></script>
```

### 4. Add namespace imports

In your `_Imports.razor`, add the following namespaces:

```razor
@using Tablazor.Components
@using Tablazor.Enums
@using Tablazor.Icons
@using Tablazor.Services
```

You're ready to go.

## Quick Start

```razor
@* Badge *@
<TabBadge Color="TabColors.Primary">New</TabBadge>

@* Button *@
<TabButton Color="TabColors.Success" OnClick="HandleClick">Save</TabButton>

@* Avatar *@
<TabAvatar ImageUrl="https://example.com/avatar.jpg" Size="AvatarSize.Large" />

@* Awaitable dialog *@
<TabButton Color="TabColors.Danger" OnClick="ConfirmDelete">Delete</TabButton>
<TabDialog @ref="_dialog"
           Title="Confirm"
           Message="Are you sure you want to delete this item?"
           Buttons="DialogButtons.YesNo"
           Color="TabColors.Danger" />

@code {
    private TabDialog _dialog = null!;

    private async Task ConfirmDelete()
    {
        var result = await _dialog.ShowAsync();
        if (result == DialogResult.Yes)
        {
            // perform delete
        }
    }
}
```

## Components

| Component | Description |
|-----------|-------------|
| `TabAvatar` | User profile images and text/icon placeholders |
| `TabAvatarList` | Grouped avatars with optional stacking |
| `TabBadge` | Small count and label indicators |
| `TabBreadcrumbs` | Navigation breadcrumb trail |
| `TabBreadcrumbItem` | Individual breadcrumb entry |
| `TabButton` | Buttons with colour, size, outline, and loading states |
| `TabCard` | Content container with header, body, footer, title, subtitle, and actions |
| `TabCarousel` | Image/content carousel with slide transitions and indicators |
| `TabCarouselSlide` | Individual carousel slide |
| `TabCountup` | Animated number counter |
| `TabDialog` | Awaitable message dialog with configurable buttons (Ok, Cancel, Yes, No) |
| `TabDivider` | Horizontal or vertical divider with optional label |
| `TabDropdown` | Dropdown menu with items, headers, and dividers |
| `TabIcon` | 4,000+ Tabler SVG icons with size and animation support |
| `TabModal` | Full modal dialog with header, body, and footer regions |
| `TabPageHeader` | Page-level header with title, subtitle, and actions |
| `TabToast` | Toast notification messages |
| `TabToastContainer` | Container that manages toast positioning and lifecycle |

## Common Parameters

All components inherit from `TabBaseComponent` and share these parameters:

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `CssClass` | `string?` | `null` | Additional CSS classes to apply |
| `Style` | `string?` | `null` | Inline CSS styles |
| `Visible` | `bool` | `true` | Whether the component renders |
| `Disabled` | `bool` | `false` | Whether the component is disabled |

All components also support [attribute splatting](https://learn.microsoft.com/en-us/aspnet/core/blazor/components/#attribute-splatting-and-arbitrary-parameters) via `@attributes` for any additional HTML attributes.

## Development

### Prerequisites

- [.NET 10.0 SDK](https://dotnet.microsoft.com/download) (also builds for .NET 8.0 and 9.0)

### Build

```bash
dotnet build src/Tablazor.sln
```

### Run Tests

```bash
dotnet test src/Tablazor.sln
```

### Run Demo Site

```bash
dotnet run --project src/Tablazor.DemoSite
```

The demo site runs at `https://localhost:7170`.

### Create NuGet Package

```bash
dotnet pack src/Tablazor/Tablazor.csproj -c Release -o ./nupkg
```

## Project Structure

```
src/
├── Tablazor/                    # Component library (multi-target: net8.0, net9.0, net10.0)
│   ├── Tablazor/
│   │   ├── Components/          # Razor components (.razor + .razor.cs code-behind)
│   │   ├── Enums/               # TabColors, sizes, shapes, dialog results, etc.
│   │   ├── Attributes/          # CssClassName, validation attributes
│   │   ├── Icons/               # 4,000+ Tabler SVG icon definitions
│   │   ├── Services/            # TabToastService, TabPageHeaderService
│   │   ├── CssBuilder.cs        # Fluent CSS class builder
│   │   ├── StyleBuilder.cs      # Fluent inline style builder
│   │   └── TabBaseComponent.cs  # Base class for all components
│   └── Microsoft/Extensions/
│       └── DependencyInjection/ # AddTablazor() extension method
├── Tablazor.DemoSite/           # Interactive demo and documentation site
tests/
└── Tablazor.Tests/              # Unit tests (xUnit + bUnit)
templates/
└── TablazorTemplate/            # dotnet new project template
```

## License

This project is licensed under the [MIT License](LICENSE).
