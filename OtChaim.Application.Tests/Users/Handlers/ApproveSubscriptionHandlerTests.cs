using FluentAssertions;
using NSubstitute;
using OtChaim.Application.Users.Commands;
using OtChaim.Application.Users.Handlers;
using OtChaim.Domain.Users;
using OtChaim.Domain.Users.Events;
using Yaref92.Events.Abstractions;

namespace OtChaim.Application.Tests.Users.Handlers;

[TestFixture]
public class ApproveSubscriptionHandlerTests
{
    [Test]
    public async Task Handle_RaisesSubscriptionApprovedEvent_AndSavesUser()
    {
        // Arrange
        var subscriber = new User("Test Subscriber", "subscriber@example.com", "1234567890");
        var subscribedTo = new User("Test SubscribedTo", "subscribedto@example.com", "0987654321");
        Guid subscriberId = subscriber.Id;
        Guid subscribedToId = subscribedTo.Id;
        IUserRepository userRepository = Substitute.For<IUserRepository>();
        IEventAggregator eventAggregator = Substitute.For<IEventAggregator>();
        eventAggregator
            .WhenForAnyArgs(eA => eA.PublishEventAsync(Arg.Any<SubscriptionRequested>(), Arg.Any<CancellationToken>()))
            .Do(callInfo =>
            {
                // Simulate event publishing
                SubscriptionRequested subscriptionRequestedEvent = callInfo.Arg<SubscriptionRequested>();
                // Here you can add any additional logic to handle the event if needed
                subscribedTo.OnSubscriptionRequested(subscriptionRequestedEvent);
            });
        eventAggregator
            .WhenForAnyArgs((eA) => eA.PublishEventAsync(Arg.Any<SubscriptionApproved>(), Arg.Any<CancellationToken>()))
            .Do(callInfo =>
            {
                // Simulate event publishing
                SubscriptionApproved subscriptionApprovedEvent = callInfo.Arg<SubscriptionApproved>();
                // Here you can add any additional logic to handle the event if needed
                subscribedTo.OnSubscriptionApproved(subscriptionApprovedEvent);
            });

        // First request a subscription
        var requestHandler = new RequestSubscriptionHandler(userRepository, eventAggregator);
        var requestCommand = new RequestSubscription(subscriberId, subscribedToId);

        userRepository.GetByIdAsync(subscriberId, Arg.Any<CancellationToken>()).Returns(subscriber);
        userRepository.GetByIdAsync(subscribedToId, Arg.Any<CancellationToken>()).Returns(subscribedTo);

        await requestHandler.Handle(requestCommand);

        // Verify initial subscription status is Pending
        Subscription subscription = subscribedTo.Subscriptions.First(s => s.SubscriberId == subscriberId && s.SubscribedToId == subscribedToId);
        subscription.Status.Should().Be(SubscriptionStatus.Pending);

        // Now approve the subscription
        var approveHandler = new ApproveSubscriptionHandler(userRepository, eventAggregator);
        var approveCommand = new ApproveSubscription(subscriberId, subscribedToId);

        // Act
        await approveHandler.Handle(approveCommand);

        // Assert
        // Verify subscription status is now Approved
        subscription = subscribedTo.Subscriptions.First(s => s.SubscriberId == subscriberId && s.SubscribedToId == subscribedToId);
        subscription.Status.Should().Be(SubscriptionStatus.Approved);
        subscription.ApprovedAt.Should().NotBeNull();

        await userRepository.Received().SaveAsync(subscriber, Arg.Any<CancellationToken>());
    }
}
