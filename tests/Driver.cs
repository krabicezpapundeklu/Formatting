/*
Copyright 2011 krabicezpapundeklu. All rights reserved.

Redistribution and use in source and binary forms, with or without modification, are
permitted provided that the following conditions are met:

   1. Redistributions of source code must retain the above copyright notice, this list of
      conditions and the following disclaimer.

   2. Redistributions in binary form must reproduce the above copyright notice, this list
      of conditions and the following disclaimer in the documentation and/or other materials
      provided with the distribution.

THIS SOFTWARE IS PROVIDED BY KRABICEZPAPUNDEKLU ''AS IS'' AND ANY EXPRESS OR IMPLIED
WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL KRABICEZPAPUNDEKLU OR
CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON
ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF
ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

The views and conclusions contained in the software and documentation are those of the
authors and should not be interpreted as representing official policies, either expressed
or implied, of krabicezpapundeklu.
*/
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