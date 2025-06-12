using System;
using System.Drawing;
using System.Windows.Forms;

namespace FitTrackerPro
{
    public class SettingsControl : UserControl
    {
        public SettingsControl()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.BackColor = Color.White;
            this.Size = new Size(900, 650);
            // ... Copy all controls and layout from SettingsForm here ...
        }
    }
} 