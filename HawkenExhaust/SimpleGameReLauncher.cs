// -----------------------------------------------------------------------
// <copyright file="SimpleLauncherGameRelauncher.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace HawkenExhaust
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Diagnostics;
    using System.ComponentModel;
    using System.IO;
    using System.Threading;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SimpleGameReLauncher
    {
        protected string gameProcessName;
        protected string gameProcessPath;
        protected string launcherProcessName;
        protected string launcherProcessPath;

        public delegate void LauncherCloseEventHandler(object sender);
        public event LauncherCloseEventHandler OnLauncherClose;

        public SimpleGameReLauncher(string gameProcessName, string gameProcessPath, string launcherProcessName, string launcherProcessPath)
        {
            this.launcherProcessName = launcherProcessName;
            this.gameProcessName = gameProcessName;

            this.launcherProcessPath = launcherProcessPath;
            this.gameProcessPath = gameProcessPath;

        }


        /// <summary>
        /// This loop runs indefinetley looking for processName and then relaunching it.
        /// It runs on the main thread so as to keep the application alive. 
        /// </summary>
        /// <param name="processName">name of the process to look for</param>
        /// <param name="processPath">path where process is located</param>
        protected void GameProcessListener(string processName, string processPath)
        {
            while (true)
            {
                Process process = null;
                while (process == null)
                {
                    if (SimpleGameReLauncher.IsProcessRunning(processName))
                    {
                        process = SimpleGameReLauncher.GetProcess(processName);
                        process.Kill();
                        process.Dispose(); //Dispose the old process.
                        break;
                    }
                    Thread.Sleep(500);
                }

                using (var gameProcess = Process.Start(Path.Combine(processPath, processName) + ".exe"))
                {
                    gameProcess.WaitForExit();
                }
            }
        }

        /// <summary>
        /// This loop runs until the launcher process has quit.
        /// To close the application, handle OnLauncherClose.
        /// </summary>
        /// <param name="launcherProcessName">name of the launcher processes</param>
        protected void LauncherQuitListener(string launcherProcessName)
        {
            BackgroundWorker launcherChecker = new BackgroundWorker();
            launcherChecker.DoWork += delegate
            {
                using (Process launcher = SimpleGameReLauncher.GetProcess(launcherProcessName))
                launcher.WaitForExit();
            };

            launcherChecker.RunWorkerCompleted += delegate
            {
                OnLauncherClose(this);
                launcherChecker.Dispose();

            };

            launcherChecker.RunWorkerAsync();
        }

        public static Process GetProcess(string processName)
        {
            Process[] processes = Process.GetProcessesByName(processName);
            if (processes.Length == 1)
            {
                return processes[0];
            }
            else
            {
                return null;
            }
        }

        public static bool IsProcessRunning(string processName)
        {
            Process[] processes = Process.GetProcessesByName(processName);
            if (processes.Length == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }



        public void checkFiles()
        {
            if (!File.Exists(Path.Combine(launcherProcessPath, launcherProcessName) + ".exe")) throw new FileNotFoundException("HawkenLauncher.exe not found"); //Edit exception messages if used generically as a library.
            if (!File.Exists(Path.Combine(gameProcessPath, gameProcessName) + ".exe")) throw new FileNotFoundException("Hawken is not installed");
        }

        public void startListener()
        {
            var launcherApp = new ProcessStartInfo(Path.Combine(launcherProcessPath, launcherProcessName) + ".exe");
            launcherApp.WorkingDirectory = launcherProcessPath;

            Process.Start(launcherApp);
            this.LauncherQuitListener(launcherProcessName);
            this.GameProcessListener(gameProcessName, gameProcessPath);
        }

        public void runAll()
        {
            this.checkFiles();
            this.startListener();
        }
    }
}
