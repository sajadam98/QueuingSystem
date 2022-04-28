using System;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

public class PatientServiceTest
{
    private readonly PatientService _sut;
    private readonly Mock<PatientRepository> _repository;
    private readonly Mock<UnitOfWork> _unitOfWork;

    public PatientServiceTest()
    {
        _unitOfWork = new Mock<UnitOfWork>();
        _repository = new Mock<PatientRepository>();
        _sut = new PatientAppService(_repository.Object,
            _unitOfWork.Object);
    }

    [Fact]
    public void Add_adds_patient_properly()
    {
        var dto = PatientFactory.GenerateAddPatientDto("dummy1");

        _sut.Add(dto);

        _repository.Verify(_ =>
            _.Add(It.Is<Patient>(_ =>
                _.FirstName == dto.FirstName &&
                _.LastName == dto.LastName &&
                _.PhoneNumber == dto.PhoneNumber &&
                _.NationalCode == dto.NationalCode)));

        _unitOfWork.Verify(_ => _.Save());
    }

    [Fact]
    public void
        Add_throw_DuplicatePatientsNationalCodeException_with_given_national_code_properly()
    {
        var dto = PatientFactory.GenerateAddPatientDto("dummy1");
        _repository.Setup(_ => _.IsNationalCodeExist(dto.NationalCode))
            .Returns(true);

        Action expected = () => _sut.Add(dto);

        expected.Should()
            .ThrowExactly<DuplicatePatientsNationalCodeException>();
    }

    [Fact]
    public async void GetAll_returns_patients_properly()
    {
        var patients = PatientFactory.GenerateGetPatientDtoList();
        _repository.Setup(_ => _.GetAll()).Returns(patients);

        var expected = await _sut.GetAll();

        expected.Should().HaveCount(3);
        expected.Should().Contain(_ => _.NationalCode == "dummy1");
        expected.Should().Contain(_ => _.NationalCode == "dummy2");
        expected.Should().Contain(_ => _.NationalCode == "dummy3");
    }

    [Fact]
    public void Update_updates_patient_with_given_id_properly()
    {
        var patientId = 1;
        _repository.Setup(_ => _.Find(patientId))
            .Returns(PatientFactory.GeneratePatient("dummy1"));
        var dto = PatientFactory.GenerateUpdatePatientDto("dummy2");

        _sut.Update(patientId, dto);

        _repository.Verify(_ => _.Update(
            It.Is<Patient>(p =>
                p.FirstName == dto.FirstName &&
                p.LastName == dto.LastName &&
                p.PhoneNumber == dto.PhoneNumber &&
                p.NationalCode == dto.NationalCode)));

        _unitOfWork.Verify(_ => _.Save());
    }

    [Fact]
    public void
        Update_throw_PatientNotFoundException_with_given_id_not_exist()
    {
        var dto = PatientFactory.GenerateUpdatePatientDto("dummy2");

        Action expected = () => _sut.Update(-1, dto);

        expected.Should().ThrowExactly<PatientNotFoundException>();
    }

    [Fact]
    public void
        Update_throw_DuplicatePatientsNationalCodeException_patient_with_given_id_is_exist()
    {
        var patientId = 1;
        var dto = PatientFactory.GenerateUpdatePatientDto("dummy2");
        _repository.Setup(_ => _.Find(patientId))
            .Returns(PatientFactory.GeneratePatient("dummy1"));
        _repository.Setup(_ => _.IsNationalCodeExist(dto.NationalCode))
            .Returns(true);

        Action expected = () => _sut.Update(patientId, dto);

        expected.Should()
            .ThrowExactly<DuplicatePatientsNationalCodeException>();
    }

    [Fact]
    public void DeActivate_de_activate_patient_with_given_id_properly()
    {
        var patientId = 1;
        _repository.Setup(_ => _.Find(patientId))
            .Returns(PatientFactory.GeneratePatient("dummy1"));

        _sut.DeActivate(patientId);

        _repository.Verify(_ => _.DeActivate(
            It.Is<Patient>(p =>
                p.FirstName == "dummy1" &&
                p.LastName == "dummy1" &&
                p.PhoneNumber == "dummy1" &&
                p.NationalCode == "dummy1")));

        _unitOfWork.Verify(_ => _.Save());
    }

    [Fact]
    public void
        DeActivate_throw_PatientNotFoundException_patient_with_given_id_not_exist()
    {
        Action expected = () => _sut.DeActivate(-1);

        expected.Should().ThrowExactly<PatientNotFoundException>();
    }

    [Fact]
    public void Activate_activate_patient_with_given_id_properly()
    {
        var patientId = 1;
        _repository.Setup(_ => _.Find(patientId))
            .Returns(PatientFactory.GeneratePatient("dummy1"));

        _sut.Activate(patientId);

        _repository.Verify(_ => _.Activate(
            It.Is<Patient>(p =>
                p.FirstName == "dummy1" &&
                p.LastName == "dummy1" &&
                p.PhoneNumber == "dummy1" &&
                p.NationalCode == "dummy1")));

        _unitOfWork.Verify(_ => _.Save());
    }

    [Fact]
    public void
        Activate_throw_PatientNotFoundException_patient_with_given_id_not_exist()
    {
        Action expected = () => _sut.Activate(-1);

        expected.Should().ThrowExactly<PatientNotFoundException>();
    }
}