using System;
using System.Drawing;
using System.Windows.Forms;

namespace FitTrackerPro
{
    public class WorkoutsForm : Form
    {
        private Label lblTitle;
        private Label lblSubtitle;
        private Button btnNewWorkout;
        private TabControl tabControl;
        private int currentUserId;
        private Timer calorieOutTimer;
        private int calorieOutCurrent = 0;
        private int calorieOutTarget = 0;

        public WorkoutsForm(int userId)
        {
            currentUserId = userId;
            BuildUI();
        }

        private void BuildUI()
        {
            this.Text = "FitTracker Pro - Workouts";
            this.Dock = DockStyle.Fill;
            this.BackColor = Color.White;

            // Header Title
            lblTitle = new Label();
            lblTitle.Text = "Workouts";
            lblTitle.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            lblTitle.Location = new Point(200, 30);
            lblTitle.AutoSize = true;

            // Subtitle
            lblSubtitle = new Label();
            lblSubtitle.Text = "Plan, track, and analyze your training sessions";
            lblSubtitle.Font = new Font("Segoe UI", 12F, FontStyle.Regular);
            lblSubtitle.Location = new Point(202, 75);
            lblSubtitle.AutoSize = true;
            lblSubtitle.ForeColor = Color.Gray;

            // TabControl
            tabControl = new TabControl();
            tabControl.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            tabControl.Size = new Size(900, 450);
            tabControl.Location = new Point(200, 130);

            // Tabs
            tabControl.TabPages.Add("today", "Today's Plan");
            tabControl.TabPages.Add("library", "Exercise Library");
            tabControl.TabPages.Add("history", "History");

            // Today's Plan tab controls
            GroupBox gbToday = new GroupBox();
            gbToday.Text = "Scheduled Workout";
            gbToday.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            gbToday.Location = new Point(30, 30);
            gbToday.Size = new Size(350, 180);
            Label lblWorkout = new Label();
            lblWorkout.Text = "Upper Body Strength Training";
            lblWorkout.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            lblWorkout.Location = new Point(20, 40);
            lblWorkout.Size = new Size(250, 22);
            Label lblDuration = new Label();
            lblDuration.Text = "Duration: 45 minutes";
            lblDuration.Location = new Point(20, 70);
            lblDuration.Size = new Size(200, 22);
            Label lblExercises = new Label();
            lblExercises.Text = "Exercises: 8";
            lblExercises.Location = new Point(20, 100);
            lblExercises.Size = new Size(200, 22);
            Label lblDifficulty = new Label();
            lblDifficulty.Text = "Difficulty: Intermediate";
            lblDifficulty.Location = new Point(20, 130);
            lblDifficulty.Size = new Size(200, 22);
            Button btnStart = new Button();
            btnStart.Text = "Start Workout";
            btnStart.Location = new Point(180, 160);
            btnStart.Size = new Size(140, 32);
            btnStart.BackColor = Color.FromArgb(0, 120, 215);
            btnStart.ForeColor = Color.White;
            btnStart.FlatStyle = FlatStyle.Flat;
            btnStart.FlatAppearance.BorderSize = 0;
            btnStart.Click += (s, e) => {
                var workoutForm = new StartWorkoutForm(currentUserId);
                workoutForm.ShowDialog();
            };
            gbToday.Controls.Add(lblWorkout);
            gbToday.Controls.Add(lblDuration);
            gbToday.Controls.Add(lblExercises);
            gbToday.Controls.Add(lblDifficulty);
            gbToday.Controls.Add(btnStart);
            tabControl.TabPages[0].Controls.Add(gbToday);

            // Schedule Workout Section
            GroupBox gbSchedule = new GroupBox();
            gbSchedule.Text = "Schedule Workout";
            gbSchedule.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            gbSchedule.Location = new Point(30, 230);
            gbSchedule.Size = new Size(350, 100);

            Label lblType = new Label();
            lblType.Text = "Type:";
            lblType.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            lblType.Location = new Point(20, 35);
            lblType.Size = new Size(50, 22);

            ComboBox cbType = new ComboBox();
            cbType.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            cbType.Location = new Point(80, 32);
            cbType.Size = new Size(120, 22);
            cbType.DropDownStyle = ComboBoxStyle.DropDownList;
            cbType.Items.AddRange(new string[] { "Arms", "Chest", "Leg" , "Shoulder" , " Back"});
            cbType.SelectedIndex = 0;

            Button btnSchedule = new Button();
            btnSchedule.Text = "Schedule";
            btnSchedule.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnSchedule.Size = new Size(100, 32);
            btnSchedule.Location = new Point(210, 30);
            btnSchedule.BackColor = Color.FromArgb(0, 120, 215);
            btnSchedule.ForeColor = Color.White;
            btnSchedule.FlatStyle = FlatStyle.Flat;
            btnSchedule.FlatAppearance.BorderSize = 0;
            btnSchedule.Click += (s, e) =>
            {
                var scheduleForm = new WorkoutScheduleForm(currentUserId, cbType.SelectedItem.ToString());
                var result = scheduleForm.ShowDialog();
                if (result == DialogResult.OK)
                {
                    // Find the MainForm and reload Dashboard
                    var mainForm = this.FindForm() as MainForm;
                    if (mainForm != null)
                    {
                        mainForm.ShowSection(0); // 0 = Dashboard
                    }
                }
            };

            gbSchedule.Controls.Add(lblType);
            gbSchedule.Controls.Add(cbType);
            gbSchedule.Controls.Add(btnSchedule);
            tabControl.TabPages[0].Controls.Add(gbSchedule);

            // Exercise Library ListView
            ListView lvLibrary = new ListView();
            lvLibrary.Location = new Point(20, 20);
            lvLibrary.Size = new Size(760, 350);
            lvLibrary.View = View.Details;
            lvLibrary.Columns.Add("Exercise", 200);
            lvLibrary.Columns.Add("Category", 150);
            lvLibrary.Columns.Add("Difficulty", 120);
            lvLibrary.FullRowSelect = true;
            lvLibrary.GridLines = true;
            // Expanded exercise library
            lvLibrary.Items.Add(new ListViewItem(new string[] { "Push-ups", "Chest", "Beginner" }));
            lvLibrary.Items.Add(new ListViewItem(new string[] { "Squats", "Legs", "Beginner" }));
            lvLibrary.Items.Add(new ListViewItem(new string[] { "Deadlifts", "Back", "Intermediate" }));
            lvLibrary.Items.Add(new ListViewItem(new string[] { "Pull-ups", "Back", "Advanced" }));
            lvLibrary.Items.Add(new ListViewItem(new string[] { "Bench Press", "Chest", "Intermediate" }));
            lvLibrary.Items.Add(new ListViewItem(new string[] { "Lunges", "Legs", "Beginner" }));
            lvLibrary.Items.Add(new ListViewItem(new string[] { "Overhead Press", "Shoulders", "Intermediate" }));
            lvLibrary.Items.Add(new ListViewItem(new string[] { "Bicep Curls", "Arms", "Beginner" }));
            lvLibrary.Items.Add(new ListViewItem(new string[] { "Tricep Dips", "Arms", "Beginner" }));
            lvLibrary.Items.Add(new ListViewItem(new string[] { "Plank", "Core", "Beginner" }));
            lvLibrary.Items.Add(new ListViewItem(new string[] { "Mountain Climbers", "Core", "Intermediate" }));
            lvLibrary.Items.Add(new ListViewItem(new string[] { "Russian Twists", "Core", "Intermediate" }));
            lvLibrary.Items.Add(new ListViewItem(new string[] { "Burpees", "Full Body", "Advanced" }));
            lvLibrary.Items.Add(new ListViewItem(new string[] { "Lat Pulldown", "Back", "Beginner" }));
            lvLibrary.Items.Add(new ListViewItem(new string[] { "Seated Row", "Back", "Beginner" }));
            lvLibrary.Items.Add(new ListViewItem(new string[] { "Leg Press", "Legs", "Intermediate" }));
            lvLibrary.Items.Add(new ListViewItem(new string[] { "Calf Raises", "Legs", "Beginner" }));
            lvLibrary.Items.Add(new ListViewItem(new string[] { "Hamstring Curl", "Legs", "Intermediate" }));
            lvLibrary.Items.Add(new ListViewItem(new string[] { "Chest Fly", "Chest", "Intermediate" }));
            lvLibrary.Items.Add(new ListViewItem(new string[] { "Incline Bench Press", "Chest", "Advanced" }));
            lvLibrary.Items.Add(new ListViewItem(new string[] { "Dumbbell Shoulder Press", "Shoulders", "Intermediate" }));
            lvLibrary.Items.Add(new ListViewItem(new string[] { "Front Raises", "Shoulders", "Beginner" }));
            lvLibrary.Items.Add(new ListViewItem(new string[] { "Lateral Raises", "Shoulders", "Beginner" }));
            lvLibrary.Items.Add(new ListViewItem(new string[] { "Reverse Fly", "Shoulders", "Intermediate" }));
            lvLibrary.Items.Add(new ListViewItem(new string[] { "Tricep Extension", "Arms", "Intermediate" }));
            lvLibrary.Items.Add(new ListViewItem(new string[] { "Hammer Curl", "Arms", "Beginner" }));
            lvLibrary.Items.Add(new ListViewItem(new string[] { "Skull Crushers", "Arms", "Advanced" }));
            lvLibrary.Items.Add(new ListViewItem(new string[] { "Sit-ups", "Core", "Beginner" }));
            lvLibrary.Items.Add(new ListViewItem(new string[] { "Bicycle Crunches", "Core", "Intermediate" }));
            lvLibrary.Items.Add(new ListViewItem(new string[] { "Leg Raises", "Core", "Intermediate" }));
            lvLibrary.Items.Add(new ListViewItem(new string[] { "Farmer's Walk", "Full Body", "Intermediate" }));
            lvLibrary.Items.Add(new ListViewItem(new string[] { "Box Jumps", "Legs", "Advanced" }));
            lvLibrary.Items.Add(new ListViewItem(new string[] { "Step-ups", "Legs", "Beginner" }));
            lvLibrary.Items.Add(new ListViewItem(new string[] { "Hip Thrust", "Glutes", "Intermediate" }));
            lvLibrary.Items.Add(new ListViewItem(new string[] { "Glute Bridge", "Glutes", "Beginner" }));
            lvLibrary.Items.Add(new ListViewItem(new string[] { "Cable Crunch", "Core", "Intermediate" }));
            lvLibrary.Items.Add(new ListViewItem(new string[] { "Face Pull", "Shoulders", "Intermediate" }));
            lvLibrary.Items.Add(new ListViewItem(new string[] { "Arnold Press", "Shoulders", "Advanced" }));
            lvLibrary.Items.Add(new ListViewItem(new string[] { "Good Morning", "Back", "Advanced" }));
            lvLibrary.Items.Add(new ListViewItem(new string[] { "Hyperextension", "Back", "Intermediate" }));
            lvLibrary.Items.Add(new ListViewItem(new string[] { "Pull-over", "Chest", "Intermediate" }));
            lvLibrary.Items.Add(new ListViewItem(new string[] { "Dumbbell Row", "Back", "Intermediate" }));
            lvLibrary.Items.Add(new ListViewItem(new string[] { "Goblet Squat", "Legs", "Beginner" }));
            lvLibrary.Items.Add(new ListViewItem(new string[] { "Sumo Deadlift", "Legs", "Advanced" }));
            lvLibrary.Items.Add(new ListViewItem(new string[] { "Romanian Deadlift", "Legs", "Intermediate" }));
            lvLibrary.Items.Add(new ListViewItem(new string[] { "Sled Push", "Full Body", "Advanced" }));
            lvLibrary.Items.Add(new ListViewItem(new string[] { "Medicine Ball Slam", "Full Body", "Intermediate" }));
            tabControl.TabPages[1].Controls.Add(lvLibrary);

            // History ListView
            // This displays all activities and workouts for the user from the UserWorkouts table
            ListView lvHistory = new ListView();
            lvHistory.Location = new Point(20, 20);
            lvHistory.Size = new Size(760, 350);
            lvHistory.View = View.Details;
            lvHistory.Columns.Add("Date", 120);
            lvHistory.Columns.Add("Workout", 200);
            lvHistory.Columns.Add("Duration", 100);
            lvHistory.Columns.Add("Calories", 100);
            lvHistory.FullRowSelect = true;
            lvHistory.GridLines = true;

            // Load all workout logs for the user
            using (var conn = new System.Data.SqlClient.SqlConnection(DatabaseHelper.ConnectionString))
            {
                conn.Open();
                string sql = "SELECT Date, WorkoutType, DurationMinutes, CaloriesBurned FROM UserWorkouts WHERE UserId = @UserId ORDER BY Date DESC";
                using (var cmd = new System.Data.SqlClient.SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", currentUserId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string date = reader.IsDBNull(0) ? "" : ((System.DateTime)reader.GetDateTime(0)).ToString("yyyy-MM-dd");
                            string type = reader.IsDBNull(1) ? "" : reader.GetString(1);
                            string duration = reader.IsDBNull(2) ? "" : reader.GetInt32(2).ToString() + " min";
                            string calories = reader.IsDBNull(3) ? "" : reader.GetInt32(3).ToString();
                            lvHistory.Items.Add(new ListViewItem(new string[] { date, type, duration, calories }));
                        }
                    }
                }
            }
            tabControl.TabPages[2].Controls.Add(lvHistory);

