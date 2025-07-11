using OtChaim.Domain.EmergencyEvents;
using OtChaim.Domain.EmergencyEvents.Events;
using Yaref92.Events.Abstractions;

namespace OtChaim.Application.EmergencyEvents.EventSubscribers;

public class EmergencyEventSubscriber :
    IAsyncEventSubscriber<EmergencyStarted>,
    IAsyncEventSubscriber<EmergencyEnded>,
    IAsyncEventSubscriber<UserStatusMarked>,
    IAsyncEventSubscriber<SubscriberNotified>
{
    private readonly IEmergencyRepository _emergencyRepository;

    public EmergencyEventSubscriber(IEmergencyRepository emergencyRepository)
    {
        _emergencyRepository = emergencyRepository;
    }

    public async Task OnNextAsync(EmergencyStarted domainEvent, CancellationToken cancellationToken = default)
    {
        // Create and persist a new Emergency
        var emergency = new Emergency(
            domainEvent.Location.Clone(),
            new[] { domainEvent.AffectedArea },
            domainEvent.Severity,
            domainEvent.EmergencyType
        );
        await _emergencyRepository.AddAsync(emergency, cancellationToken);
    }

    public async Task OnNextAsync(EmergencyEnded domainEvent, CancellationToken cancellationToken = default)
    {
        var emergency = await _emergencyRepository.GetByIdAsync(domainEvent.EmergencyId, cancellationToken);
        if (emergency != null)
        {
            emergency.Resolve();
            await _emergencyRepository.SaveAsync(emergency, cancellationToken);
        }
    }

    public async Task OnNextAsync(UserStatusMarked domainEvent, CancellationToken cancellationToken = default)
    {
        var emergency = await _emergencyRepository.GetByIdAsync(domainEvent.EmergencyId, cancellationToken);
        if (emergency != null)
        {
            emergency.AddResponse(domainEvent.UserId, domainEvent.Status == Domain.Users.UserStatus.Safe, domainEvent.Message);
            await _emergencyRepository.SaveAsync(emergency, cancellationToken);
        }
    }

    public Task OnNextAsync(SubscriberNotified domainEvent, CancellationToken cancellationToken = default)
    {
        // Implement notification logic as needed
        return Task.CompletedTask;
    }
}
