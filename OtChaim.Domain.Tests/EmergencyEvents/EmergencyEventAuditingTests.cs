using System;
using FluentAssertions;
using NUnit.Framework;
using OtChaim.Domain.EmergencyEvents;

namespace OtChaim.Domain.Tests.EmergencyEvents;

[TestFixture]
public class EmergencyEventAuditingTests
{
    [Test]
    public void CreatingEmergency_IsAuditable()
    {
        // TODO: Assert audit log or event is created
    }

    [Test]
    public void UpdatingEmergency_IsAuditable()
    {
        // TODO: Assert audit log or event is created
    }
} 