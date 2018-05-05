using ModuleInstaller.Modules.Interfaces;

namespace ModuleInstaller.Modules
{

    /// <summary>
    /// Module
    /// </summary>
    public class Module : IModule
    {

        public string Name { get; set; }
        public IModule Dependency { get; set; }

        /// <summary>
        /// Determines if the Module has the same name
        /// </summary>
        /// <param name="obj">Module to compare against</param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {

            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to Module return false.
            Module p = obj as Module;
            if ((System.Object)p == null)
            {
                return false;
            }

            // Return true if the fields match:
            return Name == p.Name;

        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// String Representation of the Module
        /// </summary>
        /// <returns>ModuleName:DependencyName</returns>
        public override string ToString()
        {

            string value = this.Name;

            if (this.Dependency != null)
            {
                value += ":" + this.Dependency.Name;
            }

            return value;
        }
    }
}
