using System;
using System.Windows.Forms;
using Microsoft.Win32;

namespace SafeConnect
{
    public partial class HiddenForm : Form
    {
        private System.ComponentModel.IContainer components = null;

        public HiddenForm()
        {
            InitializeComponent();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(0, 0);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "HiddenForm";
            this.Text = "HiddenForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.Load += new System.EventHandler(this.HiddenForm_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.HiddenForm_FormClosing);
            this.ResumeLayout(false);

        }

        private void HiddenForm_Load(object sender, EventArgs e)
        {
            //SystemEvents.TimeChanged += new EventHandler(SystemEvents_TimeChanged);
            //SystemEvents.UserPreferenceChanged += new UserPreferenceChangedEventHandler(SystemEvents_UPCChanged);
            SystemEvents.PowerModeChanged += new PowerModeChangedEventHandler(SystemEvents_PowerModeChanged);
        }

        private void HiddenForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //SystemEvents.TimeChanged -= new EventHandler(SystemEvents_TimeChanged);
            //SystemEvents.UserPreferenceChanged -= new UserPreferenceChangedEventHandler(SystemEvents_UPCChanged);
            SystemEvents.PowerModeChanged -= new PowerModeChangedEventHandler(SystemEvents_PowerModeChanged);
        }

        private void SystemEvents_TimeChanged(object sender, EventArgs e)
        {
            Logger.Log("HiddenForm: SimpleService.TimeChanged, Time changed; it is now " +
                DateTime.Now.ToLongTimeString());
        }

        private void SystemEvents_UPCChanged(object sender, UserPreferenceChangedEventArgs e)
        {
            Logger.Log("HiddenForm: SimpleService.UserPreferenceChanged, " + e.Category.ToString());
        }

        private void SystemEvents_PowerModeChanged(object sender, PowerModeChangedEventArgs e)
        {
            Logger.Log("HiddenForm: SimpleService.PowerModeChanged - " + e.Mode + " at " + DateTime.Now.ToString("f"));
            SafeConnectRefresher.HandlePowerEvent(e);
        }
    }
}
