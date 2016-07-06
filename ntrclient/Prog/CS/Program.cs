using System;
using System.Windows.Forms;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using ntrclient.Prog.Window;
using ntrclient.Extra;

namespace ntrclient.Prog.CS
{
    public static class Program
    {

        public static ScriptEngine PyEngine;
        public static NtrClient NtrClient;
        public static ScriptHelper ScriptHelper;
        public static ScriptScope GlobalScope;
        public static CmdWindow GCmdWindow;
        public static SettingsManager Sm;
        public static DebugConsole Dc;

        public static void LoadConfig()
        {
            Sm = SettingsManager.LoadFromXml("ntrconfig.xml");
            Sm.Init();
        }

        public static void SaveConfig()
        {
            SettingsManager.SaveToXml("ntrconfig.xml", Sm);
        }

        /// <summary>
        /// The main entry point for the application
        /// </summary>
        [STAThread]
        private static void Main()
        {
            try
            {
                PyEngine = Python.CreateEngine();
                NtrClient = new NtrClient();
                ScriptHelper = new ScriptHelper();

                GlobalScope = PyEngine.CreateScope();
                GlobalScope.SetVariable("nc", ScriptHelper);

                LoadConfig();

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                GCmdWindow = new CmdWindow();
                Dc = new DebugConsole();
                Application.Run(GCmdWindow);
            }
            catch (Exception e)
            {
                BugReporter br = new BugReporter(e, "Program exception");
                MessageBox.Show(
                    @"WARNING - NTRDebugger has encountered an error" + Environment.NewLine +
                    @"This error is about to crash the program, please send the generated" + Environment.NewLine +
                    @"Error log to imthe666st!" + Environment.NewLine + Environment.NewLine +
                    @"Sorry for the inconvinience -imthe666st"
                    );
            }
        }
    }
}
