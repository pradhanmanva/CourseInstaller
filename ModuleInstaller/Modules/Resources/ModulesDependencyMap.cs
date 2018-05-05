using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using ModuleInstaller.Modules.Exceptions;
using ModuleInstaller.Modules.Interfaces;

namespace ModuleInstaller.Modules
{

    /// <summary>
    /// Container for holding the mapping between Module dependencies
    /// </summary>
    public class ModulesDependencyMap<P> : IDependencyMap
      where P : IModule, new()
    {

        public List<IModule> Modules { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public ModulesDependencyMap()
        {
            this.Modules = new List<IModule>();
        }

        /// <summary>
        /// Add Module to dependency map.
        /// </summary>
        /// <param name="ModuleName">Module Name</param>
        /// <param name="dependencyName">Dependency Name</param>
        /// <remarks>When dependency already exists in map, will link instead of create</remarks>
        public IModule AddModule(string ModuleName, string dependencyName = null)
        {

            // See if Module already exists in the list
            var existingModule = FindModule(ModuleName);

            // If Module already has a dependency, throw duplicate error
            if (existingModule != null && existingModule.Dependency != null)
            {
                throw new ModuleDuplicateException(existingModule);
            }

            var newModules = new List<IModule>();

            IModule dependency = default(P);

            // When dependencyName is passed, find it or create it
            if (!string.IsNullOrWhiteSpace(dependencyName))
            {
                dependency = CreateOrFindModule(dependencyName, newModules);
            }

            // Create new Module or get it from the Module list adding its dependency
            var Module = CreateOrFindModule(ModuleName, newModules);
            Module.Dependency = dependency;

            // Determine if it contains a cycle
            if (ModuleHasCycle(Module, Module.Name))
            {
                throw new ModuleContainsCycleException(Module);
            }

            // We have gotten this far, so add the newly created pages to the Module list
            this.Modules.AddRange(newModules);
            return this.Modules.Last();

        }

        /// <summary>
        /// Create or Find the new Module and add to Module list
        /// </summary>
        /// <param name="ModuleName">Name of the Module to create</param>
        /// <param name="Modules">Temporary Module list to add the Module if created</param>
        /// <returns>Created of Found Module</returns>
        private IModule CreateOrFindModule(string ModuleName, IList Modules = null)
        {

            // See if Module already exists in the list
            var Module = FindModule(ModuleName);

            // If Module doesn't already exist, create a new instance
            if (Module == null)
            {

                Module = new P()
                {
                    Name = ModuleName
                };

                // If no Modules passed, default to that of the class
                if (Modules == null)
                {
                    Modules = this.Modules;
                }

                // Add to Module list
                // Could be a temporarily list passed in.
                Modules.Add(Module);

            }

            return Module;

        }

        /// <summary>
        /// Finds the Module name within the Module list
        /// </summary>
        /// <param name="ModuleName">Module Name to find</param>
        /// <returns>Module if found, null if otherwise</returns>
        private IModule FindModule(string ModuleName)
        {

            if (string.IsNullOrWhiteSpace(ModuleName))
            {
                throw new ArgumentException("Module name cannot be empty");
            }

            return this.Modules.Find(
              p => p.Name.Equals(
                ModuleName,
                StringComparison.CurrentCultureIgnoreCase
              ));

        }

        /// <summary>
        /// Generates a dependency map
        /// </summary>
        /// <returns>Array of dependency names</returns>
        public string[] GetMap()
        {

            var map = new List<string>();

            foreach (var Module in this.Modules)
            {
                GetModuleDependencies(Module, map);
            }

            return map.ToArray();

        }

        /// <summary>
        /// Recursively adds each dependency name in its correct order in the tree.
        /// </summary>
        /// <param name="Module">Module</param>
        /// <param name="map">Map List</param>
        private void GetModuleDependencies(IModule Module, IList map)
        {

            // Recurse through tree if dependency exists
            if (Module.Dependency != null)
            {
                GetModuleDependencies(Module.Dependency, map);
            }

            // Don't add if Module is already in map list
            if (map.Contains(Module.Name))
            {
                return;
            }

            // Add the Module Name to the list.
            map.Add(Module.Name);

        }

        /// <summary>
        /// Determines if the Module has a a cycle through recursion
        /// </summary>
        /// <param name="Module">Module to check</param>
        /// <param name="originalModuleName">Original Module Name</param>
        /// <returns></returns>
        private bool ModuleHasCycle(IModule Module, string originalModuleName)
        {

            // When dependency is null, can't be a cycle
            if (Module.Dependency == null)
            {
                return false;
            }

            // When Module and its dependency have the same name, its a cycle
            if (Module.Equals(Module.Dependency))
            {
                return true;
            }

            // When dependency Name is the same as the original Module name, its a cycle
            if (Module.Dependency.Name == originalModuleName)
            {
                return true;
            }

            // Recurse through the dependency tree
            return ModuleHasCycle(Module.Dependency, originalModuleName);

        }
    }
}
