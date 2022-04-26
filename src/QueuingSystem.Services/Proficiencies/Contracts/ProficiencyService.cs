public interface ProficiencyService : Service
{
    public void Add(AddProficiencyDto dto);
    public Task<IList<GetProficiencyDto>> GetAll();
    public void Update(int id, UpdateProficiencyDto dto);
    public void Delete(int id);
}