using ModuleInstaller.Modules.Interfaces;
using System;

namespace ModuleInstaller.Modules.Exceptions
{

    /// <summary>
    /// Module Exception Base Class
    /// </summary>
    public abstract class ModuleExceptionBase : Exception
    {

        // Override to provide name of the exception
        abstract public string Name { get; }

        public IModule Module { get; private set; }

        public ModuleExceptionBase(IModule Module)
        {
            this.Module = Module;
        }

        public override string Message
        {
            get
            {
                return string.Format("{0} [{1}]", this.Name, this.Module.ToString());
            }
        }

    }

}