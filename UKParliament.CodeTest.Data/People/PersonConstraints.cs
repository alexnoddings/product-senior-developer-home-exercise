namespace UKParliament.CodeTest.Data;

// Exposes constraints to other assemblies (eg for validation)
public static class PersonConstraints
{
    // Max lengths are based on assumptions
    public const int FirstName_MaxLength = 100;
    public const int LastName_MaxLength = 100;
    public const int Email_MaxLength = 100;
    
    public static readonly DateOnly DateOfBirth_Minimum = new DateOnly(1900, 1, 1);
    // Recomputed every time as a property getter rather than being stored as a field so that it's updated every day
    public static DateOnly DateOfBirth_Maximum => DateOnly.FromDateTime(DateTime.UtcNow.Date);
}
