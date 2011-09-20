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
        public HiddenForm()
        {
            InitializeComponent();
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
