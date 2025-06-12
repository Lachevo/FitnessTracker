using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace FitTrackerPro
{
    public class NutritionForm : Form
    {
        private Label lblTitle;
        private Label lblSubtitle;
        private Button btnLogMeal;
        private Panel[] statCards;
        private TabControl tabControl;
        private int userId;

        public NutritionForm(int userId)
        {
            this.userId = userId;
            BuildUI();
        }

        public NutritionForm() : this(0) { }

        private void BuildUI()
        {
            this.Text = "FitTracker Pro - Nutrition";
            this.Size = new Size(1000, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;

            int leftMargin = 200;
            // Header Title
            lblTitle = new Label();
            lblTitle.Text = "Nutrition";
            lblTitle.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            lblTitle.Location = new Point(leftMargin + 30, 30);
            lblTitle.AutoSize = true;

            // Subtitle
            lblSubtitle = new Label();
            lblSubtitle.Text = "Track your meals, calories, and nutrition goals";
            lblSubtitle.Font = new Font("Segoe UI", 12F, FontStyle.Regular);
            lblSubtitle.Location = new Point(leftMargin + 32, 75);
            lblSubtitle.AutoSize = true;
            lblSubtitle.ForeColor = Color.Gray;

            // Fetch user data for stats
            float weight = DatabaseHelper.GetUserWeight(userId);
            int caloriesConsumed = DatabaseHelper.GetCaloriesConsumedToday(userId);
            float proteinConsumed = DatabaseHelper.GetProteinConsumedToday(userId);
            float proteinGoal = weight * 1.5f;
            int caloriesGoal = (int)(weight * 40);
            float water = DatabaseHelper.GetWaterToday(userId);
            float waterGoal = DatabaseHelper.GetUserGoals(userId).DailyWater;

            // Stat Cards
            string[] statNames = { "Calorie Intake", "Protein", "Water" };
            string[] statValues = {
                caloriesConsumed.ToString("N0"),
                proteinConsumed.ToString("0.##") + "g",
                water.ToString("0.##") + "L"
            };
            string[] statGoals = {
                $"Goal: {caloriesGoal:N0}",
                $"Goal: {proteinGoal:0.##}g (weight × 1.5)",
                $"Goal: {waterGoal:0.##}L"
            };
            int[] statProgress = {
                Math.Min(100, (int)(caloriesConsumed * 100.0 / Math.Max(1, caloriesGoal))),
                Math.Min(100, (int)(proteinConsumed * 100.0 / Math.Max(1, proteinGoal))),
                Math.Min(100, (int)(water * 100.0 / Math.Max(1, waterGoal)))
            };
            statCards = new Panel[3];
            for (int i = 0; i < 3; i++)
            {
                Panel card = new Panel();
                card.BackColor = Color.WhiteSmoke;
                card.BorderStyle = BorderStyle.FixedSingle;
                card.Size = new Size(210, 110);
                card.Location = new Point(leftMargin + 30 + i * 220, 120);

                Label lblStatTitle = new Label();
                lblStatTitle.Text = statNames[i];
                lblStatTitle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
                lblStatTitle.Location = new Point(15, 10);
                lblStatTitle.AutoSize = true;

                Label lblStatValue = new Label();
                lblStatValue.Text = statValues[i];
                lblStatValue.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
                lblStatValue.Location = new Point(15, 35);
                lblStatValue.AutoSize = true;

                Label lblStatGoal = new Label();
                lblStatGoal.Text = statGoals[i];
                lblStatGoal.Font = new Font("Segoe UI", 8F, FontStyle.Regular);
                lblStatGoal.ForeColor = Color.Gray;
                lblStatGoal.Location = new Point(15, 70);
                lblStatGoal.AutoSize = true;

                card.Controls.Add(lblStatTitle);
                card.Controls.Add(lblStatValue);
                card.Controls.Add(lblStatGoal);
                if (i < 3) // Only show progress bar for first three
                {
                    ProgressBar pbStat = new ProgressBar();
                    pbStat.Value = statProgress[i];
                    pbStat.Size = new Size(180, 10);
                    pbStat.Location = new Point(15, 90);
                    card.Controls.Add(pbStat);
                }
                statCards[i] = card;
            }

            // TabControl
            tabControl = new TabControl();
            tabControl.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            tabControl.Size = new Size(950, 450);
            tabControl.Location = new Point(leftMargin + 30, 250);
            tabControl.TabPages.Add("today", "Today's Meals");
            tabControl.TabPages.Add("log", "Log Food");
            tabControl.TabPages.Add("water", "Water Intake");

            // Today's Meals ListView
            ListView lvMeals = new ListView();
            lvMeals.Location = new Point(20, 20);
            lvMeals.Size = new Size(850, 150);
            lvMeals.View = View.Details;
            lvMeals.Columns.Add("Meal", 120);
            lvMeals.Columns.Add("Food Name", 200);
            lvMeals.Columns.Add("Calories", 100);
            lvMeals.Columns.Add("Protein (g)", 100);
            lvMeals.FullRowSelect = true;
            lvMeals.GridLines = true;

            // Load today's meals for the user
            using (var conn = new SqlConnection(DatabaseHelper.ConnectionString))
            {
                conn.Open();
                string sql = "SELECT MealType, FoodName, Calories, Protein FROM UserMeals WHERE UserId = @UserId AND Date = @Date ORDER BY MealType";
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@Date", DateTime.Now.Date);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string mealType = reader.IsDBNull(0) ? "" : reader.GetString(0);
                            string foodName = reader.IsDBNull(1) ? "" : reader.GetString(1);
                            string calories = reader.IsDBNull(2) ? "0" : reader.GetInt32(2).ToString();
                            string protein = reader.IsDBNull(3) ? "0" : reader.GetDouble(3).ToString("0.##");
                            lvMeals.Items.Add(new ListViewItem(new string[] { mealType, foodName, calories, protein }));
                        }
                    }
                }
            }
            tabControl.TabPages[0].Controls.Add(lvMeals);

            // Move controls to the left for better balance
            int logTabLeft = 60;
            int inputOffset = 120;

            Label lblMealType = new Label();
            lblMealType.Text = "Meal Type:";
            lblMealType.Location = new Point(logTabLeft, 30);
            ComboBox cmbMealType = new ComboBox();
            cmbMealType.Location = new Point(logTabLeft + inputOffset, 30);
            cmbMealType.Size = new Size(250, 36);
            cmbMealType.Font = new Font("Segoe UI", 12F, FontStyle.Regular);
            cmbMealType.DropDownHeight = 150;
            cmbMealType.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbMealType.Items.AddRange(new string[] { "Breakfast", "Lunch", "Snack", "Dinner" });
            cmbMealType.SelectedIndex = 0;
            Label lblFoodName = new Label();
            lblFoodName.Text = "Food Name:";
            lblFoodName.Location = new Point(logTabLeft, 85);
            TextBox txtFoodName = new TextBox();
            txtFoodName.Location = new Point(logTabLeft + inputOffset, 85);
            txtFoodName.Size = new Size(350, 36);
            txtFoodName.Font = new Font("Segoe UI", 12F, FontStyle.Regular);
            Label lblCalories = new Label();
            lblCalories.Text = "Calories:";
            lblCalories.Location = new Point(logTabLeft, 140);
            NumericUpDown nudCalories = new NumericUpDown();
            nudCalories.Location = new Point(logTabLeft + inputOffset, 140);
            nudCalories.Size = new Size(150, 36);
            nudCalories.Font = new Font("Segoe UI", 12F, FontStyle.Regular);
            nudCalories.Maximum = 5000;
            nudCalories.Minimum = 0;
            Label lblProtein = new Label();
            lblProtein.Text = "Protein (g):";
            lblProtein.Location = new Point(logTabLeft, 195);
            NumericUpDown nudProtein = new NumericUpDown();
            nudProtein.Location = new Point(logTabLeft + inputOffset, 195);
            nudProtein.Size = new Size(150, 36);
            nudProtein.Font = new Font("Segoe UI", 12F, FontStyle.Regular);
            nudProtein.Maximum = 500;
            nudProtein.Minimum = 0;
            nudProtein.DecimalPlaces = 1;
            nudProtein.Increment = 0.5M;
            Button btnLogFood = new Button();
            btnLogFood.Text = "Log Food";
            btnLogFood.Location = new Point(logTabLeft + inputOffset, 250);
            btnLogFood.Size = new Size(140, 36);
            btnLogFood.BackColor = Color.FromArgb(0, 120, 215);
            btnLogFood.ForeColor = Color.White;
            btnLogFood.FlatStyle = FlatStyle.Flat;
            btnLogFood.FlatAppearance.BorderSize = 0;
            btnLogFood.Click += (s, e) =>
            {
                DatabaseHelper.LogMeal(
                    userId,
                    DateTime.Now.Date,
                    cmbMealType.SelectedItem.ToString(),
                    txtFoodName.Text,
                    (int)nudCalories.Value,
                    (float)nudProtein.Value,
                    0f, // Carbs
                    0f  // Fat
                );
                MessageBox.Show("Meal logged!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtFoodName.Text = "";
                nudCalories.Value = 0;
                nudProtein.Value = 0;
            };
            tabControl.TabPages[1].Controls.Add(lblMealType);
            tabControl.TabPages[1].Controls.Add(cmbMealType);
            tabControl.TabPages[1].Controls.Add(lblFoodName);
            tabControl.TabPages[1].Controls.Add(txtFoodName);
            tabControl.TabPages[1].Controls.Add(lblCalories);
            tabControl.TabPages[1].Controls.Add(nudCalories);
            tabControl.TabPages[1].Controls.Add(lblProtein);
            tabControl.TabPages[1].Controls.Add(nudProtein);
            tabControl.TabPages[1].Controls.Add(btnLogFood);

            // Water Intake controls
            Label lblWater = new Label();
            lblWater.Text = "Water Intake:";
            lblWater.Location = new Point(30, 30);
            lblWater.Size = new Size(120, 22);
            Button btnAddGlass = new Button();
            btnAddGlass.Text = "Add Water";
            btnAddGlass.Location = new Point(150, 70);
            btnAddGlass.Size = new Size(120, 32);
            btnAddGlass.BackColor = Color.FromArgb(0, 120, 215);
            btnAddGlass.ForeColor = Color.White;
            btnAddGlass.FlatStyle = FlatStyle.Flat;
            btnAddGlass.FlatAppearance.BorderSize = 0;
            btnAddGlass.Click += (s, e) =>
            {
                using (Form waterForm = new Form())
                {
                    waterForm.Text = "Add Water";
                    waterForm.Size = new Size(320, 180);
                    waterForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                    waterForm.StartPosition = FormStartPosition.CenterParent;
                    waterForm.MaximizeBox = false;
                    waterForm.MinimizeBox = false;
                    waterForm.BackColor = Color.White;

                    Label lbl = new Label();
                    lbl.Text = "Enter water amount (L):";
                    lbl.Font = new Font("Segoe UI", 11F, FontStyle.Regular);
                    lbl.Location = new Point(30, 30);
                    lbl.AutoSize = true;

                    NumericUpDown nudWater = new NumericUpDown();
                    nudWater.Minimum = 0;
                    nudWater.Maximum = 10;
                    nudWater.DecimalPlaces = 2;
                    nudWater.Increment = 0.1M;
                    nudWater.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
                    nudWater.Location = new Point(30, 65);
                    nudWater.Width = 120;

                    Button btnOk = new Button();
                    btnOk.Text = "Save";
                    btnOk.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
                    btnOk.BackColor = Color.FromArgb(0, 120, 215);
                    btnOk.ForeColor = Color.White;
                    btnOk.FlatStyle = FlatStyle.Flat;
                    btnOk.FlatAppearance.BorderSize = 0;
                    btnOk.Size = new Size(100, 36);
                    btnOk.Location = new Point(170, 65);
                    btnOk.DialogResult = DialogResult.OK;

                    waterForm.Controls.Add(lbl);
                    waterForm.Controls.Add(nudWater);
                    waterForm.Controls.Add(btnOk);
                    waterForm.AcceptButton = btnOk;

                    if (waterForm.ShowDialog() == DialogResult.OK)
                    {
                        float amount = (float)nudWater.Value;
                        if (amount > 0)
                        {
                            DatabaseHelper.AddWaterIntake(userId, DateTime.Now.Date, amount);
                            MessageBox.Show($"Added {amount:0.##}L of water!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            };
            tabControl.TabPages[2].Controls.Add(lblWater);
            tabControl.TabPages[2].Controls.Add(btnAddGlass);

            // Add controls to the form
            this.Controls.Add(lblTitle);
            this.Controls.Add(lblSubtitle);
            foreach (var card in statCards)
                this.Controls.Add(card);
            this.Controls.Add(tabControl);
        }
    }
} 