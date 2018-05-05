using ModuleInstaller.Modules.Interfaces;
using System;

namespace ModuleInstaller.Modules
{

    /// <summary>
    /// Module Dependency Map Generator
    /// </summary>
    public class ModulesDependencyMapGenerator : IDependencyMapGenerator
    {

        private char _delimiter;
        private IDependencyMap _ModuleDependencyMap;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="delimiter">Delimiter separating the Module from the dependency</param>
        public ModulesDependencyMapGenerator(char delimiter, IDependencyMap ModuleDependencyMap)
        {
            _delimiter = delimiter;
            _ModuleDependencyMap = ModuleDependencyMap;
        }

        /// <summary>
        /// Create Dependency Map
        /// </summary>
        /// <param name="definitions"></param>
        /// <returns>Array of dependencies in their build order</returns>
        public string[] CreateMap(string[] definitions)
        {
            FillMap(definitions);
            return this._ModuleDependencyMap.GetMap();
        }

        /// <summary>
        /// Fills dependency map with the definitions provided
        /// </summary>
        /// <param name="definitions"></param>
        private void FillMap(string[] definitions)
        {

            foreach (string definition in definitions)
            {

                // Strip out Module:dependency from string
                string[] ModuleAndDependency = definition.Split(this._delimiter);

                if (ModuleAndDependency.Length != 2)
                {
                    throw new FormatException("Dependency string is not in the correct format.");
                }

                string ModuleName = ModuleAndDependency[0].Trim();
                string dependencyName = ModuleAndDependency[1].Trim();

                this._ModuleDependencyMap.AddModule(ModuleName, dependencyName);

            }

        }

    }
}
