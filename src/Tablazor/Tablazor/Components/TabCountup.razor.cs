using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Tablazor.Enums;

namespace Tablazor.Components;

/// <summary>
/// A countup animation component that animates a number from a start value to an end value.
/// Inspired by countUp.js but implemented in pure Blazor with minimal JavaScript.
/// </summary>
/// <remarks>
/// Supports configurable duration, easing functions, number formatting, and scroll-triggered animations.
/// </remarks>
/// <example>
/// <code>
/// &lt;TabCountup EndValue="1234.56" Duration="2000" DecimalPlaces="2" /&gt;
/// </code>
/// </example>
public partial class TabCountup : TabBaseComponent
{
    private decimal _currentValue;
    private decimal _displayValue;
    private bool _isAnimating;
    private bool _isPaused;
    private bool _hasStarted;
    private DateTime _animationStartTime;
    private DateTime _pauseStartTime;
    private TimeSpan _totalPausedDuration;
    private CancellationTokenSource? _animationCts;
    private IJSObjectReference? _jsModule;
    private DotNetObjectReference<TabCountup>? _dotNetRef;
    private string _countupId = string.Empty;

    /// <summary>
    /// Gets or sets the starting value for the animation.
    /// </summary>
    /// <value>The default value is 0.</value>
    [Parameter]
    public decimal StartValue { get; set; }

    /// <summary>
    /// Gets or sets the ending value for the animation.
    /// </summary>
    [Parameter]
    public decimal EndValue { get; set; }

    /// <summary>
    /// Gets or sets the duration of the animation in milliseconds.
    /// </summary>
    /// <value>The default value is 2000 (2 seconds).</value>
    [Parameter]
    public int Duration { get; set; } = 2000;

