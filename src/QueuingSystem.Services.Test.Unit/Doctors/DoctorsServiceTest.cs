using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

public class DoctorsServiceDoctorTest : DoctorTestFixture
{
    private readonly DoctorService _sut;
    private readonly DbSet<Doctor> _doctors;

    public DoctorsServiceDoctorTest()
    {
        UnitOfWork unitOfWork = new EFUnitOfWork(DbContext);
        DoctorRepository repository = new EFDoctorRepository(DbContext);
        _sut = new DoctorAppService(repository, unitOfWork);
        _doctors = DbContext.Set<Doctor>();
    }

    [Fact]
    public void Add_adds_doctor_properly()
    {
        var dto =
            DoctorFactory.GenerateAddDoctorDto(Proficiency);

        _sut.Add(dto);

        _doctors.Should().Contain(_ =>
            _.FirstName == dto.FirstName && _.LastName == dto.LastName &&
            _.PhoneNumber == dto.phoneNumber &&
            _.NationalCode == dto.NationalCode &&
            _.ProficiencyId == dto.ProficiencyId &&
            _.PatientsPerDay == dto.PatientPerDay);
    }

    [Fact]
    public void
        Add_throw_InvalidPatientsPerDayException_when_patient_per_day_is_invalid()
    {
        var dto = DoctorFactory.GenerateAddDoctorDto(Proficiency);
        dto.PatientPerDay = 0;

        var expected = () => _sut.Add(dto);

        expected.Should().ThrowExactly<InvalidPatientsPerDayException>();
    }

    [Fact]
    public void
        Add_throw_DuplicateDoctorsNationalCodeException_when_doctors_national_code_duplicate()
    {
        var doctor = DoctorFactory.GenerateDoctor(Proficiency, "dummy");
        DbContext.Manipulate(_ => _doctors.Add(doctor));
        var dto = DoctorFactory.GenerateAddDoctorDto(Proficiency);
        dto.NationalCode = doctor.NationalCode;

        var expected = () => _sut.Add(dto);

        expected.Should()
            .ThrowExactly<DuplicateDoctorsNationalCodeException>();
    }

    [Fact]
    public async void GetAll_returns_doctors_list_properly()
    {
        var doctors = new List<Doctor>();
        var doctor1 =
            DoctorFactory.GenerateDoctor(Proficiency, "dummy1");
        doctors.Add(doctor1);
        var doctor2 =
            DoctorFactory.GenerateDoctor(Proficiency, "dummy2");
        doctors.Add(doctor2);
        var doctor3 =
            DoctorFactory.GenerateDoctor(Proficiency, "dummy3");
        doctors.Add(doctor3);
        DbContext.Manipulate(_ => _doctors.AddRange(doctors));

        var expected = await _sut.GetAll();

        expected.Should().HaveCount(3);
        CheckDoctorExist(expected, doctor1);
        CheckDoctorExist(expected, doctor2);
        CheckDoctorExist(expected, doctor3);
    }

    [Fact]
    public void Update_updates_doctor_properly()
    {
        var doctor = AddDoctorInDataBase();
        var dto = DoctorFactory.GenerateUpdateDoctorDto(Proficiency);

        _sut.Update(doctor.Id, dto);

        var expected = _doctors.First(_ => _.Id == doctor.Id);
        expected.FirstName.Should().Be(dto.FirstName);
        expected.LastName.Should().Be(dto.LastName);
        expected.NationalCode.Should().Be(dto.NationalCode);
        expected.PhoneNumber.Should().Be(dto.phoneNumber);
        expected.PatientsPerDay.Should().Be(dto.PatientPerDay);
        expected.ProficiencyId.Should().Be(dto.ProficiencyId);
    }

    private Doctor AddDoctorInDataBase()
    {
        var doctor = DoctorFactory.GenerateDoctor(Proficiency, "dummy2");
        DbContext.Manipulate(_ => _doctors.Add(doctor));
        return doctor;
    }

    [Fact]
    public void
        Update_throw_DoctorNotFoundException_doctor_with_given_id_not_exist()
    {
        AddDoctorInDataBase();
        var dto = DoctorFactory.GenerateUpdateDoctorDto(Proficiency);

        var expected = () => _sut.Update(1000, dto);

        expected.Should().ThrowExactly<DoctorNotFoundException>();
    }

    [Fact]
    public void
        Update_throw_InvalidPatientsPerDayException_when_patient_per_day_is_invalid()
    {
        var doctor = AddDoctorInDataBase();
        var dto = DoctorFactory.GenerateUpdateDoctorDto(Proficiency);
        dto.PatientPerDay = 0;

        var expected = () => _sut.Update(doctor.Id, dto);

        expected.Should().ThrowExactly<InvalidPatientsPerDayException>();
    }

    [Fact]
    public void
        Update_throw_DuplicateDoctorsNationalCodeException_when_doctors_national_code_duplicate()
    {
        var doctor = AddDoctorInDataBase();
        var dto = DoctorFactory.GenerateAddDoctorDto(Proficiency);
        dto.NationalCode = doctor.NationalCode;

        var expected = () => _sut.Add(dto);

        expected.Should()
            .ThrowExactly<DuplicateDoctorsNationalCodeException>();
    }

    [Fact]
    public void DeActivate_de_activate_doctor_with_given_id_properly()
    {
        var doctor = AddDoctorInDataBase();

        _sut.DeActivate(doctor.Id);

        var expected = _doctors.First(_ => _.Id == doctor.Id);
        expected.IsActive.Should().Be(false);
    }

    [Fact]
    public void
        DeActivate_throw_DoctorNotFoundException_doctor_with_given_id_not_exist()
    {
        AddDoctorInDataBase();

        var expected = () => _sut.DeActivate(-1);

        expected.Should().ThrowExactly<DoctorNotFoundException>();
    }

    [Fact]
    public void Activate_activate_doctor_with_given_id_properly()
    {
        var doctor = AddDoctorInDataBase();

        _sut.Activate(doctor.Id);

        var expected = _doctors.First(_ => _.Id == doctor.Id);
        expected.IsActive.Should().Be(true);
    }

    [Fact]
    public void
        Activate_throw_DoctorNotFoundException_doctor_with_given_id_not_exist()
    {
        AddDoctorInDataBase();

        var expected = () => _sut.Activate(-1);

        expected.Should().ThrowExactly<DoctorNotFoundException>();
    }

    private void CheckDoctorExist(IList<GetDoctorDto> expected,
        Doctor doctor)
    {
        expected.Should().Contain(_ =>
            _.NationalCode == doctor.NationalCode &&
            _.phoneNumber == doctor.PhoneNumber &&
            _.FirstName == doctor.FirstName &&
            _.LastName == doctor.LastName);
    }
}