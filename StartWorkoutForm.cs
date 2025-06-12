using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Diagnostics;

namespace FitTrackerPro
{
    public class StartWorkoutForm : Form
    {
        private int userId;
        private RadioButton rbUpper;
        private RadioButton rbLower;
        private ListBox lstExercises;
        private Button btnStartTimer;
        private Button btnPauseTimer;
        private Button btnResetTimer;
        private Label lblTimer;
        private Button btnFinishWorkout;
        private Timer timer;
        private Stopwatch stopwatch;
        private List<string> upperExercises = new List<string> { "Bench Press", "Rows", "Shoulder Press", "Pull Ups", "Bicep Curls", "Tricep Extensions" };
        private List<string> lowerExercises = new List<string> { "Squats", "Lunges", "Deadlifts", "Leg Press", "Calf Raises", "Hamstring Curls" };
        private NumericUpDown nudCalories;

        public StartWorkoutForm(int userId)
        {
            this.userId = userId;
            BuildUI();
        }

        private void BuildUI()
        {
            this.Text = "Start Workout";
            this.Width = 400;
            this.Height = 500;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;

            Label lblSplit = new Label { Text = "Choose Split:", Left = 30, Top = 20, Width = 100 };
            rbUpper = new RadioButton { Text = "Upper Body", Left = 140, Top = 20, Width = 100, Checked = true };
            rbLower = new RadioButton { Text = "Lower Body", Left = 250, Top = 20, Width = 100 };
            rbUpper.CheckedChanged += SplitChanged;
            rbLower.CheckedChanged += SplitChanged;

            Label lblExercises = new Label { Text = "Exercises:", Left = 30, Top = 60, Width = 100 };
            lstExercises = new ListBox { Left = 30, Top = 85, Width = 320, Height = 120 };

            Label lblTimerTitle = new Label { Text = "Workout Timer:", Left = 30, Top = 220, Width = 120 };
            lblTimer = new Label { Text = "00:00:00", Left = 160, Top = 220, Width = 100, Font = new System.Drawing.Font("Segoe UI", 12, System.Drawing.FontStyle.Bold) };
            btnStartTimer = new Button { Text = "Start", Left = 30, Top = 260, Width = 80 };
            btnPauseTimer = new Button { Text = "Pause", Left = 120, Top = 260, Width = 80 };
            btnResetTimer = new Button { Text = "Reset", Left = 210, Top = 260, Width = 80 };

            // Calories Burned input
            Label lblCalories = new Label { Text = "Calories Burned:", Left = 30, Top = 300, Width = 120 };
            nudCalories = new NumericUpDown { Left = 160, Top = 295, Width = 100, Minimum = 0, Maximum = 5000, Value = 0 };

            btnStartTimer.Click += BtnStartTimer_Click;
            btnPauseTimer.Click += BtnPauseTimer_Click;
            btnResetTimer.Click += BtnResetTimer_Click;

            btnFinishWorkout = new Button { Text = "Finish Workout", Left = 100, Top = 340, Width = 180 };
            btnFinishWorkout.Click += BtnFinishWorkout_Click;

            this.Controls.Add(lblSplit);
            this.Controls.Add(rbUpper);
            this.Controls.Add(rbLower);
            this.Controls.Add(lblExercises);
            this.Controls.Add(lstExercises);
            this.Controls.Add(lblTimerTitle);
            this.Controls.Add(lblTimer);
            this.Controls.Add(btnStartTimer);
            this.Controls.Add(btnPauseTimer);
            this.Controls.Add(btnResetTimer);
            this.Controls.Add(lblCalories);
            this.Controls.Add(nudCalories);
            this.Controls.Add(btnFinishWorkout);

            stopwatch = new Stopwatch();
            timer = new Timer();
            timer.Interval = 1000;
            timer.Tick += Timer_Tick;

            UpdateExerciseList();
        }

        private void SplitChanged(object sender, EventArgs e)
        {
            UpdateExerciseList();
        }

        private void UpdateExerciseList()
        {
            lstExercises.Items.Clear();
            if (rbUpper.Checked)
            {
                foreach (var ex in upperExercises)
                    lstExercises.Items.Add(ex);
            }
            else
            {
                foreach (var ex in lowerExercises)
                    lstExercises.Items.Add(ex);
            }
        }

        private void BtnStartTimer_Click(object sender, EventArgs e)
        {
            stopwatch.Start();
            timer.Start();
        }

        private void BtnPauseTimer_Click(object sender, EventArgs e)
        {
            stopwatch.Stop();
            timer.Stop();
        }

        private void BtnResetTimer_Click(object sender, EventArgs e)
        {
            stopwatch.Reset();
            lblTimer.Text = "00:00:00";
            timer.Stop();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            TimeSpan ts = stopwatch.Elapsed;
            lblTimer.Text = string.Format("{0:00}:{1:00}:{2:00}", ts.Hours, ts.Minutes, ts.Seconds);
        }

        private void BtnFinishWorkout_Click(object sender, EventArgs e)
        {
            stopwatch.Stop();
            timer.Stop();
            int duration = (int)stopwatch.Elapsed.TotalMinutes;
            if (duration == 0) duration = 1; // Minimum 1 minute
            string split = rbUpper.Checked ? "Upper Body" : "Lower Body";
            int calories = (int)nudCalories.Value;
            if (calories == 0) calories = duration * 7; // fallback estimate if not set
            DatabaseHelper.LogWorkout(userId, DateTime.Now.Date, split, duration, calories);
            MessageBox.Show("Workout logged!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }
    }
} 