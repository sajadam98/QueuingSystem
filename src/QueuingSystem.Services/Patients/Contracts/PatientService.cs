public interface PatientService : Service
{
    public void Add(AddPatientDto dto);
    public Task<IList<GetPatientDto>> GetAll();
    public void Update(int patientId, UpdatePatientDto dto);
    public void DeActivate(int patientId);
    public void Activate(int patientId);
}