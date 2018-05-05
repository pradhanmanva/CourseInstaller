using NUnit.Framework;
using ModuleInstaller.Modules;

namespace ModuleInstaller.Test.Modules {

  [TestFixture]
  public class TestModule {

    [Test]
    [Description("Should return a ModuleName:DependencyName string.")]
    public void TestToStringWithDependency() {

      // Arrange
      string ModuleName = "A",
          dependencyName = "B";

      var Module = new Module() {
        Name = ModuleName,
        Dependency = new Module() {
          Name = dependencyName
        }
      };

      // Act
      var actual = Module.ToString();

      // Assert
      Assert.AreEqual(actual, ModuleName + ":" + dependencyName);

    }

    [Test]
    [Description("Should return a ModuleName string.")]
    public void TestToStringWithoutDependency() {

      // Arrange
      string ModuleName = "A";

      var Module = new Module() {
        Name = ModuleName
      };

      // Act
      var actual = Module.ToString();

      // Assert
      Assert.AreEqual(actual, ModuleName);

    }

    [Test]
    [Description("Should be equal when both Modules have same name and dependency.")]
    public void TestEqualSameNameAndDependency() {

      // Arrange
      string ModuleName = "A",
          dependencyName = "B";

      var Module1 = new Module() {
        Name = ModuleName,
        Dependency = new Module() {
          Name = dependencyName
        }
      };

      var Module2 = new Module() {
        Name = ModuleName,
        Dependency = new Module() {
          Name = dependencyName
        }
      };

      // Act & Assert
      Assert.AreEqual(Module1, Module2);

    }

    [Test]
    [Description("Should be equal when both Modules have same name but different Dependencies.")]
    public void TestEqualSameNameDifferentDependency() {

      // Arrange
      string ModuleName = "A",
          dependencyName = "B";

      var Module1 = new Module() {
        Name = ModuleName,
        Dependency = new Module() {
          Name = dependencyName
        }
      };

      var Module2 = new Module() {
        Name = ModuleName
      };

      // Act & Assert
      Assert.AreEqual(Module1, Module2);

    }

    [Test]
    [Description("Should not be equal when both Modules have same name but different Dependencies.")]
    public void TestNotEqual() {

      // Arrange
      var Module1 = new Module() {
        Name = "A"
      };

      var Module2 = new Module() {
        Name = "B"
      };

      // Act & Assert
      Assert.AreNotEqual(Module1, Module2);

    }

  }

}
