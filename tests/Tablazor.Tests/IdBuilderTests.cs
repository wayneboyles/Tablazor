using System;
using System.Linq;
using System.Threading.Tasks;

namespace Tablazor;

public class IdBuilderTests : IDisposable
{
    public IdBuilderTests()
    {
        // Reset counter before each test for predictable results
        IdBuilder.Reset();
    }

    public void Dispose()
    {
        // Reset counter after each test to avoid affecting other tests
        IdBuilder.Reset();
    }

    [Fact]
    public void Next_ReturnsIdWithDefaultPrefix()
    {
        var id = IdBuilder.Next();

        Assert.StartsWith("tab-", id);
    }

    [Fact]
    public void Next_ReturnsIncrementingIds()
    {
        var id1 = IdBuilder.Next();
        var id2 = IdBuilder.Next();
        var id3 = IdBuilder.Next();

        Assert.Equal("tab-1", id1);
        Assert.Equal("tab-2", id2);
        Assert.Equal("tab-3", id3);
    }

    [Fact]
    public void Next_WithCustomPrefix_ReturnsIdWithCustomPrefix()
    {
        var id = IdBuilder.Next("custom");

        Assert.StartsWith("custom-", id);
    }

    [Fact]
    public void Next_WithCustomPrefix_ReturnsIncrementingIds()
    {
        var id1 = IdBuilder.Next("btn");
        var id2 = IdBuilder.Next("btn");

        Assert.Equal("btn-1", id1);
        Assert.Equal("btn-2", id2);
    }

    [Fact]
    public void NextCompact_ReturnsIdWithDefaultPrefix()
    {
        var id = IdBuilder.NextCompact();

        Assert.StartsWith("tab-", id);
    }

    [Fact]
    public void NextCompact_ReturnsBase62EncodedIds()
    {
        var id = IdBuilder.NextCompact();

        // First ID after reset should be "tab-1"
        Assert.Equal("tab-1", id);
    }

    [Fact]
    public void NextCompact_ProducesValidBase62Characters()
    {
        // Generate several IDs
        for (int i = 0; i < 100; i++)
        {
            var id = IdBuilder.NextCompact();
            var suffix = id.Substring(4); // Remove "tab-" prefix

            // All characters should be valid Base62 (0-9, A-Z, a-z)
            Assert.All(suffix, c =>
                Assert.True(char.IsLetterOrDigit(c), $"Invalid character '{c}' in ID '{id}'"));
        }
    }

    [Fact]
    public void NextForType_ReturnsIdWithTypeNamePrefix()
    {
        var id = IdBuilder.NextForType(typeof(string));

        Assert.StartsWith("string-", id);
    }

    [Fact]
    public void NextForType_UsesLowercaseTypeName()
    {
        var id = IdBuilder.NextForType(typeof(IdBuilderTests));

        Assert.StartsWith("idbuildertests-", id);
    }

    [Fact]
    public void NextForType_ReturnsIncrementingIds()
    {
        var id1 = IdBuilder.NextForType(typeof(int));
        var id2 = IdBuilder.NextForType(typeof(int));

        Assert.Equal("int32-1", id1);
        Assert.Equal("int32-2", id2);
    }

    [Fact]
    public void Reset_ResetsCounterToZero()
    {
        IdBuilder.Next();
        IdBuilder.Next();
        IdBuilder.Next();

        IdBuilder.Reset();

        Assert.Equal(0, IdBuilder.CurrentCount);
    }

    [Fact]
    public void Reset_NextIdStartsFromOne()
    {
        IdBuilder.Next();
        IdBuilder.Next();

        IdBuilder.Reset();

        var id = IdBuilder.Next();
        Assert.Equal("tab-1", id);
    }

    [Fact]
    public void CurrentCount_ReturnsCurrentCounterValue()
    {
        Assert.Equal(0, IdBuilder.CurrentCount);

        IdBuilder.Next();
        Assert.Equal(1, IdBuilder.CurrentCount);

        IdBuilder.Next();
        Assert.Equal(2, IdBuilder.CurrentCount);
    }

    [Fact]
    public void CurrentCount_DoesNotIncrementCounter()
    {
        IdBuilder.Next();

        var count1 = IdBuilder.CurrentCount;
        var count2 = IdBuilder.CurrentCount;
        var count3 = IdBuilder.CurrentCount;

        Assert.Equal(1, count1);
        Assert.Equal(1, count2);
        Assert.Equal(1, count3);
    }

    [Fact]
    public void AllMethods_ShareSameCounter()
    {
        var id1 = IdBuilder.Next();
        var id2 = IdBuilder.NextCompact();
        var id3 = IdBuilder.Next("custom");
        var id4 = IdBuilder.NextForType(typeof(string));

        Assert.Equal("tab-1", id1);
        Assert.Equal("tab-2", id2);
        Assert.Equal("custom-3", id3);
        Assert.Equal("string-4", id4);
    }

    [Fact]
    public async Task Next_IsThreadSafe_ProducesUniqueIds()
    {
        IdBuilder.Reset();
        const int threadCount = 10;
        const int idsPerThread = 100;
        var allIds = new System.Collections.Concurrent.ConcurrentBag<string>();

        var tasks = Enumerable.Range(0, threadCount)
            .Select(_ => Task.Run(() =>
            {
                for (int i = 0; i < idsPerThread; i++)
                {
                    allIds.Add(IdBuilder.Next());
                }
            }))
            .ToArray();

        await Task.WhenAll(tasks);

        // All IDs should be unique
        Assert.Equal(threadCount * idsPerThread, allIds.Distinct().Count());
    }

    [Fact]
    public async Task NextCompact_IsThreadSafe_ProducesUniqueIds()
    {
        IdBuilder.Reset();
        const int threadCount = 10;
        const int idsPerThread = 100;
        var allIds = new System.Collections.Concurrent.ConcurrentBag<string>();

        var tasks = Enumerable.Range(0, threadCount)
            .Select(_ => Task.Run(() =>
            {
                for (int i = 0; i < idsPerThread; i++)
                {
                    allIds.Add(IdBuilder.NextCompact());
                }
            }))
            .ToArray();

        await Task.WhenAll(tasks);

        // All IDs should be unique
        Assert.Equal(threadCount * idsPerThread, allIds.Distinct().Count());
    }

    [Fact]
    public void NextCompact_LargeNumbers_ProducesValidBase62()
    {
        // Generate many IDs to get larger numbers
        for (int i = 0; i < 1000; i++)
        {
            IdBuilder.Next();
        }

        var id = IdBuilder.NextCompact();
        var suffix = id.Substring(4); // Remove "tab-" prefix

        // Should have multiple characters for larger numbers
        Assert.True(suffix.Length >= 2);

        // All characters should be valid Base62
        Assert.All(suffix, c =>
            Assert.True(char.IsLetterOrDigit(c), $"Invalid character '{c}' in ID '{id}'"));
    }
}
