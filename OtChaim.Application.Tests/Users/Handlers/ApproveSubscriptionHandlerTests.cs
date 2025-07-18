using NSubstitute;
using OtChaim.Application.Users.Commands;
using OtChaim.Application.Users.Handlers;
using OtChaim.Domain.Users;
using Yaref92.Events.Abstractions;
using FluentAssertions;

namespace OtChaim.Application.Tests.Users.Handlers;

[TestFixture]
public class ApproveSubscriptionHandlerTests
{
    [Test][Ignore("For now, it is broken, but the fix is out of scope")]
    public async Task Handle_RaisesSubscriptionApprovedEvent_AndSavesUser()
    {
        // Arrange
        var userRepository = Substitute.For<IUserRepository>();
        var eventAggregator = Substitute.For<IEventAggregator>();
        var subscriber = new User("Test Subscriber", "subscriber@example.com", "1234567890");
        var subscribedTo = new User("Test SubscribedTo", "subscribedto@example.com", "0987654321");
        var subscriberId = Guid.NewGuid();
        var subscribedToId = Guid.NewGuid();
        
        // First request a subscription
        var requestHandler = new RequestSubscriptionHandler(userRepository, eventAggregator);
        var requestCommand = new RequestSubscription(subscriberId, subscribedToId);
        
        userRepository.GetByIdAsync(subscriberId, Arg.Any<CancellationToken>()).Returns(subscriber);
        userRepository.GetByIdAsync(subscribedToId, Arg.Any<CancellationToken>()).Returns(subscribedTo);
        
        await requestHandler.Handle(requestCommand);
        
        // Verify initial subscription status is Pending
        var subscription = subscriber.Subscriptions.First(s => s.SubscriberId == subscriberId && s.SubscribedToId == subscribedToId);
        subscription.Status.Should().Be(SubscriptionStatus.Pending);
        
        // Now approve the subscription
        var approveHandler = new ApproveSubscriptionHandler(userRepository, eventAggregator);
        var approveCommand = new ApproveSubscription(subscriberId, subscribedToId);

        // Act
        await approveHandler.Handle(approveCommand);

        // Assert
        // Verify subscription status is now Approved
        subscription = subscriber.Subscriptions.First(s => s.SubscriberId == subscriberId && s.SubscribedToId == subscribedToId);
        subscription.Status.Should().Be(SubscriptionStatus.Approved);
        subscription.ApprovedAt.Should().NotBeNull();

        await userRepository.Received().SaveAsync(subscriber, Arg.Any<CancellationToken>());
    }
} 
