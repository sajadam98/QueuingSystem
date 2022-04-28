public interface DoctorService : Service
{
    public void Add(AddDoctorDto dto);
    public Task<IList<GetDoctorDto>> GetAll();
    public void Update(int id, UpdateDoctorDto dto);
    public void DeActivate(int id);
    public void Activate(int id);
}