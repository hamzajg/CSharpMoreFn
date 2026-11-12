using System;
using CSharpMoreFn.Functional;
using Xunit;

namespace CSharpMoreFn.Tests
{
    public class FunctionalExtensionsTests
    {
        [Fact]
        public void Pipe_PassesValueThroughTransformations()
        {
            var result = 5
                .Pipe(x => x * 2)
                .Pipe(x => x + 3)
                .Pipe(x => $"Value is {x}");

            Assert.Equal("Value is 13", result);
        }

        [Fact]
        public void Compose_CombinesFunctionsRightToLeft()
        {
            Func<int, int> doubleIt = x => x * 2;
            Func<int, int> addFive = x => x + 5;

            var doubleThenAdd = addFive.Compose(doubleIt);
            Assert.Equal(15, doubleThenAdd(5)); // (5 * 2) + 5 = 15
        }

        [Fact]
        public void Curry_SplitsArguments()
        {
            Func<int, int, int, int> volume = (w, h, d) => w * h * d;
            var curried = volume.Curry();

            var areaWithWidthTen = curried(10);
            var areaWithHeightTwo = areaWithWidthTen(2);
            var totalVolume = areaWithHeightTwo(3);

            Assert.Equal(60, totalVolume);
        }
    }
}
