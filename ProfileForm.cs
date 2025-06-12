using System;
using System.Drawing;
using System.Windows.Forms;

namespace FitTrackerPro
{
    public class ProfileForm : Form
    {
        private int userId;
        private TextBox txtFirstName, txtLastName, txtEmail, txtPhone;
        private NumericUpDown nudAge, nudHeight, nudWeight;
        private ComboBox cbGender;
        private Button btnSave;
        private Label lblTitle;
        private Label lblSubtitle;
        private TabControl tabControl;

        public ProfileForm(int userId)
        {
            this.userId = userId;
            BuildUI();
        }

        private void BuildUI()
        {
            this.Text = "FitTracker Pro - Profile";
            this.Size = new Size(900, 650);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;

            // Header Title
            lblTitle = new Label();
            lblTitle.Text = "Profile";
            lblTitle.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            lblTitle.Location = new Point(210, 30);
            lblTitle.AutoSize = true;

            // Subtitle
            lblSubtitle = new Label();
            lblSubtitle.Text = "Manage your personal information and fitness goals";
            lblSubtitle.Font = new Font("Segoe UI", 12F, FontStyle.Regular);
            lblSubtitle.Location = new Point(212, 75);
            lblSubtitle.AutoSize = true;
            lblSubtitle.ForeColor = Color.Gray;

            // TabControl
            tabControl = new TabControl();
            tabControl.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            tabControl.Size = new Size(820, 450);
            tabControl.Location = new Point(210, 130);

            // Tabs
            var tabPersonal = new TabPage("Personal Info");
            var tabGoals = new TabPage("Fitness Goals");

            // Placeholder content
            // Personal Info tab controls
            int leftCol = 340;
            int topRow = 30;
            int rowSpacing = 45;
            int labelWidth = 100;
            int inputWidth = 180;

            // First Name
            Label lblFirstName = new Label();
            lblFirstName.Text = "First Name";
            lblFirstName.Location = new Point(leftCol, topRow);
            lblFirstName.Size = new Size(labelWidth, 22);
            txtFirstName = new TextBox();
            txtFirstName.Location = new Point(leftCol + labelWidth + 10, topRow);
            txtFirstName.Size = new Size(inputWidth, 22);

            // Last Name
            Label lblLastName = new Label();
            lblLastName.Text = "Last Name";
            lblLastName.Location = new Point(leftCol, topRow + rowSpacing);
            lblLastName.Size = new Size(labelWidth, 22);
            txtLastName = new TextBox();
            txtLastName.Location = new Point(leftCol + labelWidth + 10, topRow + rowSpacing);
            txtLastName.Size = new Size(inputWidth, 22);

            // Email
            Label lblEmail = new Label();
            lblEmail.Text = "Email";
            lblEmail.Location = new Point(leftCol, topRow + rowSpacing * 2);
            lblEmail.Size = new Size(labelWidth, 22);
            txtEmail = new TextBox();
            txtEmail.Location = new Point(leftCol + labelWidth + 10, topRow + rowSpacing * 2);
            txtEmail.Size = new Size(inputWidth, 22);

            // Phone
            Label lblPhone = new Label();
            lblPhone.Text = "Phone";
            lblPhone.Location = new Point(leftCol, topRow + rowSpacing * 3);
            lblPhone.Size = new Size(labelWidth, 22);
            txtPhone = new TextBox();
            txtPhone.Location = new Point(leftCol + labelWidth + 10, topRow + rowSpacing * 3);
            txtPhone.Size = new Size(inputWidth, 22);

            // Age
            Label lblAge = new Label();
            lblAge.Text = "Age";
            lblAge.Location = new Point(leftCol, topRow + rowSpacing * 4);
            lblAge.Size = new Size(labelWidth, 22);
            nudAge = new NumericUpDown();
            nudAge.Location = new Point(leftCol + labelWidth + 10, topRow + rowSpacing * 4);
            nudAge.Size = new Size(inputWidth, 22);
            nudAge.Minimum = 0;
            nudAge.Maximum = 120;

            // Gender
            Label lblGender = new Label();
            lblGender.Text = "Gender";
            lblGender.Location = new Point(leftCol, topRow + rowSpacing * 5);
            lblGender.Size = new Size(labelWidth, 22);
            cbGender = new ComboBox();
            cbGender.Location = new Point(leftCol + labelWidth + 10, topRow + rowSpacing * 5);
            cbGender.Size = new Size(inputWidth, 22);
            cbGender.DropDownStyle = ComboBoxStyle.DropDownList;
            cbGender.Items.AddRange(new string[] { "Male", "Female", "Other", "Prefer not to say" });

            // Height
            Label lblHeight = new Label();
            lblHeight.Text = "Height (cm)";
            lblHeight.Location = new Point(leftCol, topRow + rowSpacing * 6);
            lblHeight.Size = new Size(labelWidth, 22);
            nudHeight = new NumericUpDown();
            nudHeight.Location = new Point(leftCol + labelWidth + 10, topRow + rowSpacing * 6);
            nudHeight.Size = new Size(inputWidth, 22);
            nudHeight.Minimum = 50;
            nudHeight.Maximum = 250;

            // Weight
            Label lblWeight = new Label();
            lblWeight.Text = "Weight (kg)";
            lblWeight.Location = new Point(leftCol, topRow + rowSpacing * 7);
            lblWeight.Size = new Size(labelWidth, 22);
            nudWeight = new NumericUpDown();
            nudWeight.Location = new Point(leftCol + labelWidth + 10, topRow + rowSpacing * 7);
            nudWeight.Size = new Size(inputWidth, 22);
            nudWeight.Minimum = 20;
            nudWeight.Maximum = 300;

            // Save Changes Button
            btnSave = new Button();
            btnSave.Text = "Save Changes";
            btnSave.Location = new Point(leftCol + labelWidth + 10, topRow + rowSpacing * 8 - 20);
            btnSave.Size = new Size(140, 32);
            btnSave.BackColor = Color.FromArgb(0, 120, 215);
            btnSave.ForeColor = Color.White;
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += BtnSave_Click;

            // Remove placeholder label and add controls to tabPersonal
            tabPersonal.Controls.Clear();
            tabPersonal.Controls.Add(lblFirstName);
            tabPersonal.Controls.Add(txtFirstName);
            tabPersonal.Controls.Add(lblLastName);
            tabPersonal.Controls.Add(txtLastName);
            tabPersonal.Controls.Add(lblEmail);
            tabPersonal.Controls.Add(txtEmail);
            tabPersonal.Controls.Add(lblPhone);
            tabPersonal.Controls.Add(txtPhone);
            tabPersonal.Controls.Add(lblAge);
            tabPersonal.Controls.Add(nudAge);
            tabPersonal.Controls.Add(lblGender);
            tabPersonal.Controls.Add(cbGender);
            tabPersonal.Controls.Add(lblHeight);
            tabPersonal.Controls.Add(nudHeight);
            tabPersonal.Controls.Add(lblWeight);
            tabPersonal.Controls.Add(nudWeight);
            tabPersonal.Controls.Add(btnSave);

            tabControl.TabPages.Add(tabPersonal);
            tabControl.TabPages.Add(tabGoals);

            // Fitness Goals tab controls
            int goalsLeftCol = 80;
            int goalsTopRow = 40;
            int goalsRowSpacing = 45;
            int goalsLabelWidth = 140;
            int goalsInputWidth = 180;

            // Primary Goal
            Label lblPrimaryGoal = new Label();
            lblPrimaryGoal.Text = "Primary Goal";
            lblPrimaryGoal.Location = new Point(goalsLeftCol, goalsTopRow);
            lblPrimaryGoal.Size = new Size(goalsLabelWidth, 22);
            ComboBox cbPrimaryGoal = new ComboBox();
            cbPrimaryGoal.Location = new Point(goalsLeftCol + goalsLabelWidth + 10, goalsTopRow);
            cbPrimaryGoal.Size = new Size(goalsInputWidth, 22);
            cbPrimaryGoal.DropDownStyle = ComboBoxStyle.DropDownList;
            cbPrimaryGoal.Items.AddRange(new string[] { "Lose Weight", "Build Muscle", "Improve Endurance", "Stay Healthy" });
            cbPrimaryGoal.SelectedIndex = 0;

            // Activity Level
            Label lblActivityLevel = new Label();
            lblActivityLevel.Text = "Activity Level";
            lblActivityLevel.Location = new Point(goalsLeftCol, goalsTopRow + goalsRowSpacing);
            lblActivityLevel.Size = new Size(goalsLabelWidth, 22);
            ComboBox cbActivityLevel = new ComboBox();
            cbActivityLevel.Location = new Point(goalsLeftCol + goalsLabelWidth + 10, goalsTopRow + goalsRowSpacing);
            cbActivityLevel.Size = new Size(goalsInputWidth, 22);
            cbActivityLevel.DropDownStyle = ComboBoxStyle.DropDownList;
            cbActivityLevel.Items.AddRange(new string[] { "Sedentary", "Lightly Active", "Active", "Very Active" });
            cbActivityLevel.SelectedIndex = 0;

            // Target Weight
            Label lblTargetWeight = new Label();
            lblTargetWeight.Text = "Target Weight (kg)";
            lblTargetWeight.Location = new Point(goalsLeftCol, goalsTopRow + goalsRowSpacing * 2);
            lblTargetWeight.Size = new Size(goalsLabelWidth, 22);
            NumericUpDown nudTargetWeight = new NumericUpDown();
            nudTargetWeight.Location = new Point(goalsLeftCol + goalsLabelWidth + 10, goalsTopRow + goalsRowSpacing * 2);
            nudTargetWeight.Size = new Size(goalsInputWidth, 22);
            nudTargetWeight.Minimum = 20;
            nudTargetWeight.Maximum = 300;

            // Weekly Workouts
            Label lblWeeklyWorkouts = new Label();
            lblWeeklyWorkouts.Text = "Weekly Workouts";
            lblWeeklyWorkouts.Location = new Point(goalsLeftCol, goalsTopRow + goalsRowSpacing * 3);
            lblWeeklyWorkouts.Size = new Size(goalsLabelWidth, 22);
            NumericUpDown nudWeeklyWorkouts = new NumericUpDown();
            nudWeeklyWorkouts.Location = new Point(goalsLeftCol + goalsLabelWidth + 10, goalsTopRow + goalsRowSpacing * 3);
            nudWeeklyWorkouts.Size = new Size(goalsInputWidth, 22);
            nudWeeklyWorkouts.Minimum = 0;
            nudWeeklyWorkouts.Maximum = 14;

            // Daily Steps
            Label lblDailySteps = new Label();
            lblDailySteps.Text = "Daily Steps";
            lblDailySteps.Location = new Point(goalsLeftCol, goalsTopRow + goalsRowSpacing * 4);
            lblDailySteps.Size = new Size(goalsLabelWidth, 22);
            NumericUpDown nudDailySteps = new NumericUpDown();
            nudDailySteps.Location = new Point(goalsLeftCol + goalsLabelWidth + 10, goalsTopRow + goalsRowSpacing * 4);
            nudDailySteps.Size = new Size(goalsInputWidth, 22);
            nudDailySteps.Minimum = 0;
            nudDailySteps.Maximum = 50000;
            nudDailySteps.Increment = 100;

            // Daily Calories
            Label lblDailyCalories = new Label();
            lblDailyCalories.Text = "Daily Calories";
            lblDailyCalories.Location = new Point(goalsLeftCol, goalsTopRow + goalsRowSpacing * 5);
            lblDailyCalories.Size = new Size(goalsLabelWidth, 22);
            NumericUpDown nudDailyCalories = new NumericUpDown();
            nudDailyCalories.Location = new Point(goalsLeftCol + goalsLabelWidth + 10, goalsTopRow + goalsRowSpacing * 5);
            nudDailyCalories.Size = new Size(goalsInputWidth, 22);
            nudDailyCalories.Minimum = 0;
            nudDailyCalories.Maximum = 10000;
            nudDailyCalories.Increment = 50;

            // Save Goals Button
            Button btnSaveGoals = new Button();
            btnSaveGoals.Text = "Save Goals";
            btnSaveGoals.Location = new Point(goalsLeftCol + goalsLabelWidth + 10, goalsTopRow + goalsRowSpacing * 6 + 10);
            btnSaveGoals.Size = new Size(140, 32);
            btnSaveGoals.BackColor = Color.FromArgb(0, 120, 215);
            btnSaveGoals.ForeColor = Color.White;
            btnSaveGoals.FlatStyle = FlatStyle.Flat;
            btnSaveGoals.FlatAppearance.BorderSize = 0;
            btnSaveGoals.Click += (s, e) => {
                // Save to UserGoals for this user
                DatabaseHelper.SaveUserGoals(
                    cbPrimaryGoal.SelectedItem?.ToString() ?? "",
                    cbActivityLevel.SelectedItem?.ToString() ?? "",
                    (int)nudTargetWeight.Value,
                    (int)nudWeeklyWorkouts.Value,
                    (int)nudDailySteps.Value,
                    (int)nudDailyCalories.Value
                );
                MessageBox.Show("Goals saved!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };

            // Add controls to tabGoals
            tabGoals.Controls.Clear();
            tabGoals.Controls.Add(lblPrimaryGoal);
            tabGoals.Controls.Add(cbPrimaryGoal);
            tabGoals.Controls.Add(lblActivityLevel);
            tabGoals.Controls.Add(cbActivityLevel);
            tabGoals.Controls.Add(lblTargetWeight);
            tabGoals.Controls.Add(nudTargetWeight);
            tabGoals.Controls.Add(lblWeeklyWorkouts);
            tabGoals.Controls.Add(nudWeeklyWorkouts);
            tabGoals.Controls.Add(lblDailySteps);
            tabGoals.Controls.Add(nudDailySteps);
            tabGoals.Controls.Add(lblDailyCalories);
            tabGoals.Controls.Add(nudDailyCalories);
            tabGoals.Controls.Add(btnSaveGoals);

            // Add controls to the form
            this.Controls.Add(lblTitle);
            this.Controls.Add(lblSubtitle);
            this.Controls.Add(tabControl);
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string phone = txtPhone.Text.Trim();
            // Email validation
            if (!System.Text.RegularExpressions.Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                MessageBox.Show("Please enter a valid email address.", "Invalid Email", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // Phone validation: must be 10 digits, start with 09
            if (!System.Text.RegularExpressions.Regex.IsMatch(phone, @"^09\d{8}$"))
            {
                MessageBox.Show("Please enter a valid phone number in the format 0912345678.", "Invalid Phone", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            DatabaseHelper.SaveUserProfile(userId,
                txtFirstName.Text.Trim(),
                txtLastName.Text.Trim(),
                email,
                phone,
                (int)nudAge.Value,
                cbGender.SelectedItem?.ToString() ?? "",
                (int)nudHeight.Value,
                (int)nudWeight.Value
            );
            MessageBox.Show("Profile saved!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }
    }
} 