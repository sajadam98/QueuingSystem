public class DoctorBuilder
{
    private readonly Doctor _doctor = new Doctor
    {
        FirstName = "dummy",
        LastName = "dummy",
        NationalCode = "4610530546",
        IsActive = true,
        ProficiencyId = 1,
        Proficiency = new Proficiency
        {
            Id = 1,
        },
        PhoneNumber = "09172338020",
        PatientsPerDay = 2,
    };

    public DoctorBuilder WithFirstName(string firstName)
    {
        _doctor.FirstName = firstName;
        return this;
    }

    public DoctorBuilder WithId(int id)
    {
        _doctor.Id = id;
        return this;
    }

    public DoctorBuilder WithLastName(string lastName)
    {
        _doctor.LastName = lastName;
        return this;
    }

    public DoctorBuilder WithNationalCode(string nationalCode)
    {
        _doctor.NationalCode = nationalCode;
        return this;
    }

    public DoctorBuilder WithPhoneNumber(string mobile)
    {
        _doctor.PhoneNumber = mobile;
        return this;
    }

    public DoctorBuilder WithPatientsPerDay(int patientsPerDay)
    {
        _doctor.PatientsPerDay = patientsPerDay;
        return this;
    }

    public DoctorBuilder WithProficiency(Proficiency proficiency)
    {
        _doctor.Proficiency = proficiency;
        return this;
    }

    public Doctor Build()
    {
        return _doctor;
    }
}