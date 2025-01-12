using Api.Template.Domain.DataModels;
using Api.Template.Domain.Interfaces;
using Api.Template.Messages.DataProvider.Models;
using Api.Template.Shared.Interfaces;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Api.Template.Messages.DataProvider.DataProviders
{
    public class MessagesDataProvider(
        IEnvironmentConfig environmentConfig,
        ServiceBusClient serviceBusClient,
        ILogger<MessagesDataProvider> logger)
        : IMessagesDataProvider
    {
        private readonly IEnvironmentConfig _environmentConfig = environmentConfig ?? throw new ArgumentNullException(nameof(environmentConfig));
        private readonly ServiceBusClient _serviceBusClient = serviceBusClient ?? throw new ArgumentNullException(nameof(serviceBusClient));
        private readonly ILogger<MessagesDataProvider> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public async Task<UserDetail> PublishText(UserDetail userDetail)
        {
            var textMessage = new TextMessage
            {
                MessageType = MessageType.Text,
                UserDetail = userDetail
            };
            await PublishMessage(textMessage);

            _logger.LogInformation("Successfully published message into service bus topic.");
            return userDetail;
        }

        public async Task PublishFile(string fileName, Stream data, Dictionary<string, string> tags)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentNullException(nameof(fileName));

            if (data is not { Length: > 0 })
                throw new ArgumentNullException(nameof(data));

            using var streamReader = new BinaryReader(data);
            var streamData = streamReader.ReadBytes((int)data.Length);
            var fileMessage = new FileMessage
            {
                FileName = fileName,
                MessageType = MessageType.File,
                Data = streamData,
                Tags = tags
            };
            await PublishMessage(fileMessage);
        }

        #region Private Methods
        /// <summary>
        /// Publish message to the service bus topic.
        /// </summary>
        /// <typeparam name="T">Type of the message.</typeparam>
        /// <param name="message">Message which needs to be published to the service bus topic.</param>
        /// <returns>Task.</returns>
        private async Task PublishMessage<T>(T message) where T : BaseMessage
        {
            var sender = _serviceBusClient.CreateSender(_environmentConfig.ServiceBusTopic);

            var messageBody = JsonConvert.SerializeObject(message);
            var serviceBusMessage = new ServiceBusMessage(messageBody)
            {
                CorrelationId = Guid.NewGuid().ToString(),
                Subject = "test"
            };

            try
            {
                await sender.SendMessageAsync(serviceBusMessage);
            }
            finally
            {
                await sender.DisposeAsync();
            }
        }
        #endregion
    }
}
