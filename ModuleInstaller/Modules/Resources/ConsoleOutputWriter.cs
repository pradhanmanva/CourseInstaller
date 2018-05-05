using System;
using ModuleInstaller.Modules.Interfaces;

namespace ModuleInstaller
{
    /// <summary>
    /// Console Output Writer
    /// Prints the output to the Console
    /// </summary>
    public class ConsoleOutputWriter : IOutputWriter
    {
        public void WriteLine(string s)
        {
            Console.WriteLine(s);
        }
    }
}