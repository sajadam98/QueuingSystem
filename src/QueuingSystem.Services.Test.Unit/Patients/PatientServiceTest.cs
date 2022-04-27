using Microsoft.EntityFrameworkCore;
using Moq;

public class PatientServiceTest
{
    private readonly PatientService _sut;
    private readonly PatientRepository _repository;
    private readonly UnitOfWork _unitOfWork;
    private readonly EFDataContext _dbContext;
    private readonly DbSet<Patient> _patients;

    public PatientServiceTest()
    {
        _unitOfWork = new Mock<UnitOfWork>().Object;
        _dbContext = new Mock<EFDataContext>().Object;
        _patients = _dbContext.Set<Patient>();
    }
}