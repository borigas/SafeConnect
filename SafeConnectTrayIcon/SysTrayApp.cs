using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using SafeConnectCore;
using System.Configuration;
using Microsoft.Win32;

namespace SafeConnectTrayIcon
{
    public class SysTrayApp : Form
    {
        private const string URL_KEY = "URL";
        private const string USER_AGENT_KEY = "UserAgent";
        private const string SLEEP_TIME_KEY = "SleepMillis";

        System.Timers.Timer timer = new System.Timers.Timer();

        [STAThread]
        public static void Main()
        {
            Application.Run(new SysTrayApp());
        }

        private NotifyIcon trayIcon;
        private ContextMenu trayMenu;

        public SysTrayApp()
        {
            CreateTrayIcon();
            RefreshConnection();
            CreateTimer();
            SetupPowerListener();
        }

        private void CreateTrayIcon()
        {
            // Create a simple tray menu with only one item.
            trayMenu = new ContextMenu();
            trayMenu.MenuItems.Add("Exit", OnExit);

            // Create a tray icon. In this example we use a
            // standard system icon for simplicity, but you
            // can of course use your own custom icon too.
            trayIcon = new NotifyIcon();
            trayIcon.Text = "SafeConnect";
            trayIcon.MouseClick += new MouseEventHandler(trayIcon_MouseClick);
            //trayIcon.Icon = Icon.FromHandle(Properties.Resources.trayicon_refresh.GetHicon());
            //trayIcon.Icon = new Icon(SystemIcons.Application, 40, 40);

            // Add menu to tray icon and show it.
            trayIcon.ContextMenu = trayMenu;
            trayIcon.Visible = true;
        }

        private void RefreshConnection()
        {
            trayIcon.Icon = Icon.FromHandle(Properties.Resources.trayicon_refresh.GetHicon());
            bool success = SafeConnectUpdater.MakeWebRequest();
            if (success)
            {
                trayIcon.Icon = Icon.FromHandle(Properties.Resources.trayicon_success.GetHicon());
            }
            else
            {
                trayIcon.Icon = Icon.FromHandle(Properties.Resources.trayicon_failure.GetHicon());
            }
        }

        private void CreateTimer()
        {
            timer = new System.Timers.Timer();
            timer.Interval = double.Parse(ConfigurationManager.AppSettings[SLEEP_TIME_KEY]);
            timer.Elapsed += (source, args) =>
            {
                Logger.Log("Timer elapsed");
                SafeConnectUpdater.MakeWebRequest();
            };
            timer.Start();
        }

        private void SetupPowerListener()
        {
            SystemEvents.PowerModeChanged += new PowerModeChangedEventHandler(SystemEvents_PowerModeChanged);
        }

        private void ManualRefresh()
        {
            timer.Stop();
            timer.Start();
            RefreshConnection();
        }

        private void OnExit(object sender, EventArgs e)
        {
            Application.Exit();
        }

        protected override void OnLoad(EventArgs e)
        {
            Visible = false; // Hide form window.
            ShowInTaskbar = false; // Remove from taskbar.

            base.OnLoad(e);
        }

        private void trayIcon_MouseClick(object sender, MouseEventArgs args)
        {
            Logger.Log("Tray Icon Clicked");
            ManualRefresh();
        }

        private void SystemEvents_PowerModeChanged(object sender, PowerModeChangedEventArgs e)
        {
            if (e.Mode == PowerModes.Resume)
            {
                Logger.Log("PowerModeChanged to " + e.Mode);
                ManualRefresh();
            }
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                // Release the icon resource.
                trayIcon.Dispose();
            }

            base.Dispose(isDisposing);
        }
    }
}