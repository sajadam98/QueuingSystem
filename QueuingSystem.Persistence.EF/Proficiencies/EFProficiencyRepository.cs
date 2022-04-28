using Microsoft.EntityFrameworkCore;

public class EFProficiencyRepository : ProficiencyRepository
{
    private readonly DbSet<Proficiency> _proficiencies;

    public EFProficiencyRepository(EFDataContext dbContext)
    {
        _proficiencies = dbContext.Set<Proficiency>();
    }

    public void Add(Proficiency proficiency)
    {
        _proficiencies.Add(proficiency);
    }

    public async Task<IList<GetProficiencyDto>> GetAll()
    {
        return await _proficiencies.Select(_ => new GetProficiencyDto
        {
            Id = _.Id,
            Name = _.Name,
        }).ToListAsync();
    }

    public void Update(Proficiency proficiency)
    {
        _proficiencies.Update(proficiency);
    }

    public Proficiency Find(int id)
    {
        return
            _proficiencies.FirstOrDefault(_ => _.Id == id);
    }

    public void Delete(Proficiency proficiency)
    {
        _proficiencies.Remove(proficiency);
    }
}