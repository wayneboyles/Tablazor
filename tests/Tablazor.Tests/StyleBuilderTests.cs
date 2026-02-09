using System.Collections.Generic;

namespace Tablazor;

public class StyleBuilderTests
{
    [Fact]
    public void Empty_ReturnsEmptyString()
    {
        var builder = StyleBuilder.Empty();

        Assert.Equal("", builder.Build());
    }

    [Fact]
    public void Default_WithInitialValue_ReturnsValue()
    {
        var builder = StyleBuilder.Default("color: red;");

        Assert.Equal("color: red;", builder.Build());
    }

    [Fact]
    public void Default_WithInitialValueWithoutSemicolon_AddsSemicolon()
    {
        var builder = StyleBuilder.Default("color: red");

        Assert.Equal("color: red;", builder.Build());
    }

    [Fact]
    public void AddStyle_AddsPropertyAndValue()
    {
        var builder = new StyleBuilder()
            .AddStyle("color", "red");

        Assert.Equal("color: red;", builder.Build());
    }

    [Fact]
    public void AddStyle_MultipleStyles_CombinesThem()
    {
        var builder = new StyleBuilder()
            .AddStyle("color", "red")
            .AddStyle("font-size", "14px");

        Assert.Equal("color: red; font-size: 14px;", builder.Build());
    }

    [Fact]
    public void AddStyle_WithNullValue_DoesNotAdd()
    {
        var builder = new StyleBuilder()
            .AddStyle("color", null)
            .AddStyle("font-size", "14px");

        Assert.Equal("font-size: 14px;", builder.Build());
    }

    [Fact]
    public void AddStyle_WithEmptyValue_DoesNotAdd()
    {
        var builder = new StyleBuilder()
            .AddStyle("color", "")
            .AddStyle("font-size", "14px");

        Assert.Equal("font-size: 14px;", builder.Build());
    }

    [Fact]
    public void AddStyle_WithWhitespaceValue_DoesNotAdd()
    {
        var builder = new StyleBuilder()
            .AddStyle("color", "   ")
            .AddStyle("font-size", "14px");

        Assert.Equal("font-size: 14px;", builder.Build());
    }

    [Fact]
    public void AddStyle_ConditionalTrue_AddsStyle()
    {
        var builder = new StyleBuilder()
            .AddStyle("color", "red", when: true);

        Assert.Equal("color: red;", builder.Build());
    }

    [Fact]
    public void AddStyle_ConditionalFalse_DoesNotAddStyle()
    {
        var builder = new StyleBuilder()
            .AddStyle("color", "red", when: false);

        Assert.Equal("", builder.Build());
    }

    [Fact]
    public void AddStyle_NullableConditionalTrue_AddsStyle()
    {
        bool? condition = true;
        var builder = new StyleBuilder()
            .AddStyle("color", "red", when: condition);

        Assert.Equal("color: red;", builder.Build());
    }

    [Fact]
    public void AddStyle_NullableConditionalFalse_DoesNotAddStyle()
    {
        bool? condition = false;
        var builder = new StyleBuilder()
            .AddStyle("color", "red", when: condition);

        Assert.Equal("", builder.Build());
    }

    [Fact]
    public void AddStyle_NullableConditionalNull_DoesNotAddStyle()
    {
        bool? condition = null;
        var builder = new StyleBuilder()
            .AddStyle("color", "red", when: condition);

        Assert.Equal("", builder.Build());
    }

    [Fact]
    public void AddStyle_FuncConditionTrue_AddsStyle()
    {
        var builder = new StyleBuilder()
            .AddStyle("color", "red", when: () => true);

        Assert.Equal("color: red;", builder.Build());
    }

    [Fact]
    public void AddStyle_FuncConditionFalse_DoesNotAddStyle()
    {
        var builder = new StyleBuilder()
            .AddStyle("color", "red", when: () => false);

        Assert.Equal("", builder.Build());
    }

    [Fact]
    public void AddStyle_FuncValue_AddsStyle()
    {
        var builder = new StyleBuilder()
            .AddStyle("color", () => "red");

        Assert.Equal("color: red;", builder.Build());
    }

    [Fact]
    public void AddStyle_FuncValueConditionalFalse_DoesNotAddStyle()
    {
        var builder = new StyleBuilder()
            .AddStyle("color", () => "red", when: false);

        Assert.Equal("", builder.Build());
    }

    [Fact]
    public void AddRaw_AddsRawStyle()
    {
        var builder = new StyleBuilder()
            .AddRaw("color: red; font-size: 14px");

        Assert.Equal("color: red; font-size: 14px;", builder.Build());
    }

    [Fact]
    public void AddRaw_WithSemicolon_DoesNotDuplicate()
    {
        var builder = new StyleBuilder()
            .AddRaw("color: red;");

        Assert.Equal("color: red;", builder.Build());
    }

    [Fact]
    public void AddRaw_ConditionalTrue_AddsStyle()
    {
        var builder = new StyleBuilder()
            .AddRaw("color: red", when: true);

        Assert.Equal("color: red;", builder.Build());
    }

    [Fact]
    public void AddRaw_ConditionalFalse_DoesNotAddStyle()
    {
        var builder = new StyleBuilder()
            .AddRaw("color: red", when: false);

        Assert.Equal("", builder.Build());
    }

    [Fact]
    public void AddRaw_NestedStyleBuilder_CombinesStyles()
    {
        var inner = new StyleBuilder()
            .AddStyle("color", "red");

        var builder = new StyleBuilder()
            .AddStyle("font-size", "14px")
            .AddRaw(inner);

        Assert.Equal("font-size: 14px; color: red;", builder.Build());
    }

    [Fact]
    public void AddStyleFromAttributes_WithStyleAttribute_AddsStyle()
    {
        var attributes = new Dictionary<string, object>
        {
            { "style", "color: red" }
        };

        var builder = new StyleBuilder()
            .AddStyleFromAttributes(attributes);

        Assert.Equal("color: red;", builder.Build());
    }

    [Fact]
    public void AddStyleFromAttributes_WithoutStyleAttribute_DoesNotAddStyle()
    {
        var attributes = new Dictionary<string, object>
        {
            { "class", "my-class" }
        };

        var builder = new StyleBuilder()
            .AddStyleFromAttributes(attributes);

        Assert.Equal("", builder.Build());
    }

    [Fact]
    public void AddStyleFromAttributes_WithNullAttributes_DoesNotThrow()
    {
        var builder = new StyleBuilder()
            .AddStyleFromAttributes(null);

        Assert.Equal("", builder.Build());
    }

    [Fact]
    public void BuildOrNull_WithStyles_ReturnsString()
    {
        var builder = new StyleBuilder()
            .AddStyle("color", "red");

        Assert.Equal("color: red;", builder.BuildOrNull());
    }

    [Fact]
    public void BuildOrNull_WithNoStyles_ReturnsNull()
    {
        var builder = new StyleBuilder();

        Assert.Null(builder.BuildOrNull());
    }

    [Fact]
    public void ToString_ReturnsBuild()
    {
        var builder = new StyleBuilder()
            .AddStyle("color", "red");

        Assert.Equal(builder.Build(), builder.ToString());
    }

    [Fact]
    public void CombinedUsage_BuildsComplexStyle()
    {
        var isVisible = true;
        var hasBackground = false;

        var builder = new StyleBuilder()
            .AddStyle("display", "block", when: isVisible)
            .AddStyle("background-color", "blue", when: hasBackground)
            .AddStyle("color", "red")
            .AddStyle("font-size", "14px");

        Assert.Equal("display: block; color: red; font-size: 14px;", builder.Build());
    }
}
