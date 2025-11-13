using FluentAssertions;
using RozeCare.Domain.ValueObjects;
using System;
using Xunit;

namespace RozeCare.Domain.Tests.ValueObjects;

public class PhoneNumberTests
{
    [Theory]
    [InlineData("+1 555-123-4567", "+1 555-123-4567")]
    [InlineData("  0555123456  ", "0555123456")]
    public void Create_Should_Return_Normalized_Value(string input, string expected)
    {
        var number = PhoneNumber.Create(input);
        number.Value.Should().Be(expected);
    }

    [Theory]
    [InlineData("")]
    [InlineData("abc")]
    [InlineData("123")]
    public void Create_Should_Throw_When_Invalid(string input)
    {
        var action = () => PhoneNumber.Create(input);
        action.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Equality_Should_Be_Based_On_Value()
    {
        var first = PhoneNumber.Create("+15551234567");
        var second = PhoneNumber.Create("+15551234567");

        first.Should().Be(second);
    }
}
