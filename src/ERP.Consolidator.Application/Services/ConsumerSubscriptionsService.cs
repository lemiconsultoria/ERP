using ERP.Core.Interfaces;
using ERP.Domain.Events;
using Microsoft.Extensions.Hosting;

namespace ERP.Consolidator.Application.Services
{
    public class ConsumerSubscriptionsService : BackgroundService
    {
        private readonly IEventConsumer<CreditEntryEvent, Guid> _creditEntryEventConsumer;
        private readonly IEventConsumer<DebitEntryEvent, Guid> _debitEntryEventConsumer;

        public ConsumerSubscriptionsService(
            IEventConsumer<CreditEntryEvent, Guid> creditEntryEventConsumer,
            IEventConsumer<DebitEntryEvent, Guid> debitEntryEventConsumer
            )
        {
            _creditEntryEventConsumer = creditEntryEventConsumer;
            _debitEntryEventConsumer = debitEntryEventConsumer;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            _creditEntryEventConsumer.Subscribe();
            _debitEntryEventConsumer.Subscribe();
            return Task.CompletedTask;
        }
    }
}