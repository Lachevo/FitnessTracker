using System;
using System.Windows.Forms;
using System.Drawing;

namespace FitTrackerPro
{
    public class LoginForm : Form
    {
        private TextBox txtUsername;
        private TextBox txtPassword;
        private Button btnLogin;
        private LinkLabel linkSignUp;
        private Label lblError;

        public LoginForm()
        {
            BuildUI();
        }

        private void BuildUI()
        {
            this.Text = "Login";
            this.Width = 800;
            this.Height = 500;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;

            // Centered layout variables
            int formWidth = 800;
            int formHeight = 500;
            int boxWidth = 350;
            int boxHeight = 320;
            int boxLeft = (formWidth - boxWidth) / 2;
            int boxTop = (formHeight - boxHeight) / 2;

            // Title
            Label lblTitle = new Label {
                Text = "Fitness Tracker Login",
                Left = boxLeft,
                Top = 0,
                Width = boxWidth,
                Height = 60,
                AutoSize = false,
                Font = new System.Drawing.Font("Segoe UI", 22, System.Drawing.FontStyle.Bold),
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Username
            Label lblUsername = new Label {
                Text = "Username",
                Left = boxLeft + 20,
                Top = boxTop + 20,
                Width = 100,
                Font = new System.Drawing.Font("Segoe UI", 12, System.Drawing.FontStyle.Regular),
                BackColor = Color.Transparent
            };
            txtUsername = new TextBox {
                Left = boxLeft + 130,
                Top = boxTop + 18,
                Width = 180,
                Font = new System.Drawing.Font("Segoe UI", 12, System.Drawing.FontStyle.Regular)
            };

            // Password
            Label lblPassword = new Label {
                Text = "Password",
                Left = boxLeft + 20,
                Top = boxTop + 70,
                Width = 100,
                Font = new System.Drawing.Font("Segoe UI", 12, System.Drawing.FontStyle.Regular),
                BackColor = Color.Transparent
            };
            txtPassword = new TextBox {
                Left = boxLeft + 130,
                Top = boxTop + 68,
                Width = 180,
                Font = new System.Drawing.Font("Segoe UI", 12, System.Drawing.FontStyle.Regular),
                UseSystemPasswordChar = true
            };

            // Login Button
            btnLogin = new Button {
                Text = "Login",
                Left = boxLeft + 130,
                Top = boxTop + 120,
                Width = 180,
                Height = 38,
                Font = new System.Drawing.Font("Segoe UI", 12, System.Drawing.FontStyle.Bold)
            };
            btnLogin.Click += BtnLogin_Click;

            // Create Account Label
            Label lblCreateAccount = new Label {
                Text = "Don't have an account? Create one below!",
                Left = boxLeft + 20,
                Top = boxTop + 180,
                Width = boxWidth - 40,
                Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Regular),
                ForeColor = Color.Black,
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Sign Up Link
            linkSignUp = new LinkLabel {
                Text = "Sign Up",
                Left = boxLeft + 130,
                Top = boxTop + 210,
                Width = 180,
                Font = new System.Drawing.Font("Segoe UI", 12, System.Drawing.FontStyle.Underline),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };
            linkSignUp.Click += LinkSignUp_Click;

            // Error Label
            lblError = new Label {
                Left = boxLeft + 20,
                Top = boxTop + 260,
                Width = boxWidth - 40,
                Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Regular),
                ForeColor = Color.Red,
                Visible = false,
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Add controls to the form
            this.Controls.Add(lblTitle);
            this.Controls.Add(lblUsername);
            this.Controls.Add(txtUsername);
            this.Controls.Add(lblPassword);
            this.Controls.Add(txtPassword);
            this.Controls.Add(btnLogin);
            this.Controls.Add(lblCreateAccount);
            this.Controls.Add(linkSignUp);
            this.Controls.Add(lblError);
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;
            if (DatabaseHelper.ValidateUser(username, password))
            {
                int userId = DatabaseHelper.GetUserId(username);
                this.Hide();
                var mainForm = new MainForm(userId, username);
                mainForm.FormClosed += (s, args) => this.Close();
                mainForm.Show();
            }
            else
            {
                lblError.Text = "Invalid username or password.";
                lblError.Visible = true;
            }
        }

        private void LinkSignUp_Click(object sender, EventArgs e)
        {
            this.Hide();
            var signUpForm = new SignUpForm();
            signUpForm.FormClosed += (s, args) => this.Show();
            signUpForm.Show();
        }
    }
} 