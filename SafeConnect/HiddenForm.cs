using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;

namespace SafeConnect
{
    public partial class HiddenForm : Form
    {
        private NotifyIcon m_notifyicon;
        private ContextMenu m_menu;  

        public HiddenForm()
        {
            InitializeComponent();

            m_menu = new ContextMenu();
            m_menu.MenuItems.Add(0,
                new MenuItem("Refresh", new System.EventHandler(Refresh_Click)));
            m_menu.MenuItems.Add(1,
                new MenuItem("Exit", new System.EventHandler(Exit_Click)));

            m_notifyicon = new NotifyIcon();
            m_notifyicon.Text = "Right click for context menu";
            m_notifyicon.Visible = true;
            //m_notifyicon.Icon = new Icon(GetType(), "Icon1.ico");
            m_notifyicon.Icon = SystemIcons.Hand;
            m_notifyicon.ContextMenu = m_menu;
        }

        protected void Exit_Click(Object sender, System.EventArgs e)
        {
            Close();
        }
        protected void Hide_Click(Object sender, System.EventArgs e)
        {
            Hide();
        }
        protected void Refresh_Click(Object sender, System.EventArgs e)
        {
            SafeConnectRefresher.MakeWebRequest();
        }

        private void HiddenForm_Load(object sender, EventArgs e)
        {
            SystemEvents.TimeChanged += new EventHandler(SystemEvents_TimeChanged);
            SystemEvents.UserPreferenceChanged += new UserPreferenceChangedEventHandler(SystemEvents_UPCChanged);
            SystemEvents.PowerModeChanged += new PowerModeChangedEventHandler(SystemEvents_PowerModeChanged);
        }

        private void HiddenForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            SystemEvents.TimeChanged -= new EventHandler(SystemEvents_TimeChanged);
            SystemEvents.UserPreferenceChanged -= new UserPreferenceChangedEventHandler(SystemEvents_UPCChanged);
        }

        private void SystemEvents_TimeChanged(object sender, EventArgs e)
        {
            Console.WriteLine("SimpleService.TimeChanged, Time changed; it is now " +
                DateTime.Now.ToLongTimeString());
        }

        private void SystemEvents_UPCChanged(object sender, UserPreferenceChangedEventArgs e)
        {
            Console.WriteLine("SimpleService.UserPreferenceChanged, " + e.Category.ToString());
        }

        private void SystemEvents_PowerModeChanged(object sender, PowerModeChangedEventArgs e)
        {
            Console.WriteLine("SimpleService.PowerModeChanged - " + e.Mode);
        }
    }
}
