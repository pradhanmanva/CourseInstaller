using NUnit.Framework;
using NUnit;
using ModuleInstaller.Modules;
using ModuleInstaller.Modules.Exceptions;
using ModuleInstaller.Modules.Interfaces;

namespace ModuleInstaller.Test
{

    [TestFixture]
    public class TestModulesDependencyMap
    {

        private ModulesDependencyMap<ModuleMock> dependencyMap;

        [SetUp()]
        public void Initialize()
        {
            dependencyMap = new ModulesDependencyMap<ModuleMock>();
        }

        #region Tests

        #region AddModule

        [Test]
        [Description("Should add a Module to the dependency map.")]
        public void TestAddModuleToDependencyMap()
        {

            // Arrange
            string ModuleName = "Vehicle",
                   dependencyName = "Car";

            var expectedModule = new ModuleMock()
            {
                Name = ModuleName,
                Dependency = new ModuleMock()
                {
                    Name = dependencyName
                }
            };

            // Act
            var actualModule = dependencyMap.AddModule(ModuleName, dependencyName);

            // Assert
            Assert.AreEqual(expectedModule, actualModule);

        }

        [Test]
        [Description("Should add a Module and link its dependency to an existing Module.")]
        public void TestAddModuleToDependencyMapAndLink()
        {

            // Arrange
            string ModuleName = "Vehicle",
                   dependencyName = "Car";

            var expectedModule = new ModuleMock()
            {
                Name = ModuleName,
                Dependency = new ModuleMock()
                {
                    Name = dependencyName
                }
            };

            dependencyMap.AddModule(dependencyName); // Add Module dependency first

            // Act
            var actualModule = dependencyMap.AddModule(ModuleName, dependencyName);

            // Assert
            Assert.AreEqual(expectedModule, actualModule);

        }

        [Test]
        [Description("Should add a Module and a new dependency to the Module list when it doesn't already exist.")]
        public void TestAddModuleToDependencyMapAndCreateDependencyModule()
        {

            // Arrange
            string ModuleName = "Vehicle",
                   dependencyName = "Car";

            var expectedModule = new ModuleMock()
            {
                Name = ModuleName,
                Dependency = new ModuleMock()
                {
                    Name = dependencyName
                }
            };

            // Act
            var actualModule = dependencyMap.AddModule(ModuleName, dependencyName);

            // Assert
            Assert.AreEqual(expectedModule, actualModule);

        }

        #region Contains Cycle

        [Test]
        [Description("Should throw ModuleContainsCycleException Error.")]
        public void TestAddModuleThrowContainsCycleException()
        {

            // Arrange
            dependencyMap.AddModule("A", "C");
            dependencyMap.AddModule("B", "A");

            // Act & Assert
            Assert.That(() => dependencyMap.AddModule("C", "B"), Throws.TypeOf<ModuleContainsCycleException>());

        }

        [Test]
        [Description("Should throw ModuleContainsCycleException Error. Scenario from .")]
        public void TestAddModuleThrowContainsCycleException2()
        {

            // Arrange
            dependencyMap.AddModule("A");
            dependencyMap.AddModule("B", "C");
            dependencyMap.AddModule("C", "F");
            dependencyMap.AddModule("D", "A");
            dependencyMap.AddModule("E");

            // Act & Assert
            Assert.That(() => dependencyMap.AddModule("F", "B"), Throws.TypeOf<ModuleContainsCycleException>());

        }

        [Test]
        [Description("Should throw ModuleContainsCycleException Error when a Module and its dependency are the same.")]
        public void TestAddModuleThrowContainsCycleExceptionWhenSameModuleAdded()
        {
            // Arrange, Act & Assert
            Assert.That(() => dependencyMap.AddModule("A", "A"), Throws.TypeOf<ModuleContainsCycleException>());
        }

        #endregion

        [Test]
        [Description("Should throw ModuleDuplicateException Error Module has already been added.")]
        public void TestAddModuleThrowModuleDuplicateException()
        {

            // Arrange
            dependencyMap.AddModule("A", "B");

            // Act
            Assert.That(() => dependencyMap.AddModule("a"), Throws.TypeOf<ModuleDuplicateException>()); // Add same Module but lowercase

        }

        #endregion

        [Test]
        [Description("Should return a dependency map. Scenario from .")]
        public void TestGetMap()
        {

            // Arrange
            var expected = new string[] { "A", "F", "C", "B", "D", "E" };

            dependencyMap.AddModule("A");
            dependencyMap.AddModule("B", "C");
            dependencyMap.AddModule("C", "F");
            dependencyMap.AddModule("D", "A");
            dependencyMap.AddModule("E", "B");
            dependencyMap.AddModule("F");

            // Act
            var actual = dependencyMap.GetMap();

            // Assert
            CollectionAssert.AreEqual(expected, actual);

        }

        [Test]
        [Description("Should return a dependency map. Scenario 2.")]
        public void TestGetMap2()
        {

            // Arrange
            var expected = new string[] { "C", "A", "B" };
            dependencyMap.AddModule("A", "C");
            dependencyMap.AddModule("B", "C");
            dependencyMap.AddModule("C");

            // Act
            var actual = dependencyMap.GetMap();

            // Assert
            CollectionAssert.AreEqual(expected, actual);

        }

        #endregion

        public class ModuleMock : IModule
        {

            public string Name { get; set; }
            public IModule Dependency { get; set; }

            // Help out in Asserting, but also used in the actual Module class.
            public override bool Equals(object obj)
            {

                // If parameter is null return false.
                if (obj == null)
                {
                    return false;
                }

                // If parameter cannot be cast to ModuleMock return false.
                ModuleMock p = obj as ModuleMock;
                if ((System.Object)p == null)
                {
                    return false;
                }

                // Return true if the fields match:
                // TODO: do we need to match of the dependency name as well?
                return Name == p.Name;

            }

            // Help out in testing
            public override string ToString()
            {

                string value = this.Name;

                if (this.Dependency != null)
                {
                    value += ":" + this.Dependency.Name;
                }

                return value;
            }
            public override int GetHashCode()
            {
                return base.GetHashCode();
            }

        }

    }

}
