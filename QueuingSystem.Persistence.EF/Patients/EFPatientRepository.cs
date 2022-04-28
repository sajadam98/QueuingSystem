using Microsoft.EntityFrameworkCore;

public class EFPatientRepository : PatientRepository
{
    private readonly DbSet<Patient> _patients;

    public EFPatientRepository(EFDataContext dbContext)
    {
        _patients = dbContext.Set<Patient>();
    }

    public void Add(Patient patient)
    {
        _patients.Add(patient);
    }

    public async Task<IList<GetPatientDto>> GetAll()
    {
        return await _patients.Select(_ => new GetPatientDto
        {
            Id = _.Id,
            IsActive = _.IsActive,
            NationalCode = _.NationalCode,
            FirstName = _.FirstName,
            LastName = _.LastName,
            PhoneNumber = _.PhoneNumber
        }).ToListAsync();
    }

    public Patient Find(int id)
    {
        return _patients.FirstOrDefault(_ => _.Id == id);
    }

    public void Update(Patient patient)
    {
        _patients.Update(patient);
    }

    public bool IsNationalCodeExist(string nationalCode)
    {
        return _patients.Where(_ => _.NationalCode != nationalCode)
            .Any(_ => _.NationalCode == nationalCode);
    }
    
    public void DeActivate(Patient patient)
    {
        _patients.Update(patient);
    }
    
    public void Activate(Patient patient)
    {
        _patients.Update(patient);
    }
}