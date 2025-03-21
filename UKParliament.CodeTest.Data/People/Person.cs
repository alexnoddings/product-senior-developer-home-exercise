﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace UKParliament.CodeTest.Data;

public class Person
{
    public int Id { get; set; }

    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateOnly DateOfBirth { get; set; }
    
    public int DepartmentId { get; set; }
    public Department Department { get; set; } = null!;
}

public class PersonEntityTypeConfiguration : IEntityTypeConfiguration<Person>
{
    public void Configure(EntityTypeBuilder<Person> builder)
    {
        builder.HasKey(x => x.Id);

        builder
            .Property(x => x.FirstName)
            .IsRequired()
            // Max length is just an assumption
            .HasMaxLength(PersonConstraints.FirstName_MaxLength);
        
        builder
            .Property(x => x.LastName)
            .IsRequired()
            .HasMaxLength(PersonConstraints.LastName_MaxLength);
        
        builder
            .Property(x => x.Email)
            .IsRequired()
            .HasMaxLength(PersonConstraints.Email_MaxLength);
        
        builder
            .HasOne(x => x.Department)
            .WithMany(x => x.People)
            .HasForeignKey(x => x.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
        
        builder.HasData(
            new Person { Id = 1, DepartmentId = 1, FirstName = "Gordon", LastName = "Freeman", Email = "gfreeman@bm.com", DateOfBirth = DateOnly.Parse("1990-03-16") },
            new Person { Id = 2, DepartmentId = 1, FirstName = "Daniel", LastName = "Jackson", Email = "danjackson@sgc.gov", DateOfBirth = DateOnly.Parse("1990-03-16") },
            new Person { Id = 3, DepartmentId = 2, FirstName = "Molly", LastName = "Rankin", Email = "mrankin@aol.com", DateOfBirth = DateOnly.Parse("1990-03-16") },
            new Person { Id = 4, DepartmentId = 2, FirstName = "Stu", LastName = "Mackenzie", Email = "stumac@yahoomail.com", DateOfBirth = DateOnly.Parse("1990-03-16") },
            new Person { Id = 5, DepartmentId = 2, FirstName = "Liz", LastName = "Stokes", Email = "thebeth@hotmail.co.nz", DateOfBirth = DateOnly.Parse("1990-03-16") }
        );
    }
}
