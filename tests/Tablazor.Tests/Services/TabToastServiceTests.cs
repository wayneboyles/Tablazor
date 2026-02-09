using System;
using System.Collections.Generic;
using Tablazor.Enums;
using Tablazor.Services;

namespace Tablazor.Services;

public class TabToastServiceTests
{
    [Fact]
    public void Show_CreatesToastInstance()
    {
        var service = new TabToastService();

        var toast = service.Show(new ToastOptions
        {
            Title = "Test",
            Message = "Message"
        });

        Assert.NotNull(toast);
        Assert.NotEmpty(toast.Id);
        Assert.Single(service.Toasts);
    }

    [Fact]
    public void Show_WithMessage_CreatesToast()
    {
        var service = new TabToastService();

        var toast = service.Show("Test message");

        Assert.Equal("Test message", toast.Options.Message);
    }

    [Fact]
    public void Show_WithTitleAndMessage_CreatesToast()
    {
        var service = new TabToastService();

        var toast = service.Show("Title", "Message");

        Assert.Equal("Title", toast.Options.Title);
        Assert.Equal("Message", toast.Options.Message);
    }

    [Fact]
    public void Show_WithColor_AppliesColor()
    {
        var service = new TabToastService();

        var toast = service.Show("Message", TabColors.Success);

        Assert.Equal(TabColors.Success, toast.Options.Color);
    }

    [Fact]
    public void ShowSuccess_CreatesSuccessToast()
    {
        var service = new TabToastService();

        var toast = service.ShowSuccess("Success message");

        Assert.Equal("Success", toast.Options.Title);
        Assert.Equal("Success message", toast.Options.Message);
        Assert.Equal(TabColors.Success, toast.Options.Color);
    }

    [Fact]
    public void ShowSuccess_WithCustomTitle_UsesCustomTitle()
    {
        var service = new TabToastService();

        var toast = service.ShowSuccess("Message", "Custom Title");

        Assert.Equal("Custom Title", toast.Options.Title);
    }

    [Fact]
    public void ShowError_CreatesDangerToast()
    {
        var service = new TabToastService();

        var toast = service.ShowError("Error message");

        Assert.Equal("Error", toast.Options.Title);
        Assert.Equal("Error message", toast.Options.Message);
        Assert.Equal(TabColors.Danger, toast.Options.Color);
    }

    [Fact]
    public void ShowWarning_CreatesWarningToast()
    {
        var service = new TabToastService();

        var toast = service.ShowWarning("Warning message");

        Assert.Equal("Warning", toast.Options.Title);
        Assert.Equal("Warning message", toast.Options.Message);
        Assert.Equal(TabColors.Warning, toast.Options.Color);
    }

    [Fact]
    public void ShowInfo_CreatesInfoToast()
    {
        var service = new TabToastService();

        var toast = service.ShowInfo("Info message");

        Assert.Equal("Info", toast.Options.Title);
        Assert.Equal("Info message", toast.Options.Message);
        Assert.Equal(TabColors.Info, toast.Options.Color);
    }

    [Fact]
    public void Remove_RemovesToast()
    {
        var service = new TabToastService();
        var toast = service.Show("Message");

        service.Remove(toast);

        Assert.Empty(service.Toasts);
    }

    [Fact]
    public void Remove_ById_RemovesToast()
    {
        var service = new TabToastService();
        var toast = service.Show("Message");

        service.Remove(toast.Id);

        Assert.Empty(service.Toasts);
    }

    [Fact]
    public void Remove_InvokesOnCloseCallback()
    {
        var service = new TabToastService();
        var callbackInvoked = false;
        var toast = service.Show(new ToastOptions
        {
            Message = "Message",
            OnClose = () => callbackInvoked = true
        });

        service.Remove(toast);

        Assert.True(callbackInvoked);
    }

    [Fact]
    public void Clear_RemovesAllToasts()
    {
        var service = new TabToastService();
        service.Show("Message 1");
        service.Show("Message 2");
        service.Show("Message 3");

        service.Clear();

        Assert.Empty(service.Toasts);
    }

    [Fact]
    public void Clear_InvokesAllOnCloseCallbacks()
    {
        var service = new TabToastService();
        var callbackCount = 0;
        service.Show(new ToastOptions { Message = "1", OnClose = () => callbackCount++ });
        service.Show(new ToastOptions { Message = "2", OnClose = () => callbackCount++ });
        service.Show(new ToastOptions { Message = "3", OnClose = () => callbackCount++ });

        service.Clear();

        Assert.Equal(3, callbackCount);
    }

    [Fact]
    public void OnChange_IsInvoked_WhenToastAdded()
    {
        var service = new TabToastService();
        var changeInvoked = false;
        service.OnChange += () => changeInvoked = true;

        service.Show("Message");

        Assert.True(changeInvoked);
    }

    [Fact]
    public void OnChange_IsInvoked_WhenToastRemoved()
    {
        var service = new TabToastService();
        var toast = service.Show("Message");
        var changeCount = 0;
        service.OnChange += () => changeCount++;

        service.Remove(toast);

        Assert.Equal(1, changeCount);
    }

    [Fact]
    public void OnChange_IsInvoked_WhenCleared()
    {
        var service = new TabToastService();
        service.Show("Message");
        var changeCount = 0;
        service.OnChange += () => changeCount++;

        service.Clear();

        Assert.Equal(1, changeCount);
    }

    [Fact]
    public void ToastOptions_DefaultValues_AreCorrect()
    {
        var options = new ToastOptions();

        Assert.True(options.Closable);
        Assert.Equal(5000, options.AutoCloseDelay);
        Assert.False(options.Translucent);
        Assert.Equal(TabColors.Default, options.Color);
        Assert.Null(options.Title);
        Assert.Null(options.Message);
        Assert.Null(options.Icon);
    }

    [Fact]
    public void ToastInstance_HasUniqueId()
    {
        var service = new TabToastService();
        var toast1 = service.Show("Message 1");
        var toast2 = service.Show("Message 2");

        Assert.NotEqual(toast1.Id, toast2.Id);
    }

    [Fact]
    public void ToastInstance_HasCreatedAtTimestamp()
    {
        var before = DateTime.UtcNow;
        var service = new TabToastService();
        var toast = service.Show("Message");
        var after = DateTime.UtcNow;

        Assert.True(toast.CreatedAt >= before);
        Assert.True(toast.CreatedAt <= after);
    }

    [Fact]
    public void Toasts_ReturnsReadOnlyList()
    {
        var service = new TabToastService();
        service.Show("Message");

        var toasts = service.Toasts;

        Assert.IsAssignableFrom<IReadOnlyList<ToastInstance>>(toasts);
    }

    [Fact]
    public void Remove_NonExistentToast_DoesNotThrow()
    {
        var service = new TabToastService();
        var toast = new ToastInstance(new ToastOptions { Message = "Not added" });

        var exception = Record.Exception(() => service.Remove(toast));

        Assert.Null(exception);
    }

    [Fact]
    public void Remove_NonExistentId_DoesNotThrow()
    {
        var service = new TabToastService();

        var exception = Record.Exception(() => service.Remove("non-existent-id"));

        Assert.Null(exception);
    }
}
