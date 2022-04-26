
using FluentMigrator;

[Migration(202204261300)]
public class _202204261300_InitialTables : Migration
{
    public override void Up()
    {
        CreateProficienciesTable();
        CreateDoctorsTable();
        CreatePatientsTable();
        CreateAppintmentTable();
    }

    public override void Down()
    {
        Delete.ForeignKey("FK_Doctors_Proficiencies");
        Delete.ForeignKey("FK_Appintments_Patients");
        Delete.ForeignKey("FK_Appintments_Doctors");
        Delete.Table("Doctors");
        Delete.Table("Proficiencies");
        Delete.Table("Patients");
        Delete.Table("Appintments");
    }

    void CreateDoctorsTable()
    {
        Create.Table("Doctors").WithColumn("Id").AsInt32().PrimaryKey().Identity()
            .WithColumn("FirstName").AsString(50).NotNullable()
            .WithColumn("IsActive").AsBoolean().WithDefaultValue(true)
            .WithColumn("LastName").AsString(70).NotNullable()
            .WithColumn("NationalCode").AsString(10).NotNullable().Unique()
            .WithColumn("PhoneNumber").AsString(11).NotNullable()
            .WithColumn("PatientsPerDay").AsInt32().NotNullable()
            .WithColumn("ProficiencyId").AsInt32().NotNullable()
            .ForeignKey("FK_Doctors_Proficiencies", "Proficiencies", "Id");
    }

    void CreateProficienciesTable()
    {
        Create.Table("Proficiencies").WithColumn("Id").AsInt32().PrimaryKey().Identity()
            .WithColumn("Name").AsString(50).NotNullable();
    }

    void CreatePatientsTable()
    {
        Create.Table("Patients").WithColumn("Id").AsInt32().PrimaryKey().Identity()
            .WithColumn("NationalCode").AsString(10).Unique()
            .WithColumn("IsActive").AsBoolean().WithDefaultValue(true)
            .WithColumn("FirstName").AsString(50).NotNullable()
            .WithColumn("LastName").AsString(70).NotNullable()
            .WithColumn("PhoneNumber").AsString(11).NotNullable();
    }

    void CreateAppintmentTable()
    {
        Create.Table("Appointments").WithColumn("Id").AsInt32().PrimaryKey().Identity()
            .WithColumn("Date").AsDateTime().NotNullable()
            .WithColumn("PatientId").AsInt32().NotNullable()
            .ForeignKey("FK_Appintments_Patients", "Patients", "Id")
            .WithColumn("DoctorId").AsInt32().NotNullable()
            .ForeignKey("FK_Appintments_Doctors", "Doctors", "Id");
    }
}