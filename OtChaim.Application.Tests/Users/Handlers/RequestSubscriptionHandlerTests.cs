using FluentAssertions;
using NSubstitute;
using OtChaim.Application.Users.Commands;
using OtChaim.Application.Users.Handlers;
using OtChaim.Domain.Users;
using OtChaim.Domain.Users.Events;
using Yaref92.Events.Abstractions;

namespace OtChaim.Application.Tests.Users.Handlers;

[TestFixture]
public class RequestSubscriptionHandlerTests
{
    [Test]
    public async Task Handle_RaisesSubscriptionRequestedEvent_AndSavesUser()
    {
        // Arrange
        IUserRepository userRepository = Substitute.For<IUserRepository>();
        IEventAggregator eventAggregator = Substitute.For<IEventAggregator>();
        User subscriber = new User("Test Subscriber", "subscriber@example.com", "1234567890");
        User subscribedTo = new User("Test SubscribedTo", "subscribedto@example.com", "0987654321");
        eventAggregator
            .WhenForAnyArgs(eA => eA.PublishEventAsync(Arg.Any<SubscriptionRequested>(), Arg.Any<CancellationToken>()))
            .Do(callInfo =>
            {
                // Simulate event publishing
                var subscriptionRequestedEvent = callInfo.Arg<SubscriptionRequested>();
                // Here you can add any additional logic to handle the event if needed
                subscribedTo.OnSubscriptionRequested(subscriptionRequestedEvent);
            });
        RequestSubscriptionHandler handler = new RequestSubscriptionHandler(userRepository, eventAggregator);
        RequestSubscription command = new RequestSubscription(subscriber.Id, subscribedTo.Id);

        userRepository.GetByIdAsync(command.SubscriberId, Arg.Any<CancellationToken>()).Returns(subscriber);
        userRepository.GetByIdAsync(command.SubscribedToId, Arg.Any<CancellationToken>()).Returns(subscribedTo);

        // Act
        await handler.Handle(command);

        // Assert
        // Check that a subscription was added to the subscriber
        subscribedTo.Subscriptions.Should().ContainSingle(s =>
            s.SubscriberId == command.SubscriberId &&
            s.SubscribedToId == command.SubscribedToId);
        await userRepository.Received().SaveAsync(subscriber, Arg.Any<CancellationToken>());
    }
}
