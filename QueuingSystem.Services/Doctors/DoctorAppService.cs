public class DoctorAppService : DoctorService
{
    private readonly DoctorRepository _doctorRepository;
    private readonly UnitOfWork _unitOfWork;

    public DoctorAppService(DoctorRepository doctorRepository,
        UnitOfWork unitOfWork)
    {
        _doctorRepository = doctorRepository;
        _unitOfWork = unitOfWork;
    }

    public void Add(AddDoctorDto dto)
    {
        if (dto.PatientPerDay < 1)
        {
            throw new InvalidPatientsPerDayException();
        }

        var isNationalCodeExist =
            _doctorRepository.IsNationalCodeExist(dto.NationalCode);
        if (isNationalCodeExist)
        {
            throw new DuplicateDoctorsNationalCodeException();
        }

        var doctor = new Doctor
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            NationalCode = dto.NationalCode,
            PhoneNumber = dto.phoneNumber,
            PatientsPerDay = dto.PatientPerDay,
            ProficiencyId = dto.ProficiencyId,
        };

        _doctorRepository.Add(doctor);
        _unitOfWork.Save();
    }

    public async Task<IList<GetDoctorDto>> GetAll()
    {
        return await _doctorRepository.GetAll();
    }

    public void DeActivate(int id)
    {
        var doctor = _doctorRepository.Find(id);
        if (doctor == null)
        {
            throw new DoctorNotFoundException();
        }

        doctor.IsActive = false;
        _doctorRepository.Update(doctor);
        _unitOfWork.Save();
    }

    public void Activate(int id)
    {
        var doctor = _doctorRepository.Find(id);
        if (doctor == null)
        {
            throw new DoctorNotFoundException();
        }

        doctor.IsActive = true;
        _doctorRepository.Update(doctor);
        _unitOfWork.Save();
    }

    public void Update(int id, UpdateDoctorDto dto)
    {
        var doctor = _doctorRepository.Find(id);
        if (doctor == null)
        {
            throw new DoctorNotFoundException();
        }

        if (dto.PatientPerDay < 1)
        {
            throw new InvalidPatientsPerDayException();
        }

        var isNationalCodeExist =
            _doctorRepository.IsNationalCodeExist(dto.NationalCode);
        if (isNationalCodeExist)
        {
            throw new DuplicateDoctorsNationalCodeException();
        }

        doctor.FirstName = dto.FirstName;
        doctor.LastName = dto.LastName;
        doctor.PhoneNumber = dto.phoneNumber;
        doctor.NationalCode = dto.NationalCode;
        doctor.PatientsPerDay = dto.PatientPerDay;
        doctor.ProficiencyId = dto.ProficiencyId;
        _doctorRepository.Update(doctor);
        _unitOfWork.Save();
    }
}