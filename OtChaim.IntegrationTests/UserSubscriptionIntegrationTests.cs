using Microsoft.Extensions.DependencyInjection;
using OtChaim.Application.Users.Commands;
using OtChaim.Application.Users.Handlers;
using OtChaim.Domain.Users;
using FluentAssertions;

namespace OtChaim.IntegrationTests;

public class UserSubscriptionIntegrationTests : IntegrationTestBase
{
    [Test]
    [Explicit("Integration test for requesting a subscription.")]
    [Category("Integration")]
    public async Task RequestSubscriptionHandler_AddsSubscriptionToSubscribedToUser()
    {
        // Arrange
        IUserRepository userRepository = Provider.GetRequiredService<IUserRepository>();
        RequestSubscriptionHandler handler = Provider.GetRequiredService<RequestSubscriptionHandler>();
        var subscriber = new User("subscriber", "subscriber@test.com", "0000000");
        var subscribedTo = new User("subscribedTo", "subscribedto@test.com", "1111111");
        await userRepository.AddAsync(subscriber);
        await userRepository.AddAsync(subscribedTo);
        Guid subscriberId = subscriber.Id;
        Guid subscribedToId = subscribedTo.Id;
        var command = new RequestSubscription(subscriberId, subscribedToId);

        // Act
        await handler.Handle(command);
        User updatedSubscribedTo = await userRepository.GetByIdAsync(subscribedToId);

        // Assert
        updatedSubscribedTo.Subscriptions.Should().NotBeEmpty();
        updatedSubscribedTo.Subscriptions[0].SubscriberId.Should().Be(subscriberId);
    }

    [Test]
    [Explicit("Integration test for approving a subscription.")]
    [Category("Integration")]
    public async Task ApproveSubscriptionHandler_ApprovesSubscription()
    {
        // Arrange
        var userRepository = Provider.GetRequiredService<IUserRepository>();
        var requestHandler = Provider.GetRequiredService<RequestSubscriptionHandler>();
        var approveHandler = Provider.GetRequiredService<ApproveSubscriptionHandler>();
        var subscriber = new User("subscriber", "subscriber@test.com", "0000000");
        var subscribedTo = new User("subscribedTo", "subscribedto@test.com", "1111111");
        await userRepository.AddAsync(subscriber);
        await userRepository.AddAsync(subscribedTo);
        var requestCommand = new RequestSubscription(subscriber.Id, subscribedTo.Id);
        await requestHandler.Handle(requestCommand);
        var approveCommand = new ApproveSubscription(subscriber.Id, subscribedTo.Id);

        // Act
        await approveHandler.Handle(approveCommand);
        var updatedSubscribedTo = await userRepository.GetByIdAsync(subscribedTo.Id);

        // Assert
        updatedSubscribedTo.Subscriptions.Should().NotBeEmpty();
        updatedSubscribedTo.Subscriptions[0].SubscriberId.Should().Be(subscriber.Id);
        updatedSubscribedTo.Subscriptions[0].Status.Should().Be(SubscriptionStatus.Approved);
    }

    [Test]
    [Explicit("Integration test for rejecting a subscription.")]
    [Category("Integration")]
    public async Task RejectSubscriptionHandler_RejectsSubscription()
    {
        // Arrange
        var userRepository = Provider.GetRequiredService<IUserRepository>();
        var requestHandler = Provider.GetRequiredService<RequestSubscriptionHandler>();
        var rejectHandler = Provider.GetRequiredService<RejectSubscriptionHandler>();
        var subscriber = new User("subscriber", "subscriber@test.com", "0000000");
        var subscribedTo = new User("subscribedTo", "subscribedto@test.com", "1111111");
        await userRepository.AddAsync(subscriber);
        await userRepository.AddAsync(subscribedTo);
        var requestCommand = new RequestSubscription(subscriber.Id, subscribedTo.Id);
        await requestHandler.Handle(requestCommand);
        var rejectCommand = new RejectSubscription(subscriber.Id, subscribedTo.Id);

        // Act
        await rejectHandler.Handle(rejectCommand);
        var updatedSubscribedTo = await userRepository.GetByIdAsync(subscribedTo.Id);

        // Assert
        updatedSubscribedTo.Subscriptions.Should().NotBeEmpty();
        updatedSubscribedTo.Subscriptions[0].SubscriberId.Should().Be(subscriber.Id);
        updatedSubscribedTo.Subscriptions[0].Status.Should().Be(SubscriptionStatus.Rejected);
    }
}
