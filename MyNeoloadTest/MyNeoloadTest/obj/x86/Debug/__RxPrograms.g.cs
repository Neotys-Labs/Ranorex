using System;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Runtime.CompilerServices;
using Ranorex;
using Ranorex.Core.Resolver;

namespace __RxMain
{
    /// <summary>
    /// Modified Program class that ensures that the Ranorex System gets initialized and cleaned up
    /// correctly. This file is auto-generated during project build. This class will invoke the
    /// original 'Main' method implementation as needed.
    /// </summary>
    [GeneratedCode("Ranorex", "8.1")]
    public class __RxProgram
    {
        /// <summary>
        /// The entry point of the application.
        /// </summary>
        /// <param name="args">Contains any number of command line arguments.</param>
        /// <returns>The return value of the projects original 'Main' implementation.</returns>
        [STAThread]
        public static int Main(string[] args)
        {
            try
            {
                AssemblyLoader.Initialize();

                RanorexInit();

                return MainInvoker.Invoke(Assembly.GetExecutingAssembly(), "", args);
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.ToString());
                return PrerequisiteChecker.HandleTestExecutableMainException(exc);
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void RanorexInit()
        {
            TestingBootstrapper.SetupCore();
        }
    }
}
