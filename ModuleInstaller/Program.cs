using ModuleInstaller.Modules.Interfaces;
using ModuleInstaller.Modules;
using ModuleInstaller.Types;
using System;

namespace ModuleInstaller
{

    /// <summary>
    /// Program to execute at runtime
    /// </summary>
    public class Program
    {

        // Interface used to output results
        private IOutputWriter _writer;
        private IDependencyMapGenerator _generator;
        private string[] _definitions;

        public Program(IOutputWriter writer, IDependencyMapGenerator generator)
        {
            // Allow for Lazy dependency injection
            _writer = writer;
            _generator = generator;
        }

        public static int Main(string[] args)
        {

            var dependencyMap = new ModulesDependencyMap<Module>();

            // Lazy dependency injection
            var program = new Program(new ConsoleOutputWriter(), new ModulesDependencyMapGenerator(':', dependencyMap));

            return (int)program.Run(args);

        }

        /// <summary>
        /// Execute the program
        /// </summary>
        /// <param name="args">Array of Arguments</param>
        public ConsoleReturnTypes Run(string[] args)
        {

            ConsoleReturnTypes result;

            try
            {

                // Get the argument from the command line
                result = ConsumeArguments(args);

                // When we have a failure, stop and inform user
                if (result != ConsoleReturnTypes.Success)
                {
                    HandleError(result);
                    return result;
                }

                var dependencyMap = _generator.CreateMap(this._definitions);
                WriteLine(string.Join(", ", dependencyMap));

            }
            catch (Exception e)
            {
                result = ConsoleReturnTypes.Rejected;
                HandleError(result, e.Message);
            }

            return result;

        }

        /// <summary>
        /// Displays the failure to the user
        /// </summary>
        /// <param name="failureType"></param>
        /// <param name="details">Additional details</param>
        private void HandleError(ConsoleReturnTypes failureType, string details = null)
        {

            switch (failureType)
            {
                case ConsoleReturnTypes.NoArguments:
                case ConsoleReturnTypes.ArgumentsIncorrectFormat:
                    WriteLine("Enter a list of dependencies.");
                    WriteLine("Usage: \"<Module>: <dependency>, ...\"");
                    WriteLine("Usage Example: Moduleinstallerexcercise \"KittenService: CamelCaser, CamelCaser:\"");
                    break;

                case ConsoleReturnTypes.TooManyArguments:
                    WriteLine("Only provide one argument.");
                    break;

                default:
                    string line = string.Format("An error occurred: {0}. \nDetails: {1}.", Enum.GetName(typeof(ConsoleReturnTypes), failureType), details);
                    WriteLine(line);
                    break;

            }

        }

        /// <summary>
        /// Parses the arguments
        /// </summary>
        /// <param name="args">Argument Array</param>
        /// <returns>0 as a success, anything greater as an error</returns>
        private ConsoleReturnTypes ConsumeArguments(string[] args)
        {

            // Must have only one argument
            if (args.Length == 0)
            {
                return ConsoleReturnTypes.NoArguments;
            }

            // Can only handle one argument
            if (args.Length > 1)
            {
                return ConsoleReturnTypes.TooManyArguments;
            }

            // Get the combined Modules
            var ModulesList = args[0];

            // Verify the argument isn't empty and contains our delimiter.
            if (string.IsNullOrEmpty(ModulesList) || !ModulesList.Contains(":"))
            {
                return ConsoleReturnTypes.ArgumentsIncorrectFormat;
            }

            ParseModulesList(ModulesList);

            // Can't just have a : and no Module
            if (this._definitions.Length == 1 && this._definitions[0] == ":")
            {
                return ConsoleReturnTypes.ArgumentsIncorrectFormat;
            }

            return ConsoleReturnTypes.Success;

        }

        /// <summary>
        /// Parse Modules List
        /// </summary>
        /// <param name="ModulesList">Modules List</param>
        private void ParseModulesList(string ModulesList)
        {

            // Split each Module:dependency
            var splitDefinitions = ModulesList.Split(',');

            // Remove any trailing or leading white spaces
            for (int i = 0; i < splitDefinitions.Length; i++)
            {
                splitDefinitions[i] = splitDefinitions[i].Trim();
            }

            // Set the definitions for this program instance
            this._definitions = splitDefinitions;

        }

        /// <summary>
        /// Write string as a line to output
        /// </summary>
        /// <param name="s">Line to output</param>
        private void WriteLine(string s)
        {
            _writer.WriteLine(s);
        }
    }
}