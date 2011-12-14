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
		#region Constants and Fields

		private const string GallioBinEnvironmentVariable = "GALLIO_BIN";

		#endregion

        #region Public Methods

        public static int Main()
        {
            NativeConsole console = NativeConsole.Instance;

            var launcher = new TestLauncher
                {
                    EchoResults = true,
                    Logger = new FilteredLogger(new RichConsoleLogger(console), Verbosity.Normal),
                    ProgressMonitorProvider = new RichConsoleProgressMonitorProvider(console),
                    RuntimeSetup = new RuntimeSetup(),
                    TestProject = { TestRunnerFactoryName = StandardTestRunnerFactoryNames.Local }
                };

			string gallioBin =
				Environment.GetEnvironmentVariable(GallioBinEnvironmentVariable) ??
				InstallationConfiguration.LoadFromRegistry().InstallationFolder;

			if(string.IsNullOrEmpty(gallioBin))
			{
				Console.Error.WriteLine("Cannot find Gallio installation directory.");
				Console.Error.WriteLine("Specify it with \"{0}\" environment variable.", GallioBinEnvironmentVariable);
				return 1;
			}

            launcher.AddFilePattern(typeof(Driver).Assembly.Location);
            launcher.RuntimeSetup.AddPluginDirectory(gallioBin);

            TestLauncherResult result = launcher.Run();

            ConsoleColor originalConsoleColor = console.ForegroundColor;

            console.ForegroundColor = result.ResultCode == ResultCode.Success
                                          ? ConsoleColor.DarkGreen
                                          : ConsoleColor.Red;

            console.WriteLine();
            console.WriteLine(result.ResultSummary);
            console.WriteLine();

            console.ForegroundColor = originalConsoleColor;

            if (result.ResultCode != ResultCode.Success && Debugger.IsAttached)
            {
                Debugger.Break();
            }

            return result.ResultCode;
        }

        #endregion
    }
}