using System;
using System.Collections.Generic;
using OtChaim.Domain.Common;

namespace OtChaim.Domain.Users
{
    public class User : Entity
    {
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string PhoneNumber { get; private set; }
        public bool IsActive { get; private set; }
        private readonly List<Guid> _subscriberIds = new();
        public IReadOnlyList<Guid> SubscriberIds => _subscriberIds.AsReadOnly();

        private User() { } // For EF Core

        public User(string name, string email, string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty", nameof(name));
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email cannot be empty", nameof(email));
            if (string.IsNullOrWhiteSpace(phoneNumber))
                throw new ArgumentException("Phone number cannot be empty", nameof(phoneNumber));

            Name = name;
            Email = email;
            PhoneNumber = phoneNumber;
            IsActive = true;
        }

        public void AddSubscriber(Guid subscriberId)
        {
            if (!_subscriberIds.Contains(subscriberId))
            {
                _subscriberIds.Add(subscriberId);
            }
        }

        public void RemoveSubscriber(Guid subscriberId)
        {
            _subscriberIds.Remove(subscriberId);
        }

        public void Deactivate()
        {
            IsActive = false;
        }

        public void Activate()
        {
            IsActive = true;
        }
    }
} 