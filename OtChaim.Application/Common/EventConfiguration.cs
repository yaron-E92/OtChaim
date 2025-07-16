using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using OtChaim.Application.EmergencyEvents.Commands;
using OtChaim.Application.EmergencyEvents.EventSubscribers;
using OtChaim.Application.EmergencyEvents.Handlers;
using OtChaim.Application.Users.Commands;
using OtChaim.Application.Users.EventSubscribers;
using OtChaim.Application.Users.Handlers;
using OtChaim.Domain.EmergencyEvents.Events;
using OtChaim.Domain.Users.Events;
using Yaref92.Events;
using Yaref92.Events.Abstractions;
using Yaref92.Events.Serialization;
using Yaref92.Events.Transports;

namespace OtChaim.Application.Common;

/// <summary>
/// Provides extension methods for configuring event aggregation and dependency injection.
/// </summary>
public static class EventConfiguration
{
    private const int ListenPort = 9000;

    /// <summary>
    /// Adds and configures the event aggregator and related services to the service collection.
    /// </summary>
    /// <param name="services">The service collection to configure.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddEventAggregator(this IServiceCollection services)
    {
        // Register event subscribers
        services.AddScoped<SubscriptionEventSubscriber>();
        services.AddScoped<EmergencyEventSubscriber>();

        // Register command handlers
        services.AddScoped<ICommandHandler<RequestSubscription>, RequestSubscriptionHandler>();
        services.AddScoped<ICommandHandler<ApproveSubscription>, ApproveSubscriptionHandler>();
        services.AddScoped<ICommandHandler<RejectSubscription>, RejectSubscriptionHandler>();
        services.AddScoped<ICommandHandler<StartEmergency>, StartEmergencyHandler>();
        services.AddScoped<ICommandHandler<EndEmergency>, EndEmergencyHandler>();
        services.AddScoped<ICommandHandler<MarkUserStatus>, MarkUserStatusHandler>();

        // Register logger
        services.AddSingleton<ILoggerFactory, NullLoggerFactory>(); // TODO: put an actual logger factory
        services.AddSingleton<ILogger<EventAggregator>, Logger<EventAggregator>>();

        // Register the networked aggregator as a singleton
        services.AddSingleton<IEventAggregator>(provider =>
        {
            ILogger<EventAggregator>? logger = provider.GetService<ILogger<EventAggregator>>();
            EventAggregator localAggregator = new EventAggregator(logger);
            JsonEventSerializer serializer = new JsonEventSerializer();
            TCPEventTransport transport = new TCPEventTransport(ListenPort, serializer); // Choose your port
            NetworkedEventAggregator networkedAggregator = new NetworkedEventAggregator(localAggregator, transport);
            transport.StartListeningAsync();

            // Optionally connect to peers here, or expose transport for runtime connections
            // await transport.ConnectToPeerAsync("remotehost", 9000);

            ConfigureEventAggregator(networkedAggregator, provider);

            return networkedAggregator;
        });

        return services;
    }

    /// <summary>
    /// Registers event types and subscribes event subscribers to the event aggregator.
    /// </summary>
    /// <param name="eventAggregator">The event aggregator to configure.</param>
    /// <param name="serviceProvider">The service provider for resolving subscribers.</param>
    /// <returns>The configured event aggregator.</returns>
    public static IEventAggregator ConfigureEventAggregator(IEventAggregator eventAggregator, IServiceProvider serviceProvider)
    {
        // Register all event types
        eventAggregator.RegisterEventType<SubscriptionRequested>();
        eventAggregator.RegisterEventType<SubscriptionApproved>();
        eventAggregator.RegisterEventType<SubscriptionRejected>();
        eventAggregator.RegisterEventType<EmergencyStarted>();
        eventAggregator.RegisterEventType<EmergencyEnded>();
        eventAggregator.RegisterEventType<UserStatusMarked>();
        eventAggregator.RegisterEventType<SubscriberNotified>();

        // Subscribe to events
        SubscriptionEventSubscriber subscriptionSubscriber = serviceProvider.GetRequiredService<SubscriptionEventSubscriber>();
        eventAggregator.SubscribeToEventType<SubscriptionRequested>(subscriptionSubscriber);
        eventAggregator.SubscribeToEventType<SubscriptionApproved>(subscriptionSubscriber);
        eventAggregator.SubscribeToEventType<SubscriptionRejected>(subscriptionSubscriber);

        EmergencyEventSubscriber emergencySubscriber = serviceProvider.GetRequiredService<EmergencyEventSubscriber>();
        eventAggregator.SubscribeToEventType<EmergencyStarted>(emergencySubscriber);
        eventAggregator.SubscribeToEventType<EmergencyEnded>(emergencySubscriber);
        eventAggregator.SubscribeToEventType<UserStatusMarked>(emergencySubscriber);
        eventAggregator.SubscribeToEventType<SubscriberNotified>(emergencySubscriber);

        return eventAggregator;
    }
}
