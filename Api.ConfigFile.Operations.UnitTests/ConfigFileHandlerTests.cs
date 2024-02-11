using Api.ConfigFile.Operations.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Api.ConfigFile.Operations.UnitTests;

[TestClass]
public class ConfigFileHandlerTests
{
    private readonly ConfigFileHandler _configFileHandler = new ConfigFileHandler("test-config.txt");
    
    [TestInitialize()]
    public void Startup()
    {
        File.Copy("original-test-config.txt", "test-config.txt", true);
    }
    
    [TestMethod]
    public void ShouldReadFileAndReturnDefaultAndServerConfigs()
    {
        // when
        var serverConfigs = _configFileHandler.ReadConfigSettings();
        
        // then
        Assert.AreEqual(2, serverConfigs.Keys.Count);
        Assert.IsTrue(serverConfigs.ContainsKey("default"));
        Assert.IsTrue(serverConfigs.ContainsKey("SRVTST0003"));

       var actualDefaultServerConfigs = serverConfigs.GetValueOrDefault("default");
       Assert.IsNotNull(actualDefaultServerConfigs);
       Assert.IsNotNull(actualDefaultServerConfigs.Configs);
       Assert.AreEqual(4, actualDefaultServerConfigs.Configs.Count);
       Assert.AreEqual("SERVER1TEST", actualDefaultServerConfigs.Configs.GetValueOrDefault("SERVER_NAME"));
       Assert.AreEqual("http://dummy.domain.company.com/test.html", actualDefaultServerConfigs.Configs.GetValueOrDefault("URL"));
       Assert.AreEqual("SQL_SERVER-1", actualDefaultServerConfigs.Configs.GetValueOrDefault("DB"));
       Assert.AreEqual("10.200.0.4", actualDefaultServerConfigs.Configs.GetValueOrDefault("IP_ADDRESS"));

       
       var actualServerThreeConfigs = serverConfigs.GetValueOrDefault("SRVTST0003");
       Assert.IsNotNull(actualServerThreeConfigs);
       Assert.IsNotNull(actualServerThreeConfigs.Configs);
       Assert.AreEqual(2, actualServerThreeConfigs.Configs.Count);
       Assert.AreEqual("MRAPPPOOLPORTL0003", actualServerThreeConfigs.Configs.GetValueOrDefault("SERVER_NAME"));
       Assert.AreEqual("10.200.0.100", actualServerThreeConfigs.Configs.GetValueOrDefault("IP_ADDRESS"));
    }

    [TestMethod]
    public void ShouldUpdateConfigSettingsForDefaultServer()
    {
        // given
        var updatedConfigs = new Dictionary<string, string>
        {
            ["SERVER_NAME"] = "NEW_SERVER",
            ["DB"] = "SQL_SERVER_2"
        };

        // when
        _configFileHandler.UpdateConfigSettings("default", updatedConfigs);
        
        // then
        var serverConfigs = _configFileHandler.ReadConfigSettings();

        var actualDefaultServerConfigs = serverConfigs.GetValueOrDefault("default");
        Assert.IsNotNull(actualDefaultServerConfigs);
        Assert.IsNotNull(actualDefaultServerConfigs.Configs);
        Assert.AreEqual(4, actualDefaultServerConfigs.Configs.Count);
        Assert.AreEqual("NEW_SERVER", actualDefaultServerConfigs.Configs.GetValueOrDefault("SERVER_NAME"));
        Assert.AreEqual("http://dummy.domain.company.com/test.html", actualDefaultServerConfigs.Configs.GetValueOrDefault("URL"));
        Assert.AreEqual("SQL_SERVER_2", actualDefaultServerConfigs.Configs.GetValueOrDefault("DB"));
        Assert.AreEqual("10.200.0.4", actualDefaultServerConfigs.Configs.GetValueOrDefault("IP_ADDRESS"));
        
    }
    
