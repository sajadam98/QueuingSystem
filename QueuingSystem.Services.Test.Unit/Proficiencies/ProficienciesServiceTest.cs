using System.Linq;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

public class ProficienciesServiceTest
{
    private readonly ProficiencyRepository _repository;
    private readonly ProficiencyService _sut;
    private readonly UnitOfWork _unitOfWork;
    private readonly EFDataContext _dbContext;
    private readonly DbSet<Proficiency> _proficiencies;

    public ProficienciesServiceTest()
    {
        _dbContext =
            new EFInMemoryDatabase()
                .CreateDataContext<EFDataContext>();
        _unitOfWork = new EFUnitOfWork(_dbContext);
        _repository = new EFProficiencyRepository(_dbContext);
        _sut = new ProficiencyAppService(_repository, _unitOfWork);
        _proficiencies = _dbContext.Set<Proficiency>();
    }

    [Fact]
    public void Add_adds_proficiency_properly()
    {
        var dto = ProficiencyFactory.GenerateAddProficiencyDto("Dentist");

        _sut.Add(dto);

        _proficiencies.Should()
            .Contain(_ => _.Name == dto.Name);
    }

    [Fact]
    public async void GetAll_returns_all_proficiencies()
    {
        GenerateProficienciesInDataBase();

        var expected = await _sut.GetAll();

        expected.Should().HaveCount(3);
        expected.Should().Contain(_ => _.Name == "dummy1");
        expected.Should().Contain(_ => _.Name == "dummy2");
        expected.Should().Contain(_ => _.Name == "dummy3");
    }

    [Fact]
    public void Update_updates_proficiency_with_given_id_properly()
    {
        var proficiency = AddProficiencyInDataBase();
        var dto = new UpdateProficiencyDto
        {
            Name = "Edited Dentistry",
        };

        _sut.Update(proficiency.Id, dto);

        var expected =
            _proficiencies.First(_ => _.Id == proficiency.Id);
        expected.Name.Should().Be(dto.Name);
    }

    [Fact]
    public void
        Update_throw_ProficiencyNotFoundWithGivenIdException_when_proficiency_with_given_id_not_exist()
    {
        var dto = new UpdateProficiencyDto
        {
            Name = "Edited Dentistry",
        };

        var expected = () => _sut.Update(-1, dto);

        expected.Should()
            .ThrowExactly<ProficiencyNotFoundWithGivenIdException>();
    }

    [Fact]
    public async void Delete_deletes_proficiency_with_given_id_properly()
    {
        var proficiency = AddProficiencyInDataBase();

        _sut.Delete(proficiency.Id);

        var expected = await _sut.GetAll();

        expected.Should().HaveCount(0);
        expected.Should().NotContain(_ => _.Name == proficiency.Name);
    }

    [Fact]
    public void
        Delete_throw_ProficiencyNotFoundWithGivenIdException_when_proficiency_with_given_id_not_exist()
    {
        var proficiency = AddProficiencyInDataBase();

        var expected = () => _sut.Delete(-1);

        expected.Should()
            .ThrowExactly<ProficiencyNotFoundWithGivenIdException>();
    }

    private Proficiency AddProficiencyInDataBase()
    {
        var proficiency =
            ProficiencyFactory.GenerateProficiency("Dentistry");
        _dbContext.Manipulate(_ => _proficiencies.Add(proficiency));
        return proficiency;
    }

    private void GenerateProficienciesInDataBase()
    {
        var proficiencies = ProficiencyFactory.GenerateProficiencyList();
        _dbContext.Manipulate(_ =>
            _proficiencies.AddRange(proficiencies));
    }
}