using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class DoctorEntityMap : IEntityTypeConfiguration<Doctor>
{
    public void Configure(EntityTypeBuilder<Doctor> _)
    {
        _.ToTable("Doctors");
        _.HasKey(p => p.Id);
        _.Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
        _.Property(p => p.NationalCode).IsRequired().HasMaxLength(10);
        _.HasIndex(p => p.NationalCode).IsUnique();
        _.Property(p => p.IsActive).HasDefaultValue(true);
        _.Property(p => p.FirstName).IsRequired().HasMaxLength(50);
        _.Property(p => p.LastName).IsRequired().HasMaxLength(70);
        _.Property(p => p.PhoneNumber).IsRequired().HasMaxLength(11);
        _.Property(p => p.PatientsPerDay).IsRequired();
        _.HasOne(p => p.Proficiency)
            .WithMany(p => p.Doctors)
            .HasForeignKey(p => p.ProficiencyId);
        _.HasMany(p => p.Appointments)
            .WithOne(p => p.Doctor)
            .HasForeignKey(p => p.DoctorId)
            .OnDelete(DeleteBehavior.ClientNoAction);
    }
}