using System;
using System.Drawing;
using System.Windows.Forms;

namespace FitTrackerPro
{
    public class DashboardForm : Form
    {
        private Label lblTitle;
        private Label lblWelcome;
        private int currentUserId;
        private Button btnStartWorkout;

        public DashboardForm(int userId)
        {
            currentUserId = userId;
            BuildUI();
        }

        public DashboardForm() : this(0) { }

        private void BuildUI()
        {
            this.Text = "FitTracker Pro - Dashboard";
            this.Size = new Size(1000, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;

            int leftMargin = 200; // Shift everything to the right of the sidebar

            // Fetch user data
            int caloriesConsumed = DatabaseHelper.GetCaloriesConsumedToday(currentUserId);
            int caloriesBurned = DatabaseHelper.GetCaloriesBurnedToday(currentUserId);
            int netCalories = caloriesConsumed - caloriesBurned;
            int steps = DatabaseHelper.GetStepsToday(currentUserId);
            float water = DatabaseHelper.GetWaterToday(currentUserId);
            int activeMinutes = DatabaseHelper.GetActiveMinutesFromWorkoutsToday(currentUserId);
            var goals = DatabaseHelper.GetUserGoals(currentUserId);
            float proteinConsumed = DatabaseHelper.GetProteinConsumedToday(currentUserId);
            float weight = DatabaseHelper.GetUserWeight(currentUserId);
            float proteinGoal = weight * 1.5f;
            int caloriesGoal = (int)(weight * 40);

            // Header Title
            lblTitle = new Label();
            lblTitle.Text = "Dashboard";
            lblTitle.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            lblTitle.Location = new Point(leftMargin + 30, 30);
            lblTitle.AutoSize = true;

            // Welcome Message
            lblWelcome = new Label();
            lblWelcome.Text = "Welcome back! Here's your fitness overview for today.";
            lblWelcome.Font = new Font("Segoe UI", 12F, FontStyle.Regular);
            lblWelcome.Location = new Point(leftMargin + 32, 75);
            lblWelcome.AutoSize = true;
            lblWelcome.ForeColor = Color.Gray;

            // Add controls to the form
            this.Controls.Add(lblTitle);
            this.Controls.Add(lblWelcome);

            // Quick Stats Cards
            int cardWidth = 210;
            int cardHeight = 120;
            int cardSpacing = 20;
            int cardTop = 120;
            int cardLeftStart = leftMargin + 30;

            // Show user's weight at the top right
            Label lblWeightInfo = new Label();
            lblWeightInfo.Text = $"Your weight: {weight} kg";
            lblWeightInfo.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            lblWeightInfo.AutoSize = true;
            lblWeightInfo.Location = new Point(this.Width - lblWeightInfo.PreferredWidth - 60, 30);
            lblWeightInfo.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.Controls.Add(lblWeightInfo);

            // Card 1: Protein Intake
            Panel cardProtein = new Panel();
            cardProtein.BackColor = Color.WhiteSmoke;
            cardProtein.BorderStyle = BorderStyle.FixedSingle;
            cardProtein.Size = new Size(cardWidth, cardHeight + 60); // Make room for workout info
            cardProtein.Location = new Point(cardLeftStart, cardTop);

            Label lblProteinTitle = new Label();
            lblProteinTitle.Text = "Protein Intake";
            lblProteinTitle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblProteinTitle.Location = new Point(15, 10);
            lblProteinTitle.AutoSize = true;

            Label lblProteinValue = new Label();
            lblProteinValue.Text = proteinConsumed.ToString("0.##") + "g";
            lblProteinValue.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblProteinValue.Location = new Point(15, 35);
            lblProteinValue.AutoSize = true;

            Label lblProteinSub = new Label();
            lblProteinSub.Text = $"Goal: {proteinGoal:0.##}g (weight × 1.5)";
            lblProteinSub.Font = new Font("Segoe UI", 8F, FontStyle.Regular);
            lblProteinSub.ForeColor = Color.Gray;
            lblProteinSub.Location = new Point(15, 70);
            lblProteinSub.AutoSize = true;

            ProgressBar pbProtein = new ProgressBar();
            pbProtein.Value = Math.Min(100, (int)(proteinConsumed * 100.0 / Math.Max(0.01, proteinGoal)));
            pbProtein.Size = new Size(cardWidth - 30, 10);
            pbProtein.Location = new Point(15, 95);

            // Workout info for today
            // (No longer needed: workout info label in protein card)
            cardProtein.Controls.Add(lblProteinTitle);
            cardProtein.Controls.Add(lblProteinValue);
            cardProtein.Controls.Add(lblProteinSub);
            cardProtein.Controls.Add(pbProtein);

            // Card 2: Calories (Net Calories)
            Panel cardCalories = new Panel();
            cardCalories.BackColor = Color.WhiteSmoke;
            cardCalories.BorderStyle = BorderStyle.FixedSingle;
            cardCalories.Size = new Size(cardWidth, cardHeight);
            cardCalories.Location = new Point(cardLeftStart + (cardWidth + cardSpacing) * 1, cardTop);

            Label lblCaloriesTitle = new Label();
            lblCaloriesTitle.Text = "Net Calories";
            lblCaloriesTitle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblCaloriesTitle.Location = new Point(15, 10);
            lblCaloriesTitle.AutoSize = true;

            Label lblCaloriesValue = new Label();
            lblCaloriesValue.Text = netCalories.ToString("N0");
            lblCaloriesValue.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblCaloriesValue.Location = new Point(15, 35);
            lblCaloriesValue.AutoSize = true;

            Label lblCaloriesSub = new Label();
            lblCaloriesSub.Text = $"Goal: {caloriesGoal:N0} cal (weight × 40)";
            lblCaloriesSub.Font = new Font("Segoe UI", 8F, FontStyle.Regular);
            lblCaloriesSub.ForeColor = Color.Gray;
            lblCaloriesSub.Location = new Point(15, 70);
            lblCaloriesSub.AutoSize = true;

            ProgressBar pbCalories = new ProgressBar();
            // Net calorie progress is (calorie intake - calorie burned) / calorie intake goal
            int netCaloriePercent = (int)((caloriesConsumed - caloriesBurned) * 100.0 / Math.Max(1, caloriesGoal));
            pbCalories.Value = Math.Max(0, Math.Min(100, netCaloriePercent));
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
            lblMinutesSub.Text = $"Goal: {goals.ActiveMinutes} minutes (from workouts)";
            lblMinutesSub.Font = new Font("Segoe UI", 8F, FontStyle.Regular);
            lblMinutesSub.ForeColor = Color.Gray;
            lblMinutesSub.Location = new Point(15, 70);
            lblMinutesSub.AutoSize = true;

            ProgressBar pbMinutes = new ProgressBar();
            pbMinutes.Value = Math.Min(100, (int)(activeMinutes * 100.0 / Math.Max(1, goals.ActiveMinutes)));
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
            lblWaterValue.Text = water.ToString("0.##") + "L";
            lblWaterValue.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblWaterValue.Location = new Point(15, 35);
            lblWaterValue.AutoSize = true;

            Label lblWaterSub = new Label();
            lblWaterSub.Text = $"Goal: {goals.DailyWater:0.##}L";
            lblWaterSub.Font = new Font("Segoe UI", 8F, FontStyle.Regular);
            lblWaterSub.ForeColor = Color.Gray;
            lblWaterSub.Location = new Point(15, 70);
            lblWaterSub.AutoSize = true;

            ProgressBar pbWater = new ProgressBar();
            pbWater.Value = Math.Min(100, (int)(water * 100.0 / Math.Max(0.01, goals.DailyWater)));
            pbWater.Size = new Size(cardWidth - 30, 10);
            pbWater.Location = new Point(15, 95);

            cardWater.Controls.Add(lblWaterTitle);
            cardWater.Controls.Add(lblWaterValue);
            cardWater.Controls.Add(lblWaterSub);
            cardWater.Controls.Add(pbWater);

            // Add cards to the form
            this.Controls.Add(cardProtein);
            this.Controls.Add(cardCalories);
            this.Controls.Add(cardMinutes);
            this.Controls.Add(cardWater);

            // Workout summary box below protein card
            Panel workoutSummaryPanel = new Panel();
            workoutSummaryPanel.BackColor = Color.WhiteSmoke;
            workoutSummaryPanel.BorderStyle = BorderStyle.FixedSingle;
            workoutSummaryPanel.Size = new Size(cardWidth + 60, 160);
            workoutSummaryPanel.Location = new Point(cardLeftStart, cardTop + cardProtein.Height + 20);

            Label lblWorkoutSummary = new Label();
            lblWorkoutSummary.Text = "Today's Workout Summary";
            lblWorkoutSummary.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblWorkoutSummary.Location = new Point(10, 10);
            lblWorkoutSummary.AutoSize = true;
            workoutSummaryPanel.Controls.Add(lblWorkoutSummary);

            var workoutsTodayFull = DatabaseHelper.GetWorkoutsTodayFull(currentUserId);
            if (workoutsTodayFull.Count == 0)
            {
                Label lblNoWorkout = new Label();
                lblNoWorkout.Text = "No workout completed today.";
                lblNoWorkout.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
                lblNoWorkout.Location = new Point(10, 40);
                lblNoWorkout.AutoSize = true;
                workoutSummaryPanel.Controls.Add(lblNoWorkout);
            }
            else
            {
                ListView lv = new ListView();
                lv.Location = new Point(10, 35);
                lv.Size = new Size(cardWidth + 30, 100);
                lv.View = View.Details;
                lv.Columns.Add("Type", 80);
                lv.Columns.Add("Duration (min)", 90);
                lv.Columns.Add("Calories", 70);
                lv.Columns.Add("Date", 80);
                lv.Columns.Add("Status", 80);
                lv.HeaderStyle = ColumnHeaderStyle.Nonclickable;
                lv.FullRowSelect = true;
                lv.GridLines = false;
                foreach (var row in workoutsTodayFull)
                {
                    string workoutDate = DateTime.Now.ToString("yyyy-MM-dd");
                    string status = "Completed";
                    // If you want to show the actual date, you need to fetch it from the DB. For now, assume today.
                    lv.Items.Add(new ListViewItem(new string[] { row.Type, row.Duration.ToString(), row.Calories.ToString(), workoutDate, status }));
                }
                workoutSummaryPanel.Controls.Add(lv);
            }
            this.Controls.Add(workoutSummaryPanel);

            // Goals Progress GroupBox
            GroupBox gbGoals = new GroupBox();
            gbGoals.Text = "Goals Progress";
            gbGoals.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            gbGoals.Location = new Point(cardLeftStart + 470, cardTop + cardHeight + 30);
            gbGoals.Size = new Size(400, 200);

            // Fetch weights for weight loss progress
            float startWeight = DatabaseHelper.GetUserWeight(currentUserId);
            float dailyWeight = 0;
            float targetWeight = 0;
            var userGoalsRow = DatabaseHelper.LoadUserGoals();
            if (userGoalsRow != null)
            {
                if (userGoalsRow.Table.Columns.Contains("DailyWeight") && userGoalsRow["DailyWeight"] != DBNull.Value)
                    dailyWeight = Convert.ToSingle(userGoalsRow["DailyWeight"]);
                if (userGoalsRow.Table.Columns.Contains("TargetWeight") && userGoalsRow["TargetWeight"] != DBNull.Value)
                    targetWeight = Convert.ToSingle(userGoalsRow["TargetWeight"]);
            }
            // Use dailyWeight as the current weight (from Daily Weight button)
            float weightDiff = (dailyWeight > 0) ? (startWeight - dailyWeight) : 0;
            string weightDiffLabel = weightDiff > 0 ? $"{weightDiff:0.##} kg lost" : (weightDiff < 0 ? $"{Math.Abs(weightDiff):0.##} kg gained" : "No change");
            float totalLossNeeded = (startWeight - targetWeight > 0) ? (startWeight - targetWeight) : 1;
            int weightLossProgress = (int)(weightDiff * 100.0 / totalLossNeeded);
            weightLossProgress = Math.Max(0, Math.Min(100, weightLossProgress));

            // Sleep Quality: get sleep duration from SleepForm/DB
            float sleepHours = DatabaseHelper.GetSleepHoursToday(currentUserId); // Implement this method if needed
            string sleepLabel = $"{sleepHours:0.##}/8 hrs";
            int sleepProgress = Math.Min(100, (int)(sleepHours * 100.0 / 8));

            // Weight Loss
            Label lblGoalWeight = new Label();
            lblGoalWeight.Text = "Weight Loss: " + $"{weightDiffLabel} (Start: {startWeight:0.##}, Today: {dailyWeight:0.##}, Target: {targetWeight:0.##})";
            lblGoalWeight.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            lblGoalWeight.Location = new Point(20, 35);
            lblGoalWeight.Size = new Size(300, 22);
            ProgressBar pbGoalWeight = new ProgressBar();
            pbGoalWeight.Value = Math.Max(0, Math.Min(100, weightLossProgress));
            pbGoalWeight.Size = new Size(150, 10);
            pbGoalWeight.Location = new Point(220, 40);
            gbGoals.Controls.Add(lblGoalWeight);
            gbGoals.Controls.Add(pbGoalWeight);

            // Sleep Quality
            Label lblGoalSleep = new Label();
            lblGoalSleep.Text = "Sleep Quality: " + sleepLabel;
            lblGoalSleep.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            lblGoalSleep.Location = new Point(20, 75);
            lblGoalSleep.Size = new Size(200, 22);
            ProgressBar pbGoalSleep = new ProgressBar();
            pbGoalSleep.Value = sleepProgress;
            pbGoalSleep.Size = new Size(150, 10);
            pbGoalSleep.Location = new Point(220, 80);
            gbGoals.Controls.Add(lblGoalSleep);
            gbGoals.Controls.Add(pbGoalSleep);

            this.Controls.Add(gbGoals);

            // Quick Actions Buttons
            int btnTop = cardTop + cardHeight + 250;
            int btnLeft = cardLeftStart;
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
                if (i == 0)
                    btn.Click += BtnStartWorkout_Click;
                else if (i == 1)
                    btn.Click += BtnLogMeal_Click;
                else if (i == 2)
                    btn.Click += BtnAddWater_Click;
                else if (i == 3)
                    btn.Click += BtnDailyWeight_Click;
                this.Controls.Add(btn);
            }
        }

