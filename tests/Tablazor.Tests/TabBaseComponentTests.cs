using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

namespace Tablazor;

public class TabBaseComponentTests : BunitContext
{
    /// <summary>
    /// Concrete test component for testing TabBaseComponent functionality
    /// </summary>
    private class TestComponent : TabBaseComponent
    {
        [Parameter]
        public string? TestCssClass { get; set; }

        public new string GetId() => base.GetId();

        public new string GetCssClass() => base.GetCssClass();

        protected override string BuildCssClass()
        {
            return TestCssClass ?? "test-class";
        }
    }

    public TabBaseComponentTests()
    {
        Services.AddLogging();
    }

    [Fact]
    public void GetId_ReturnsCustomId_WhenProvidedInAdditionalAttributes()
    {
        var component = new TestComponent
        {
            AdditionalAttributes = new Dictionary<string, object> { { "id", "custom-id" } }
        };

        var id = component.GetId();

        Assert.Equal("custom-id", id);
    }

    [Fact]
    public void GetId_GeneratesId_WhenNoIdProvided()
    {
        var component = new TestComponent();

        var id = component.GetId();

        Assert.StartsWith("tab-", id);
    }

    [Fact]
    public void GetId_GeneratesId_WhenIdIsEmpty()
    {
        var component = new TestComponent
        {
            AdditionalAttributes = new Dictionary<string, object> { { "id", "" } }
        };

        var id = component.GetId();

        Assert.StartsWith("tab-", id);
    }

    [Fact]
    public void GetId_GeneratesId_WhenIdIsWhitespace()
    {
        var component = new TestComponent
        {
            AdditionalAttributes = new Dictionary<string, object> { { "id", "   " } }
        };

        var id = component.GetId();

        Assert.StartsWith("tab-", id);
    }

    [Fact]
    public void GetCssClass_ReturnsBuildCssClassResult_WhenNoAdditionalClass()
    {
        var component = new TestComponent
        {
            TestCssClass = "my-class"
        };

        var cssClass = component.GetCssClass();

        Assert.Equal("my-class", cssClass);
    }

    [Fact]
    public void GetCssClass_CombinesWithAdditionalClass_WhenClassAttributeProvided()
    {
        var component = new TestComponent
        {
            TestCssClass = "base-class",
            AdditionalAttributes = new Dictionary<string, object> { { "class", "extra-class" } }
        };

        var cssClass = component.GetCssClass();

        Assert.Equal("base-class extra-class", cssClass);
    }

    [Fact]
    public void GetCssClass_IgnoresEmptyAdditionalClass()
    {
        var component = new TestComponent
        {
            TestCssClass = "base-class",
            AdditionalAttributes = new Dictionary<string, object> { { "class", "" } }
        };

        var cssClass = component.GetCssClass();

        Assert.Equal("base-class", cssClass);
    }

    [Fact]
    public void Visible_DefaultsToTrue()
    {
        var component = new TestComponent();

        Assert.True(component.Visible);
    }

    [Fact]
    public void Disabled_DefaultsToFalse()
    {
        var component = new TestComponent();

        Assert.False(component.Disabled);
    }
}