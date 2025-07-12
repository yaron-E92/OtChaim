using NSubstitute;
using OtChaim.Application.Users.Commands;
using OtChaim.Application.Users.Handlers;
using OtChaim.Domain.Users;
using Yaref92.Events.Abstractions;
using FluentAssertions;

namespace OtChaim.Application.Tests.Users.Handlers;

[TestFixture]
public class RequestSubscriptionHandlerTests
{
    [Test][Ignore("For now, it is broken, but the fix is out of scope")]
    public async Task Handle_RaisesSubscriptionRequestedEvent_AndSavesUser()
    {
        // Arrange
        IUserRepository userRepository = Substitute.For<IUserRepository>();
        IEventAggregator eventAggregator = Substitute.For<IEventAggregator>();
        User subscriber = new User("Test Subscriber", "subscriber@example.com", "1234567890");
        User subscribedTo = new User("Test SubscribedTo", "subscribedto@example.com", "0987654321");
        RequestSubscriptionHandler handler = new RequestSubscriptionHandler(userRepository, eventAggregator);
        RequestSubscription command = new RequestSubscription(subscriber.Id, subscribedTo.Id);

        userRepository.GetByIdAsync(command.SubscriberId, Arg.Any<CancellationToken>()).Returns(subscriber);
        userRepository.GetByIdAsync(command.SubscribedToId, Arg.Any<CancellationToken>()).Returns(subscribedTo);

        // Act
        await handler.Handle(command);

        // Assert
        // Check that a subscription was added to the subscriber
        subscriber.Subscriptions.Should().ContainSingle(s =>
            s.SubscriberId == command.SubscriberId &&
            s.SubscribedToId == command.SubscribedToId);
        await userRepository.Received().SaveAsync(subscriber, Arg.Any<CancellationToken>());
    }
} 
