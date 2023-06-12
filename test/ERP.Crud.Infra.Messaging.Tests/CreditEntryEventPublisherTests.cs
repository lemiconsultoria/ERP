using ERP.Core.Interfaces;
using ERP.Domain.Events;
using Xunit;

namespace ERP.Crud.Infra.Messaging.Tests
{
    public class CreditEntryEventPublisherTests
    {
        [Fact]
        public void Should_Publish_Events_Is_Valid()
        {
            var _configuration = TestHelper.BuildConfiguration(TestHelper.GetPathOfTest(), "appsettings.json");

            IEventPublisher<CreditEntryEvent> _eventPublisher = new CreditEntryEventPublisher<CreditEntryEvent>(_configuration);

            var command = new CreditEntryEvent
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

            IEventPublisher<CreditEntryEvent> _eventPublisher = new CreditEntryEventPublisher<CreditEntryEvent>(_configuration);

            var command = new CreditEntryEvent
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