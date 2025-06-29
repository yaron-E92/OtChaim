using Microsoft.Extensions.DependencyInjection;
using OtChaim.Application.Users.EventSubscribers;
using OtChaim.Domain.EmergencyEvents.Events;
using OtChaim.Domain.Users.Events;
using Yaref92.Events;

namespace OtChaim.Application.Common;

public static class EventConfiguration
{
    public static IServiceCollection AddEventAggregator(this IServiceCollection services)
    {
        // Register the event aggregator as a singleton
        services.AddSingleton<IEventAggregator, EventAggregator>();
        
        // Register event subscribers
        services.AddScoped<SubscriptionEventSubscriber>();
        
        return services;
    }

    public static IEventAggregator ConfigureEventAggregator(IEventAggregator eventAggregator, IServiceProvider serviceProvider)
    {
        // Register all event types
        eventAggregator.RegisterEventType<SubscriptionRequested>();
        eventAggregator.RegisterEventType<SubscriptionApproved>();
        eventAggregator.RegisterEventType<SubscriptionRejected>();
        eventAggregator.RegisterEventType<UserStatusMarked>();
        eventAggregator.RegisterEventType<SubscriberNotified>();

        // Subscribe to events
        var subscriptionSubscriber = serviceProvider.GetRequiredService<SubscriptionEventSubscriber>();
        eventAggregator.SubscribeToEventType(subscriptionSubscriber);

        return eventAggregator;
    }
} 