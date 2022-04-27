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
        var proficiency =
            ProficiencyFactory.GenerateProficiency("Dentistry");
        _dbContext.Manipulate(_ =>
            _dbContext.Set<Proficiency>().Add(proficiency));
        var dto = DoctorFactory.GenerateAddDoctorDto(proficiency);

        _sut.Add(dto);

        var expected = _doctors.Should().Contain(_ =>
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
        var proficiency =
            ProficiencyFactory.GenerateProficiency("Dentistry");
        _dbContext.Manipulate(_ =>
            _dbContext.Set<Proficiency>().Add(proficiency));
        var dto = DoctorFactory.GenerateAddDoctorDto(proficiency);
        dto.PatientPerDay = 0;


        var expected = () => _sut.Add(dto);

        expected.Should().ThrowExactly<InvalidPatientsPerDayException>();
    }

    [Fact]
    public void
        Add_throw_DuplicateDoctorsNationalCodeException_when_doctors_national_code_duplicate()
    {
        var proficiency =
            ProficiencyFactory.GenerateProficiency("Dentistry");
        _dbContext.Manipulate(_ =>
            _dbContext.Set<Proficiency>().Add(proficiency));
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
        var proficiency =
            ProficiencyFactory.GenerateProficiency("Dentistry");
        _dbContext.Manipulate(_ =>
            _dbContext.Set<Proficiency>().Add(proficiency));
        var doctors = DoctorFactory.GenerateDoctorsList(proficiency);
        _dbContext.Manipulate(_ => _doctors.AddRange(doctors));

        var expected = await _sut.GetAll();

        expected.Should().HaveCount(3);
        expected.Should().Contain(_ => _.NationalCode == "dummy1");
        expected.Should().Contain(_ => _.NationalCode == "dummy2");
        expected.Should().Contain(_ => _.NationalCode == "dummy3");
    }

    [Fact]
    public void Update_updates_doctor_properly()
    {
        var proficiency =
            ProficiencyFactory.GenerateProficiency("Dentistry");
        _dbContext.Manipulate(_ =>
            _dbContext.Set<Proficiency>().Add(proficiency));
        var doctor = DoctorFactory.GenerateDoctor(proficiency, "dummy2");
        _dbContext.Manipulate(_ => _doctors.Add(doctor));
        var dto = DoctorFactory.GenerateUpdateDoctorDto(proficiency);
    
        _sut.Update(doctor.Id, dto);
    
        var expected = _doctors.First(_ => _.Id == doctor.Id);
        expected!.FirstName.Should().Be(dto.FirstName);
        expected.LastName.Should().Be(dto.LastName);
        expected.NationalCode.Should().Be(dto.NationalCode);
        expected.PhoneNumber.Should().Be(dto.phoneNumber);
        expected.PatientsPerDay.Should().Be(dto.PatientPerDay);
        expected.ProficiencyId.Should().Be(dto.ProficiencyId);
    }
    
    [Fact]
    public void
        Update_throw_DoctorNotFoundException_doctor_with_given_id_not_exist()
    {
        var proficiency =
            ProficiencyFactory.GenerateProficiency("Dentistry");
        _dbContext.Manipulate(_ =>
            _dbContext.Set<Proficiency>().Add(proficiency));
        var doctor = DoctorFactory.GenerateDoctor(proficiency,"dummy");
        _dbContext.Manipulate(_ => _doctors.Add(doctor));
        var dto = DoctorFactory.GenerateUpdateDoctorDto(proficiency);
    
    
        var expected = () => _sut.Update(1000, dto);
    
        expected.Should().ThrowExactly<DoctorNotFoundException>();
    }
    
    [Fact]
    public void
        Update_throw_InvalidPatientsPerDayException_when_patient_per_day_is_invalid()
    {
        var proficiency =
            ProficiencyFactory.GenerateProficiency("Dentistry");
        _dbContext.Manipulate(_ =>
            _dbContext.Set<Proficiency>().Add(proficiency));
        var doctor = DoctorFactory.GenerateDoctor(proficiency, "dummy2");
        _dbContext.Manipulate(_ => _doctors.Add(doctor));
        var dto = DoctorFactory.GenerateUpdateDoctorDto(proficiency);
        dto.PatientPerDay = 0;
    
    
        var expected = () => _sut.Update(doctor.Id, dto);
    
        expected.Should().ThrowExactly<InvalidPatientsPerDayException>();
    }
    
    [Fact]
    public void
        Update_throw_DuplicateDoctorsNationalCodeException_when_doctors_national_code_duplicate()
    {
        var proficiency =
            ProficiencyFactory.GenerateProficiency("Dentistry");
        _dbContext.Manipulate(_ =>
            _dbContext.Set<Proficiency>().Add(proficiency));
        var doctor = DoctorFactory.GenerateDoctor(proficiency, "dummy");
        _dbContext.Manipulate(_ => _doctors.Add(doctor));
        var dto = DoctorFactory.GenerateAddDoctorDto(proficiency);
    
    
        var expected = () => _sut.Add(dto);
    
        expected.Should()
            .ThrowExactly<DuplicateDoctorsNationalCodeException>();
    }
    
    [Fact]
    public void DeActivate_de_activate_doctor_with_given_id_properly()
    {
        var proficiency =
            ProficiencyFactory.GenerateProficiency("Dentistry");
        _dbContext.Manipulate(_ =>
            _dbContext.Set<Proficiency>().Add(proficiency));
        var doctor = DoctorFactory.GenerateDoctor(proficiency, "dummy2");
        _dbContext.Manipulate(_ => _doctors.Add(doctor));
    
        _sut.DeActivate(doctor.Id);
    
        var expected = _doctors.First(_ => _.Id == doctor.Id);
        expected.IsActive.Should().Be(false);
    }
    
    [Fact]
    public void
        DeActivate_throw_DoctorNotFoundException_doctor_with_given_id_not_exist()
    {
        var proficiency =
            ProficiencyFactory.GenerateProficiency("Dentistry");
        _dbContext.Manipulate(_ =>
            _dbContext.Set<Proficiency>().Add(proficiency));
        var doctor = DoctorFactory.GenerateDoctor(proficiency, "dummy");
        _dbContext.Manipulate(_ => _doctors.Add(doctor));
    
        var expected = () => _sut.DeActivate(1000);
    
        expected.Should().ThrowExactly<DoctorNotFoundException>();
    }
    
    [Fact]
    public void Activate_activate_doctor_with_given_id_properly()
    {
        var proficiency =
            ProficiencyFactory.GenerateProficiency("Dentistry");
        _dbContext.Manipulate(_ =>
            _dbContext.Set<Proficiency>().Add(proficiency));
        var doctor = DoctorFactory.GenerateDoctor(proficiency, "dummy");
        _dbContext.Manipulate(_ => _doctors.Add(doctor));
    
        _sut.Activate(doctor.Id);
    
        var expected = _doctors.First(_ => _.Id == doctor.Id);
        expected.IsActive.Should().Be(true);
    }
    
    [Fact]
    public void
        Activate_throw_DoctorNotFoundException_doctor_with_given_id_not_exist()
    {
        var proficiency =
            ProficiencyFactory.GenerateProficiency("Dentistry");
        _dbContext.Manipulate(_ =>
            _dbContext.Set<Proficiency>().Add(proficiency));
        var doctor = DoctorFactory.GenerateDoctor(proficiency, "dummy");
        _dbContext.Manipulate(_ => _doctors.Add(doctor));
    
        var expected = () => _sut.Activate(1000);
    
        expected.Should().ThrowExactly<DoctorNotFoundException>();
    }
}