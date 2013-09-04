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

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SimpleGameReLauncher
    {
        string gameProcessName;
        string gameProcessPath;
        string launcherProcessName;
        string launcherProcessPath;

        public SimpleGameReLauncher(string gameProcessName, string gameProcessPath, string launcherProcessName, string launcherProcessPath)
        {
            this.launcherProcessName = launcherProcessName;
            this.gameProcessName = gameProcessName;

            this.launcherProcessPath = launcherProcessPath;
            this.gameProcessPath = gameProcessPath;
            
        }

        private void GameProcessListener(string processName, string processPath)
        {
            while (true)
            {
                Process process = null;
                while (process == null)
                {
                    if (this.GetRunningProcess(processName) != null)
                    {
                        process = this.GetRunningProcess(processName);
                    }
                }
                process.Kill();
                var gameProcess = Process.Start(Path.Combine(processPath, processName) + ".exe");
                gameProcess.WaitForExit();
            }
        }

        private void LauncherQuitListener(string launcherProcessName)
        {
            BackgroundWorker launcherChecker = new BackgroundWorker();
            launcherChecker.DoWork += delegate
            {
                while (true)
                {
                    if (this.GetRunningProcess(launcherProcessName) == null)
                    {
                        break;
                    }
                }
                return;
            };

            launcherChecker.RunWorkerCompleted += delegate
            {
                Environment.Exit(0); ;
            };

            launcherChecker.RunWorkerAsync();
        }

        private Process GetRunningProcess(string processName)
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


        public void checkFiles()
        {
            if (!File.Exists(Path.Combine(launcherProcessPath, launcherProcessName) + ".exe")) throw new FileNotFoundException("HawkenLauncher.exe not found");
            if (!File.Exists(Path.Combine(gameProcessPath, gameProcessName) + ".exe")) throw new FileNotFoundException("Hawken is not installed");

        }

        public void startListener()
        {
            Process.Start(Path.Combine(launcherProcessPath, launcherProcessName)+".exe");
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
