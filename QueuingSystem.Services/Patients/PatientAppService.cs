public class PatientAppService : PatientService
{
    private readonly PatientRepository _repository;
    private readonly UnitOfWork _unitOfWork;

    public PatientAppService(PatientRepository repository,
        UnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public void Add(AddPatientDto dto)
    {
        var isNationalCodeExist =
            _repository.IsNationalCodeExist(dto.NationalCode);
        if (isNationalCodeExist)
        {
            throw new DuplicatePatientsNationalCodeException();
        }

        var patient = new Patient
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            NationalCode = dto.NationalCode,
            PhoneNumber = dto.PhoneNumber,
        };
        _repository.Add(patient);
        _unitOfWork.Save();
    }

    public async Task<IList<GetPatientDto>> GetAll()
    {
        return await _repository.GetAll();
    }

    public void Update(int patientId, UpdatePatientDto dto)
    {
        var patient = _repository.Find(patientId);
        if (patient == null)
        {
            throw new PatientNotFoundException();
        }

        var isNationalCodeExist =
            _repository.IsNationalCodeExist(dto.NationalCode);
        if (isNationalCodeExist)
        {
            throw new DuplicatePatientsNationalCodeException();
        }

        patient.FirstName = dto.FirstName;
        patient.LastName = dto.LastName;
        patient.NationalCode = dto.NationalCode;
        patient.PhoneNumber = dto.PhoneNumber;
        _repository.Update(patient);
        _unitOfWork.Save();
    }

    public void DeActivate(int patientId)
    {
        var patient = _repository.Find(patientId);
        if (patient == null)
        {
            throw new PatientNotFoundException();
        }
        patient.IsActive = false;
        
        _repository.DeActivate(patient);
        _unitOfWork.Save();
    }
    
    public void Activate(int patientId)
    {
        var patient = _repository.Find(patientId);
        if (patient == null)
        {
            throw new PatientNotFoundException();
        }
        patient.IsActive = true;
        
        _repository.Activate(patient);
        _unitOfWork.Save();
    }
}