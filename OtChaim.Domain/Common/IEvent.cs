namespace OtChaim.Domain.Common;

public interface IEvent
{
    DateTime OccurredOn { get; }
}