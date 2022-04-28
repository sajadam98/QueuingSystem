using System;
public class DoctorTestFixture : IDisposable
{
    protected readonly Proficiency Proficiency;
    protected readonly EFDataContext DbContext;

    protected DoctorTestFixture()
    {
        DbContext =
            new EFInMemoryDatabase()
                .CreateDataContext<EFDataContext>();
        Proficiency = AddProficiencyInDataBase();
    }

    public void Dispose()
    {
    }

    private Proficiency AddProficiencyInDataBase()
    {
        var proficiency = ProficiencyFactory.GenerateProficiency("dummy");
        DbContext.Manipulate(_ =>
            DbContext.Set<Proficiency>().Add(proficiency));
        return proficiency;
    }
}