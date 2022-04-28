public class AppointmentAppService : AppointmentService
{
    private readonly AppointmentRepository _repository;
    private readonly DoctorRepository _doctorRepository;
    private readonly UnitOfWork _unitOfWork;

    public AppointmentAppService(AppointmentRepository repository,
        DoctorRepository doctorRepository,
        UnitOfWork unitOfWork)
    {
        _repository = repository;
        _doctorRepository = doctorRepository;
        _unitOfWork = unitOfWork;
    }

    public void Add(AddAppointmentDto dto)
    {
        var doctor =
            _doctorRepository.Find(dto.DoctorId);
        if (doctor == null)
        {
            throw new DoctorNotFoundException();
        }

        var isAppointmentDuplicate =
            _repository.IsAppointmentDuplicate(dto.DoctorId, dto.PatientId,
                dto.Date);
        if (isAppointmentDuplicate)
        {
            throw new DuplicateAppointmentException();
        }

        var appointment = new Appointment
        {
            Date = dto.Date,
            DoctorId = doctor.Id,
            PatientId = dto.PatientId,
        };
        var doctorsAppointmentInDay = _repository
            .GetDoctorsAppointmentInDay(dto.DoctorId, dto.Date);

        if (doctorsAppointmentInDay >= doctor.PatientsPerDay)
        {
            throw new DoctorsAppointmentPerDayIsCompletedException();
        }

        _repository.Add(appointment);
        _unitOfWork.Save();
    }

    public async Task<IList<GetAppointmentDto>> GetAll()
    {
        return await _repository.GetAll();
    }

    public void Update(int id, UpdateAppointmentDto dto)
    {
        var appointment = _repository.Find(id);
        if (appointment == null)
        {
            throw new AppointmentNotFoundException();
        }

        var isAppointmentDuplicate =
            _repository.IsAppointmentDuplicate(dto.DoctorId,
                dto.PatientId,
                dto.Date);
        if (isAppointmentDuplicate)
        {
            throw new DuplicateAppointmentException();
        }

        appointment.DoctorId = dto.DoctorId;
        appointment.PatientId = dto.PatientId;
        appointment.Date = dto.Date;

        _repository.Update(appointment);
        _unitOfWork.Save();
    }

    public void Delete(int id)
    {
        var appointment = _repository.Find(id);
        if (appointment == null)
        {
            throw new AppointmentNotFoundException();
        }

        _repository.Delete(appointment);
        _unitOfWork.Save();
    }
}