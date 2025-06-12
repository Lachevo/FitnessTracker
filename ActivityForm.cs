using System;
using System.Drawing;
using System.Windows.Forms;

namespace FitTrackerPro
{
    public class ActivityForm : Form
    {
        private Label lblTitle;
        private Label lblSubtitle;
        private Panel panelMessage;
        private Label lblMessage;
        private int userId;

        public ActivityForm(int userId)
        {
            this.userId = userId;
            InitializeComponent();
        }

        public ActivityForm() : this(0) { }

        private void InitializeComponent()
        {
            this.Text = "FitTracker Pro - Activity";
            this.Size = new Size(900, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;

            // Header Title
            lblTitle = new Label();
            lblTitle.Text = "Activity Tracking";
            lblTitle.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            int leftMargin = 200; // To match sidebar offset
            lblTitle.Location = new Point(leftMargin + 30, 30);
            lblTitle.AutoSize = true;

            // Subtitle
            lblSubtitle = new Label();
            lblSubtitle.Text = "Monitor your daily movement and exercise";
            lblSubtitle.Font = new Font("Segoe UI", 12F, FontStyle.Regular);
            lblSubtitle.Location = new Point(leftMargin + 32, 75);
            lblSubtitle.AutoSize = true;
            lblSubtitle.ForeColor = Color.Gray;

            // Activities Grid
            TableLayoutPanel grid = new TableLayoutPanel();
            grid.ColumnCount = 3;
            grid.RowCount = 2;
            grid.Location = new Point(leftMargin + 30, 130);
            grid.Size = new Size(760, 350);
            grid.BackColor = Color.White;
            grid.Padding = new Padding(0);
            grid.CellBorderStyle = TableLayoutPanelCellBorderStyle.None;
            grid.ColumnStyles.Clear();
            for (int i = 0; i < 3; i++) grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            grid.RowStyles.Clear();
            for (int i = 0; i < 2; i++) grid.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));

            var activities = new[]
            {
                new { Name = "Walking", Icon = "🚶", Desc = "Track your daily steps and walking sessions." },
                new { Name = "Running", Icon = "🏃", Desc = "Log your runs and monitor your pace." },
                new { Name = "Cycling", Icon = "🚴", Desc = "Record your cycling workouts and distance." },
                new { Name = "Swimming", Icon = "🏊", Desc = "Track your swim laps and duration." },
                new { Name = "Yoga", Icon = "🧘", Desc = "Log your yoga and stretching routines." },
                new { Name = "HIIT", Icon = "🔥", Desc = "High-Intensity Interval Training sessions." }
            };

            int idx = 0;
            foreach (var act in activities)
            {
                Panel card = new Panel();
                card.Size = new Size(230, 150);
                card.Margin = new Padding(18);
                card.BackColor = Color.FromArgb(245, 247, 250);
                card.BorderStyle = BorderStyle.FixedSingle;

                Label lblIcon = new Label();
                lblIcon.Text = act.Icon;
                lblIcon.Font = new Font("Segoe UI Emoji", 36F, FontStyle.Regular);
                lblIcon.Location = new Point(15, 10);
                lblIcon.Size = new Size(60, 60);

                Label lblName = new Label();
                lblName.Text = act.Name;
                lblName.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
                lblName.Location = new Point(85, 20);
                lblName.Size = new Size(120, 30);

                Label lblDesc = new Label();
                lblDesc.Text = act.Desc;
                lblDesc.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
                lblDesc.Location = new Point(15, 80);
                lblDesc.Size = new Size(200, 50);
                lblDesc.ForeColor = Color.Gray;

                card.Controls.Add(lblIcon);
                card.Controls.Add(lblName);
                card.Controls.Add(lblDesc);

                // Make card clickable
                card.Cursor = Cursors.Hand;
                card.Click += (s, e) => OpenActivityLogForm(act.Name);
                foreach (Control c in card.Controls) c.Click += (s, e) => OpenActivityLogForm(act.Name);

                grid.Controls.Add(card, idx % 3, idx / 3);
                idx++;
            }

            // Add controls to the form
            this.Controls.Add(lblTitle);
            this.Controls.Add(lblSubtitle);
            this.Controls.Add(grid);
        }

        private void OpenActivityLogForm(string activityName)
        {
            if (userId <= 0)
            {
                MessageBox.Show("Invalid user. Please log in again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (activityName == "Running")
            {
                new TrackRunForm(userId).ShowDialog();
            }
            else if (activityName == "Cycling")
            {
                new TrackRunForm(userId) { Text = "Track Cycling" }.ShowDialog();
            }
            else
            {
                new LogSimpleActivityForm(userId, activityName).ShowDialog();
            }
        }
    }
} 