using Microsoft.EntityFrameworkCore;

public class EfAppointmentRepository : AppointmentRepository
{
    private readonly DbSet<Appointment> _appointments;

    public EfAppointmentRepository(EFDataContext dbContext)
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
            Doctor = _.Doctor.FirstName + " " + _.Doctor.LastName,
            Patient = _.Patient.FirstName + " " + _.Patient.LastName
        }).ToListAsync();
    }

    public Appointment Find(int id)
    {
        return _appointments.FirstOrDefault(_ => _.Id == id);
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