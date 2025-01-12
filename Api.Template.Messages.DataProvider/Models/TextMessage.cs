using Api.Template.Domain.DataModels;

namespace Api.Template.Messages.DataProvider.Models
{
    public class TextMessage :BaseMessage
    {
        public UserDetail? UserDetail { get; set; }
    }
}
