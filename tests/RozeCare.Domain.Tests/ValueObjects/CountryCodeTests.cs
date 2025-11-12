using FluentAssertions;
using RozeCare.Domain.ValueObjects;
using Xunit;

namespace RozeCare.Domain.Tests.ValueObjects;

public class CountryCodeTests
{
    [Theory]
    [InlineData("br", "BR")]
    [InlineData(" Usa ", "USA")]
    public void Create_Should_Normalize_Input(string input, string expected)
    {
        var code = CountryCode.Create(input);
        code.Value.Should().Be(expected);
    }

    [Theory]
    [InlineData("")]
    [InlineData("a")]
    [InlineData("toolong")]
    public void Create_Should_Throw_When_Invalid(string input)
    {
        var action = () => CountryCode.Create(input);
        action.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Equality_Should_Be_Based_On_Value()
    {
        var first = CountryCode.Create("pt");
        var second = CountryCode.Create("PT");

        first.Should().Be(second);
    }
}
