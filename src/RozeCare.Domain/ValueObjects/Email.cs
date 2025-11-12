using System.Text.RegularExpressions;

namespace RozeCare.Domain.ValueObjects;

public sealed class Email : IEquatable<Email>
{
    public static readonly Regex Pattern = new("^[^@\\s]+@[^@\\s]+\\.[^@\\s]+$", RegexOptions.Compiled);

    public string Value { get; }

    private Email(string value)
    {
        Value = value;
    }

    public static Email Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Email cannot be empty.", nameof(value));
        }

        if (!Pattern.IsMatch(value))
        {
            throw new ArgumentException("Invalid email format.", nameof(value));
        }

        return new Email(value.Trim().ToLowerInvariant());
    }

    public override string ToString() => Value;

    public override int GetHashCode() => Value.GetHashCode(StringComparison.Ordinal);

    public override bool Equals(object? obj) => obj is Email other && Equals(other);

    public bool Equals(Email? other) => other is not null && Value == other.Value;

    public static implicit operator string(Email email) => email.Value;
}
