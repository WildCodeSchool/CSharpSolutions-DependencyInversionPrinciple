using System;
using System.IO;
using System.Diagnostics;
using System.Linq;

namespace S9___SOLID_Design_3___Dependency_inversion_principle
{
    namespace Problem
    {
        public class Program
        {
            public static void Main()
            {
                Terminal terminal = new Terminal();
                while (!terminal.Exited)
                {
                    Command command = terminal.PromptCommand();
                    terminal.ExecuteCommand(command);
                }
            }
        }

        public class Terminal
        {
            public bool Exited { get; set; }
            private string _promptString;

            public Terminal()
            {
                _promptString = String.Format("{0}$ ", System.Security.Principal.WindowsIdentity.GetCurrent().Name);
                Exited = false;
            }

            public string Prompt()
            {
                Console.Write(_promptString);
                string userCommand = Console.ReadLine();
                return userCommand;
            }

            public Command PromptCommand()
            {
                string commandLine = Prompt();
                return new Command(commandLine);
            }

            public void ExecuteCommand(Command command)
            {
                try
                {
                    command.Launch();
                    if (command.Output.Length > 0)
                    {
                        Console.WriteLine(command.Output);
                    }
                }
                catch (InvalidOperationException exception)
                {
                    Console.Error.WriteLine("{0}: path not found", command);
                }
            }

        }

        public class Command
        {
            public string Executable { get; private set; }
            public string[] Arguments { get; private set; }
            public string Output { get; private set; }
            public Command(string line)
            {
                string[] splittedLine = line.Split(' ');
                Executable = splittedLine[0];
                Arguments = splittedLine.Skip(1).ToArray();
                Output = "";
            }

            public string ExecutableFullPath
            {
                get
                {
                    string executableWithExtension;
                    if (Executable.EndsWith(".exe"))
                    {
                        executableWithExtension = Executable;
                    }
                    else
                    {
                        executableWithExtension = Executable + ".exe";
                    }
                    if (File.Exists(executableWithExtension))
                    {
                        return Path.GetFullPath(Executable);
                    }

                    string values = Environment.GetEnvironmentVariable("PATH");
                    foreach (var path in values.Split(Path.PathSeparator))
                    {
                        string fullPath = Path.Combine(path, executableWithExtension);
                        if (File.Exists(fullPath))
                        {
                            return fullPath;
                        }
                    }
                    return null;
                }
            }

            public void Launch()
            {
                string commandOutput;
                if (Executable.Length == 0)
                {
                    return;
                }
                using (Process process = new Process())
                {
                    process.StartInfo.FileName = ExecutableFullPath;
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.Arguments = String.Join(' ', Arguments);
                    process.Start();

                    commandOutput = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();
                }
                Output = commandOutput;
            }

            public override string ToString()
            {
                if (ExecutableFullPath is null)
                {
                    return Executable;
                }
                else
                {
                    return ExecutableFullPath;
                }
            }
        }
    }

    namespace Solution
    {
        public class Program
        {
            public static void Main()
            {
                Terminal terminal = new Terminal();
                while (!terminal.Exited)
                {
                    Command command = terminal.PromptCommand();
                    terminal.ExecuteCommand(command);
                }
            }
        }

        public class Terminal
        {
            public bool Exited { get; set; }
            private string _promptString;

            public Terminal()
            {
                _promptString = String.Format("{0}$ ", System.Security.Principal.WindowsIdentity.GetCurrent().Name);
                Exited = false;
            }

            public string Prompt()
            {
                Console.Write(_promptString);
                string userCommand = Console.ReadLine();
                return userCommand;
            }

            public Command PromptCommand()
            {
                string commandLine = Prompt();
                return new Command(commandLine);
            }

            public void ExecuteCommand(ICommand command)
            {
                try
                {
                    command.Launch();
                    if (command.Output.Length > 0)
                    {
                        Console.WriteLine(command.Output);
                    }
                }
                catch (InvalidOperationException exception)
                {
                    Console.Error.WriteLine("{0}: path not found", command);
                }
            }

        }

        public interface ICommand
        {
            string Output { get; }
            public void Launch();
        }


        public class Command : ICommand
        {
            public string Executable { get; private set; }
            public string[] Arguments { get; private set; }
            public string Output { get; private set; }
            public Command(string line)
            {
                string[] splittedLine = line.Split(' ');
                Executable = splittedLine[0];
                Arguments = splittedLine.Skip(1).ToArray();
                Output = "";
            }

            public string ExecutableFullPath
            {
                get
                {
                    string executableWithExtension;
                    if (Executable.EndsWith(".exe"))
                    {
                        executableWithExtension = Executable;
                    }
                    else
                    {
                        executableWithExtension = Executable + ".exe";
                    }
                    if (File.Exists(executableWithExtension))
                    {
                        return Path.GetFullPath(Executable);
                    }

                    var values = Environment.GetEnvironmentVariable("PATH");
                    foreach (var path in values.Split(Path.PathSeparator))
                    {
                        var fullPath = Path.Combine(path, executableWithExtension);
                        if (File.Exists(fullPath))
                        {
                            return fullPath;
                        }
                    }
                    return null;
                }
            }

            public void Launch()
            {
                string commandOutput;
                if (Executable.Length == 0)
                {
                    return;
                }
                using (Process process = new Process())
                {
                    process.StartInfo.FileName = ExecutableFullPath;
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.Arguments = String.Join(' ', Arguments);
                    process.Start();

                    commandOutput = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();
                }
                Output = commandOutput;
            }

            public override string ToString()
            {
                if (ExecutableFullPath is null)
                {
                    return Executable;
                }
                else
                {
                    return ExecutableFullPath;
                }
            }
        }
    }
}
