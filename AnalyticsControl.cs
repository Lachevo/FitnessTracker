using System;
using System.Drawing;
using System.Windows.Forms;

namespace FitTrackerPro
{
    public class AnalyticsControl : UserControl
    {
        public AnalyticsControl()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.BackColor = Color.White;
            this.Size = new Size(700, 400);
            // ... Copy all controls and layout from AnalyticsForm here ...
        }
    }
} 