    [TestMethod]
    public void ShouldNotUpdateConfigSettingsForSpecificServer()
    {
        // given
        var updatedConfigs = new Dictionary<string, string>
        {
            ["SERVER_NAME"] = "NEW_SERVER",
            ["IP_ADDRESS"] = "10.200.0.100"
        };

        // when
        _configFileHandler.UpdateConfigSettings("SRVTST0003", updatedConfigs);
        
        // then
        var serverConfigs = _configFileHandler.ReadConfigSettings();
        
        var actualServerThreeConfigs = serverConfigs.GetValueOrDefault("SRVTST0003");
        Assert.IsNotNull(actualServerThreeConfigs);
        Assert.IsNotNull(actualServerThreeConfigs.Configs);
        Assert.AreEqual(2, actualServerThreeConfigs.Configs.Count);
        Assert.AreEqual("MRAPPPOOLPORTL0003", actualServerThreeConfigs.Configs.GetValueOrDefault("SERVER_NAME"));
        Assert.AreEqual("10.200.0.100", actualServerThreeConfigs.Configs.GetValueOrDefault("IP_ADDRESS"));
        
    }

    [TestMethod]
    public void ShouldAddNewConfigSettingsForAllServers()
    {
        // given
        var newConfigs = new Dictionary<string, string>
        {
            ["TEST-KEY-1"] = "TEST_VAL_1",
            ["TEST-KEY-2"] = "TEST_VAL_2"
        };

        // when
        _configFileHandler.AddConfigSettings("default", newConfigs);
        _configFileHandler.AddConfigSettings("SRVTST0003", newConfigs);
        
        // then
        var serverConfigs = _configFileHandler.ReadConfigSettings();
        Assert.AreEqual(2, serverConfigs.Keys.Count);

        var actualDefaultServerConfigs = serverConfigs.GetValueOrDefault("default");
        Assert.IsNotNull(actualDefaultServerConfigs);
        Assert.IsNotNull(actualDefaultServerConfigs.Configs);
        Assert.AreEqual(6, actualDefaultServerConfigs.Configs.Count);
        Assert.AreEqual("SERVER1TEST", actualDefaultServerConfigs.Configs.GetValueOrDefault("SERVER_NAME"));
        Assert.AreEqual("http://dummy.domain.company.com/test.html", actualDefaultServerConfigs.Configs.GetValueOrDefault("URL"));
        Assert.AreEqual("SQL_SERVER-1", actualDefaultServerConfigs.Configs.GetValueOrDefault("DB"));
        Assert.AreEqual("10.200.0.4", actualDefaultServerConfigs.Configs.GetValueOrDefault("IP_ADDRESS"));
        Assert.AreEqual("TEST_VAL_1", actualDefaultServerConfigs.Configs.GetValueOrDefault("TEST-KEY-1"));
        Assert.AreEqual("TEST_VAL_2", actualDefaultServerConfigs.Configs.GetValueOrDefault("TEST-KEY-2"));

       
        var actualServerThreeConfigs = serverConfigs.GetValueOrDefault("SRVTST0003");
        Assert.IsNotNull(actualServerThreeConfigs);
        Assert.IsNotNull(actualServerThreeConfigs.Configs);
        Assert.AreEqual(4, actualServerThreeConfigs.Configs.Count);
        Assert.AreEqual("MRAPPPOOLPORTL0003", actualServerThreeConfigs.Configs.GetValueOrDefault("SERVER_NAME"));
        Assert.AreEqual("10.200.0.100", actualServerThreeConfigs.Configs.GetValueOrDefault("IP_ADDRESS"));
        Assert.AreEqual("TEST_VAL_1", actualDefaultServerConfigs.Configs.GetValueOrDefault("TEST-KEY-1"));
        Assert.AreEqual("TEST_VAL_2", actualDefaultServerConfigs.Configs.GetValueOrDefault("TEST-KEY-2"));
    }
}