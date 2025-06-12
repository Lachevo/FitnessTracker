using System;
using System.Windows.Forms;

namespace FitTrackerPro
{
    public class AddWaterForm : Form
    {
        private int userId;
        private NumericUpDown nudWater;
        private Button btnSave;

        public AddWaterForm(int userId)
        {
            this.userId = userId;
            BuildUI();
        }

        private void BuildUI()
        {
            this.Text = "Add Water";
            this.Width = 300;
            this.Height = 200;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;

            Label lblWater = new Label { Text = "Water (L)", Left = 30, Top = 40, Width = 80 };
            nudWater = new NumericUpDown { Left = 120, Top = 35, Width = 100, DecimalPlaces = 2, Increment = 0.1M, Maximum = 10, Minimum = 0 };

            btnSave = new Button { Text = "Save", Left = 120, Top = 80, Width = 100 };
            btnSave.Click += BtnSave_Click;

            this.Controls.Add(lblWater);
            this.Controls.Add(nudWater);
            this.Controls.Add(btnSave);
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            DatabaseHelper.AddWaterIntake(userId, DateTime.Now.Date, (float)nudWater.Value);
            MessageBox.Show("Water intake logged!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }
    }
} 