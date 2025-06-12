using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;

namespace FitTrackerPro
{
    public class ProgressForm : Form
    {
        private Label lblTitle;
        private Label lblSubtitle;
        private Panel panelMessage;
        private Label lblMessage;
        private Panel[] goalCards;
        private ListView lvHistory;
        private ComboBox cbExercise;
        private NumericUpDown nudReps, nudSets, nudWeight, nudDistance, nudDuration;
        private DateTimePicker dtpDate;
        private Button btnLog;
        private int userId = 0;

        public ProgressForm() : this(0) { }
        public ProgressForm(int userId)
        {
            this.userId = userId;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "FitTracker Pro - Progress";
            this.Size = new Size(900, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;

            int leftMargin = 220;
            // Header Title
            lblTitle = new Label();
            lblTitle.Text = "Progress Tracking";
            lblTitle.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            lblTitle.Location = new Point(leftMargin + 30, 30);
            lblTitle.AutoSize = true;

            // Subtitle
            lblSubtitle = new Label();
            lblSubtitle.Text = "Log your reps, sets, weights, and more for each exercise";
            lblSubtitle.Font = new Font("Segoe UI", 12F, FontStyle.Regular);
            lblSubtitle.Location = new Point(leftMargin + 32, 75);
            lblSubtitle.AutoSize = true;
            lblSubtitle.ForeColor = Color.Gray;

            // Exercise selection
            Label lblExercise = new Label { Text = "Exercise:", Location = new Point(leftMargin + 30, 130), Size = new Size(80, 28) };
            cbExercise = new ComboBox { Location = new Point(leftMargin + 120, 130), Size = new Size(200, 28), DropDownStyle = ComboBoxStyle.DropDownList };
            cbExercise.Items.AddRange(new string[] { "Bench Press", "Squat", "Deadlift", "Pull-up", "Push-up", "Running", "Cycling", "Custom..." });
            cbExercise.SelectedIndex = 0;
            cbExercise.SelectedIndexChanged += (s, e) => LoadProgressHistory();

            // Reps
            Label lblReps = new Label { Text = "Reps:", Location = new Point(leftMargin + 30, 170), Size = new Size(80, 28) };
            nudReps = new NumericUpDown { Location = new Point(leftMargin + 120, 170), Size = new Size(80, 28), Maximum = 1000, Minimum = 0 };
            // Sets
            Label lblSets = new Label { Text = "Sets:", Location = new Point(leftMargin + 220, 170), Size = new Size(80, 28) };
            nudSets = new NumericUpDown { Location = new Point(leftMargin + 300, 170), Size = new Size(80, 28), Maximum = 100, Minimum = 0 };
            // Weight
            Label lblWeight = new Label { Text = "Weight (kg):", Location = new Point(leftMargin + 30, 210), Size = new Size(90, 28) };
            nudWeight = new NumericUpDown { Location = new Point(leftMargin + 120, 210), Size = new Size(80, 28), Maximum = 1000, Minimum = 0, DecimalPlaces = 1, Increment = 0.5M };
            // Distance
            Label lblDistance = new Label { Text = "Distance (km):", Location = new Point(leftMargin + 220, 210), Size = new Size(100, 28) };
            nudDistance = new NumericUpDown { Location = new Point(leftMargin + 320, 210), Size = new Size(80, 28), Maximum = 1000, Minimum = 0, DecimalPlaces = 2, Increment = 0.1M };
            // Duration
            Label lblDuration = new Label { Text = "Duration (min):", Location = new Point(leftMargin + 30, 250), Size = new Size(100, 28) };
            nudDuration = new NumericUpDown { Location = new Point(leftMargin + 140, 250), Size = new Size(80, 28), Maximum = 1000, Minimum = 0 };
            // Date
            Label lblDate = new Label { Text = "Date:", Location = new Point(leftMargin + 240, 250), Size = new Size(50, 28) };
            dtpDate = new DateTimePicker { Location = new Point(leftMargin + 300, 250), Size = new Size(150, 28), Format = DateTimePickerFormat.Short, Value = DateTime.Now };

            // Log Button
            btnLog = new Button { Text = "Log Progress", Location = new Point(leftMargin + 120, 300), Size = new Size(180, 36), BackColor = Color.FromArgb(0, 120, 215), ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            btnLog.FlatAppearance.BorderSize = 0;
            btnLog.Click += BtnLog_Click;

            // Progress History ListView
            lvHistory = new ListView();
            lvHistory.Location = new Point(leftMargin + 30, 360);
            lvHistory.Size = new Size(800, 180);
            lvHistory.View = View.Details;
            lvHistory.Columns.Add("Date", 100);
            lvHistory.Columns.Add("Exercise", 120);
            lvHistory.Columns.Add("Reps", 60);
            lvHistory.Columns.Add("Sets", 60);
            lvHistory.Columns.Add("Weight (kg)", 90);
            lvHistory.Columns.Add("Distance (km)", 90);
            lvHistory.Columns.Add("Duration (min)", 90);
            lvHistory.FullRowSelect = true;
            lvHistory.GridLines = true;

            // Add controls
            this.Controls.Add(lblTitle);
            this.Controls.Add(lblSubtitle);
            this.Controls.Add(lblExercise);
            this.Controls.Add(cbExercise);
            this.Controls.Add(lblReps);
            this.Controls.Add(nudReps);
            this.Controls.Add(lblSets);
            this.Controls.Add(nudSets);
            this.Controls.Add(lblWeight);
            this.Controls.Add(nudWeight);
            this.Controls.Add(lblDistance);
            this.Controls.Add(nudDistance);
            this.Controls.Add(lblDuration);
            this.Controls.Add(nudDuration);
            this.Controls.Add(lblDate);
            this.Controls.Add(dtpDate);
            this.Controls.Add(btnLog);
            this.Controls.Add(lvHistory);

            LoadProgressHistory();
        }

        private void BtnLog_Click(object sender, EventArgs e)
        {
            // Save progress to DB (simple implementation, you can expand this)
            using (var conn = new SqlConnection(DatabaseHelper.ConnectionString))
            {
                conn.Open();
                string sql = "INSERT INTO UserProgress (UserId, Date, Exercise, Reps, Sets, Weight, Distance, Duration) VALUES (@UserId, @Date, @Exercise, @Reps, @Sets, @Weight, @Distance, @Duration)";
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@Date", dtpDate.Value.Date);
                    cmd.Parameters.AddWithValue("@Exercise", cbExercise.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@Reps", (int)nudReps.Value);
                    cmd.Parameters.AddWithValue("@Sets", (int)nudSets.Value);
                    cmd.Parameters.AddWithValue("@Weight", (float)nudWeight.Value);
                    cmd.Parameters.AddWithValue("@Distance", (float)nudDistance.Value);
                    cmd.Parameters.AddWithValue("@Duration", (int)nudDuration.Value);
                    cmd.ExecuteNonQuery();
                }
            }
            MessageBox.Show("Progress logged!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadProgressHistory();
        }

        private void LoadProgressHistory()
        {
            lvHistory.Items.Clear();
            using (var conn = new SqlConnection(DatabaseHelper.ConnectionString))
            {
                conn.Open();
                string sql = "SELECT Date, Exercise, Reps, Sets, Weight, Distance, Duration FROM UserProgress WHERE UserId = @UserId ORDER BY Date DESC";
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string date = reader.IsDBNull(0) ? "" : ((DateTime)reader.GetDateTime(0)).ToString("yyyy-MM-dd");
                            string exercise = reader.IsDBNull(1) ? "" : reader.GetString(1);
                            string reps = reader.IsDBNull(2) ? "" : reader.GetInt32(2).ToString();
                            string sets = reader.IsDBNull(3) ? "" : reader.GetInt32(3).ToString();
                            string weight = reader.IsDBNull(4) ? "" : reader.GetDouble(4).ToString("0.##");
                            string distance = reader.IsDBNull(5) ? "" : reader.GetDouble(5).ToString("0.##");
                            string duration = reader.IsDBNull(6) ? "" : reader.GetInt32(6).ToString();
                            lvHistory.Items.Add(new ListViewItem(new string[] { date, exercise, reps, sets, weight, distance, duration }));
                        }
                    }
                }
            }
        }
    }
} 