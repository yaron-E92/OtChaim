using Microsoft.Extensions.DependencyInjection;
using OtChaim.Application.EmergencyEvents.EventSubscribers;
using OtChaim.Application.EmergencyEvents.Handlers;
using OtChaim.Application.Users.EventSubscribers;
using OtChaim.Application.Users.Handlers;
using OtChaim.Domain.EmergencyEvents.Events;
using OtChaim.Domain.Users.Events;
using Yaref92.Events;
using Yaref92.Events.Abstractions;

namespace OtChaim.Application.Common;

public static class EventConfiguration
{
    public static IServiceCollection AddEventAggregator(this IServiceCollection services)
    {
        // Register the event aggregator as a singleton
        services.AddSingleton<IEventAggregator, EventAggregator>();
        
        // Register event subscribers
        services.AddScoped<SubscriptionEventSubscriber>();
        services.AddScoped<EmergencyEventSubscriber>();
        
        // Register command handlers
        services.AddScoped<RequestSubscriptionHandler>();
        services.AddScoped<ApproveSubscriptionHandler>();
        services.AddScoped<RejectSubscriptionHandler>();
        services.AddScoped<StartEmergencySituationHandler>();
        services.AddScoped<MarkUserStatusHandler>();
        
        return services;
    }

    public static IEventAggregator ConfigureEventAggregator(IEventAggregator eventAggregator, IServiceProvider serviceProvider)
    {
        // Register all event types
        eventAggregator.RegisterEventType<SubscriptionRequested>();
        eventAggregator.RegisterEventType<SubscriptionApproved>();
        eventAggregator.RegisterEventType<SubscriptionRejected>();
        eventAggregator.RegisterEventType<EmergencySituationStarted>();
        eventAggregator.RegisterEventType<EmergencySituationEnded>();
        eventAggregator.RegisterEventType<UserStatusMarked>();
        eventAggregator.RegisterEventType<SubscriberNotified>();

        // Subscribe to events
        SubscriptionEventSubscriber subscriptionSubscriber = serviceProvider.GetRequiredService<SubscriptionEventSubscriber>();
        eventAggregator.SubscribeToEventType<SubscriptionRequested>(subscriptionSubscriber);
        eventAggregator.SubscribeToEventType<SubscriptionApproved>(subscriptionSubscriber);
        eventAggregator.SubscribeToEventType<SubscriptionRejected>(subscriptionSubscriber);

        EmergencyEventSubscriber emergencySubscriber = serviceProvider.GetRequiredService<EmergencyEventSubscriber>();
        eventAggregator.SubscribeToEventType<EmergencySituationStarted>(emergencySubscriber);
        eventAggregator.SubscribeToEventType<EmergencySituationEnded>(emergencySubscriber);
        eventAggregator.SubscribeToEventType<UserStatusMarked>(emergencySubscriber);
        eventAggregator.SubscribeToEventType<SubscriberNotified>(emergencySubscriber);

        return eventAggregator;
    }
} 
