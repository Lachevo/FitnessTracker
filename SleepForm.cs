using System;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;

namespace FitTrackerPro
{
    public class SleepForm : Form
    {
        private Label lblTitle;
        private Label lblSubtitle;
        private Label lblTimer;
        private Button btnStart, btnStop, btnReset;
        private ProgressBar pbSleep;
        private Stopwatch stopwatch;
        private Timer timer;
        private int userId;

        public SleepForm(int userId = 0)
        {
            this.userId = userId;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "FitTracker Pro - Sleep";
            this.Size = new Size(700, 400);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;

            int leftMargin = 200;

            // Header Title
            lblTitle = new Label();
            lblTitle.Text = "Sleep & Recovery";
            lblTitle.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            lblTitle.Location = new Point(leftMargin + 30, 30);
            lblTitle.AutoSize = true;

            // Subtitle
            lblSubtitle = new Label();
            lblSubtitle.Text = "Track your sleep patterns and recovery";
            lblSubtitle.Font = new Font("Segoe UI", 12F, FontStyle.Regular);
            lblSubtitle.Location = new Point(leftMargin + 32, 75);
            lblSubtitle.AutoSize = true;
            lblSubtitle.ForeColor = Color.Gray;

            // Sleep Timer Label
            lblTimer = new Label();
            lblTimer.Text = "00:00:00";
            lblTimer.Font = new Font("Segoe UI", 32F, FontStyle.Bold);
            lblTimer.Location = new Point(leftMargin + 50, 140);
            lblTimer.Size = new Size(300, 60);

            // Progress Bar
            pbSleep = new ProgressBar();
            pbSleep.Size = new Size(400, 24);
            pbSleep.Location = new Point(leftMargin + 50, 210);
            pbSleep.Maximum = 480; // 8 hours * 60 minutes
            pbSleep.Value = 0;

            // Start/Stop/Reset Buttons
            btnStart = new Button();
            btnStart.Text = "Start";
            btnStart.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnStart.Size = new Size(100, 36);
            btnStart.Location = new Point(leftMargin + 50, 260);
            btnStart.BackColor = Color.FromArgb(0, 120, 215);
            btnStart.ForeColor = Color.White;
            btnStart.FlatStyle = FlatStyle.Flat;
            btnStart.FlatAppearance.BorderSize = 0;
            btnStart.Click += BtnStart_Click;

            btnStop = new Button();
            btnStop.Text = "Stop";
            btnStop.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnStop.Size = new Size(100, 36);
            btnStop.Location = new Point(leftMargin + 170, 260);
            btnStop.BackColor = Color.FromArgb(220, 53, 69);
            btnStop.ForeColor = Color.White;
            btnStop.FlatStyle = FlatStyle.Flat;
            btnStop.FlatAppearance.BorderSize = 0;
            btnStop.Click += BtnStop_Click;

            btnReset = new Button();
            btnReset.Text = "Reset";
            btnReset.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnReset.Size = new Size(100, 36);
            btnReset.Location = new Point(leftMargin + 290, 260);
            btnReset.BackColor = Color.FromArgb(108, 117, 125);
            btnReset.ForeColor = Color.White;
            btnReset.FlatStyle = FlatStyle.Flat;
            btnReset.FlatAppearance.BorderSize = 0;
            btnReset.Click += BtnReset_Click;

            // Timer logic
            stopwatch = new Stopwatch();
            timer = new Timer();
            timer.Interval = 1000;
            timer.Tick += Timer_Tick;

            // Remove old message panel
            // Add controls to the form
            this.Controls.Add(lblTitle);
            this.Controls.Add(lblSubtitle);
            this.Controls.Add(lblTimer);
            this.Controls.Add(pbSleep);
            this.Controls.Add(btnStart);
            this.Controls.Add(btnStop);
            this.Controls.Add(btnReset);
        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            stopwatch.Start();
            timer.Start();
        }

        private void BtnStop_Click(object sender, EventArgs e)
        {
            stopwatch.Stop();
            timer.Stop();
            // Log sleep to database
            if (userId > 0)
            {
                int sleepMinutes = (int)stopwatch.Elapsed.TotalMinutes;
                if (sleepMinutes > 0)
                {
                    using (var conn = new System.Data.SqlClient.SqlConnection(DatabaseHelper.ConnectionString))
                    {
                        conn.Open();
                        string sql = @"IF EXISTS (SELECT 1 FROM UserSleep WHERE UserId = @UserId AND Date = @Date)
                                        UPDATE UserSleep SET SleepMinutes = @SleepMinutes WHERE UserId = @UserId AND Date = @Date
                                        ELSE
                                        INSERT INTO UserSleep (UserId, Date, SleepMinutes) VALUES (@UserId, @Date, @SleepMinutes)";
                        using (var cmd = new System.Data.SqlClient.SqlCommand(sql, conn))
                        {
                            cmd.Parameters.AddWithValue("@UserId", userId);
                            cmd.Parameters.AddWithValue("@Date", DateTime.Now.Date);
                            cmd.Parameters.AddWithValue("@SleepMinutes", sleepMinutes);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    MessageBox.Show($"Sleep logged: {sleepMinutes / 60.0:F2} hours.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            stopwatch.Reset();
            lblTimer.Text = "00:00:00";
            pbSleep.Value = 0;
            timer.Stop();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            TimeSpan ts = stopwatch.Elapsed;
            lblTimer.Text = string.Format("{0:00}:{1:00}:{2:00}", ts.Hours, ts.Minutes, ts.Seconds);
            int totalMinutes = (int)ts.TotalMinutes;
            pbSleep.Value = Math.Min(pbSleep.Maximum, totalMinutes);
            if (totalMinutes >= pbSleep.Maximum)
            {
                timer.Stop();
                stopwatch.Stop();
                MessageBox.Show("Congratulations! You reached 8 hours of sleep!", "Sleep Goal", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
} 