using NSubstitute;
using OtChaim.Application.Users.Commands;
using OtChaim.Application.Users.Handlers;
using OtChaim.Domain.Users;
using Yaref92.Events.Abstractions;

namespace OtChaim.Application.Tests.Users.Handlers;

[TestFixture]
public class RequestSubscriptionHandlerTests
{
    [Test]
    public async Task Handle_RaisesSubscriptionRequestedEvent_AndSavesUser()
    {
        // Arrange
        var userRepository = Substitute.For<IUserRepository>();
        var eventAggregator = Substitute.For<IEventAggregator>();
        var subscriber = new User("Test Subscriber", "subscriber@example.com", "1234567890");
        var subscribedTo = new User("Test SubscribedTo", "subscribedto@example.com", "0987654321");
        var handler = new RequestSubscriptionHandler(userRepository, eventAggregator);
        var command = new RequestSubscription(Guid.NewGuid(), Guid.NewGuid());

        userRepository.GetByIdAsync(command.SubscriberId, Arg.Any<CancellationToken>()).Returns(subscriber);
        userRepository.GetByIdAsync(command.SubscribedToId, Arg.Any<CancellationToken>()).Returns(subscribedTo);

        // Act
        await handler.Handle(command);

        // Assert
        // Check that a subscription was added to the subscriber
        Assert.That(subscriber.Subscriptions, Has.Some.Matches<Subscription>(s =>
            s.SubscriberId == command.SubscriberId &&
            s.SubscribedToId == command.SubscribedToId));
        await userRepository.Received().SaveAsync(subscriber, Arg.Any<CancellationToken>());
    }
} 
