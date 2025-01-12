namespace Api.Template.Sql.DataProvider.Interfaces
{
    public interface ITextFileReader
    {
        Task<string?> ReadTextFile(string fileName);
    }
}
