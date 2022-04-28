using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

public class DoctorsSeviceTest
{
    private readonly DoctorRepository _repository;
    private readonly DoctorService _sut;
    private readonly UnitOfWork _unitOfWork;
    private readonly EFDataContext _dbContext;
    private readonly DbSet<Doctor> _doctors;

    public DoctorsSeviceTest()
    {
        _dbContext =
            new EFInMemoryDatabase()
                .CreateDataContext<EFDataContext>();
        _unitOfWork = new EFUnitOfWork(_dbContext);
        _repository = new EFDoctorRepository(_dbContext);
        _sut = new DoctorAppService(_repository, _unitOfWork);
        _doctors = _dbContext.Set<Doctor>();
    }

    [Fact]
    public void Add_adds_doctor_properly()
    {
        var proficiency = AddProficiencyInDataBase();
        var dto = DoctorFactory.GenerateAddDoctorDto(proficiency);

        _sut.Add(dto);

        _doctors.Should().Contain(_ =>
            _.FirstName == dto.FirstName && _.LastName == dto.LastName &&
            _.PhoneNumber == dto.phoneNumber &&
            _.NationalCode == dto.NationalCode &&
            _.ProficiencyId == dto.ProficiencyId &&
            _.PatientsPerDay == dto.PatientPerDay);
    }

    private Proficiency AddProficiencyInDataBase()
    {
        var proficiency = ProficiencyFactory.GenerateProficiency("dummy");
        _dbContext.Manipulate(_ =>
            _dbContext.Set<Proficiency>().Add(proficiency));
        return proficiency;
    }

    [Fact]
    public void
        Add_throw_InvalidPatientsPerDayException_when_patient_per_day_is_invalid()
    {
        var proficiency = AddProficiencyInDataBase();
        var dto = DoctorFactory.GenerateAddDoctorDto(proficiency);
        dto.PatientPerDay = 0;

        var expected = () => _sut.Add(dto);

        expected.Should().ThrowExactly<InvalidPatientsPerDayException>();
    }

    [Fact]
    public void
        Add_throw_DuplicateDoctorsNationalCodeException_when_doctors_national_code_duplicate()
    {
        var proficiency = AddProficiencyInDataBase();
        var doctor = DoctorFactory.GenerateDoctor(proficiency, "dummy");
        _dbContext.Manipulate(_ => _doctors.Add(doctor));
        var dto = DoctorFactory.GenerateAddDoctorDto(proficiency);
        dto.NationalCode = doctor.NationalCode;

        var expected = () => _sut.Add(dto);

        expected.Should()
            .ThrowExactly<DuplicateDoctorsNationalCodeException>();
    }

    [Fact]
    public async void GetAll_returns_doctors_list_properly()
    {
        var proficiency = AddProficiencyInDataBase();
        var doctors = new List<Doctor>();
        var doctor1 =
            DoctorFactory.GenerateDoctor(proficiency, "dummy1");
        doctors.Add(doctor1);
        var doctor2 =
            DoctorFactory.GenerateDoctor(proficiency, "dummy2");
        doctors.Add(doctor2);
        var doctor3 =
            DoctorFactory.GenerateDoctor(proficiency, "dummy3");
        doctors.Add(doctor3);
        _dbContext.Manipulate(_ => _doctors.AddRange(doctors));

        var expected = await _sut.GetAll();

        expected.Should().HaveCount(3);
        CheckDoctorExist(expected, doctor1);
        CheckDoctorExist(expected, doctor2);
        CheckDoctorExist(expected, doctor3);
    }

    [Fact]
    public void Update_updates_doctor_properly()
    {
        var proficiency = AddProficiencyInDataBase();
        var doctor = AddDoctorInDataBase(proficiency);
        var dto = DoctorFactory.GenerateUpdateDoctorDto(proficiency);

        _sut.Update(doctor.Id, dto);

        var expected = _doctors.First(_ => _.Id == doctor.Id);
        expected.FirstName.Should().Be(dto.FirstName);
        expected.LastName.Should().Be(dto.LastName);
        expected.NationalCode.Should().Be(dto.NationalCode);
        expected.PhoneNumber.Should().Be(dto.phoneNumber);
        expected.PatientsPerDay.Should().Be(dto.PatientPerDay);
        expected.ProficiencyId.Should().Be(dto.ProficiencyId);
    }

    private Doctor AddDoctorInDataBase(Proficiency proficiency)
    {
        var doctor = DoctorFactory.GenerateDoctor(proficiency, "dummy2");
        _dbContext.Manipulate(_ => _doctors.Add(doctor));
        return doctor;
    }

    [Fact]
    public void
        Update_throw_DoctorNotFoundException_doctor_with_given_id_not_exist()
    {
        var proficiency = AddProficiencyInDataBase();
        AddDoctorInDataBase(proficiency);
        var dto = DoctorFactory.GenerateUpdateDoctorDto(proficiency);

        var expected = () => _sut.Update(1000, dto);

        expected.Should().ThrowExactly<DoctorNotFoundException>();
    }

    [Fact]
    public void
        Update_throw_InvalidPatientsPerDayException_when_patient_per_day_is_invalid()
    {
        var proficiency = AddProficiencyInDataBase();
        var doctor = AddDoctorInDataBase(proficiency);
        var dto = DoctorFactory.GenerateUpdateDoctorDto(proficiency);
        dto.PatientPerDay = 0;

        var expected = () => _sut.Update(doctor.Id, dto);

        expected.Should().ThrowExactly<InvalidPatientsPerDayException>();
    }

    [Fact]
    public void
        Update_throw_DuplicateDoctorsNationalCodeException_when_doctors_national_code_duplicate()
    {
        var proficiency = AddProficiencyInDataBase();
        var doctor = AddDoctorInDataBase(proficiency);
        var dto = DoctorFactory.GenerateAddDoctorDto(proficiency);
        dto.NationalCode = doctor.NationalCode;

        var expected = () => _sut.Add(dto);

        expected.Should()
            .ThrowExactly<DuplicateDoctorsNationalCodeException>();
    }

    [Fact]
    public void DeActivate_de_activate_doctor_with_given_id_properly()
    {
        var proficiency = AddProficiencyInDataBase();
        var doctor = AddDoctorInDataBase(proficiency);

        _sut.DeActivate(doctor.Id);

        var expected = _doctors.First(_ => _.Id == doctor.Id);
        expected.IsActive.Should().Be(false);
    }

    [Fact]
    public void
        DeActivate_throw_DoctorNotFoundException_doctor_with_given_id_not_exist()
    {
        var proficiency =
            AddProficiencyInDataBase();
        var doctor = AddDoctorInDataBase(proficiency);

        var expected = () => _sut.DeActivate(-1);

        expected.Should().ThrowExactly<DoctorNotFoundException>();
    }

    [Fact]
    public void Activate_activate_doctor_with_given_id_properly()
    {
        var proficiency = AddProficiencyInDataBase();
        var doctor = AddDoctorInDataBase(proficiency);

        _sut.Activate(doctor.Id);

        var expected = _doctors.First(_ => _.Id == doctor.Id);
        expected.IsActive.Should().Be(true);
    }

    [Fact]
    public void
        Activate_throw_DoctorNotFoundException_doctor_with_given_id_not_exist()
    {
        var proficiency = AddProficiencyInDataBase();
        var doctor = AddDoctorInDataBase(proficiency);

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
