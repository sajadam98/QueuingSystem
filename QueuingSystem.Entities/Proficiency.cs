public class Proficiency
{
    public Proficiency()
    {
        Doctors = new List<Doctor>();
    }
    public int Id { get; set; }
    public string Name { get; set; }
    public List<Doctor> Doctors { get; set; }
}