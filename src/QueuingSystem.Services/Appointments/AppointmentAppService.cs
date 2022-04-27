public class AppointmentAppService : AppointmentService
{
    private readonly AppointmentRepository _repository;
    private readonly UnitOfWork _unitOfWork;

    public AppointmentAppService(AppointmentRepository repository,
        UnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public void Add(AddAppointmentDto dto)
    {
        var appointment = new Appointment
        {
            Date = dto.Date,
            DoctorId = dto.DoctorId,
            Doctor = new Doctor
            {
                Id = dto.DoctorId,
            },
            PatientId = dto.PatientId,
            Patient = new Patient
            {
                Id = dto.PatientId,
            },
        };
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