namespace RozeCare.Domain.ValueObjects;

public sealed class CountryCode : IEquatable<CountryCode>
{
    public string Value { get; }

    private CountryCode(string value)
    {
        Value = value;
    }

    public static CountryCode Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Country code is required.", nameof(value));
        }

        var normalized = value.Trim().ToUpperInvariant();
        if (normalized.Length is < 2 or > 3)
        {
            throw new ArgumentException("Country code must contain 2 or 3 characters.", nameof(value));
        }

        return new CountryCode(normalized);
    }

    public override string ToString() => Value;

    public override bool Equals(object? obj) => obj is CountryCode other && Equals(other);

    public bool Equals(CountryCode? other) => other is not null && Value == other.Value;

    public override int GetHashCode() => Value.GetHashCode(StringComparison.Ordinal);

    public static implicit operator string(CountryCode code) => code.Value;
}
