# Tablazor

A Blazor component library built on the [Tabler](https://tabler.io/) UI kit.

## Features

- **Multi-Target Support** - Supports .NET 8.0, 9.0, and 10.0
- **Tabler Design System** - Beautiful, consistent UI components based on Tabler
- **Blazor Native** - Built specifically for Blazor with full component lifecycle support
- **Accessible** - Components support ARIA attributes and keyboard navigation
- **4000+ Icons** - Includes the complete Tabler Icons set

## Installation

```bash
dotnet add package Tablazor
```

## Quick Start

1. Add the Tabler CSS to your `App.razor` or `index.html`:

```html
<link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/@tabler/core@latest/dist/css/tabler.min.css">
```

2. Add the namespace to your `_Imports.razor`:

```razor
@using Tablazor.Components
@using Tablazor.Enums
```

3. Use components in your Blazor pages:

```razor
<TabBadge Color="TabColors.Primary">New</TabBadge>

<TabAvatar ImageUrl="https://example.com/avatar.jpg" />

<TabAvatarList Stacked="true">
    <TabAvatar ImageUrl="..." />
    <TabAvatar ImageUrl="..." />
    <TabAvatar>+3</TabAvatar>
</TabAvatarList>
```

## Components

### Badge

Small count and labeling components for adding extra information to interface elements.

```razor
<TabBadge>Default</TabBadge>
<TabBadge Color="TabColors.Success">Success</TabBadge>
<TabBadge Color="TabColors.Primary" Shape="BadgeShape.Pill">Pill</TabBadge>
<TabBadge Color="TabColors.Info" Size="BadgeSize.Large">Large</TabBadge>
<TabBadge Color="TabColors.Warning" Href="/notifications">Link Badge</TabBadge>
```

**Parameters:**
| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `Color` | `TabColors` | `Default` | Badge color |
| `Shape` | `BadgeShape` | `Default` | Badge shape (Default, Pill) |
| `Size` | `BadgeSize` | `Default` | Badge size (Small, Default, Large) |
| `Href` | `string?` | `null` | URL to render badge as a link |
| `Visible` | `bool` | `true` | Whether the badge is visible |

### Avatar

User profile images and placeholders for personalizing the interface.

```razor
<TabAvatar ImageUrl="https://example.com/user.jpg" />
<TabAvatar Color="TabColors.Primary">JD</TabAvatar>
<TabAvatar Color="TabColors.Success" Icon="user" />
<TabAvatar ImageUrl="..." Size="AvatarSize.Large" StatusColor="TabColors.Success" />
```

**Parameters:**
| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `ImageUrl` | `string?` | `null` | URL of the avatar image |
| `Color` | `TabColors` | `Default` | Background color for text/icon avatars |
| `Size` | `AvatarSize` | `Default` | Avatar size (ExtraSmall, Small, Default, Large, ExtraLarge) |
| `Shape` | `AvatarShape` | `Default` | Avatar shape (Default, Round, Square) |
| `Icon` | `string?` | `null` | Icon name to display |
| `StatusColor` | `TabColors` | `Default` | Color of status indicator badge |
| `Visible` | `bool` | `true` | Whether the avatar is visible |

### Avatar List

Group multiple avatars together, with optional stacking for overlapping display.

```razor
<TabAvatarList>
    <TabAvatar ImageUrl="..." />
    <TabAvatar ImageUrl="..." />
</TabAvatarList>

<TabAvatarList Stacked="true">
    <TabAvatar ImageUrl="..." />
    <TabAvatar ImageUrl="..." />
    <TabAvatar>+5</TabAvatar>
</TabAvatarList>
```

**Parameters:**
| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `Stacked` | `bool` | `false` | Display avatars as overlapping stack |
| `Visible` | `bool` | `true` | Whether the list is visible |

## Common Parameters

All components inherit from `TabBaseComponent` and share these parameters:

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `CssClass` | `string?` | `null` | Additional CSS classes |
| `Style` | `string?` | `null` | Inline CSS styles |
| `Visible` | `bool` | `true` | Whether the component renders |
| `Disabled` | `bool` | `false` | Whether the component is disabled |

Components also support attribute splatting for additional HTML attributes.

## Development

### Prerequisites

- .NET 10.0 SDK

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

The demo site runs at https://localhost:7170

## Project Structure

```
src/
├── Tablazor/                    # Main component library
│   └── Tablazor/
│       ├── Components/          # Razor components
│       ├── Enums/               # Color, size, shape enumerations
│       ├── Attributes/          # Custom validation attributes
│       └── Icons/               # Tabler Icons (4000+)
├── Tablazor.DemoSite/           # Demo and documentation site
tests/
└── Tablazor.Tests/              # Unit tests (xUnit + bUnit)
```

## License

[Add your license here]
