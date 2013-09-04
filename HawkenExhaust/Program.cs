using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Drawing;
using Microsoft.Win32;


namespace HawkenExhaust
{
    class Program
    {
        static void Main(string[] args)
        {

            string hawkenPath = 
                Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
                "MeteorEntertainment",
                "Hawken"); //Assign to the default path for now.
       
            try
            {
                hawkenPath = Program.GetHawkenInstallPath(); //Try to get path from registry
            }
            catch (Exception)
            {
                MessageBox.Show("Hawken not found. Please install Hawken from http://www.playhawken.com/ before playing");
                Process.Start("http://www.playhawken.com/");
                Environment.Exit(0);
            }

            Application.EnableVisualStyles();
            var notifyIcon = new NotifyIcon();
            notifyIcon.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            notifyIcon.Text = "Hawken is running. HawkenExhaust 0.1";
            notifyIcon.Visible = true;

            try
            {
                new SimpleLauncherGameRelauncher("HawkenGame-Win32-Shipping",
                    Path.Combine(hawkenPath, "Binaries", "Win32"),
                    "HawkenLauncher",
                    hawkenPath).runAll();
            }
            catch (FileNotFoundException e)
            {
                MessageBox.Show(String.Format("Error: {0}", e.Message), "Exception occured when launching Hawken");
            }
        }

        public static string GetHawkenInstallPath()
        {
            string hawkenDefaultPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
                "MeteorEntertainment",
                "Hawken");
            string hawkenPath = Registry.GetValue(@"HKEY_CURRENT_USER\Software\MeteorEntertainment\Hawken", "InstDir", hawkenDefaultPath).ToString();
            return Path.Combine(hawkenPath, "InstalledHawkenFiles");
        }
    }
}
