using System;
using System.Drawing;
using System.Windows.Forms;

namespace FitTrackerPro
{
    public class ActivityControl : UserControl
    {
        public ActivityControl()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // ActivityControl
            // 
            this.BackColor = System.Drawing.Color.White;
            this.Name = "ActivityControl";
            this.Size = new System.Drawing.Size(700, 456);
            this.ResumeLayout(false);

        }
    }
} 