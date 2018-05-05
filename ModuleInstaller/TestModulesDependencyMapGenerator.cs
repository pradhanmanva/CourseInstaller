using NUnit.Framework;
using ModuleInstaller.Modules;
using System.Collections.Generic;
using ModuleInstaller.Modules.Interfaces;

namespace ModuleInstaller.Test {

  [TestFixture]
  public class TestModulesDependencyMapGenerator {

    private ModulesDependencyMapGenerator generator;
    private ModulesDependencyMapMock dependencyMap;

    [SetUp()]
    public void Initialize() {
      dependencyMap = new ModulesDependencyMapMock();
      generator = new ModulesDependencyMapGenerator(':', dependencyMap);
    }

    [Test]
    [Description("Should return a dependency map array list.")]
    public void TestCreateMapReturnsDependencyMapList() {

      // Arrange
      string[] definitions = { "Vehicle: Car", "Car:" };
      string[] expected = { "Car", "Vehicle" };

      // Act
      var dependencyMapList = generator.CreateMap(definitions);

      // Assert
      CollectionAssert.AreEqual(expected, dependencyMapList);

    }

    [Test]
    [Description("Should fill the Modules in the dependency map.")]
    public void TestCreateMapFillModulesDependencyMap() {

      // Arrange
      string[] definitions = { "Vehicle: Car", "Car:" };

      var expectedModulesAdded = new Dictionary<string, string>() {
        { "Vehicle", "Car" },
        { "Car", "" }
      };

      // Act
      var dependencyMapList = generator.CreateMap(definitions);

      // Assert
      CollectionAssert.AreEqual(expectedModulesAdded, dependencyMap.Modules);

    }

  }

  public class ModulesDependencyMapMock : IDependencyMap {

    public Dictionary<string, string> Modules { get; private set; }

    public ModulesDependencyMapMock() {
      this.Modules = new Dictionary<string, string>();
    }

    public IModule AddModule(string ModuleName, string dependencyName) {
      this.Modules.Add(ModuleName, dependencyName);
      return null;
    }

    public string[] GetMap() {
      return new string[] { "Car", "Vehicle" };
    }

  }

}
