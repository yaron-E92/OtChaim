using System;
using System.Threading;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using OtChaim.Application.Users.Commands;
using OtChaim.Application.Users.Handlers;
using OtChaim.Domain.Users;
using OtChaim.Domain.Users.Events;
using Yaref92.Events.Abstractions;
using FluentAssertions;

namespace OtChaim.Application.Tests.Users.Handlers;

[TestFixture]
public class RejectSubscriptionHandlerTests
{
    [Test]
    public async Task Handle_RaisesSubscriptionRejectedEvent_AndSavesUser()
    {
        // Arrange
        var userRepository = Substitute.For<IUserRepository>();
        var eventAggregator = Substitute.For<IEventAggregator>();
        var subscriber = new User("Test Subscriber", "subscriber@example.com", "1234567890");
        var subscribedTo = new User("Test SubscribedTo", "subscribedto@example.com", "0987654321");
        var subscriberId = subscriber.Id;
        var subscribedToId = subscribedTo.Id;
        eventAggregator
            .WhenForAnyArgs(eA => eA.PublishEventAsync(Arg.Any<SubscriptionRequested>(), Arg.Any<CancellationToken>()))
            .Do(callInfo =>
            {
                // Simulate event publishing
                var subscriptionRequestedEvent = callInfo.Arg<SubscriptionRequested>();
                // Here you can add any additional logic to handle the event if needed
                subscribedTo.OnSubscriptionRequested(subscriptionRequestedEvent);
            });
        eventAggregator
            .WhenForAnyArgs((eA) => eA.PublishEventAsync(Arg.Any<SubscriptionRejected>(), Arg.Any<CancellationToken>()))
            .Do(callInfo =>
            {
                // Simulate event publishing
                var subscriptionRejectedEvent = callInfo.Arg<SubscriptionRejected>();
                // Here you can add any additional logic to handle the event if needed
                subscribedTo.OnSubscriptionRejected(subscriptionRejectedEvent);
            });

        // First request a subscription
        var requestHandler = new RequestSubscriptionHandler(userRepository, eventAggregator);
        var requestCommand = new RequestSubscription(subscriberId, subscribedToId);
        
        userRepository.GetByIdAsync(subscriberId, Arg.Any<CancellationToken>()).Returns(subscriber);
        userRepository.GetByIdAsync(subscribedToId, Arg.Any<CancellationToken>()).Returns(subscribedTo);
        
        await requestHandler.Handle(requestCommand);
        
        // Verify initial subscription status is Pending
        var subscription = subscribedTo.Subscriptions.First(s => s.SubscriberId == subscriberId && s.SubscribedToId == subscribedToId);
        subscription.Status.Should().Be(SubscriptionStatus.Pending);
        
        // Now reject the subscription
        var rejectHandler = new RejectSubscriptionHandler(userRepository, eventAggregator);
        var rejectCommand = new RejectSubscription(subscriberId, subscribedToId);

        // Act
        await rejectHandler.Handle(rejectCommand);

        // Assert
        // Verify subscription status is now Rejected
        subscription = subscribedTo.Subscriptions.First(s => s.SubscriberId == subscriberId && s.SubscribedToId == subscribedToId);
        subscription.Status.Should().Be(SubscriptionStatus.Rejected);
        
        await userRepository.Received().SaveAsync(subscriber, Arg.Any<CancellationToken>());
    }
} 
