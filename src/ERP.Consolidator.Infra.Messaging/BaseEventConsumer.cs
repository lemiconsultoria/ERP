using ERP.Consolidator.Domain.Entities;
using ERP.Consolidator.Domain.Interfaces.Repositories;
using ERP.Core.Helpers;
using ERP.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace ERP.Consolidator.Infra.Messaging
{
    public abstract class BaseEventConsumer<TMessage, TPrimaryKey> :
        IEventConsumer<TMessage, TPrimaryKey>
        where TMessage : class, Core.Messages.IMessage<TPrimaryKey>
        where TPrimaryKey : struct
    {
        private readonly string _exchange;
        private readonly string _exchangeRetry;

        private readonly string _hostName;
        private readonly string _password;
        private readonly string _userName;

        protected string _queueName;
        protected string _queueNameRetry;

        protected readonly IDateToProcessRepository _dateToProcessRepository;

        public BaseEventConsumer(IConfiguration configuration, IDateToProcessRepository dateToProcessRepository)
        {
            _hostName = configuration.GetSection("RabbitMQ").GetSection("HostName").Value ?? "";
            _password = configuration.GetSection("RabbitMQ").GetSection("Password").Value ?? "";
            _userName = configuration.GetSection("RabbitMQ").GetSection("UserName").Value ?? "";
            _exchange = configuration.GetSection("RabbitMQ").GetSection("Exchange").Value ?? "";

            _exchangeRetry = $"{_exchange}_retry";

            _queueName = "";
            _queueNameRetry = $"{_queueName}_retry";

            _dateToProcessRepository = dateToProcessRepository;
        }

        public virtual void Subscribe()
        {
            var _connection = CreateConnection();

            var _channel = CreateModel(_connection);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (channel, evt) =>
            {
                try
                {
                    var content = Encoding.UTF8.GetString(evt.Body.ToArray());

                    TMessage message = JsonSerializer.Deserialize<TMessage>(content) ?? throw new ArgumentNullException(nameof(TMessage));

                    HandleMessage(message);
                }
                catch (Exception ex)
                {
                    _channel.BasicNack(evt.DeliveryTag, false, false);

                    LogHelper.RiseError(ex, "");

                    throw;
                }
                _channel.BasicAck(evt.DeliveryTag, false);
            };
            _channel.BasicConsume(_queueName, false, consumer);
        }

        public virtual void HandleMessage(TMessage message)
        {
        }

        private IConnection CreateConnection()
        {
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = _hostName,
                    UserName = _userName,
                    Password = _password,
                };

                return factory.CreateConnection();
            }
            catch (Exception ex)
            {
                LogHelper.RiseError(ex, "");
                throw;
            }
        }

        private IModel CreateModel(IConnection connection)
        {
            try
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
            catch (Exception ex)
            {
                LogHelper.RiseError(ex, "");
                throw;
            }
        }

        protected void RegisterDateToProcess(DateOnly dateOfIssue)
        {
            var dateNotExists = _dateToProcessRepository.DateNotExists(dateOfIssue);

            if (dateNotExists)
            {
                var dateToProcessEntity = new DateToProcess
                {
                    ReferenceDate = dateOfIssue
                };
                _dateToProcessRepository.Add(dateToProcessEntity);
            }
        }
    }
}