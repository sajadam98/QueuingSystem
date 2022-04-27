public class AppointmentFactory
{
    public static AddAppointmentDto GenerateAddAppointmentDto()
    {
        var dto = new AddAppointmentDto
        {
            Date = DateTime.Now,
            DoctorId = 1,
            PatientId = 1,
        };

        return dto;
    }

    public static Task<IList<GetAppointmentDto>>
        GenerateGetAppointmentDtoList()
    {
        var appointments = new List<GetAppointmentDto>
        {
            new GetAppointmentDto
            {
                Id = 1,
                Date = DateTime.Now.AddDays(-1),
                Doctor = "dummy1",
                Patient = "dummy1",
            },
            new GetAppointmentDto
            {
                Id = 2,
                Date = DateTime.Now.AddDays(-2),
                Doctor = "dummy2",
                Patient = "dummy2",
            },
            new GetAppointmentDto
            {
                Id = 3,
                Date = DateTime.Now.AddDays(-3),
                Doctor = "dummy3",
                Patient = "dummy3",
            },
            new GetAppointmentDto
            {
                Id = 4,
                Date = DateTime.Now.AddDays(-4),
                Doctor = "dummy4",
                Patient = "dummy4",
            },
            new GetAppointmentDto
            {
                Id = 5,
                Date = DateTime.Now.AddDays(-5),
                Doctor = "dummy5",
                Patient = "dummy5",
            },
        };
        return Task.FromResult<IList<GetAppointmentDto>>(appointments);
    }
}