using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SqlClient;
using FitTrackerPro;

namespace FitTrackerPro
{
    public class MainForm : Form
    {
        private Panel sidebar;
        private Button[] navButtons;
        private string[] navNames = { "Dashboard", "Workouts", "Activity", "Nutrition", "Sleep", "Goals", "Profile", "Settings" };
        private int currentUserId;
        private string currentUsername;
        private Panel mainPanel;

        public MainForm(int userId, string username = "")
        {
            currentUserId = userId;
            currentUsername = username;
            InitializeComponent();
            ShowSection(0); // Show Dashboard on load
        }

        public MainForm() : this(0) { }

        private void InitializeComponent()
        {
            this.Text = "FitTracker Pro";
            this.Size = new Size(1200, 800);
            this.MinimumSize = new Size(900, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;

            // Sidebar
            sidebar = new Panel();
            sidebar.Size = new Size(180, 800);
            sidebar.Dock = DockStyle.Left;
            sidebar.BackColor = Color.FromArgb(30, 41, 59);
            sidebar.BorderStyle = BorderStyle.FixedSingle;
            this.Controls.Add(sidebar);

            // Navigation Buttons
            navButtons = new Button[navNames.Length];
            for (int i = 0; i < navNames.Length; i++)
            {
                navButtons[i] = new Button();
                navButtons[i].Text = (navNames[i] == "Goals" ? "Progress" : navNames[i]);
                navButtons[i].Size = new Size(160, 48);
                navButtons[i].Location = new Point(10, 30 + i * 55);
                navButtons[i].Font = new Font("Segoe UI", 12F, FontStyle.Bold);
                navButtons[i].BackColor = Color.FromArgb(30, 41, 59);
                navButtons[i].ForeColor = Color.White;
                navButtons[i].FlatStyle = FlatStyle.Flat;
                navButtons[i].FlatAppearance.BorderSize = 0;
                navButtons[i].Tag = i;
                navButtons[i].Click += NavButton_Click;
                sidebar.Controls.Add(navButtons[i]);
            }

            // Main content panel
            mainPanel = new Panel();
            mainPanel.Dock = DockStyle.Fill;
            mainPanel.BackColor = Color.White;
            this.Controls.Add(mainPanel);
        }

        private void NavButton_Click(object sender, EventArgs e)
        {
            int idx = (int)((Button)sender).Tag;
            ShowSection(idx);
        }

        public void ShowSection(int idx)
        {
            mainPanel.Controls.Clear();
            Form formToShow = null;
            switch (idx)
            {
                case 0: formToShow = new DashboardForm(currentUserId); break;
                case 1: formToShow = new WorkoutsForm(currentUserId); break;
                case 2: formToShow = new ActivityForm(currentUserId); break;
                case 3: formToShow = new NutritionForm(currentUserId); break;
                case 4: formToShow = new SleepForm(currentUserId); break;
                case 5: formToShow = new ProgressForm(currentUserId); break;
                case 6: formToShow = new ProfileForm(currentUserId); break;
                case 7: formToShow = new SettingsForm(currentUserId, currentUsername); break;
            }
            if (formToShow != null)
            {
                formToShow.TopLevel = false;
                formToShow.FormBorderStyle = FormBorderStyle.None;
                formToShow.Dock = DockStyle.Fill;
                mainPanel.Controls.Add(formToShow);
                formToShow.Show();
            }
        }
    }
} 