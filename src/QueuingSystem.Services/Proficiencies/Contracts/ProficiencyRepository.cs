public interface ProficiencyRepository : Repository
{
    public void Add(Proficiency proficiency);
    public Task<IList<GetProficiencyDto>> GetAll();
    public void Update(Proficiency proficiency);
    public Proficiency Find(int id);
    public void Delete(Proficiency proficiency);
}