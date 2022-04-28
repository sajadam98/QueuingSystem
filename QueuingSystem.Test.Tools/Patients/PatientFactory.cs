public class PatientFactory
{
    public static AddPatientDto GenerateAddPatientDto(string attributes)
    {
        var dto = new AddPatientDto
        {
            FirstName = attributes,
            LastName = attributes,
            NationalCode = attributes,
            PhoneNumber = attributes,
        };
        return dto;
    }

    public static Task<IList<GetPatientDto>> GenerateGetPatientDtoList()
    {
        var patients = new List<GetPatientDto>
        {
            new GetPatientDto
            {
                FirstName = "dummy1",
                LastName = "dummy1",
                NationalCode = "dummy1",
                PhoneNumber = "dummy1",
            },
            new GetPatientDto
            {
                FirstName = "dummy2",
                LastName = "dummy2",
                NationalCode = "dummy2",
                PhoneNumber = "dummy2",
            },
            new GetPatientDto
            {
                FirstName = "dummy3",
                LastName = "dummy3",
                NationalCode = "dummy3",
                PhoneNumber = "dummy3",
            },
        };
        return Task.FromResult<IList<GetPatientDto>>(patients);
    }

    public static Patient GeneratePatient(string attributes)
    {
        var patient = new Patient
        {
            FirstName = attributes,
            LastName = attributes,
            NationalCode = attributes,
            PhoneNumber = attributes,
        };
        return patient;
    }

    public static UpdatePatientDto GenerateUpdatePatientDto(string attributes)
    {
        var dto = new UpdatePatientDto
        {
            FirstName = attributes,
            LastName = attributes,
            NationalCode = attributes,
            PhoneNumber = attributes,
        };
        return dto;
    }
}