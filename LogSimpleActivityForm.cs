using System;
using System.Windows.Forms;

namespace FitTrackerPro
{
    public class LogSimpleActivityForm : Form
    {
        private int userId;
        private string activityName;
        private NumericUpDown nudDuration, nudCalories;
        private Button btnSave;

        public LogSimpleActivityForm(int userId, string activityName)
        {
            this.userId = userId;
            this.activityName = activityName;
            BuildUI();
        }

        private void BuildUI()
        {
            this.Text = $"Log {activityName}";
            this.Width = 350;
            this.Height = 220;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;

            Label lblDuration = new Label { Text = "Duration (min)", Left = 30, Top = 30, Width = 100 };
            nudDuration = new NumericUpDown { Left = 140, Top = 25, Width = 120, Maximum = 500, Minimum = 0 };

            Label lblCalories = new Label { Text = "Calories Burned", Left = 30, Top = 70, Width = 100 };
            nudCalories = new NumericUpDown { Left = 140, Top = 65, Width = 120, Maximum = 5000, Minimum = 0 };

            btnSave = new Button { Text = "Save", Left = 140, Top = 120, Width = 120 };
            btnSave.Click += BtnSave_Click;

            this.Controls.Add(lblDuration);
            this.Controls.Add(nudDuration);
            this.Controls.Add(lblCalories);
            this.Controls.Add(nudCalories);
            this.Controls.Add(btnSave);
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (userId <= 0)
            {
                MessageBox.Show("Invalid user. Please log in again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            DatabaseHelper.LogWorkout(
                userId,
                DateTime.Now.Date,
                activityName,
                (int)nudDuration.Value,
                (int)nudCalories.Value
            );
            MessageBox.Show($"{activityName} logged!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }
    }
} 