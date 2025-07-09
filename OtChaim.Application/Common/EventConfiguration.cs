using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using OtChaim.Application.EmergencyEvents.EventSubscribers;
using OtChaim.Application.EmergencyEvents.Handlers;
using OtChaim.Application.Users.EventSubscribers;
using OtChaim.Application.Users.Handlers;
using OtChaim.Domain.EmergencyEvents.Events;
using OtChaim.Domain.Users.Events;
using System.Net.NetworkInformation;
using Yaref92.Events;
using Yaref92.Events.Abstractions;
using Yaref92.Events.Serialization;
using Yaref92.Events.Transports;

namespace OtChaim.Application.Common;

public static class EventConfiguration
{
    private const int ListenPort = 9000;

    public static IServiceCollection AddEventAggregator(this IServiceCollection services)
    {
        // Register event subscribers
        services.AddScoped<SubscriptionEventSubscriber>();
        services.AddScoped<EmergencyEventSubscriber>();
        services.AddScoped<EndEmergencyHandler>();

        // Register command handlers
        services.AddScoped<RequestSubscriptionHandler>();
        services.AddScoped<ApproveSubscriptionHandler>();
        services.AddScoped<RejectSubscriptionHandler>();
        services.AddScoped<StartEmergencyHandler>();
        services.AddScoped<MarkUserStatusHandler>();

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
