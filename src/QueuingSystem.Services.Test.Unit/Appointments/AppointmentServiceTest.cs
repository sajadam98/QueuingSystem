using System;
using System.Collections.Generic;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

public class AppointmentServiceTest
{
    private readonly AppointmentAppService _sut;
    private readonly EFDataContext _dbContext;
    private readonly AppointmentRepository _repository;
    private readonly DoctorRepository _doctorRepository;
    private readonly UnitOfWork _unitOfWork;
    private readonly DbSet<Appointment> _appointments;

    public AppointmentServiceTest()
    {
        _dbContext =
            new EFInMemoryDatabase()
                .CreateDataContext<EFDataContext>();
        _unitOfWork = new EFUnitOfWork(_dbContext);
        _repository = new EFAppointmentRepository(_dbContext);
        _doctorRepository = new EFDoctorRepository(_dbContext);
        _sut = new AppointmentAppService(_repository, _doctorRepository,
            _unitOfWork);
        _appointments = _dbContext.Set<Appointment>();
    }

    [Fact]
    public void Add_adds_appointment_properly()
    {
        var doctor = AddDoctorToDataBase("dummy", "dummy");
        var patient = AddPatientToDataBase("dummy");
        var dto =
            AppointmentFactory.GenerateAddAppointmentDto(doctor.Id,
                patient.Id);

        _sut.Add(dto);

        var expected = _appointments.Should().Contain(_ =>
            _.Date == dto.Date && _.DoctorId == doctor.Id &&
            _.PatientId == dto.PatientId);
    }

    [Fact]
    public void
        Add_throw_DoctorsAppointmentPerDayIsCompletedException_doctors_appointments_per_day_is_completed()
    {
        var doctor = AddDoctorToDataBase("dummy", "dummy", 1);
        var patient = AddPatientToDataBase("dummy");
        var patient2 = AddPatientToDataBase("dummy2");
        AddAppointmentToDataBase(doctor, patient);
        var dto =
            AppointmentFactory.GenerateAddAppointmentDto(doctor.Id,
                patient2.Id);

        var expected = () => _sut.Add(dto);

        expected.Should()
            .ThrowExactly<DoctorsAppointmentPerDayIsCompletedException>();
    }

    [Fact]
    public void
        Add_throw_DoctorNotFoundException_doctor_with_given_id_not_exist()
    {
        var doctor = AddDoctorToDataBase("dummy", "dummy");
        var patient = AddPatientToDataBase("dummy");
        AddAppointmentToDataBase(doctor, patient);
        var dto =
            AppointmentFactory.GenerateAddAppointmentDto(-1,
                patient.Id);

        var expected = () => _sut.Add(dto);

        expected.Should()
            .ThrowExactly<DoctorNotFoundException>();
    }

    [Fact]
    public void
        Add_throw_DuplicateAppointmentException_appointment_is_exist_with_given_doctor_and_patient_in_this_day()
    {
        var doctor = AddDoctorToDataBase("dummy", "dummy1");
        var patient = AddPatientToDataBase("dummy");
        AddAppointmentToDataBase(doctor, patient);
        var dto =
            AppointmentFactory.GenerateAddAppointmentDto(doctor.Id,
                patient.Id);

        var expected = () => _sut.Add(dto);

        expected.Should().ThrowExactly<DuplicateAppointmentException>();
    }

    [Fact]
    public async void GetAll_return_appointments_properly()
    {
        var doctor = AddDoctorToDataBase("dummy", "dummy");
        var patient = AddPatientToDataBase("dummy1");
        var patient2 = AddPatientToDataBase("dummy2");
        var appointments = new List<Appointment>();
        AddAppointmentToDataBase(appointments, doctor, patient, patient2);

        var expected = await _sut.GetAll();

        expected.Should().HaveCount(2);
        CheckGetAllResult(expected, doctor, patient, -1, 1);
        CheckGetAllResult(expected, doctor, patient2, -2, 2);
    }

    [Fact]
    public void Update_updates_appointment_with_given_id_properly()
    {
        var doctor = AddDoctorToDataBase("dummy", "dummy1");
        var doctor2 = AddDoctorToDataBase("dummy", "dummy2");
        var patient = AddPatientToDataBase("dummy");
        var patient2 = AddPatientToDataBase("dummy2");
        var appointment = AddAppointmentToDataBase(doctor, patient);
        var dto =
            AppointmentFactory.GenerateUpdateAppointmentDto(doctor2.Id,
                patient2.Id);

        _sut.Update(appointment.Id, dto);

        _appointments.Should().Contain(_ =>
            _.Date.Date == dto.Date.Date && _.DoctorId == doctor2.Id &&
            _.PatientId == patient2.Id);
    }

