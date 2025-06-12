using System;
using System.Drawing;
using System.Windows.Forms;

namespace FitTrackerPro
{
    public class SettingsForm : Form
    {
        private Label lblTitle;
        private Label lblSubtitle;
        private TabControl tabControl;
        private int userId;
        private string username;

        public SettingsForm(int userId = 0, string username = "")
        {
            this.userId = userId;
            this.username = username;
            BuildUI();
        }

        private void BuildUI()
        {
            this.Text = "FitTracker Pro - Settings";
            this.Size = new Size(900, 650);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;

            // Header Title
            lblTitle = new Label();
            lblTitle.Text = "Settings";
            lblTitle.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            lblTitle.Location = new Point(210, 30);
            lblTitle.AutoSize = true;

            // Subtitle
            lblSubtitle = new Label();
            lblSubtitle.Text = "Manage your account settings and preferences";
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
            var tabAccount = new TabPage("Account");

            // Account tab controls
            Label lblCurrentPwd = new Label();
            lblCurrentPwd.Text = "Current Password:";
            lblCurrentPwd.Location = new Point(210, 30);
            TextBox txtCurrentPwd = new TextBox();
            txtCurrentPwd.Location = new Point(360, 30);
            txtCurrentPwd.Size = new Size(200, 22);
            txtCurrentPwd.PasswordChar = '*';
            Label lblNewPwd = new Label();
            lblNewPwd.Text = "New Password:";
            lblNewPwd.Location = new Point(210, 70);
            TextBox txtNewPwd = new TextBox();
            txtNewPwd.Location = new Point(360, 70);
            txtNewPwd.Size = new Size(200, 22);
            txtNewPwd.PasswordChar = '*';
            Label lblConfirmPwd = new Label();
            lblConfirmPwd.Text = "Confirm New Password:";
            lblConfirmPwd.Location = new Point(210, 110);
            TextBox txtConfirmPwd = new TextBox();
            txtConfirmPwd.Location = new Point(360, 110);
            txtConfirmPwd.Size = new Size(200, 22);
            txtConfirmPwd.PasswordChar = '*';
            Button btnUpdatePwd = new Button();
            btnUpdatePwd.Text = "Update Password";
            btnUpdatePwd.Location = new Point(360, 150);
            btnUpdatePwd.Size = new Size(140, 28);
            btnUpdatePwd.BackColor = Color.FromArgb(0, 120, 215);
            btnUpdatePwd.ForeColor = Color.White;
            btnUpdatePwd.FlatStyle = FlatStyle.Flat;
            btnUpdatePwd.FlatAppearance.BorderSize = 0;
            btnUpdatePwd.Click += (s, e) => {
                if (string.IsNullOrWhiteSpace(this.username))
                {
                    MessageBox.Show("Username is not set. Please log in again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                // Debug: Show username being used for validation
                // MessageBox.Show($"DEBUG: Username used for validation: '{this.username}'", "Debug Info");
                string currentPwd = txtCurrentPwd.Text;
                string newPwd = txtNewPwd.Text;
                string confirmPwd = txtConfirmPwd.Text;
                if (string.IsNullOrWhiteSpace(currentPwd) || string.IsNullOrWhiteSpace(newPwd) || string.IsNullOrWhiteSpace(confirmPwd))
                {
                    MessageBox.Show("Please fill in all password fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                // Validate current password using stored username
                if (!DatabaseHelper.ValidateUser(this.username, currentPwd))
                {
                    MessageBox.Show("Current password is incorrect.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (newPwd != confirmPwd)
                {
                    MessageBox.Show("New passwords do not match.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (newPwd.Length < 8)
                {
                    MessageBox.Show("New password must be at least 8 characters.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                // Password format: at least one uppercase, one lowercase, one number, one special character
                if (!System.Text.RegularExpressions.Regex.IsMatch(newPwd, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d]).{8,}$"))
                {
                    MessageBox.Show("Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                DatabaseHelper.UpdateUserPassword(this.userId, newPwd);
                MessageBox.Show("Password updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtCurrentPwd.Text = "";
                txtNewPwd.Text = "";
                txtConfirmPwd.Text = "";
            };
            tabAccount.Controls.Add(lblCurrentPwd);
            tabAccount.Controls.Add(txtCurrentPwd);
            tabAccount.Controls.Add(lblNewPwd);
            tabAccount.Controls.Add(txtNewPwd);
            tabAccount.Controls.Add(lblConfirmPwd);
            tabAccount.Controls.Add(txtConfirmPwd);
            tabAccount.Controls.Add(btnUpdatePwd);

            tabControl.TabPages.Add(tabAccount);

            // Add controls to the form
            this.Controls.Add(lblTitle);
            this.Controls.Add(lblSubtitle);
            this.Controls.Add(tabControl);
        }
    }
} 