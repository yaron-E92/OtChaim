using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using OtChaim.Domain.EmergencyEvents;
using OtChaim.Domain.Common;

namespace OtChaim.Domain.Tests.EmergencyEvents;

[TestFixture]
public class EmergencyAreaAssociationTests
{
    [Test]
    public void Emergency_CanBeCreatedWithAffectedAreas()
    {
        // TODO: Assert affected areas are set
    }

    [Test]
    public void Emergency_DefaultsToAreaFromLocation_WhenAffectedAreasNotProvided()
    {
        // TODO: Assert default area logic
    }
} 