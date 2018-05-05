namespace ModuleInstaller.Modules.Interfaces
{

    /// <summary>
    /// Dependency Map Interface
    /// Maps the modules to their dependencies
    /// </summary>
    public interface IDependencyMap
    {
        IModule AddModule(string ModuleName, string dependencyName);
        string[] GetMap();
    }

}
