namespace ModuleInstaller.Modules.Interfaces
{

    /// <summary>
    /// Dependency Map Generator Interface
    /// Creates the dependency map
    /// </summary>
    public interface IDependencyMapGenerator
    {
        string[] CreateMap(string[] definitions);
    }

}
