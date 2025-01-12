namespace Api.Template.Domain.ReadModels
{
    public class RequestParam
    {
        public string? Filter { get; set; }

        public string? Sort { get; set; }

        public int? Offset { get; set; }

        public int? Limit { get; set; }
    }
}
