public class DoctorFactory
{
    public static AddDoctorDto GenerateAddDoctorDto(
        Proficiency proficiency)
    {
        var dto = new AddDoctorDto
        {
            FirstName = "Sajad",
            LastName = "Amiri",
            phoneNumber = "09172338020",
            NationalCode = "2420698843",
            PatientPerDay = 5,
            ProficiencyId = proficiency.Id,
        };
        return dto;
    }

    public static Doctor GenerateDoctor(Proficiency proficiency)
    {
        var doctor = new Doctor
        {
            FirstName = "Sajad",
            LastName = "Amiri",
            PhoneNumber = "09172338020",
            NationalCode = "2420698843",
            PatientsPerDay = 5,
            ProficiencyId = proficiency.Id,
        };
        return doctor;
    }

    public static List<Doctor> GenerateDoctorsList(Proficiency proficiency)
    {
        var doctors = new List<Doctor>
        {
            new Doctor
            {
                FirstName = "dummy1",
                LastName = "dummy1",
                PhoneNumber = "09172338020",
                NationalCode = "dummy1",
                PatientsPerDay = 5,
                ProficiencyId = proficiency.Id,
            },
            new Doctor
            {
                FirstName = "dummy2",
                LastName = "dummy2",
                PhoneNumber = "09172338020",
                NationalCode = "dummy2",
                PatientsPerDay = 4,
                ProficiencyId = proficiency.Id,
            },
            new Doctor
            {
                FirstName = "dummy3",
                LastName = "dummy3",
                PhoneNumber = "09172338020",
                NationalCode = "dummy3",
                PatientsPerDay = 3,
                ProficiencyId = proficiency.Id,
            },
        };
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