public class Patient : Person
{
    public Patient()
    {
        Appointments = new List<Appointment>();
    }
    public List<Appointment> Appointments { get; set; }
}