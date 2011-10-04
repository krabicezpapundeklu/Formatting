namespace Krabicezpapundeklu.Formatting.Tests
{
    using System;

    using Gallio.Runner;
    using Gallio.Runtime;
    using Gallio.Runtime.ConsoleSupport;
    using Gallio.Runtime.Logging;
    using Gallio.Runtime.ProgressMonitoring;

    public static class Driver
    {
        public static int Main()
        {
            NativeConsole console = NativeConsole.Instance;

            var launcher = new TestLauncher {RuntimeSetup = new RuntimeSetup()};

            launcher.AddFilePattern(typeof(Driver).Assembly.Location);
            launcher.Logger = new FilteredLogger(new RichConsoleLogger(console), Verbosity.Normal);
            launcher.RuntimeSetup.AddPluginDirectory(InstallationConfiguration.LoadFromRegistry().InstallationFolder);
            launcher.TestProject.TestRunnerFactoryName = StandardTestRunnerFactoryNames.Local;
            launcher.ProgressMonitorProvider = new RichConsoleProgressMonitorProvider(console);

            launcher.EchoResults = true;

            TestLauncherResult result = launcher.Run();

            ConsoleColor originalConsoleColor = console.ForegroundColor;

            console.ForegroundColor = result.ResultCode == ResultCode.Success
                ? ConsoleColor.DarkGreen
                : ConsoleColor.Red;

            console.WriteLine();
            console.WriteLine(result.ResultSummary);
            console.WriteLine();

            console.ForegroundColor = originalConsoleColor;

            return result.ResultCode;
        }
    }
}
