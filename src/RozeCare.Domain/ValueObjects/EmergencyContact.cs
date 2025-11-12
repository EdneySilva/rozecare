namespace RozeCare.Domain.ValueObjects;

public sealed class EmergencyContact
{
    public string Name { get; }

    public PhoneNumber Phone { get; }

    public string Relationship { get; }

    private EmergencyContact(string name, PhoneNumber phone, string relationship)
    {
        Name = name;
        Phone = phone;
        Relationship = relationship;
    }

    public static EmergencyContact Create(string name, string phone, string relationship)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name is required.", nameof(name));
        }

        if (string.IsNullOrWhiteSpace(relationship))
        {
            throw new ArgumentException("Relationship is required.", nameof(relationship));
        }

        return new EmergencyContact(name.Trim(), PhoneNumber.Create(phone), relationship.Trim());
    }
}
