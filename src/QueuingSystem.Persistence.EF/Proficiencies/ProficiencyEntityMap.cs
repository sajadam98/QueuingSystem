
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ProficiencyEntityMap : IEntityTypeConfiguration<Proficiency>
{
    public void Configure(EntityTypeBuilder<Proficiency> _)
    {
        _.ToTable("Proficiencies");
        _.HasKey(p => p.Id);
        _.Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
        _.Property(p => p.Name).IsRequired().HasMaxLength(50);
        _.HasMany(p => p.Doctors).WithOne(p => p.Proficiency)
            .HasForeignKey(p => p.ProficiencyId);
    }
}