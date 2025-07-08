using OtChaim.Domain.EmergencyEvents.Events;
using Yaref92.Events.Abstractions;

namespace OtChaim.Application.EmergencyEvents.EventSubscribers;

public class EmergencyEventSubscriber :
    IAsyncEventSubscriber<EmergencyStarted>,
    IAsyncEventSubscriber<EmergencyEnded>,
    IAsyncEventSubscriber<UserStatusMarked>,
    IAsyncEventSubscriber<SubscriberNotified>
{
    // In a real application, you would inject repositories and services here
    // For now, this is a placeholder for the domain logic

    public async Task OnNextAsync(EmergencyStarted domainEvent, CancellationToken cancellationToken = default)
    {
        // Handle emergency situation started event
        // This could involve:
        // - Creating an Emergency entity
        // - Notifying subscribers in the affected area
        // - Logging the emergency
        throw new NotImplementedException();
    }

    public Task OnNextAsync(EmergencyEnded domainEvent, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task OnNextAsync(UserStatusMarked domainEvent, CancellationToken cancellationToken = default)
    {
        // Handle user status marked event
        // This could involve:
        // - Updating user status in the emergency
        // - Notifying emergency coordinators
        // - Logging the status change
        throw new NotImplementedException();
    }

    public Task OnNextAsync(SubscriberNotified domainEvent, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
