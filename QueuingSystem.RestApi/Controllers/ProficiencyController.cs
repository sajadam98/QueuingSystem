using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/Proficiencies")]
public class ProficiencyController : Controller
{
    private readonly ProficiencyService _service;

    public ProficiencyController(ProficiencyService service)
    {
        _service = service;
    }

    [HttpPost]
    public void CreateProficiency(AddProficiencyDto dto)
    {
        _service.Add(dto);
    }

    [HttpGet]
    public async Task<IList<GetProficiencyDto>> GetAll()
    {
        return await _service.GetAll();
    }

    [HttpPut("{id}")]
    public void Update(int id, UpdateProficiencyDto dto)
    {
        _service.Update(id,  dto);
    }

    [HttpDelete("{id}")]
    public void Delete(int id)
    {
        _service.Delete(id);
    }
}