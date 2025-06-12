using System;
using System.Windows.Forms;

namespace FitTrackerPro
{
    public class LogMealForm : Form
    {
        private int userId;
        private ComboBox cbMealType;
        private NumericUpDown nudCalories, nudProtein, nudCarbs, nudFat;
        private Button btnSave;
        private TextBox txtFoodName;

        public LogMealForm(int userId)
        {
            this.userId = userId;
            BuildUI();
        }

        private void BuildUI()
        {
            this.Text = "Log Meal";
            this.Width = 350;
            this.Height = 400;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;

            Label lblMealType = new Label { Text = "Meal Type", Left = 30, Top = 30, Width = 80 };
            cbMealType = new ComboBox { Left = 120, Top = 25, Width = 150, DropDownStyle = ComboBoxStyle.DropDownList };
            cbMealType.Items.AddRange(new string[] { "Breakfast", "Lunch", "Dinner", "Snack" });
            cbMealType.SelectedIndex = 0;

            Label lblFoodName = new Label { Text = "Food Name", Left = 30, Top = 70, Width = 80 };
            txtFoodName = new TextBox { Left = 120, Top = 65, Width = 150 };

            Label lblCalories = new Label { Text = "Calories", Left = 30, Top = 110, Width = 80 };
            nudCalories = new NumericUpDown { Left = 120, Top = 105, Width = 150, Maximum = 5000, Minimum = 0 };

            Label lblProtein = new Label { Text = "Protein (g)", Left = 30, Top = 150, Width = 80 };
            nudProtein = new NumericUpDown { Left = 120, Top = 145, Width = 150, Maximum = 500, Minimum = 0, DecimalPlaces = 1, Increment = 0.5M };

            Label lblCarbs = new Label { Text = "Carbs (g)", Left = 30, Top = 190, Width = 80 };
            nudCarbs = new NumericUpDown { Left = 120, Top = 185, Width = 150, Maximum = 500, Minimum = 0, DecimalPlaces = 1, Increment = 0.5M };

            Label lblFat = new Label { Text = "Fat (g)", Left = 30, Top = 230, Width = 80 };
            nudFat = new NumericUpDown { Left = 120, Top = 225, Width = 150, Maximum = 200, Minimum = 0, DecimalPlaces = 1, Increment = 0.5M };

            btnSave = new Button { Text = "Save", Left = 120, Top = 280, Width = 150 };
            btnSave.Click += BtnSave_Click;

            this.Controls.Add(lblMealType);
            this.Controls.Add(cbMealType);
            this.Controls.Add(lblFoodName);
            this.Controls.Add(txtFoodName);
            this.Controls.Add(lblCalories);
            this.Controls.Add(nudCalories);
            this.Controls.Add(lblProtein);
            this.Controls.Add(nudProtein);
            this.Controls.Add(lblCarbs);
            this.Controls.Add(nudCarbs);
            this.Controls.Add(lblFat);
            this.Controls.Add(nudFat);
            this.Controls.Add(btnSave);
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            DatabaseHelper.LogMeal(
                userId,
                DateTime.Now.Date,
                cbMealType.SelectedItem.ToString(),
                txtFoodName.Text,
                (int)nudCalories.Value,
                (float)nudProtein.Value,
                (float)nudCarbs.Value,
                (float)nudFat.Value
            );
            MessageBox.Show("Meal logged!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }
    }
} 