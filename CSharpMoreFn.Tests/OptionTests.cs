using System;
using CSharpMoreFn.Functional;
using Xunit;

namespace CSharpMoreFn.Tests
{
    public class OptionTests
    {
        [Fact]
        public void Some_CreatesOptionWithValue()
        {
            var opt = Option<int>.Some(42);
            Assert.True(opt.IsSome);
            Assert.False(opt.IsNone);
            Assert.Equal(42, opt.GetOrElse(0));
            Assert.Equal("Some(42)", opt.ToString());
        }

        [Fact]
        public void None_CreatesEmptyOption()
        {
            var opt = Option<string>.None;
            Assert.False(opt.IsSome);
            Assert.True(opt.IsNone);
            Assert.Equal("fallback", opt.GetOrElse("fallback"));
            Assert.Equal("None", opt.ToString());
        }

        [Fact]
        public void Some_ThrowsWhenNullPassed()
        {
            Assert.Throws<ArgumentNullException>(() => Option<string>.Some(null!));
        }

        [Fact]
        public void Map_TransformsValueWhenSome()
        {
            var opt = Option<int>.Some(5);
            var mapped = opt.Map(x => x * 2);
            Assert.Equal(10, mapped.GetOrElse(0));
        }

        [Fact]
        public void Map_ReturnsNoneWhenNone()
        {
            var opt = Option<int>.None;
            var mapped = opt.Map(x => x * 2);
            Assert.True(mapped.IsNone);
        }

        [Fact]
        public void Bind_ChainsOptions()
        {
            Option<int> Half(int n) => n % 2 == 0 ? Option<int>.Some(n / 2) : Option<int>.None;

            var result1 = Option<int>.Some(8).Bind(Half);
            Assert.Equal(4, result1.GetOrElse(0));

            var result2 = Option<int>.Some(7).Bind(Half);
            Assert.True(result2.IsNone);
        }

        [Fact]
        public void Match_InvokesCorrectBranch()
        {
            var some = Option<string>.Some("Antigravity");
            var result = some.Match(
                onSome: v => $"Hello {v}",
                onNone: () => "Nobody"
            );
            Assert.Equal("Hello Antigravity", result);

            var none = Option<string>.None;
            var noneResult = none.Match(
                onSome: v => $"Hello {v}",
                onNone: () => "Nobody"
            );
            Assert.Equal("Nobody", noneResult);
        }

        [Fact]
        public void Where_FiltersValues()
        {
            var some = Option<int>.Some(42);
            var filteredOut = some.Where(x => x > 50);
            var kept = some.Where(x => x > 20);

            Assert.True(filteredOut.IsNone);
            Assert.True(kept.IsSome);
            Assert.Equal(42, kept.GetOrElse(0));
        }

        [Fact]
        public void Equals_ComparesByValue()
        {
            Assert.Equal(Option<int>.Some(10), Option<int>.Some(10));
            Assert.NotEqual(Option<int>.Some(10), Option<int>.Some(20));
            Assert.Equal(Option<int>.None, Option<int>.None);
        }
    }
}
