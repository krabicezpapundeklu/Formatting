namespace Krabicezpapundeklu.Formatting.Tests
{
    using System;
    using System.Diagnostics;

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

            var launcher = new TestLauncher
                           {
                               EchoResults = true,
                               Logger = new FilteredLogger(new RichConsoleLogger(console), Verbosity.Normal),
                               ProgressMonitorProvider = new RichConsoleProgressMonitorProvider(console),
                               RuntimeSetup = new RuntimeSetup(),
                               TestProject = {TestRunnerFactoryName = StandardTestRunnerFactoryNames.Local}
                           };

            launcher.AddFilePattern(typeof(Driver).Assembly.Location);
            launcher.RuntimeSetup.AddPluginDirectory(InstallationConfiguration.LoadFromRegistry().InstallationFolder);

            TestLauncherResult result = launcher.Run();

            ConsoleColor originalConsoleColor = console.ForegroundColor;

            console.ForegroundColor = result.ResultCode == ResultCode.Success
                ? ConsoleColor.DarkGreen
                : ConsoleColor.Red;

            console.WriteLine();
            console.WriteLine(result.ResultSummary);
            console.WriteLine();

            console.ForegroundColor = originalConsoleColor;

            if(result.ResultCode != ResultCode.Success && Debugger.IsAttached)
                Debugger.Break();

            return result.ResultCode;
        }
    }
}
