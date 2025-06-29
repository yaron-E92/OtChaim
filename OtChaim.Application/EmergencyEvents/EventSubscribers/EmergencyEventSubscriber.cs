using System.Threading;
using System.Threading.Tasks;
using OtChaim.Domain.EmergencyEvents.Events;
using Yaref92.Events;

namespace OtChaim.Application.EmergencyEvents.EventSubscribers;

public class EmergencyEventSubscriber : 
    IAsyncEventSubscriber<EmergencySituationStarted>,
    IAsyncEventSubscriber<UserStatusMarked>
{
    // In a real application, you would inject repositories and services here
    // For now, this is a placeholder for the domain logic

    public async Task OnNextAsync(EmergencySituationStarted @event, CancellationToken cancellationToken = default)
    {
        // Handle emergency situation started event
        // This could involve:
        // - Creating an Emergency entity
        // - Notifying subscribers in the affected area
        // - Logging the emergency
        await Task.CompletedTask;
    }

    public async Task OnNextAsync(UserStatusMarked @event, CancellationToken cancellationToken = default)
    {
        // Handle user status marked event
        // This could involve:
        // - Updating user status in the emergency
        // - Notifying emergency coordinators
        // - Logging the status change
        await Task.CompletedTask;
    }
}