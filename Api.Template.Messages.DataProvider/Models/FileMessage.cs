namespace Api.Template.Messages.DataProvider.Models
{
    public class FileMessage : BaseMessage
    {
        public string? FileName { get; set; }

        public byte[]? Data { get; set; }

        public Dictionary<string, string> Tags { get; set; } = new();
    }
}
