using ModuleInstaller.Modules.Interfaces;
using ModuleInstaller.Types;
using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace ModuleInstaller.Test
{

    [TestFixture]
    public class TestModuleInstaller
    {

        private ConsoleOutputWriterMock writer;
        private ModuleDependencyMapGeneratorMock generator;
        private Program program;

        [SetUp()]
        public void Initialize()
        {
            writer = new ConsoleOutputWriterMock();
            generator = new ModuleDependencyMapGeneratorMock();
            program = new Program(writer, generator);
        }

        [Test]
        [Description("Should write successful output to screen.")]
        public void TestMainOutput()
        {

            // Arrange
            string[] input = { "Vehicle: Car, Car:" };
            var expectedOutput = "Car, Vehicle";

            // Act
            program.Run(input);

            // Assert
            Assert.AreEqual(expectedOutput, writer.GetLastLine());

        }

        [Test]
        [Description("Should fail when no argument passed.")]
        public void TestMainNoArguments()
        {

            // Arrange
            string[] input = { };

            // Act
            var result = program.Run(input);

            // Assert
            Assert.AreEqual(ConsoleReturnTypes.NoArguments, result);

        }

        [Test]
        [Description("Should fail when more than one argument passed.")]
        public void TestMainArgumentsMoreThanOne()
        {

            // Arrange
            string[] input = { "Argument1", "Argument2" };

            // Act
            var result = program.Run(input);

            // Assert
            Assert.AreEqual(ConsoleReturnTypes.TooManyArguments, result);

        }

        [Test]
        [Description("Should fail with no colon in argument")]
        public void TestMainArgumentNotContainColon()
        {

            // Arrange
            string[] input = { "Argument1 with no colon" };

            // Act
            var result = program.Run(input);

            // Assert
            Assert.AreEqual(ConsoleReturnTypes.ArgumentsIncorrectFormat, result);

        }

        [Test]
        [Description("Should contain a colon.")]
        public void TestMainArgumentContainsColon()
        {

            // Arrange
            string[] input = { "Vehicle:" };

            // Act
            var result = program.Run(input);

            // Assert
            Assert.AreEqual(ConsoleReturnTypes.Success, result);

        }

        [Test]
        [Description("Argument cannot be empty.")]
        public void TestMainArgumentEmptyString()
        {

            // Arrange
            string[] input = { "" }; // Empty String

            // Act
            var result = program.Run(input);

            // Assert
            Assert.AreEqual(ConsoleReturnTypes.ArgumentsIncorrectFormat, result);

        }

        [Test]
        [Description("Argument cannot have just a colon.")]
        public void TestMainArgumentJustColon()
        {

            // Arrange
            string[] input = { ":" };

            // Act
            var result = program.Run(input);

            // Assert
            Assert.AreEqual(ConsoleReturnTypes.ArgumentsIncorrectFormat, result);

        }

        [Test]
        [Description("Inform User that something went wrong.")]
        public void TestRunInformUserOfFailure()
        {

            // Arrange
            string[] input = { "" }; // Empty String

            // Act
            var result = program.Run(input);

            // Assert
            Assert.IsTrue(writer.HasBeenCalled());

        }

        [Test]
        [Description("Parse the dependency list string into an array of Modules.")]
        public void TestRunParseDependencyList()
        {

            // Arrange
            string[] input = { "Vehicle: Car, Car:" };
            string[] expected = { "Vehicle: Car", "Car:" };

            // Act
            var result = program.Run(input);

            // Assert
            CollectionAssert.AreEqual(expected, generator.Definitions);

        }

        [Test]
        [Description("Catches & handles an unknown error and display a safe message to the user.")]
        public void TestRunHandleUnknownError()
        {

            // Arrange
            string[] input = { "Vehicle: Car, Car:" };
            generator.ThrowError = true;

            // Act
            var result = program.Run(input);

            // Assert
            Assert.AreEqual(ConsoleReturnTypes.Rejected, result);
            Assert.IsTrue(writer.HasBeenCalled());

        }

    }

    /// <summary>
    /// Stub ConsoleOutputWriter for mocking anything written to the screen
    /// </summary>
    public class ConsoleOutputWriterMock : IOutputWriter
    {

        private List<string> _writtenLines = new List<string>();

        public void WriteLine(string s)
        {
            _writtenLines.Add(s);
        }

        public string GetLastLine()
        {
            return _writtenLines.Last();
        }

        public bool HasBeenCalled()
        {
            return _writtenLines.Count > 0;
        }

    }

    /// <summary>
    /// Stub ModuleDependencyMapGenerator for mocking the map creation
    /// </summary>
    public class ModuleDependencyMapGeneratorMock : IDependencyMapGenerator
    {

        public bool ThrowError { get; set; }
        public string[] Definitions { get; set; }

        public string[] CreateMap(string[] definitions)
        {

            if (this.ThrowError)
            {
                throw new Exception("Exception");
            }
            else
            {
                this.Definitions = definitions;
                return new string[] { "Car", "Vehicle" };
            }

        }

    }

}