            // Fetch calories burned today
            int caloriesBurned = DatabaseHelper.GetCaloriesBurnedToday(currentUserId);
            calorieOutTarget = caloriesBurned;

            // Calorie Outtake Stat Card
            Panel cardCalorieOut = new Panel();
            cardCalorieOut.BackColor = Color.WhiteSmoke;
            cardCalorieOut.BorderStyle = BorderStyle.FixedSingle;
            cardCalorieOut.Size = new Size(210, 110);
            cardCalorieOut.Location = new Point(900, 30); // Top right

            Label lblCalorieOutTitle = new Label();
            lblCalorieOutTitle.Text = "Calorie Outtake";
            lblCalorieOutTitle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblCalorieOutTitle.Location = new Point(15, 10);
            lblCalorieOutTitle.AutoSize = true;

            Label lblCalorieOutValue = new Label();
            lblCalorieOutValue.Text = caloriesBurned.ToString("N0");
            lblCalorieOutValue.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblCalorieOutValue.Location = new Point(15, 35);
            lblCalorieOutValue.AutoSize = true;

            // Add green progress bar for calorie outtake
            int calorieBurnGoal = 500; // Example daily goal
            ProgressBar pbCalorieOut = new ProgressBar();
            pbCalorieOut.Value = Math.Min(100, (int)(100.0 * caloriesBurned / Math.Max(1, calorieBurnGoal)));
            pbCalorieOut.Size = new Size(180, 10);
            pbCalorieOut.Location = new Point(15, 80);
            pbCalorieOut.ForeColor = Color.LimeGreen;
            pbCalorieOut.BackColor = Color.White;
            cardCalorieOut.Controls.Add(lblCalorieOutTitle);
            cardCalorieOut.Controls.Add(lblCalorieOutValue);
            cardCalorieOut.Controls.Add(pbCalorieOut);
            this.Controls.Add(cardCalorieOut);

            // Animation for calorie outtake value
            calorieOutCurrent = 0;
            lblCalorieOutValue.Text = "0";
            calorieOutTimer = new Timer();
            calorieOutTimer.Interval = 15; // ms
            calorieOutTimer.Tick += (s, e) => {
                if (calorieOutCurrent < calorieOutTarget)
                {
                    int step = Math.Max(1, (calorieOutTarget - calorieOutCurrent) / 10);
                    calorieOutCurrent += step;
                    if (calorieOutCurrent > calorieOutTarget) calorieOutCurrent = calorieOutTarget;
                    lblCalorieOutValue.Text = calorieOutCurrent.ToString("N0");
                }
                else
                {
                    lblCalorieOutValue.Text = calorieOutTarget.ToString("N0");
                    calorieOutTimer.Stop();
                }
            };
            calorieOutTimer.Start();

            // Add controls to the form
            this.Controls.Add(lblTitle);
            this.Controls.Add(lblSubtitle);
            this.Controls.Add(tabControl);
        }
    }
} 