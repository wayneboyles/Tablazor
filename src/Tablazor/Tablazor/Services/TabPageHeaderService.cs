using Microsoft.AspNetCore.Components;
using Tablazor.Components;

namespace Tablazor.Services;

/// <summary>
/// Service that provides methods to update the <see cref="TabPageHeader"/>
/// component
/// </summary>
public sealed class TabPageHeaderService
{
    private string? _subtitle;
    private bool _showBreadcrumbs;
    private string _title = string.Empty;
    private RenderFragment? _childContent;
    
    /// <summary>
    /// Called when the page header changes.
    /// </summary>
    public event Action? OnChange;

    /// <summary>
    /// Gets or sets the page actions
    /// </summary>
    public RenderFragment? ChildContent
    {
        get => _childContent;
        set
        {
            if (_childContent != value)
            {
                _childContent = value;
                NotifyStateChanged();
            }
        }
    }

    /// <summary>
    /// The main title of the page header.
    /// </summary>
    public string Title
    {
        get => _title;
        set
        {
            if (_title != value)
            {
                _title = value;
                NotifyStateChanged();
            }
        }
    }
    
    /// <summary>
    /// The subtitle of the page header.
    /// </summary>
    public string? Subtitle 
    {
        get => _subtitle;
        set
        {
            if (_subtitle != value)
            {
                _subtitle = value;
                NotifyStateChanged();
            }
        }
    }

    /// <summary>
    /// Whether to show breadcrumbs in the header
    /// </summary>
    public bool ShowBreadcrumbs
    {
        get => _showBreadcrumbs;
        set
        {
            if (_showBreadcrumbs != value)
            {
                _showBreadcrumbs = value;
                NotifyStateChanged();
            }
        }
    }

    /// <summary>
    /// Sets the page header.
    /// </summary>
    /// <param name="title">The page title</param>
    /// <param name="subtitle">The optional page subtitle</param>
    /// <param name="childContent">The optional page actions to render</param>
    /// <param name="showBreadcrumbs">Whether to show breadcrumbs</param>
    public void SetPageHeader(string title, string? subtitle = null, RenderFragment? childContent = null, bool showBreadcrumbs = false)
    {
        _title = title;
        _subtitle = subtitle ?? string.Empty;
        _childContent = childContent ?? null;
        _showBreadcrumbs = showBreadcrumbs;
        
        NotifyStateChanged();
    }
    
    /// <summary>
    /// Clears the page header.
    /// </summary>
    public void Clear()
    {
        _title = string.Empty;
        _subtitle = string.Empty;
        _childContent = null;
        _showBreadcrumbs = false;

        NotifyStateChanged();
    }
    
    private void NotifyStateChanged() => OnChange?.Invoke();
}