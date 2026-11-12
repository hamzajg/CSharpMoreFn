using System;
using CSharpMoreFn.Functional;
using Xunit;

namespace CSharpMoreFn.Tests
{
    public class ResultTests
    {
        [Fact]
        public void Ok_CreatesSuccessfulResult()
        {
            var res = Result<int, string>.Ok(100);
            Assert.True(res.IsSuccess);
            Assert.False(res.IsFailure);
            Assert.Equal("Ok(100)", res.ToString());
        }

        [Fact]
        public void Fail_CreatesFailureResult()
        {
            var res = Result<int, string>.Fail("Not found");
            Assert.False(res.IsSuccess);
            Assert.True(res.IsFailure);
            Assert.Equal("Fail(Not found)", res.ToString());
        }

        [Fact]
        public void Map_TransformsSuccessValueOnly()
        {
            var ok = Result<int, string>.Ok(5);
            var mappedOk = ok.Map(x => x * 10);
            Assert.Equal(50, mappedOk.ValueOr(_ => 0));

            var fail = Result<int, string>.Fail("error");
            var mappedFail = fail.Map(x => x * 10);
            Assert.True(mappedFail.IsFailure);
        }

        [Fact]
        public void Bind_ChainsResultsInRailway()
        {
            Result<int, string> ParsePositive(string input)
            {
                if (int.TryParse(input, out var n) && n > 0)
                    return Result<int, string>.Ok(n);
                return Result<int, string>.Fail("Must be a positive integer");
            }

            Result<double, string> SquareRoot(int n)
            {
                return Result<double, string>.Ok(Math.Sqrt(n));
            }

            var success = ParsePositive("16").Bind(SquareRoot);
            Assert.True(success.IsSuccess);
            Assert.Equal(4.0, success.ValueOr(_ => 0.0));

            var fail1 = ParsePositive("-5").Bind(SquareRoot);
            Assert.True(fail1.IsFailure);

            var fail2 = ParsePositive("abc").Bind(SquareRoot);
            Assert.True(fail2.IsFailure);
        }

        [Fact]
        public void Match_HandlesBothCases()
        {
            var ok = Result<string, int>.Ok("Passed");
            var okMsg = ok.Match(
                onSuccess: v => $"Result: {v}",
                onFailure: err => $"Error code: {err}"
            );
            Assert.Equal("Result: Passed", okMsg);

            var fail = Result<string, int>.Fail(404);
            var failMsg = fail.Match(
                onSuccess: v => $"Result: {v}",
                onFailure: err => $"Error code: {err}"
            );
            Assert.Equal("Error code: 404", failMsg);
        }

        [Fact]
        public void Tap_ExecutesSideEffectOnlyOnSuccess()
        {
            var tapped = false;
            Result<int, string>.Ok(42).Tap(_ => tapped = true);
            Assert.True(tapped);

            var errorTapped = false;
            Result<int, string>.Fail("err").Tap(_ => errorTapped = true);
            Assert.False(errorTapped);
        }
    }
}
