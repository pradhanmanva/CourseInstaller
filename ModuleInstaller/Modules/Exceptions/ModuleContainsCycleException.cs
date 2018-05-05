using ModuleInstaller.Modules.Interfaces;

namespace ModuleInstaller.Modules.Exceptions
{

    /// <summary>
    /// Module Contains Cycle Exception
    /// </summary>
    public class ModuleContainsCycleException : ModuleExceptionBase
    {

        public ModuleContainsCycleException(IModule Module)
          : base(Module) { }

        public override string Name
        {
            get
            {
                return "Module Contains Cycle Exception";
            }
        }

    }

}
