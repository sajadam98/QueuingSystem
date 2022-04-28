public interface DoctorRepository : Repository
{
    public void Add(Doctor doctor);
    public Task<IList<GetDoctorDto>> GetAll();
    public void Update(Doctor doctor);
    public void Delete(Doctor doctor);
    public Doctor Find(int id);

    public bool IsNationalCodeExist(string nationalCode);
}