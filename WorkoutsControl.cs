using System;
using System.Drawing;
using System.Windows.Forms;

namespace FitTrackerPro
{
    public class WorkoutsControl : UserControl
    {
        public WorkoutsControl()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.BackColor = Color.White;
            this.Size = new Size(900, 650);
            // ... Copy all controls and layout from WorkoutsForm here ...
        }
    }
} 