using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Xunit;

public class AppointmentServiceTest
{
    private readonly AppointmentAppService _sut;
    private readonly Mock<AppointmentRepository> _repository;
    private readonly Mock<UnitOfWork> _unitOfWork;

    public AppointmentServiceTest()
    {
        _unitOfWork = new Mock<UnitOfWork>();
        _repository = new Mock<AppointmentRepository>();
        _sut = new AppointmentAppService(_repository.Object,
            _unitOfWork.Object);
    }

    [Fact]
    public void Add_adds_appointment_properly()
    {
        var dto = AppointmentFactory.GenerateAddAppointmentDto();

        _sut.Add(dto);

        _repository.Verify(_ => _.Add(
            It.Is<Appointment>(_ =>
                _.Date == dto.Date && _.DoctorId == dto.DoctorId &&
                _.PatientId == dto.PatientId)));

        _unitOfWork.Verify(_ => _.Save());
    }

    [Fact]
    public async void GetAll()
    {
        var appointments = AppointmentFactory.GenerateGetAppointmentDtoList();
        _repository.Setup(_ => _.GetAll()).Returns(appointments);

        var expected = await _sut.GetAll();

        expected.Should().HaveCount(5);
        expected.Should().Contain(_ => _.Id == 1);
        expected.Should().Contain(_ => _.Id == 2);
        expected.Should().Contain(_ => _.Id == 3);
        expected.Should().Contain(_ => _.Id == 4);
        expected.Should().Contain(_ => _.Id == 5);
    }
}