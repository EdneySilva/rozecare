using System.Text.RegularExpressions;

namespace RozeCare.Domain.ValueObjects;

public sealed class PhoneNumber : IEquatable<PhoneNumber>
{
    private static readonly Regex Pattern = new("^[+0-9][0-9\\-\\s]{7,18}$", RegexOptions.Compiled);

    public string Value { get; }

    private PhoneNumber(string value)
    {
        Value = value;
    }

    public static PhoneNumber Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Phone number is required.", nameof(value));
        }

        var normalized = value.Trim();
        if (!Pattern.IsMatch(normalized))
        {
            throw new ArgumentException("Invalid phone number format.", nameof(value));
        }

        return new PhoneNumber(normalized);
    }

    public override string ToString() => Value;

    public override int GetHashCode() => Value.GetHashCode(StringComparison.Ordinal);

    public override bool Equals(object? obj) => obj is PhoneNumber other && Equals(other);

    public bool Equals(PhoneNumber? other) => other is not null && Value == other.Value;

    public static implicit operator string(PhoneNumber number) => number.Value;
}