        private void BtnStartWorkout_Click(object sender, EventArgs e)
        {
            var workoutForm = new StartWorkoutForm(currentUserId);
            workoutForm.ShowDialog();
        }

        private void BtnLogMeal_Click(object sender, EventArgs e)
        {
            var mealForm = new LogMealForm(currentUserId);
            mealForm.ShowDialog();
        }

        private void BtnAddWater_Click(object sender, EventArgs e)
        {
            var waterForm = new AddWaterForm(currentUserId);
            waterForm.ShowDialog();
        }

        private void BtnDailyWeight_Click(object sender, EventArgs e)
        {
            // Prompt for weight
            using (Form weightForm = new Form())
            {
                weightForm.Text = "Log Daily Weight";
                weightForm.Size = new Size(350, 200);
                weightForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                weightForm.StartPosition = FormStartPosition.CenterParent;
                weightForm.MaximizeBox = false;
                weightForm.MinimizeBox = false;
                weightForm.BackColor = Color.White;

                Label lbl = new Label();
                lbl.Text = "Enter your weight for today (kg):";
                lbl.Font = new Font("Segoe UI", 11F, FontStyle.Regular);
                lbl.Location = new Point(30, 30);
                lbl.AutoSize = true;

                NumericUpDown nudWeight = new NumericUpDown();
                nudWeight.Minimum = 20;
                nudWeight.Maximum = 300;
                nudWeight.DecimalPlaces = 1;
                nudWeight.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
                nudWeight.Location = new Point(30, 65);
                nudWeight.Width = 120;
                float userWeight = DatabaseHelper.GetUserWeight(currentUserId);
                if (userWeight >= (float)nudWeight.Minimum && userWeight <= (float)nudWeight.Maximum)
                    nudWeight.Value = (decimal)userWeight;
                else
                    nudWeight.Value = nudWeight.Minimum;

                Button btnOk = new Button();
                btnOk.Text = "Save";
                btnOk.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
                btnOk.BackColor = Color.FromArgb(0, 120, 215);
                btnOk.ForeColor = Color.White;
                btnOk.FlatStyle = FlatStyle.Flat;
                btnOk.FlatAppearance.BorderSize = 0;
                btnOk.Size = new Size(100, 36);
                btnOk.Location = new Point(180, 110);
                btnOk.DialogResult = DialogResult.OK;

                weightForm.Controls.Add(lbl);
                weightForm.Controls.Add(nudWeight);
                weightForm.Controls.Add(btnOk);
                weightForm.AcceptButton = btnOk;

                if (weightForm.ShowDialog() == DialogResult.OK)
                {
                    float newWeight = (float)nudWeight.Value;
                    // Save to UserGoals.DailyWeight for this user
                    DatabaseHelper.SaveDailyWeight(currentUserId, newWeight);
                    float profileWeight = DatabaseHelper.GetUserWeight(currentUserId);
                    string msg, title;
                    if (newWeight < profileWeight)
                    {
                        msg = "🎉 You are losing weight! Keep up the great work!";
                        title = "Great Progress!";
                    }
                    else
                    {
                        msg = "Keep going! Stay consistent and you'll see results.";
                        title = "Keep Going!";
                    }
                    MessageBox.Show(msg, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // DashboardForm
            // 
            this.ClientSize = new System.Drawing.Size(921, 317);
            this.Name = "DashboardForm";
            this.ResumeLayout(false);

        }
    }
} 