public interface AppointmentService : Service
{
    public void Add(AddAppointmentDto dto);
    public Task<IList<GetAppointmentDto>> GetAll();
    public void Update(int id, UpdateAppointmentDto dto);
    public void Delete(int id);
}