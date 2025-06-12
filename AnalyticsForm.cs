using System;
using System.Drawing;
using System.Windows.Forms;

namespace FitTrackerPro
{
    public class AnalyticsForm : Form
    {
        private Label lblTitle;
        private Label lblSubtitle;
        private Panel panelMessage;
        private Label lblMessage;

        public AnalyticsForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblSubtitle = new System.Windows.Forms.Label();
            this.panelMessage = new System.Windows.Forms.Panel();
            this.lblMessage = new System.Windows.Forms.Label();
            this.panelMessage.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(30, 30);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(435, 65);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Analytics & Reports";
            // 
            // lblSubtitle
            // 
            this.lblSubtitle.AutoSize = true;
            this.lblSubtitle.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblSubtitle.ForeColor = System.Drawing.Color.Gray;
            this.lblSubtitle.Location = new System.Drawing.Point(32, 75);
            this.lblSubtitle.Name = "lblSubtitle";
            this.lblSubtitle.Size = new System.Drawing.Size(460, 32);
            this.lblSubtitle.TabIndex = 1;
            this.lblSubtitle.Text = "Detailed insights into your fitness journey";
            // 
            // panelMessage
            // 
            this.panelMessage.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panelMessage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelMessage.Controls.Add(this.lblMessage);
            this.panelMessage.Location = new System.Drawing.Point(100, 150);
            this.panelMessage.Name = "panelMessage";
            this.panelMessage.Size = new System.Drawing.Size(500, 80);
            this.panelMessage.TabIndex = 2;
            // 
            // lblMessage
            // 
            this.lblMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblMessage.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic);
            this.lblMessage.ForeColor = System.Drawing.Color.Gray;
            this.lblMessage.Location = new System.Drawing.Point(0, 0);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(498, 78);
            this.lblMessage.TabIndex = 0;
            this.lblMessage.Text = "Analytics module coming soon...";
            this.lblMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // AnalyticsForm
            // 
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(678, 474);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblSubtitle);
            this.Controls.Add(this.panelMessage);
            this.Name = "AnalyticsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FitTracker Pro - Analytics";
            this.panelMessage.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
} 