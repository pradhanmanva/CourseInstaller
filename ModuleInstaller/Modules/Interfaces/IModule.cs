namespace ModuleInstaller.Modules.Interfaces
{

    /// <summary>
    /// Module Interface
    /// </summary>
    public interface IModule
    {

        string Name { get; set; }
        IModule Dependency { get; set; }

    }

}
