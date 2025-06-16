namespace OtChaim.Domain.Common
{
    public interface IEventHandler<in TEvent> where TEvent : IEvent
    {
        Task Handle(TEvent domainEvent, CancellationToken cancellationToken = default);
    }
}