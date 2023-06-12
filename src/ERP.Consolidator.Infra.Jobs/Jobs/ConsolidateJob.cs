using ERP.Consolidator.Domain.Commands.EntryBalance;
using ERP.Core.Interfaces;
using Quartz;

namespace ERP.Consolidator.Infra.Jobs.Jobs
{
    public class ConsolidateJob : IJob
    {
        private readonly ICommandHandler<ConsolidateCommand, ConsolidateCommandResult> _consolidateCommandHandler;

        public ConsolidateJob(ICommandHandler<ConsolidateCommand, ConsolidateCommandResult> consolidateCommandHandler)
        {
            _consolidateCommandHandler = consolidateCommandHandler;
        }

        public Task Execute(IJobExecutionContext context)
        {
            var command = new ConsolidateCommand();

            _consolidateCommandHandler.Handle(command);

            return Task.CompletedTask;
        }
    }
}