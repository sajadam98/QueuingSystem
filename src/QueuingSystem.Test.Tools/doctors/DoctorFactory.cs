public class DoctorFactory
{
    public static AddDoctorDto GenerateAddDoctorDto(
        Proficiency proficiency)
    {
        var dto = new AddDoctorDto
        {
            FirstName = "dummy",
            LastName = "dummy",
            phoneNumber = "dummy",
            NationalCode = "dummy",
            PatientPerDay = 5,
            ProficiencyId = proficiency.Id,
        };
        return dto;
    }

    public static Doctor GenerateDoctor(Proficiency proficiency,
        string nationalCode)
    {
        var doctor = new DoctorBuilder().WithProficiency(proficiency)
            .WithNationalCode(nationalCode);
        return doctor.Build();
    }

    public static List<Doctor> GenerateDoctorsList(Proficiency proficiency)
    {
        var doctors = new List<Doctor>();
        doctors.Add(
            DoctorFactory.GenerateDoctor(proficiency, "dummy1"));
        doctors.Add(
            DoctorFactory.GenerateDoctor(proficiency, "dummy2"));
        doctors.Add(
            DoctorFactory.GenerateDoctor(proficiency, "dummy3"));
        return doctors;
    }

    public static UpdateDoctorDto GenerateUpdateDoctorDto(
        Proficiency proficiency)
    {
        var dto = new UpdateDoctorDto
        {
            FirstName = "dummy",
            LastName = "dummy",
            phoneNumber = "09161234567",
            NationalCode = "dummy",
            PatientPerDay = 4,
            ProficiencyId = proficiency.Id,
        };
        return dto;
    }
}