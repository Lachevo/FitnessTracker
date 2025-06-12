using System;
using System.Windows.Forms;

namespace FitTrackerPro
{
    public class SignUpForm : Form
    {
        private TextBox txtUsername;
        private TextBox txtPassword;
        private TextBox txtConfirmPassword;
        private Button btnSignUp;
        private LinkLabel linkBackToLogin;
        private Label lblError;

        public SignUpForm()
        {
            BuildUI();
        }

        private void BuildUI()
        {
            this.Text = "Sign Up";
            this.Width = 350;
            this.Height = 350;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;

            Label lblTitle = new Label { Text = "Create Account", Left = 100, Top = 20, Width = 150, Font = new System.Drawing.Font("Segoe UI", 12, System.Drawing.FontStyle.Bold) };
            Label lblUsername = new Label { Text = "Username", Left = 40, Top = 70, Width = 80 };
            txtUsername = new TextBox { Left = 130, Top = 65, Width = 150 };
            Label lblPassword = new Label { Text = "Password", Left = 40, Top = 110, Width = 80 };
            txtPassword = new TextBox { Left = 130, Top = 105, Width = 150, UseSystemPasswordChar = true };
            Label lblConfirmPassword = new Label { Text = "Confirm Password", Left = 10, Top = 150, Width = 110 };
            txtConfirmPassword = new TextBox { Left = 130, Top = 145, Width = 150, UseSystemPasswordChar = true };
            btnSignUp = new Button { Text = "Sign Up", Left = 130, Top = 190, Width = 150 };
            btnSignUp.Click += BtnSignUp_Click;
            linkBackToLogin = new LinkLabel { Text = "Back to Login", Left = 130, Top = 230, Width = 150, TextAlign = System.Drawing.ContentAlignment.MiddleCenter };
            linkBackToLogin.Click += LinkBackToLogin_Click;
            lblError = new Label { Left = 40, Top = 270, Width = 240, ForeColor = System.Drawing.Color.Red, Visible = false };

            this.Controls.Add(lblTitle);
            this.Controls.Add(lblUsername);
            this.Controls.Add(txtUsername);
            this.Controls.Add(lblPassword);
            this.Controls.Add(txtPassword);
            this.Controls.Add(lblConfirmPassword);
            this.Controls.Add(txtConfirmPassword);
            this.Controls.Add(btnSignUp);
            this.Controls.Add(linkBackToLogin);
            this.Controls.Add(lblError);
        }

        private void BtnSignUp_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;
            string confirmPassword = txtConfirmPassword.Text;
            if (password != confirmPassword)
            {
                lblError.Text = "Passwords do not match.";
                lblError.Visible = true;
                return;
            }
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                lblError.Text = "Username and password required.";
                lblError.Visible = true;
                return;
            }
            // Password format: at least one uppercase, one lowercase, one number, one special character
            if (password.Length < 8 || !System.Text.RegularExpressions.Regex.IsMatch(password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d]).{8,}$"))
            {
                lblError.Text = "Password must be at least 8 characters and contain uppercase, lowercase, number, and special character.";
                lblError.Visible = true;
                return;
            }
            int userId = DatabaseHelper.RegisterUser(username, password);
            if (userId == -1)
            {
                lblError.Text = "Username already exists.";
                lblError.Visible = true;
                return;
            }
            // Success
            this.Hide();
            var profileForm = new ProfileForm(userId);
            profileForm.FormClosed += (s, args) => {
                var mainForm = new MainForm(userId);
                mainForm.FormClosed += (s2, args2) => this.Close();
                mainForm.Show();
            };
            profileForm.Show();
        }

        private void LinkBackToLogin_Click(object sender, EventArgs e)
        {
            this.Hide();
            var loginForm = new LoginForm();
            loginForm.FormClosed += (s, args) => this.Close();
            loginForm.Show();
        }
    }
} 