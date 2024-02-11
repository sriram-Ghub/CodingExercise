using System.Security.Claims;
using Api.ConfigFile.Operations.Controllers;
using Api.ConfigFile.Operations.Model;
using Api.ConfigFile.Operations.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Moq;

namespace Api.ConfigFile.Operations.UnitTests;

[TestClass]
public class ConfigControllerTests
{
    private Mock<ISettingsService> _mockSettingsService;
    private MockRepository _mockRepository;
    private ConfigController _configController;


    [TestInitialize]
    public void TestInitialize()
    {
        _mockRepository = new MockRepository(MockBehavior.Strict);
        _mockSettingsService = _mockRepository.Create<ISettingsService>();
        _configController = new ConfigController(_mockSettingsService.Object);
    }
    

    [TestMethod]
    public void GetConfigSettings_ReturnsOkResult()
    {
        // given
        _mockSettingsService.Setup(x => x.GetConfigSettings(It.IsNotNull<string>()))
            .Returns(new ConfigModel());
        // when
        var result = _configController.GetConfigSettings("default") as OkObjectResult;

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result.Value, typeof(ConfigModel));
        _mockRepository.VerifyAll();
    }
}