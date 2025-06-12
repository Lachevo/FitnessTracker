using System;
using System.Windows.Forms;

namespace FitTrackerPro
{
    public class TrackRunForm : Form
    {
        private int userId;
        private NumericUpDown nudDistance, nudDuration, nudCalories;
        private Button btnSave;

        public TrackRunForm(int userId)
        {
            this.userId = userId;
            BuildUI();
        }

        private void BuildUI()
        {
            this.Text = "Track Run";
            this.Width = 350;
            this.Height = 250;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;

            Label lblDistance = new Label { Text = "Distance (km)", Left = 30, Top = 30, Width = 100 };
            nudDistance = new NumericUpDown { Left = 140, Top = 25, Width = 120, DecimalPlaces = 2, Increment = 0.1M, Maximum = 100, Minimum = 0 };

            Label lblDuration = new Label { Text = "Duration (min)", Left = 30, Top = 70, Width = 100 };
            nudDuration = new NumericUpDown { Left = 140, Top = 65, Width = 120, Maximum = 500, Minimum = 0 };

            Label lblCalories = new Label { Text = "Calories Burned", Left = 30, Top = 110, Width = 100 };
            nudCalories = new NumericUpDown { Left = 140, Top = 105, Width = 120, Maximum = 5000, Minimum = 0 };

            btnSave = new Button { Text = "Save", Left = 140, Top = 160, Width = 120 };
            btnSave.Click += BtnSave_Click;

            this.Controls.Add(lblDistance);
            this.Controls.Add(nudDistance);
            this.Controls.Add(lblDuration);
            this.Controls.Add(nudDuration);
            this.Controls.Add(lblCalories);
            this.Controls.Add(nudCalories);
            this.Controls.Add(btnSave);
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            DatabaseHelper.LogRun(
                userId,
                DateTime.Now.Date,
                (float)nudDistance.Value,
                (int)nudDuration.Value,
                (int)nudCalories.Value
            );
            MessageBox.Show("Run logged!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }
    }
} 