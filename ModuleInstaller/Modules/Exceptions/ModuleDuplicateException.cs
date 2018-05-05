using ModuleInstaller.Modules.Interfaces;

namespace ModuleInstaller.Modules.Exceptions
{

    /// <summary>
    /// Module Is a Duplicate Exception
    /// </summary>
    public class ModuleDuplicateException : ModuleExceptionBase
    {

        public ModuleDuplicateException(IModule Module)
          : base(Module) { }

        public override string Name
        {
            get
            {
                return "Module already added";
            }
        }

    }
}
