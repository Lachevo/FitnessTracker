using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace FitTrackerPro
{
    public class WorkoutScheduleForm : Form
    {
        private Label lblTitle;
        private ListBox lstExercises;
        private Label lblTimer;
        private Button btnStart, btnPause, btnReset, btnClose, btnFinish;
        private Timer timer;
        private Stopwatch stopwatch;
        private string muscleGroup;
        private int userId;
        private Dictionary<string, List<string>> exercisesByGroup = new Dictionary<string, List<string>>
        {
            { "Arms", new List<string> { "Bicep Curls", "Tricep Extensions", "Hammer Curls", "Skull Crushers" } },
            { "Chest", new List<string> { "Bench Press", "Push-ups", "Chest Flyes", "Incline Press" } },
            { "Leg", new List<string> { "Squats", "Lunges", "Leg Press", "Calf Raises" } },
            { "Shoulder", new List<string> { "Shoulder Press", "Lateral Raises", "Front Raises", "Reverse Flyes" } },
            { "Back", new List<string> { "Pull-ups", "Deadlifts", "Rows", "Lat Pulldowns" } }
        };
        private NumericUpDown nudCalories;

        public WorkoutScheduleForm(int userId, string muscleGroup)
        {
            this.userId = userId;
            this.muscleGroup = muscleGroup;
            BuildUI();
        }

        private void BuildUI()
        {
            this.Text = $"{muscleGroup} Workout Schedule";
            this.Size = new Size(400, 500);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;

            lblTitle = new Label { Text = $"{muscleGroup} Workout", Font = new Font("Segoe UI", 16, FontStyle.Bold), Location = new Point(30, 20), AutoSize = true };
            lstExercises = new ListBox { Location = new Point(30, 60), Size = new Size(320, 120) };
            if (exercisesByGroup.ContainsKey(muscleGroup))
            {
                foreach (var ex in exercisesByGroup[muscleGroup])
                    lstExercises.Items.Add(ex);
            }
            else
            {
                lstExercises.Items.Add("No exercises found for this group.");
            }

            Label lblTimerTitle = new Label { Text = "Workout Timer:", Location = new Point(30, 200), Width = 120 };
            lblTimer = new Label { Text = "00:00:00", Location = new Point(160, 200), Width = 100, Font = new Font("Segoe UI", 12, FontStyle.Bold) };
            btnStart = new Button { Text = "Start", Location = new Point(30, 240), Width = 80 };
            btnPause = new Button { Text = "Pause", Location = new Point(120, 240), Width = 80 };
            btnReset = new Button { Text = "Reset", Location = new Point(210, 240), Width = 80 };
            btnClose = new Button { Text = "Close", Location = new Point(120, 300), Width = 120 };
            btnFinish = new Button { Text = "Finish Workout", Location = new Point(120, 400), Width = 120 };

            // Calories Burned input
            Label lblCalories = new Label { Text = "Calories Burned:", Location = new Point(30, 350), Width = 120 };
            nudCalories = new NumericUpDown { Location = new Point(160, 350), Width = 100, Minimum = 0, Maximum = 5000, Value = 0 };

            btnStart.Click += BtnStart_Click;
            btnPause.Click += BtnPause_Click;
            btnReset.Click += BtnReset_Click;
            btnClose.Click += (s, e) => this.Close();
            btnFinish.Click += BtnFinish_Click;

            stopwatch = new Stopwatch();
            timer = new Timer();
            timer.Interval = 1000;
            timer.Tick += Timer_Tick;

            this.Controls.Add(lblTitle);
            this.Controls.Add(lstExercises);
            this.Controls.Add(lblTimerTitle);
            this.Controls.Add(lblTimer);
            this.Controls.Add(btnStart);
            this.Controls.Add(btnPause);
            this.Controls.Add(btnReset);
            this.Controls.Add(btnClose);
            this.Controls.Add(lblCalories);
            this.Controls.Add(nudCalories);
            this.Controls.Add(btnFinish);
        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            stopwatch.Start();
            timer.Start();
        }

        private void BtnPause_Click(object sender, EventArgs e)
        {
            stopwatch.Stop();
            timer.Stop();
        }

        private void BtnReset_Click(object sender, EventArgs e)
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

        private void BtnFinish_Click(object sender, EventArgs e)
        {
            stopwatch.Stop();
            timer.Stop();
            int duration = (int)stopwatch.Elapsed.TotalMinutes;
            if (duration == 0) duration = 1; // Minimum 1 minute
            int calories = (int)nudCalories.Value;
            if (calories == 0) calories = duration * 7; // fallback estimate if not set
            DatabaseHelper.LogWorkout(userId, DateTime.Now.Date, muscleGroup, duration, calories);
            MessageBox.Show("Workout logged!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
} 