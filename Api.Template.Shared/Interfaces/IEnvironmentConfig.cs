namespace Api.Template.Shared.Interfaces
{
    public interface IEnvironmentConfig
    {
        public string ServiceBusNameSpace { get;}

        public string ServiceBusTopic { get; }

    }
}