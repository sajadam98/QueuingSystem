public class ProficiencyFactory
{
    
    public static AddProficiencyDto GenerateAddProficiencyDto(string title)
    {
        var dto = new AddProficiencyDto
        {
            Name = title,
        };
        return dto;
    }

    public static Proficiency GenerateProficiency(string title)
    {
        return new Proficiency
        {
            Name = title,
        };
    }

    public static List<Proficiency> GenerateProficiencyList()
    {
        var proficiencies = new List<Proficiency>
        {
            new Proficiency {Name = "dummy1"},
            new Proficiency {Name = "dummy2"},
            new Proficiency {Name = "dummy3"}
        };

        return proficiencies;
    }
}