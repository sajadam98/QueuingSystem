using Microsoft.EntityFrameworkCore;

public class EFDoctorRepository : DoctorRepository
{
    private readonly DbSet<Doctor> _doctors;

    public EFDoctorRepository(EFDataContext dbContext)
    {
        _doctors = dbContext.Set<Doctor>();
    }

    public void Add(Doctor doctor)
    {
        _doctors.Add(doctor);
    }

    public async Task<IList<GetDoctorDto>> GetAll()
    {
        return await _doctors.Select(_ => new GetDoctorDto
        {
            Id = _.Id,
            phoneNumber = _.PhoneNumber,
            FirstName = _.FirstName,
            IsActive = _.IsActive,
            LastName = _.LastName,
            NationalCode = _.NationalCode,
        }).ToListAsync();
    }

    public void Update(Doctor doctor)
    {
        _doctors.Update(doctor);
    }

    public void Delete(Doctor doctor)
    {
        _doctors.Remove(doctor);
    }

    public Doctor Find(int id)
    {
        return _doctors.FirstOrDefault(_ => _.Id == id);
    }

    public bool IsNationalCodeExist(string nationalCode)
    {
        return _doctors.Any(_ => _.NationalCode == nationalCode);
    }
}