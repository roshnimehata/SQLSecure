using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using System.Threading;

using Infragistics.Win.AppStyling;

using Idera.SQLsecure.Core.Logger;
using System.Security.Principal;

namespace Idera.SQLsecure.UI.Console
{
    static class Program
    {
        static public Controller gController;
        static public Forms.Form_Splash splashScreen;
        private static LogX logX = new LogX("Idera.SQLsecure.UI.Console.Program");
        public static WindowsImpersonationContext targetRepositoryImpersonationContext = null;//SQLsecure 3.1 (Tushar)--Supporting windows auth for repository connection

        #region Helpers
        private static void startup()
        {
            // Initialize diagnostic logging.
            LogX.Initialize();
            logX = new LogX("Idera.SQLsecure.UI.Console.Program");

            logX.loggerX.Info("Console started at ", DateTime.Now.ToString());
        }

        private static void shutdown()
        {
            logX.loggerX.Info("Console ended at ", DateTime.Now.ToString());
        }

        private static void ShowSplashScreen()
        {
            //      Splash splash = new Splash();
            splashScreen = new Forms.Form_Splash();
            splashScreen.ShowDialog();
        }


        #endregion

        #region Main
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            // Initialize the Console (sets up logging, etc.).
            startup();

            // Start splash screen.
            Thread splashThread = new Thread(new ThreadStart(ShowSplashScreen));
            splashThread.Start();

            // Start the application.
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            StyleManager.Load(
                Assembly.GetExecutingAssembly().GetManifestResourceStream(
                    "Idera.SQLsecure.UI.Console.Styles.Office2007Black.isl"));

            gController = new Controller(); // Contruct this object in this order
            Application.Run(new MainForm());

            // Save the user options.
            Utility.UserData.Current.Save();

            //Start-SQLsecure 3.1 (Tushar)--Supporting windows auth for repository connection
            if (targetRepositoryImpersonationContext != null)
            {
                targetRepositoryImpersonationContext.Undo();
                targetRepositoryImpersonationContext.Dispose();
                targetRepositoryImpersonationContext = null;
            }
            //End-SQLsecure 3.1 (Tushar)--Supporting windows auth for repository connection
            // Exiting utility, do shutdown processing.
            shutdown();
        }
        #endregion
    }
}