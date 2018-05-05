**Module installer with dependencies**

You suddenly have a curious aspiration to create module installer that can install modules and handle dependencies on a system. You want to be able to give the installer a list of module with dependencies, and have it install the module in order such that an install won’t fail due to a missing dependency.

This exercise is to write the code that will determine the order of install.

**Requirements**

Please complete the exercise in either C# (preferred as all work at BahFed will be C#) or Java.

Please write all the unit test cases to test your code along with edge cases- input, expected output, output.

Please submit your code repository zipped and emailed (not on github).

The program should accept an array of strings defining dependencies. Each string contains the name of a module followed by a colon and space, then any dependencies required by that module. For simplicity we’ll assume a module can have at most one dependency.

The program should output a comma separated list of module names in the order of install, such that a module’s dependency will always precede that module.

The program should reject as invalid a dependency specification that contains cycles.

**Usage**

The console application accepts one command line argument. This argument will contain all the Modules and their dependencies. Each module and its dependency is separated by a comma wrapped by double quotes.

    C:\myinstaller "CarService: VehiculeService, VehiculeService:" Example Inputs::: CarService: VehiculeService, VehiculeService:

Represents two modules, “CarService” and “VehiculeService”, where “CarService” depends on “VehiculeService”.

In this case the output should be: VehiculeService, CarService Indicating that VehiculeService needs to be installed before CarService. Valid input

    "VehiculeService:, RepairService: RepairShop, RepairShop: Shop, CarService: VehiculeService, TireService: RepairService, Shop:"

A valid output for the above would be:  

    VehiculeService, Shop, RepairShop, RepairService, CarService, TireService 
Input that should be rejected (contains cycles)

"VehiculeService:, RepairService: RepairShop, RepairShop: Shop, CarService: VehiculeService, TireService:, Shop: RepairService"

**How the CourseInstallerWorks**

CourseInstaller

 - Modules
     - Exceptions
         - `ModuleContainsCycleExceptions` - Raised when the input contains Cycle
         - `ModuleDuplicateException` - Raised when Duplicate Entries are entered
         - `ModuleExceptionBase` - Base Exception Class
     - Interfaces
         - `IDependencyMap`
         - `IDependencyMapGenerator`
         - `IModule`
         - `IOutputWriter`
     - Resources
         - `ConsoleOutputWriter` - Outputs to Console
         - `Module` - maps the accessor methods and primitive methods for `Module` Object
         - `ModulesDependencyMap` - Checks for all the conditions for successful CRUD over Modules
         - `ModulesDependencyMapGenerator` - Creates a dependency map of the given input
 - Properties
 - Types
     - `ConsoleReturnTypes` -Enumeration for the error/exception type returned
 - `TestModule` - Testing the equality of `Module` names and `Dependency` names
 - `TestModuleInstaller` - Testing the Input to the `ModuleInstaller` (no arguments, more arguments, wrong arguments or other exceptions)
 - `TestModulesDependencyMap` - Testing the input to the `ModulesDependencyMap` by handling all the dependency conditions like cycles, wrong inputs, duplicates, etc.
 - `TestModulesDependencyMapGenerator` - Testing the input to the `ModulesDependencyMapGenerator` for conditions like if the map is filled or if the map is returned properly.