    [Fact]
    public void
        Update_throw_AppointmentNotFoundException_appointment_with_given_id_not_exist()
    {
        var doctor = AddDoctorToDataBase("dummy", "dummy1");
        var doctor2 = AddDoctorToDataBase("dummy", "dummy2");
        var patient = AddPatientToDataBase("dummy");
        var patient2 = AddPatientToDataBase("dummy2");
        var appointment = AddAppointmentToDataBase(doctor, patient);
        var dto =
            AppointmentFactory.GenerateUpdateAppointmentDto(doctor2.Id,
                patient2.Id);

        var expected = () => _sut.Update(-1, dto);

        expected.Should().ThrowExactly<AppointmentNotFoundException>();
    }

    [Fact]
    public void
        Update_throw_DuplicateAppointmentException_appointment_is_exist_with_given_doctor_and_patient_in_this_day()
    {
        var doctor = AddDoctorToDataBase("dummy", "dummy1");
        var doctor2 = AddDoctorToDataBase("dummy", "dummy2");
        var patient = AddPatientToDataBase("dummy");
        var patient2 = AddPatientToDataBase("dummy2");
        var appointment = AddAppointmentToDataBase(doctor, patient);
        AddAppointmentToDataBase(doctor2, patient2);
        var dto =
            AppointmentFactory.GenerateUpdateAppointmentDto(doctor2.Id,
                patient2.Id);

        var expected = () => _sut.Update(appointment.Id, dto);

        expected.Should().ThrowExactly<DuplicateAppointmentException>();
    }

    [Fact]
    public void Delete_deletes_appointment_with_given_id_properly()
    {
        var doctor = AddDoctorToDataBase("dummy", "dummy1");
        var patient = AddPatientToDataBase("dummy");
        var appointment = AddAppointmentToDataBase(doctor, patient);

        _sut.Delete(appointment.Id);

        _appointments.Should().HaveCount(0);
        _appointments.Should().NotContain(_ =>
            _.Date.Date == appointment.Date.Date &&
            _.DoctorId == appointment.DoctorId &&
            _.PatientId == appointment.PatientId);
    }

    [Fact]
    public void
        Delete_throw_AppointmentNotFoundException_appointment_with_given_id_not_exist()
    {
        var doctor = AddDoctorToDataBase("dummy", "dummy1");
        var patient = AddPatientToDataBase("dummy");
        var appointment = AddAppointmentToDataBase(doctor, patient);

        var expected = () => _sut.Delete(-1);

        expected.Should().ThrowExactly<AppointmentNotFoundException>();
    }

    private Appointment AddAppointmentToDataBase(Doctor doctor,
        Patient patient)
    {
        var appointment = new Appointment
        {
            Date = DateTime.Now,
            DoctorId = doctor.Id,
            PatientId = patient.Id,
        };
        _dbContext.Manipulate(_ => _appointments.Add(appointment));
        return appointment;
    }

    private void CheckGetAllResult(
        IList<GetAppointmentDto> expected, Doctor doctor, Patient patient,
        int timeDifference, int id)
    {
        expected.Should().Contain(_ =>
            _.Id == id &&
            _.Date.Date == DateTime.Now.AddDays(timeDifference).Date &&
            _.DoctorsFirstName == doctor.FirstName &&
            _.DoctorsLastName == doctor.LastName &&
            _.PatientFirstName == patient.FirstName &&
            _.PatientLastName == patient.LastName);
    }

    private void AddAppointmentToDataBase(List<Appointment> appointments,
        Doctor doctor,
        Patient patient, Patient patient2)
    {
        GenerateAppointmentList(appointments, doctor, patient, patient2);
        _dbContext.Manipulate(_ => _appointments.AddRange(appointments));
    }

    private static void GenerateAppointmentList(
        List<Appointment> appointments,
        Doctor doctor, Patient patient, Patient patient2)
    {
        appointments.Add(new Appointment
        {
            Date = DateTime.Now.AddDays(-1),
            DoctorId = doctor.Id,
            PatientId = patient.Id,
        });
        appointments.Add(new Appointment
        {
            Date = DateTime.Now.AddDays(-2),
            DoctorId = doctor.Id,
            PatientId = patient2.Id,
        });
    }

    private Doctor AddDoctorToDataBase(string proficiencyTitle,
        string doctorNationalCode, int appointmentPerDay = 5)
    {
        var proficiency =
            ProficiencyFactory.GenerateProficiency(proficiencyTitle);
        var doctor =
            DoctorFactory.GenerateDoctor(proficiency, doctorNationalCode);
        doctor.PatientsPerDay = appointmentPerDay;
        _dbContext.Manipulate(_ => _dbContext.Set<Doctor>().Add(doctor));
        return doctor;
    }

    private Patient AddPatientToDataBase(string attribute)
    {
        var patient = PatientFactory.GeneratePatient(attribute);
        _dbContext.Manipulate(_ => _dbContext.Set<Patient>().Add(patient));
        return patient;
    }
}