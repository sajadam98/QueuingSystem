using Microsoft.EntityFrameworkCore;

public class EFAppointmentRepository : AppointmentRepository
{
    private readonly DbSet<Appointment> _appointments;

    public EFAppointmentRepository(EFDataContext dbContext)
    {
        _appointments = dbContext.Set<Appointment>();
    }

    public void Add(Appointment appointment)
    {
        _appointments.Add(appointment);
    }

    public async Task<IList<GetAppointmentDto>> GetAll()
    {
        return await _appointments.Select(_ => new GetAppointmentDto
        {
            Id = _.Id,
            Date = _.Date.Date,
            DoctorsFirstName = _.Doctor.FirstName,
            DoctorsLastName = _.Doctor.LastName,
            PatientFirstName = _.Patient.FirstName,
            PatientLastName = _.Patient.LastName
        }).ToListAsync();
    }

    public Appointment Find(int id)
    {
        return _appointments.FirstOrDefault(_ => _.Id == id);
    }

    public int GetDoctorsAppointmentInDay(int doctorId, DateTime date)
    {
        return _appointments.Count(_ =>
            _.DoctorId == doctorId && _.Date.Date == date.Date);
    }

    public bool IsAppointmentDuplicate(int doctorId, int patientId,
        DateTime date)
    {
        return _appointments.Any(_ =>
            _.DoctorId == doctorId && _.PatientId == patientId &&
            _.Date.Date == date.Date);
    }

    public void Delete(Appointment appointment)
    {
        _appointments.Remove(appointment);
    }

    public void Update(Appointment appointment)
    {
        _appointments.Update(appointment);
    }
}