using FluentAssertions;
using NSubstitute;
using OtChaim.Application.Services;
using OtChaim.Domain.EmergencyEvents;
using OtChaim.Domain.Users;
using System.Collections.ObjectModel;

namespace OtChaim.Application.Tests.Services;

[TestFixture]
public class EmergencyDataServiceTests
{
    private EmergencyDataService _service = null!;

    [SetUp]
    public void Setup()
    {
        _service = new EmergencyDataService(Substitute.For<IEmergencyRepository>(), Substitute.For<IUserRepository>());
    }

    [Test]
    public void Constructor_ShouldInitializeService()
    {
        // Assert
        _service.Should().NotBeNull();
    }

    [Test]
    public void LoadActiveEmergenciesAsync_ShouldReturnEmptyCollection()
    {
        // Arrange
        var emergencies = new ObservableCollection<Emergency>();

        // Act
        var result = _service.LoadActiveEmergenciesAsync(emergencies);

        // Assert
        result.Should().NotBeNull();
        emergencies.Should().BeEmpty();
    }

    [Test]
    public void LoadUsersAsync_ShouldReturnEmptyCollection()
    {
        // Arrange
        var users = new ObservableCollection<User>();

        // Act
        var result = _service.LoadUsersAsync(users);

        // Assert
        result.Should().NotBeNull();
        users.Should().BeEmpty();
    }

    [Test]
    public async Task LoadActiveEmergenciesAsync_ShouldCompleteSuccessfully()
    {
        // Arrange
        var emergencies = new ObservableCollection<Emergency>();

        // Act & Assert
        var action = () => _service.LoadActiveEmergenciesAsync(emergencies);
        await action.Should().NotThrowAsync();
    }

    [Test]
    public async Task LoadUsersAsync_ShouldCompleteSuccessfully()
    {
        // Arrange
        var users = new ObservableCollection<User>();

        // Act & Assert
        var action = () => _service.LoadUsersAsync(users);
        await action.Should().NotThrowAsync();
    }

    [Test]
    public async Task LoadActiveEmergenciesAsync_WithNullCollection_ShouldHandleGracefully()
    {
        // Act & Assert
        var action = () => _service.LoadActiveEmergenciesAsync(null!);
        await action.Should().NotThrowAsync();
    }

    [Test]
    public async Task LoadUsersAsync_WithNullCollection_ShouldHandleGracefully()
    {
        // Act & Assert
        var action = () => _service.LoadUsersAsync(null!);
        await action.Should().NotThrowAsync();
    }
}
