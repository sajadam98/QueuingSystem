
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class PatientEntityMap : IEntityTypeConfiguration<Patient>
{
    public void Configure(EntityTypeBuilder<Patient> _)
    {
        _.ToTable("Patients");
        _.HasKey(p => p.Id);
        _.Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
        _.Property(p => p.NationalCode).IsRequired().HasMaxLength(10);
        _.HasIndex(p => p.NationalCode).IsUnique();
        _.Property(p => p.IsActive).HasDefaultValue(true);
        _.Property(p => p.PhoneNumber).IsRequired().HasMaxLength(11);
        _.Property(p => p.FirstName).IsRequired().HasMaxLength(50);
        _.Property(p => p.LastName).IsRequired().HasMaxLength(70);
        _.HasMany(p => p.Appointments).WithOne(p => p.Patient)
            .HasForeignKey(p => p.PatientId)
            .OnDelete(DeleteBehavior.ClientNoAction);
    }
}