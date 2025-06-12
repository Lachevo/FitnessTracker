using System;
using System.Drawing;
using System.Windows.Forms;

namespace FitTrackerPro
{
    public class DashboardControl : UserControl
    {
        private int currentUserId;

        public DashboardControl(int userId)
        {
            currentUserId = userId;
            BuildUI();
        }

        public DashboardControl() : this(0) { }

        private void BuildUI()
        {
            this.BackColor = Color.White;
            this.Size = new Size(1000, 700);

            // Header Title
            Label lblTitle = new Label();
            lblTitle.Text = "Dashboard";
            lblTitle.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            lblTitle.Location = new Point(30, 30);
            lblTitle.AutoSize = true;

            // Welcome Message
            Label lblWelcome = new Label();
            lblWelcome.Text = "Welcome back! Here's your fitness overview for today.";
            lblWelcome.Font = new Font("Segoe UI", 12F, FontStyle.Regular);
            lblWelcome.Location = new Point(32, 75);
            lblWelcome.AutoSize = true;
            lblWelcome.ForeColor = Color.Gray;

            this.Controls.Add(lblTitle);
            this.Controls.Add(lblWelcome);

            // Load data
            var activity = DatabaseHelper.LoadUserActivity(currentUserId, DateTime.Today);
            var goals = DatabaseHelper.LoadUserGoals();
            int stepsToday = activity != null ? Convert.ToInt32(activity["Steps"]) : 0;
            int caloriesToday = activity != null ? Convert.ToInt32(activity["CaloriesBurned"]) : 0;
            int activeMinutes = activity != null ? Convert.ToInt32(activity["ActiveMinutes"]) : 0;
            float waterToday = activity != null ? Convert.ToSingle(activity["WaterIntake"]) : 0f;
            int stepsGoal = goals != null ? Convert.ToInt32(goals["DailySteps"]) : 10000;
            int caloriesGoal = goals != null ? Convert.ToInt32(goals["DailyCalories"]) : 2500;
            int minutesGoal = 60;
            float waterGoal = 8f;

            // Quick Stats Cards
            int cardWidth = 210;
            int cardHeight = 120;
            int cardSpacing = 20;
            int cardTop = 120;
            int cardLeftStart = 30;

            // Card 1: Steps Today
            Panel cardSteps = new Panel();
            cardSteps.BackColor = Color.WhiteSmoke;
            cardSteps.BorderStyle = BorderStyle.FixedSingle;
            cardSteps.Size = new Size(cardWidth, cardHeight);
            cardSteps.Location = new Point(cardLeftStart, cardTop);

            Label lblStepsTitle = new Label();
            lblStepsTitle.Text = "Steps Today";
            lblStepsTitle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblStepsTitle.Location = new Point(15, 10);
            lblStepsTitle.AutoSize = true;

            Label lblStepsValue = new Label();
            lblStepsValue.Text = stepsToday.ToString("N0");
            lblStepsValue.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblStepsValue.Location = new Point(15, 35);
            lblStepsValue.AutoSize = true;

            Label lblStepsSub = new Label();
            int stepsPercent = stepsGoal > 0 ? (int)((stepsToday * 100.0) / stepsGoal) : 0;
            lblStepsSub.Text = $"{stepsPercent}% of goal";
            lblStepsSub.Font = new Font("Segoe UI", 8F, FontStyle.Regular);
            lblStepsSub.ForeColor = Color.Gray;
            lblStepsSub.Location = new Point(15, 70);
            lblStepsSub.AutoSize = true;

            ProgressBar pbSteps = new ProgressBar();
            pbSteps.Value = Math.Min(100, stepsPercent);
            pbSteps.Size = new Size(cardWidth - 30, 10);
            pbSteps.Location = new Point(15, 95);

            cardSteps.Controls.Add(lblStepsTitle);
            cardSteps.Controls.Add(lblStepsValue);
            cardSteps.Controls.Add(lblStepsSub);
            cardSteps.Controls.Add(pbSteps);

            // Card 2: Calories Burned
            Panel cardCalories = new Panel();
            cardCalories.BackColor = Color.WhiteSmoke;
            cardCalories.BorderStyle = BorderStyle.FixedSingle;
            cardCalories.Size = new Size(cardWidth, cardHeight);
            cardCalories.Location = new Point(cardLeftStart + (cardWidth + cardSpacing) * 1, cardTop);

            Label lblCaloriesTitle = new Label();
            lblCaloriesTitle.Text = "Calories Burned";
            lblCaloriesTitle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblCaloriesTitle.Location = new Point(15, 10);
            lblCaloriesTitle.AutoSize = true;

            Label lblCaloriesValue = new Label();
            lblCaloriesValue.Text = caloriesToday.ToString("N0");
            lblCaloriesValue.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblCaloriesValue.Location = new Point(15, 35);
            lblCaloriesValue.AutoSize = true;

            Label lblCaloriesSub = new Label();
            int caloriesPercent = caloriesGoal > 0 ? (int)((caloriesToday * 100.0) / caloriesGoal) : 0;
            lblCaloriesSub.Text = $"Goal: {caloriesGoal:N0} cal";
            lblCaloriesSub.Font = new Font("Segoe UI", 8F, FontStyle.Regular);
            lblCaloriesSub.ForeColor = Color.Gray;
            lblCaloriesSub.Location = new Point(15, 70);
            lblCaloriesSub.AutoSize = true;

            ProgressBar pbCalories = new ProgressBar();
            pbCalories.Value = Math.Min(100, caloriesPercent);
            pbCalories.Size = new Size(cardWidth - 30, 10);
            pbCalories.Location = new Point(15, 95);

            cardCalories.Controls.Add(lblCaloriesTitle);
            cardCalories.Controls.Add(lblCaloriesValue);
            cardCalories.Controls.Add(lblCaloriesSub);
            cardCalories.Controls.Add(pbCalories);

            // Card 3: Active Minutes
            Panel cardMinutes = new Panel();
            cardMinutes.BackColor = Color.WhiteSmoke;
            cardMinutes.BorderStyle = BorderStyle.FixedSingle;
            cardMinutes.Size = new Size(cardWidth, cardHeight);
            cardMinutes.Location = new Point(cardLeftStart + (cardWidth + cardSpacing) * 2, cardTop);

            Label lblMinutesTitle = new Label();
            lblMinutesTitle.Text = "Active Minutes";
            lblMinutesTitle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblMinutesTitle.Location = new Point(15, 10);
            lblMinutesTitle.AutoSize = true;

            Label lblMinutesValue = new Label();
            lblMinutesValue.Text = activeMinutes.ToString();
            lblMinutesValue.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblMinutesValue.Location = new Point(15, 35);
            lblMinutesValue.AutoSize = true;

            Label lblMinutesSub = new Label();
            int minutesPercent = minutesGoal > 0 ? (int)((activeMinutes * 100.0) / minutesGoal) : 0;
            lblMinutesSub.Text = $"Goal: {minutesGoal} minutes";
            lblMinutesSub.Font = new Font("Segoe UI", 8F, FontStyle.Regular);
            lblMinutesSub.ForeColor = Color.Gray;
            lblMinutesSub.Location = new Point(15, 70);
            lblMinutesSub.AutoSize = true;

            ProgressBar pbMinutes = new ProgressBar();
            pbMinutes.Value = Math.Min(100, minutesPercent);
            pbMinutes.Size = new Size(cardWidth - 30, 10);
            pbMinutes.Location = new Point(15, 95);

            cardMinutes.Controls.Add(lblMinutesTitle);
            cardMinutes.Controls.Add(lblMinutesValue);
            cardMinutes.Controls.Add(lblMinutesSub);
            cardMinutes.Controls.Add(pbMinutes);

            // Card 4: Water Intake
            Panel cardWater = new Panel();
            cardWater.BackColor = Color.WhiteSmoke;
            cardWater.BorderStyle = BorderStyle.FixedSingle;
            cardWater.Size = new Size(cardWidth, cardHeight);
            cardWater.Location = new Point(cardLeftStart + (cardWidth + cardSpacing) * 3, cardTop);

            Label lblWaterTitle = new Label();
            lblWaterTitle.Text = "Water Intake";
            lblWaterTitle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblWaterTitle.Location = new Point(15, 10);
            lblWaterTitle.AutoSize = true;

            Label lblWaterValue = new Label();
            lblWaterValue.Text = waterToday.ToString("0.##") + "L";
            lblWaterValue.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblWaterValue.Location = new Point(15, 35);
            lblWaterValue.AutoSize = true;

            Label lblWaterSub = new Label();
            int waterPercent = waterGoal > 0 ? (int)((waterToday * 100.0) / waterGoal) : 0;
            lblWaterSub.Text = $"Goal: {waterGoal}L";
            lblWaterSub.Font = new Font("Segoe UI", 8F, FontStyle.Regular);
            lblWaterSub.ForeColor = Color.Gray;
            lblWaterSub.Location = new Point(15, 70);
            lblWaterSub.AutoSize = true;

            ProgressBar pbWater = new ProgressBar();
            pbWater.Value = Math.Min(100, waterPercent);
            pbWater.Size = new Size(cardWidth - 30, 10);
            pbWater.Location = new Point(15, 95);

            cardWater.Controls.Add(lblWaterTitle);
            cardWater.Controls.Add(lblWaterValue);
            cardWater.Controls.Add(lblWaterSub);
            cardWater.Controls.Add(pbWater);

            // Add cards to the form
            this.Controls.Add(cardSteps);
            this.Controls.Add(cardCalories);
            this.Controls.Add(cardMinutes);
            this.Controls.Add(cardWater);

            // Recent Workouts ListView
            ListView lvWorkouts = new ListView();
            lvWorkouts.Location = new Point(30, cardTop + cardHeight + 30);
            lvWorkouts.Size = new Size(450, 160);
            lvWorkouts.View = View.Details;
            lvWorkouts.Columns.Add("Workout", 160);
            lvWorkouts.Columns.Add("Duration", 80);
            lvWorkouts.Columns.Add("Time", 120);
            lvWorkouts.Columns.Add("Status", 80);
            lvWorkouts.FullRowSelect = true;
            lvWorkouts.GridLines = true;
            lvWorkouts.Items.Add(new ListViewItem(new string[] { "Upper Body Strength", "45 min", "2 hours ago", "Completed" }));
            lvWorkouts.Items.Add(new ListViewItem(new string[] { "Morning Run", "30 min", "Yesterday", "Completed" }));
            lvWorkouts.Items.Add(new ListViewItem(new string[] { "HIIT Training", "25 min", "2 days ago", "Completed" }));
            this.Controls.Add(lvWorkouts);

            // Goals Progress GroupBox
            GroupBox gbGoals = new GroupBox();
            gbGoals.Text = "Goals Progress";
            gbGoals.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            gbGoals.Location = new Point(500, cardTop + cardHeight + 30);
            gbGoals.Size = new Size(400, 200);

            string[] goalNames = { "Weekly Workouts", "Weight Loss", "Daily Steps", "Sleep Quality" };
            string[] goalValues = { "4/5", "3.2/5 kg", "8547/10000", "7.5/8 hrs" };
            int[] goalProgress = { 80, 64, 85, 94 };
            for (int i = 0; i < 4; i++)
            {
                Label lblGoal = new Label();
                lblGoal.Text = goalNames[i] + ": " + goalValues[i];
                lblGoal.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
                lblGoal.Location = new Point(20, 35 + i * 40);
                lblGoal.Size = new Size(200, 22);
                ProgressBar pbGoal = new ProgressBar();
                pbGoal.Value = goalProgress[i];
                pbGoal.Size = new Size(150, 10);
                pbGoal.Location = new Point(220, 40 + i * 40);
                gbGoals.Controls.Add(lblGoal);
                gbGoals.Controls.Add(pbGoal);
            }
            this.Controls.Add(gbGoals);

            // Quick Actions Buttons
            int btnTop = cardTop + cardHeight + 250;
            int btnLeft = 30;
            int btnWidth = 180;
            int btnHeight = 60;
            int btnSpacing = 30;
            string[] btnTexts = { "Start Workout", "Log Meal", "Add Water", "Daily Weight" };
            for (int i = 0; i < 4; i++)
            {
                Button btn = new Button();
                btn.Text = btnTexts[i];
                btn.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
                btn.Size = new Size(btnWidth, btnHeight);
                btn.Location = new Point(btnLeft + i * (btnWidth + btnSpacing), btnTop);
                btn.BackColor = Color.FromArgb(0, 120, 215);
                btn.ForeColor = Color.White;
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                this.Controls.Add(btn);
            }
        }
    }
} 