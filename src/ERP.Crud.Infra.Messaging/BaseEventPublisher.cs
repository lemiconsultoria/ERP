using ERP.Core.Helpers;
using ERP.Core.Interfaces;
using ERP.Core.Messages;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace ERP.Crud.Infra.Messaging
{
    public abstract class BaseEventPublisher<TMessage> : IEventPublisher<TMessage> where TMessage : Message
    {
        private readonly string _exchange;
        private readonly string _exchangeRetry;

        private readonly string _hostName;
        private readonly string _password;
        private readonly string _userName;

        protected string _queueName;
        protected string _queueNameRetry;

        public BaseEventPublisher(IConfiguration configuration)
        {
            _hostName = configuration.GetSection("RabbitMQ")?.GetSection("HostName").Value ?? "";
            _password = configuration.GetSection("RabbitMQ")?.GetSection("Password").Value ?? "";
            _userName = configuration.GetSection("RabbitMQ")?.GetSection("UserName").Value ?? "";
            _exchange = configuration.GetSection("RabbitMQ")?.GetSection("Exchange").Value ?? "";

            _exchangeRetry = $"{_exchange}_retry";

            _queueName = "";
            _queueNameRetry = $"{_queueName}_retry";
        }

        public bool Publish(TMessage message)
        {
            try
            {
                var _connection = CreateConnection();

                using var channel = CreateModel(_connection);

                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;

                byte[] body = GetMessageAsByteArray(message);

                channel.BasicPublish(exchange: _exchange, routingKey: _queueName, basicProperties: properties, body: body);

                return true;
            }
            catch (Exception ex)
            {
                LogHelper.RiseError(ex);
                return false;
            }
        }

        private IConnection CreateConnection()
        {
            var factory = new ConnectionFactory
            {
                HostName = _hostName,
                UserName = _userName,
                Password = _password,
            };

            return factory.CreateConnection();
        }

        private IModel CreateModel(IConnection connection)
        {
            var channel = connection.CreateModel();

            channel.ExchangeDeclare(_exchange, ExchangeType.Direct, durable: true);

            var _queueNameProps = new Dictionary<string, object>
                    {
                        {"x-dead-letter-exchange", _exchangeRetry},
                        {"x-dead-letter-routing-key", _queueNameRetry}
                    };

            channel.QueueDeclare(_queueName, true, false, false, _queueNameProps);

            channel.QueueBind(queue: _queueName,
                              exchange: _exchange,
                              routingKey: _queueName);

            channel.ExchangeDeclare(_exchangeRetry, ExchangeType.Direct, durable: true);

            var _queueNameRetrysProps = new Dictionary<string, object>
                    {
                        {"x-dead-letter-exchange", _exchange},
                        {"x-dead-letter-routing-key", _queueName},
                        {"x-message-ttl", 30000}
                    };

            channel.QueueDeclare(_queueNameRetry, true, false, false, _queueNameRetrysProps);

            channel.QueueBind(queue: _queueNameRetry,
                              exchange: _exchangeRetry,
                              routingKey: _queueNameRetry);

            return channel;
        }

        private byte[] GetMessageAsByteArray(TMessage message)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize<TMessage>(message, options);
            var body = Encoding.UTF8.GetBytes(json);
            return body;
        }
    }
}