    /// <summary>
    /// Gets or sets the number of decimal places to display.
    /// </summary>
    /// <value>The default value is 0.</value>
    [Parameter]
    public int DecimalPlaces { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to use grouping separators (thousands separators).
    /// </summary>
    /// <value>The default value is <c>true</c>.</value>
    [Parameter]
    public bool UseGrouping { get; set; } = true;

    /// <summary>
    /// Gets or sets the grouping separator character.
    /// </summary>
    /// <value>The default value is ",".</value>
    [Parameter]
    public string Separator { get; set; } = ",";

    /// <summary>
    /// Gets or sets the decimal separator character.
    /// </summary>
    /// <value>The default value is ".".</value>
    [Parameter]
    public string DecimalSeparator { get; set; } = ".";

    /// <summary>
    /// Gets or sets the prefix text to display before the number.
    /// </summary>
    [Parameter]
    public string? Prefix { get; set; }

    /// <summary>
    /// Gets or sets the suffix text to display after the number.
    /// </summary>
    [Parameter]
    public string? Suffix { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to use easing.
    /// </summary>
    /// <value>The default value is <c>true</c>.</value>
    [Parameter]
    public bool UseEasing { get; set; } = true;

    /// <summary>
    /// Gets or sets the easing function to use.
    /// </summary>
    /// <value>The default value is <see cref="CountupEasing.EaseOutExpo"/>.</value>
    [Parameter]
    public CountupEasing Easing { get; set; } = CountupEasing.EaseOutExpo;

    /// <summary>
    /// Gets or sets a value indicating whether to start the animation automatically.
    /// </summary>
    /// <value>The default value is <c>true</c>.</value>
    [Parameter]
    public bool AutoStart { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether to use scroll spy (start animation when element is visible).
    /// </summary>
    /// <value>The default value is <c>false</c>.</value>
    [Parameter]
    public bool EnableScrollSpy { get; set; }

    /// <summary>
    /// Gets or sets the delay in milliseconds before starting animation after element becomes visible.
    /// </summary>
    /// <value>The default value is 0.</value>
    [Parameter]
    public int ScrollSpyDelay { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to only animate once when using scroll spy.
    /// </summary>
    /// <value>The default value is <c>true</c>.</value>
    [Parameter]
    public bool ScrollSpyOnce { get; set; } = true;

    /// <summary>
    /// Gets or sets the frame rate for the animation in frames per second.
    /// </summary>
    /// <value>The default value is 60.</value>
    [Parameter]
    public int FrameRate { get; set; } = 60;

    /// <summary>
    /// Gets or sets the callback invoked when the animation starts.
    /// </summary>
    [Parameter]
    public EventCallback OnStart { get; set; }

    /// <summary>
    /// Gets or sets the callback invoked when the animation completes.
    /// </summary>
    [Parameter]
    public EventCallback OnComplete { get; set; }

    /// <summary>
    /// Gets or sets the callback invoked when the animation is paused or resumed.
    /// </summary>
    [Parameter]
    public EventCallback<bool> OnPauseResume { get; set; }

    /// <summary>
    /// Gets or sets the callback invoked on each animation frame with the current value.
    /// </summary>
    [Parameter]
    public EventCallback<decimal> OnUpdate { get; set; }

    /// <summary>
    /// Gets or sets custom content to render. If provided, the formatted value is passed as context.
    /// </summary>
    [Parameter]
    public RenderFragment<string>? ChildContent { get; set; }

    /// <summary>
    /// Gets a value indicating whether the animation is currently running.
    /// </summary>
    public bool IsAnimating => _isAnimating && !_isPaused;

    /// <summary>
    /// Gets a value indicating whether the animation is paused.
    /// </summary>
    public bool IsPaused => _isPaused;

    /// <summary>
    /// Gets the current animated value.
    /// </summary>
    public decimal CurrentValue => _displayValue;

    /// <summary>
    /// Gets the formatted display string.
    /// </summary>
    public string FormattedValue => FormatNumber(_displayValue);

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        base.OnInitialized();
        _countupId = GetId();
        _currentValue = StartValue;
        _displayValue = StartValue;
    }

    /// <inheritdoc />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender)
        {
            if (EnableScrollSpy && JsRuntime is not null)
            {
                await SetupScrollSpyAsync();
            }
            else if (AutoStart)
            {
                await StartAsync();
            }
        }
    }

    private async Task SetupScrollSpyAsync()
    {
        if (JsRuntime is null)
        {
            return;
        }

        try
        {
            _jsModule = await JsRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Tablazor/Tablazor/Components/TabCountup.razor.js");

            _dotNetRef = DotNetObjectReference.Create(this);

            await _jsModule.InvokeVoidAsync("setupScrollSpy", _countupId, _dotNetRef);
        }
        catch (JSException)
        {
            // Fallback to auto-start if JS fails
            if (AutoStart)
            {
                await StartAsync();
            }
        }
    }

    /// <summary>
    /// Called from JavaScript when the element becomes visible in the viewport.
    /// </summary>
    [JSInvokable]
    public async Task OnElementVisible()
    {
        if (_hasStarted && ScrollSpyOnce)
        {
            return;
        }

        if (ScrollSpyDelay > 0)
        {
            await Task.Delay(ScrollSpyDelay);
        }

        await StartAsync();
    }

    /// <summary>
    /// Starts or restarts the countup animation.
    /// </summary>
    public async Task StartAsync()
    {
        if (_isAnimating)
        {
            await ResetAsync();
        }

        _hasStarted = true;
        _isAnimating = true;
        _isPaused = false;
        _currentValue = StartValue;
        _displayValue = StartValue;
        _totalPausedDuration = TimeSpan.Zero;
        _animationStartTime = DateTime.UtcNow;

        _animationCts?.Cancel();
        _animationCts = new CancellationTokenSource();

        if (OnStart.HasDelegate)
        {
            await OnStart.InvokeAsync();
        }

        StateHasChanged();

        _ = AnimateAsync(_animationCts.Token);
    }

    /// <summary>
    /// Resets the countup to the starting value.
    /// </summary>
    public async Task ResetAsync()
    {
        _animationCts?.CancelAsync();
        _isAnimating = false;
        _isPaused = false;
        _currentValue = StartValue;
        _displayValue = StartValue;
        _totalPausedDuration = TimeSpan.Zero;

        await InvokeAsync(StateHasChanged);
    }

    /// <summary>
    /// Updates the end value and animates to the new value.
    /// </summary>
    /// <param name="newEndValue">The new end value to animate to.</param>
    public async Task UpdateAsync(decimal newEndValue)
    {
        _animationCts?.CancelAsync();

        // Start from current position
        StartValue = _displayValue;
        EndValue = newEndValue;

        await StartAsync();
    }

    /// <summary>
    /// Toggles between paused and running states.
    /// </summary>
    public async Task PauseResumeAsync()
    {
        if (!_isAnimating)
        {
            return;
        }

        _isPaused = !_isPaused;

        if (_isPaused)
        {
            _pauseStartTime = DateTime.UtcNow;
        }
        else
        {
            _totalPausedDuration += DateTime.UtcNow - _pauseStartTime;
        }

        if (OnPauseResume.HasDelegate)
        {
            await OnPauseResume.InvokeAsync(_isPaused);
        }

        StateHasChanged();
    }

    private async Task AnimateAsync(CancellationToken cancellationToken)
    {
        var frameDelay = 1000 / FrameRate;
        var durationMs = Duration;

        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                if (_isPaused)
                {
                    await Task.Delay(frameDelay, cancellationToken);
                    continue;
                }

                var elapsed = DateTime.UtcNow - _animationStartTime - _totalPausedDuration;
                var progress = Math.Min(1.0, elapsed.TotalMilliseconds / durationMs);

                if (UseEasing)
                {
                    progress = ApplyEasing(progress);
                }

                var range = EndValue - StartValue;
                _displayValue = StartValue + (decimal)progress * range;

                // Round to decimal places
                _displayValue = Math.Round(_displayValue, DecimalPlaces);

                if (OnUpdate.HasDelegate)
                {
                    await OnUpdate.InvokeAsync(_displayValue);
                }

                await InvokeAsync(StateHasChanged);

                if (progress >= 1.0)
                {
                    _displayValue = EndValue;
                    _isAnimating = false;

                    await InvokeAsync(StateHasChanged);

                    if (OnComplete.HasDelegate)
                    {
                        await OnComplete.InvokeAsync();
                    }

                    break;
                }

                await Task.Delay(frameDelay, cancellationToken);
            }
        }
        catch (TaskCanceledException)
        {
            // Animation was canceled
        }
    }

