namespace UKParliament.CodeTest.Data;

// Exposes constraints to other assemblies (eg for validation)
public static class PersonConstraints
{
    public const int FirstName_MaxLength = 100;
    public const int LastName_MaxLength = 100;
    
    public static readonly DateOnly DateOfBirth_Minimum = new DateOnly(1900, 1, 1);
}
