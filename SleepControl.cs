using System;
using System.Drawing;
using System.Windows.Forms;

namespace FitTrackerPro
{
    public class SleepControl : UserControl
    {
        public SleepControl()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.BackColor = Color.White;
            this.Size = new Size(700, 400);
            // ... Copy all controls and layout from SleepForm here ...
        }
    }
} 