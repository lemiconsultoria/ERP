using ERP.Core.Interfaces;
using ERP.Domain.Events;
using Xunit;

namespace ERP.Crud.Infra.Messaging.Tests
{
    public class DebitEntryEventPublisherTests
    {
        [Fact]
        public void Should_Publish_Events_Is_Valid()
        {
            var _configuration = TestHelper.BuildConfiguration(TestHelper.GetPathOfTest(), "appsettings.json");

            IEventPublisher<DebitEntryEvent> _eventPublisher = new DebitEntryEventPublisher<DebitEntryEvent>(_configuration);

            var command = new DebitEntryEvent
            {
                Id = 1,
                DateOfIssue = DateTime.Now,
                Value = 123
            };
            var isSuccess = _eventPublisher.Publish(command);
            Assert.True(isSuccess);
        }

        [Fact]
        public void Should_Publish_Events_Is_InValid()
        {
            var _configuration = TestHelper.BuildConfiguration(TestHelper.GetPathOfTest(), "arq_invalid");

            IEventPublisher<DebitEntryEvent> _eventPublisher = new DebitEntryEventPublisher<DebitEntryEvent>(_configuration);

            var command = new DebitEntryEvent
            {
                Id = 1,
                DateOfIssue = DateTime.Now,
                Value = 123
            };
            var isSuccess = _eventPublisher.Publish(command);
            Assert.True(!isSuccess);
        }
    }
}