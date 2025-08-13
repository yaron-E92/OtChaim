using OtChaim.Domain.EmergencyEvents;
using OtChaim.Domain.EmergencyEvents.Events;
using Yaref92.Events.Abstractions;

namespace OtChaim.Application.EmergencyEvents.EventSubscribers;

/// <summary>
/// Subscribes to emergency-related domain events and updates the emergency repository accordingly.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="EmergencyEventSubscriber"/> class.
/// </remarks>
/// <param name="emergencyRepository">The emergency repository to update.</param>
/// <param name="eventAggregator">The event aggregator for publishing events.</param>
public class EmergencyEventSubscriber(IEmergencyRepository emergencyRepository, IEventAggregator eventAggregator) :
    IAsyncEventSubscriber<EmergencyStarted>,
    IAsyncEventSubscriber<EmergencyEnded>,
    IAsyncEventSubscriber<UserStatusMarked>,
    IAsyncEventSubscriber<SubscriberNotified>
{
    private readonly IEmergencyRepository _emergencyRepository = emergencyRepository;
    private readonly IEventAggregator _eventAggregator = eventAggregator;

    /// <summary>
    /// Handles the <see cref="EmergencyStarted"/> event by creating and persisting a new emergency.
    /// </summary>
    public async Task OnNextAsync(EmergencyStarted domainEvent, CancellationToken cancellationToken = default)
    {
        // First, raise EmergencyAltered event to signal the alteration is being processed
        var alteredEvent = new EmergencyAltered(domainEvent.EmergencyId, EmergencyAlterationType.Created);
        await _eventAggregator.PublishEventAsync(alteredEvent, cancellationToken);

        // Create and persist a new Emergency
        var emergency = new Emergency(domainEvent.EmergencyId, domainEvent.InitiatorUserId,
            domainEvent.Location.Clone(),
            [.. domainEvent.AffectedAreas],
            domainEvent.Type,
            domainEvent.Description,
            domainEvent.Attachments
        );
        await _emergencyRepository.AddAsync(emergency, cancellationToken);
        
        // Raise EmergencyAlterationPersisted event after successful persistence
        var persistedEvent = new EmergencyAlterationPersisted(domainEvent.EmergencyId, EmergencyAlterationType.Created);
        await _eventAggregator.PublishEventAsync(persistedEvent, cancellationToken);
    }

    /// <summary>
    /// Handles the <see cref="EmergencyEnded"/> event by resolving the emergency.
    /// </summary>
    public async Task OnNextAsync(EmergencyEnded domainEvent, CancellationToken cancellationToken = default)
    {
        // First, raise EmergencyAltered event to signal the alteration is being processed
        var alteredEvent = new EmergencyAltered(domainEvent.EmergencyId, EmergencyAlterationType.Resolved);
        await _eventAggregator.PublishEventAsync(alteredEvent, cancellationToken);

        Emergency? emergency = await _emergencyRepository.GetByIdAsync(domainEvent.EmergencyId, cancellationToken);
        if (emergency is not null)
        {
            emergency.Resolve();
            await _emergencyRepository.SaveAsync(emergency, cancellationToken);
            
            // Raise EmergencyAlterationPersisted event after successful persistence
            var persistedEvent = new EmergencyAlterationPersisted(domainEvent.EmergencyId, EmergencyAlterationType.Resolved);
            await _eventAggregator.PublishEventAsync(persistedEvent, cancellationToken);
        }
    }

    /// <summary>
    /// Handles the <see cref="UserStatusMarked"/> event by adding a response to the emergency.
    /// </summary>
    public async Task OnNextAsync(UserStatusMarked domainEvent, CancellationToken cancellationToken = default)
    {
        // First, raise EmergencyAltered event to signal the alteration is being processed
        var alteredEvent = new EmergencyAltered(domainEvent.EmergencyId, EmergencyAlterationType.StatusUpdated);
        await _eventAggregator.PublishEventAsync(alteredEvent, cancellationToken);

        Emergency? emergency = await _emergencyRepository.GetByIdAsync(domainEvent.EmergencyId, cancellationToken);
        if (emergency is not null)
        {
            emergency.AddResponse(domainEvent.UserId, domainEvent.Status == Domain.Users.UserStatus.Safe, domainEvent.Message);
            await _emergencyRepository.SaveAsync(emergency, cancellationToken);
            
            // Raise EmergencyAlterationPersisted event after successful persistence
            var persistedEvent = new EmergencyAlterationPersisted(domainEvent.EmergencyId, EmergencyAlterationType.StatusUpdated);
            await _eventAggregator.PublishEventAsync(persistedEvent, cancellationToken);
        }
    }

    /// <summary>
    /// Handles the <see cref="SubscriberNotified"/> event. (No-op)
    /// </summary>
    public Task OnNextAsync(SubscriberNotified domainEvent, CancellationToken cancellationToken = default)
    {
        // Implement notification logic as needed
        return Task.CompletedTask;
    }
}
