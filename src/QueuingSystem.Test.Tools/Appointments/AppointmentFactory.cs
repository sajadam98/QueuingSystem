public class AppointmentFactory
{
    public static AddAppointmentDto GenerateAddAppointmentDto(int doctorId, int patientId)
    {
        var dto = new AddAppointmentDto
        {
            Date = DateTime.Now,
            DoctorId = doctorId,
            PatientId = patientId,
        };
        return dto;
    }
    
    public static UpdateAppointmentDto GenerateUpdateAppointmentDto(int doctorId, int patientId)
    {
        var dto = new UpdateAppointmentDto
        {
            Date = DateTime.Now,
            DoctorId = doctorId,
            PatientId = patientId,
        };
        return dto;
    }
}