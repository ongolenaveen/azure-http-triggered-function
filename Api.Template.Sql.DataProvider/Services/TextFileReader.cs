using System.IO.Abstractions;
using Api.Template.Sql.DataProvider.Interfaces;

namespace Api.Template.Sql.DataProvider.Services
{
    public class TextFileReader(IFileSystem fileSystem) : ITextFileReader
    {
        public TextFileReader() : this(new FileSystem())
        {
        }
        /// <summary>
        /// Read file content from a given file path.
        /// </summary>
        /// <param name="fileName">File path.</param>
        /// <returns>File content.</returns>
        public async Task<string?> ReadTextFile(string fileName)
        {
            if (!fileSystem.File.Exists(fileName))
                return null;

            var sqlQuery = await fileSystem.File.ReadAllTextAsync(fileName);
            return sqlQuery;
        }
    }
}
