public interface AppointmentRepository : Repository
{
    public void Add(Appointment appointment);
    public Task<IList<GetAppointmentDto>> GetAll();
    public void Update(Appointment appointment);
    public void Delete(Appointment appointment);
    public Appointment Find(int id);
    public int GetDoctorsAppointmentInDay(int dtoDoctorId, DateTime dtoDate);
}