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
- **50+ Components** — Buttons, Badges, Avatars, Cards, Modals, Dialogs, Toasts, Carousels, Dropdowns, Popovers, Breadcrumbs, Tables, Accordions, Alerts, and more
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

In your `App.razor` (or `index.html` for WASM), add the Tabler CSS and JS references.  Note that I'm working hard to
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

### Accordion

| Component | Description |
|-----------|-------------|
| `TabAccordion` | Manages collapsible content panels with optional flush styling and multi-panel support |
| `TabAccordionItem` | A collapsible panel used inside `TabAccordion` to display expandable content sections |

### Alert

| Component | Description |
|-----------|-------------|
| `TabAlert` | Displays contextual messages with colours, icons, titles, dismissal, and auto-close |

### Avatar

| Component | Description |
|-----------|-------------|
| `TabAvatar` | User profile images, icons, or initials with status indicators in various sizes and shapes |
| `TabAvatarList` | A group of `TabAvatar` components with optional stacking |

### Badge

| Component | Description |
|-----------|-------------|
| `TabBadge` | Small count and label indicators with customisable shape and size |

### Breadcrumbs

| Component | Description |
|-----------|-------------|
| `TabBreadcrumbs` | Navigation breadcrumb trail with auto-generation or manual definition |
| `TabBreadcrumbItem` | Individual breadcrumb entry with optional link and icon |

### Button

| Component | Description |
|-----------|-------------|
| `TabButton` | Buttons with colour, size, outline, icon, and loading states |

### Card

| Component | Description |
|-----------|-------------|
| `TabCard` | Content container with status borders and padding variations |
| `TabCardHeader` | Card header for titles, subtitles, and action buttons |
| `TabCardBody` | Card content area; may be repeated for distinct sections |
| `TabCardFooter` | Card footer for action buttons or supplementary information |
| `TabCardTitle` | Heading element within a card header (h1–h6) |
| `TabCardSubtitle` | Secondary context text within a card header |
| `TabCardActions` | Aligns action buttons, dropdowns, or interactive elements in a card header |

### Carousel

| Component | Description |
|-----------|-------------|
| `TabCarousel` | Cycles through slides with images, captions, keyboard navigation, and auto-play |
| `TabCarouselSlide` | Individual carousel slide with image, caption, and custom content |

### Countup

| Component | Description |
|-----------|-------------|
| `TabCountup` | Animated number counter from a start to end value with easing and scroll-trigger support |

### Dialog

| Component | Description |
|-----------|-------------|
| `TabDialog` | Awaitable message dialog with configurable buttons (Ok, Cancel, Yes, No) |

### Divider

| Component | Description |
|-----------|-------------|
| `TabDivider` | Divider with optional text label, alignment, and colour |

### Dropdown

| Component | Description |
|-----------|-------------|
| `TabDropdown` | Toggleable dropdown menu supporting various directions and open/close callbacks |
| `TabDropdownMenu` | Menu container holding items, dividers, and headers with alignment options |
| `TabDropdownItem` | Individual dropdown item rendered as a button or anchor |
| `TabDropdownHeader` | Header label within a dropdown for categorising item groups |
| `TabDropdownDivider` | Visual divider between groups of items in a dropdown |

### Dynamic Form

| Component | Description |
|-----------|-------------|
| `TabDynamicForm` | Auto-generates form inputs from data-annotation attributes on a model |

### Footer

| Component | Description |
|-----------|-------------|
| `TabFooter` | Page footer with optional version display and responsive container |

### Icon

| Component | Description |
|-----------|-------------|
| `TabIcon` | 4,000+ Tabler SVG icons with customisable size, stroke width, colour, and animations |

### Layout

| Component | Description |
|-----------|-------------|
| `TabLayout` | Outermost page wrapper containing the full page structure |
| `TabNavbar` | Horizontal top navigation bar with brand, navigation links, and right-side items |
| `TabSidebar` | Vertical sidebar with brand area, navigation links, and dark/condensed variants |
| `TabPageWrapper` | Wrapper for the main page content area including body and footer |
| `TabPageBody` | Main content area with optional responsive container |
| `TabPageHeader` | Page-level header with title, subtitle, and action buttons |
| `SetTabPageHeader` | Helper component for setting `TabPageHeader` title, subtitle, and breadcrumbs from child pages |

### List

| Component | Description |
|-----------|-------------|
| `TabList` | Displays collections as Tabler list-groups with selection and bulk actions |

### Modal

| Component | Description |
|-----------|-------------|
| `TabModal` | Full modal dialog with size, centred layout, and scroll options |
| `TabModalHeader` | Modal header with custom content and close button |
| `TabModalTitle` | Heading element within a modal header (h1–h6) |
| `TabModalBody` | Main content area of a modal dialog |
| `TabModalFooter` | Footer section of a modal dialog for action buttons |

### Off-Canvas

| Component | Description |
|-----------|-------------|
| `TabOffCanvas` | Panel that slides in from any viewport edge with header, footer, and backdrop options |

### Placeholder

| Component | Description |
|-----------|-------------|
| `TabPlaceholderContainer` | Applies glow or wave animation to a group of placeholder elements |
| `TabPlaceholder` | A single loading skeleton span with customisable columns and colour |

### Popover

| Component | Description |
|-----------|-------------|
| `TabPopover` | Contextual overlay anchored to a trigger element with click, hover, and focus triggers |

### Ribbon

| Component | Description |
|-----------|-------------|
| `TabRibbon` | Decorative corner/edge labels to highlight featured, new, or promoted content |

### Table

| Component | Description |
|-----------|-------------|
| `TabTable` | Data-driven table with sortable columns, row selection, and custom cell templates |
| `TabTableColumn` | Column definition specifying header text, field mapping, sorting, and cell templates |

### Toast

| Component | Description |
|-----------|-------------|
| `TabToastContainer` | Manages and displays toast notifications from `TabToastService` with positioning |
| `TabToast` | Toast notification with auto-dismiss, icon, colour, and custom content support |

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

## License

This project is licensed under the [MIT License](LICENSE).
