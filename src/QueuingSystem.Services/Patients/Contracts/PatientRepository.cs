public interface PatientRepository : Repository
{
    public void Add(Patient patient);
    public Task<IList<GetPatientDto>> GetAll();
    public void Update(Patient patient);
    public void DeActivate(Patient patient);
    public void Activate(Patient patient);
    public Patient Find(int id);
    public bool IsNationalCodeExist(string nationalCode);
}