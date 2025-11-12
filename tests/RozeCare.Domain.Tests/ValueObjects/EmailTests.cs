using FluentAssertions;
using RozeCare.Domain.ValueObjects;
using Xunit;

namespace RozeCare.Domain.Tests.ValueObjects;

public class EmailTests
{
    [Fact]
    public void Create_Should_Normalize_Email()
    {
        var email = Email.Create("User@Example.com");
        email.Value.Should().Be("user@example.com");
    }

    [Fact]
    public void Create_Should_Throw_When_Invalid()
    {
        var action = () => Email.Create("invalid");
        action.Should().Throw<ArgumentException>();
    }
}
