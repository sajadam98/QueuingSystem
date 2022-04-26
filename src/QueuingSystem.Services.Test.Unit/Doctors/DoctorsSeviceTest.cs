using Microsoft.EntityFrameworkCore;

public class DoctorsSeviceTest
{
    private readonly DoctorRepository _repository;
    private readonly DoctorService _sut;
    private readonly UnitOfWork _unitOfWork;
    private readonly EFDataContext _dbContext;
    private readonly DbSet<Doctor> _proficiencies;
    
    public DoctorsSeviceTest()
    {
        _dbContext =
            new EFInMemoryDatabase()
                .CreateDataContext<EFDataContext>();
        _unitOfWork = new EFUnitOfWork(_dbContext);
        _repository = new EFDoctorRepository(_dbContext);
        _sut = new DoctorAppService(_repository, _unitOfWork);
        _proficiencies = _dbContext.Set<Doctor>();
    }
}