    private double ApplyEasing(double t)
    {
        return Easing switch
        {
            CountupEasing.Linear => t,
            CountupEasing.EaseIn => t * t,
            CountupEasing.EaseOut => t * (2 - t),
            CountupEasing.EaseInOut => t < 0.5 ? 2 * t * t : -1 + (4 - 2 * t) * t,
            CountupEasing.EaseOutExpo => t == 1 ? 1 : 1 - Math.Pow(2, -10 * t),
            CountupEasing.EaseOutCubic => 1 - Math.Pow(1 - t, 3),
            CountupEasing.EaseOutElastic => EaseOutElastic(t),
            CountupEasing.EaseOutBounce => EaseOutBounce(t),
            _ => t
        };
    }

    private static double EaseOutElastic(double t)
    {
        if (t == 0 || t == 1)
        {
            return t;
        }

        const double p = 0.3;
        const double s = p / 4;

        return Math.Pow(2, -10 * t) * Math.Sin((t - s) * (2 * Math.PI) / p) + 1;
    }

    private static double EaseOutBounce(double t)
    {
        const double n1 = 7.5625;
        const double d1 = 2.75;

        if (t < 1 / d1)
        {
            return n1 * t * t;
        }
        else if (t < 2 / d1)
        {
            t -= 1.5 / d1;
            return n1 * t * t + 0.75;
        }
        else if (t < 2.5 / d1)
        {
            t -= 2.25 / d1;
            return n1 * t * t + 0.9375;
        }
        else
        {
            t -= 2.625 / d1;
            return n1 * t * t + 0.984375;
        }
    }

    private string FormatNumber(decimal value)
    {
        var absoluteValue = Math.Abs(value);
        var isNegative = value < 0;

        // Format with decimal places
        var format = DecimalPlaces > 0
            ? $"F{DecimalPlaces}"
            : "F0";

        var formatted = absoluteValue.ToString(format, CultureInfo.InvariantCulture);

        // Replace decimal separator
        if (DecimalPlaces > 0 && DecimalSeparator != ".")
        {
            formatted = formatted.Replace(".", DecimalSeparator);
        }

        // Add grouping separators
        if (UseGrouping)
        {
            formatted = AddGroupingSeparators(formatted);
        }

        // Add prefix/suffix and sign
        var result = $"{Prefix}{(isNegative ? "-" : "")}{formatted}{Suffix}";

        return result;
    }

    private string AddGroupingSeparators(string number)
    {
        var parts = number.Split(DecimalSeparator.ToCharArray(), 2);
        var integerPart = parts[0];
        var decimalPart = parts.Length > 1 ? parts[1] : null;

        // Add separators to integer part (right to left, every 3 digits)
        var chars = integerPart.ToCharArray();
        var result = new System.Text.StringBuilder();
        var count = 0;

        for (var i = chars.Length - 1; i >= 0; i--)
        {
            if (count > 0 && count % 3 == 0)
            {
                result.Insert(0, Separator);
            }

            result.Insert(0, chars[i]);
            count++;
        }

        if (decimalPart != null)
        {
            result.Append(DecimalSeparator);
            result.Append(decimalPart);
        }

        return result.ToString();
    }

    /// <inheritdoc />
    protected override string BuildCssClass()
    {
        return new CssBuilder("countup")
            .AddClass("countup-animating", _isAnimating && !_isPaused)
            .AddClass("countup-paused", _isPaused)
            .AddClass("countup-complete", _hasStarted && !_isAnimating)
            .AddClass(CssClass)
            .Build();
    }

    /// <inheritdoc />
    public override async ValueTask DisposeAsync()
    {
        _animationCts?.CancelAsync();
        _animationCts?.Dispose();

        if (_jsModule is not null)
        {
            try
            {
                await _jsModule.InvokeVoidAsync("cleanup", _countupId);
                await _jsModule.DisposeAsync();
            }
            catch (JSDisconnectedException)
            {
                // Ignore if JS runtime is disconnected
            }
        }

        _dotNetRef?.Dispose();

        await base.DisposeAsync();
    }
}
