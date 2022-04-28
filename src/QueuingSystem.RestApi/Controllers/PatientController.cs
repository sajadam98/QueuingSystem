using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/patients")]
public class PatientController : Controller
{
    private readonly PatientService _service;

    public PatientController(PatientService service)
    {
        _service = service;
    }

    [HttpPost]
    public void CreatePatient(AddPatientDto dto)
    {
        _service.Add(dto);
    }

    [HttpGet]
    public async Task<IList<GetPatientDto>> GetAll()
    {
        return await _service.GetAll();
    }

    [HttpPut("{id}")]
    public void Update(int id, UpdatePatientDto dto)
    {
        _service.Update(id, dto);
    }

    [HttpPatch("{id}/deactivate")]
    public void DeActivate(int id)
    {
        _service.DeActivate(id);
    }
    
    [HttpPatch("{id}/activate")]
    public void Activate(int id)
    {
        _service.Activate(id);
    }

}