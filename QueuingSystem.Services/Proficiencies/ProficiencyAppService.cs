public class ProficiencyAppService : ProficiencyService
{
    private readonly ProficiencyRepository _repository;
    private readonly UnitOfWork _unitOfWork;

    public ProficiencyAppService(ProficiencyRepository repository,
        UnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public void Add(AddProficiencyDto dto)
    {
        var proficiency = new Proficiency
        {
            Name = dto.Name,
        };

        _repository.Add(proficiency);
        _unitOfWork.Save();
    }

    public Task<IList<GetProficiencyDto>> GetAll()
    {
        return _repository.GetAll();
    }

    public void Update(int id, UpdateProficiencyDto dto)
    {
        var proficiency = _repository.Find(id);
        if (proficiency == null)
        {
            throw new ProficiencyNotFoundWithGivenIdException();
        }

        proficiency.Name = dto.Name;

        _repository.Update(proficiency);
        _unitOfWork.Save();
    }

    public void Delete(int id)
    {
        var proficiency = _repository.Find(id);
        if (proficiency == null)
        {
            throw new ProficiencyNotFoundWithGivenIdException();
        }
        _repository.Delete(proficiency);
        _unitOfWork.Save();
    }
}