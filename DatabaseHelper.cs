using System;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;

namespace FitTrackerPro
{
    public static class DatabaseHelper
    {
        public static string ConnectionString = "Data Source=NiwayL-Agriplm;Initial Catalog=FitnessDb;Integrated Security=True;TrustServerCertificate=True";

        public static void InitializeDatabase()
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                // Users table
                string usersTable = @"IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Users' AND xtype='U')
BEGIN
    CREATE TABLE Users (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Username NVARCHAR(100) NOT NULL UNIQUE,
        PasswordHash NVARCHAR(255) NOT NULL
    )
END";
                using (var cmd = new SqlCommand(usersTable, conn))
                {
                    cmd.ExecuteNonQuery();
                }
                // UserProfile table
                string userTable = @"IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='UserProfile' AND xtype='U')
BEGIN
    CREATE TABLE UserProfile (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        FirstName NVARCHAR(100),
        LastName NVARCHAR(100),
        Email NVARCHAR(100),
        Phone NVARCHAR(50),
        Age INT,
        Gender NVARCHAR(20),
        Height INT,
        Weight INT
    )
END";
                using (var cmd = new SqlCommand(userTable, conn))
                {
                    cmd.ExecuteNonQuery();
                }
                // UserGoals table
                string goalsTable = @"IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='UserGoals' AND xtype='U')
BEGIN
    CREATE TABLE UserGoals (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        PrimaryGoal NVARCHAR(100),
        ActivityLevel NVARCHAR(100),
        TargetWeight INT,
        WeeklyWorkouts INT,
        DailySteps INT,
        DailyCalories INT
    )
END";
                using (var cmd = new SqlCommand(goalsTable, conn))
                {
                    cmd.ExecuteNonQuery();
                }
                // UserActivity table
                string activityTable = @"IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='UserActivity' AND xtype='U')
BEGIN
    CREATE TABLE UserActivity (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Date DATE,
        Steps INT,
        CaloriesBurned INT,
        WaterIntake FLOAT,
        ActiveMinutes INT
    )
END";
                using (var cmd = new SqlCommand(activityTable, conn))
                {
                    cmd.ExecuteNonQuery();
                }
                // Add UserWorkouts table
                string workoutsTable = @"IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='UserWorkouts' AND xtype='U')
BEGIN
    CREATE TABLE UserWorkouts (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Username NVARCHAR(100) NOT NULL,
        Date DATE,
        WorkoutType NVARCHAR(100),
        DurationMinutes INT,
        CaloriesBurned INT
    )
END";
                using (var cmd = new SqlCommand(workoutsTable, conn))
                {
                    cmd.ExecuteNonQuery();
                }
                // Add UserMeals table
                string mealsTable = @"IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='UserMeals' AND xtype='U')
BEGIN
    CREATE TABLE UserMeals (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Username NVARCHAR(100) NOT NULL,
        Date DATE,
        MealType NVARCHAR(50),
        Calories INT,
        Protein FLOAT,
        Carbs FLOAT,
        Fat FLOAT
    )
END";
                using (var cmd = new SqlCommand(mealsTable, conn))
                {
                    cmd.ExecuteNonQuery();
                }
                // Add FoodName column to UserMeals if it doesn't exist
                string addFoodNameCol = @"IF NOT EXISTS (SELECT * FROM sys.columns WHERE Name = N'FoodName' AND Object_ID = Object_ID(N'UserMeals'))
ALTER TABLE UserMeals ADD FoodName NVARCHAR(255) NULL;";
                using (var cmd = new SqlCommand(addFoodNameCol, conn))
                {
                    cmd.ExecuteNonQuery();
                }
                // Add UserSleep table
                string sleepTable = @"IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='UserSleep' AND xtype='U')
BEGIN
    CREATE TABLE UserSleep (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        UserId INT NOT NULL,
        Date DATE NOT NULL,
        SleepMinutes INT NOT NULL
    )
END";
                using (var cmd = new SqlCommand(sleepTable, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void SaveUserProfile(int userId, string firstName, string lastName, string email, string phone, int age, string gender, int height, int weight)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                string sql = @"UPDATE UserProfile SET FirstName = @FirstName, LastName = @LastName, Email = @Email, Phone = @Phone, Age = @Age, Gender = @Gender, Height = @Height, Weight = @Weight WHERE UserId = @UserId";
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@FirstName", firstName);
                    cmd.Parameters.AddWithValue("@LastName", lastName);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Phone", phone);
                    cmd.Parameters.AddWithValue("@Age", age);
                    cmd.Parameters.AddWithValue("@Gender", gender);
                    cmd.Parameters.AddWithValue("@Height", height);
                    cmd.Parameters.AddWithValue("@Weight", weight);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static DataRow LoadUserProfile()
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                string sql = "SELECT * FROM UserProfile LIMIT 1;";
                using (var cmd = new SqlCommand(sql, conn))
                using (var da = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                        return dt.Rows[0];
                    return null;
                }
            }
        }

        public static void SaveUserGoals(string primaryGoal, string activityLevel, int targetWeight, int weeklyWorkouts, int dailySteps, int dailyCalories)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                string clearSql = "DELETE FROM UserGoals;";
                using (var cmd = new SqlCommand(clearSql, conn))
                {
                    cmd.ExecuteNonQuery();
                }
                string sql = @"INSERT INTO UserGoals (PrimaryGoal, ActivityLevel, TargetWeight, WeeklyWorkouts, DailySteps, DailyCalories)
                               VALUES (@PrimaryGoal, @ActivityLevel, @TargetWeight, @WeeklyWorkouts, @DailySteps, @DailyCalories);";
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@PrimaryGoal", primaryGoal);
                    cmd.Parameters.AddWithValue("@ActivityLevel", activityLevel);
                    cmd.Parameters.AddWithValue("@TargetWeight", targetWeight);
                    cmd.Parameters.AddWithValue("@WeeklyWorkouts", weeklyWorkouts);
                    cmd.Parameters.AddWithValue("@DailySteps", dailySteps);
                    cmd.Parameters.AddWithValue("@DailyCalories", dailyCalories);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static DataRow LoadUserGoals()
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                string sql = "SELECT TOP 1 * FROM UserGoals;";
                using (var cmd = new SqlCommand(sql, conn))
                using (var da = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                        return dt.Rows[0];
                    return null;
                }
            }
        }

        public static void SaveUserActivity(DateTime date, int steps, int caloriesBurned, float waterIntake, int activeMinutes)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                string clearSql = "DELETE FROM UserActivity WHERE Date = @Date;";
                using (var cmd = new SqlCommand(clearSql, conn))
                {
                    cmd.Parameters.AddWithValue("@Date", date.ToString("yyyy-MM-dd"));
                    cmd.ExecuteNonQuery();
                }
                string sql = @"INSERT INTO UserActivity (Date, Steps, CaloriesBurned, WaterIntake, ActiveMinutes)
                               VALUES (@Date, @Steps, @CaloriesBurned, @WaterIntake, @ActiveMinutes);";
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Date", date.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@Steps", steps);
                    cmd.Parameters.AddWithValue("@CaloriesBurned", caloriesBurned);
                    cmd.Parameters.AddWithValue("@WaterIntake", waterIntake);
                    cmd.Parameters.AddWithValue("@ActiveMinutes", activeMinutes);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static DataRow LoadUserActivity(int userId, DateTime date)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                string sql = "SELECT TOP 1 * FROM UserActivity WHERE UserId = @UserId AND Date = @Date";
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@Date", date.ToString("yyyy-MM-dd"));
                    using (var da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        if (dt.Rows.Count > 0)
                            return dt.Rows[0];
                        return null;
                    }
                }
            }
        }

        public static int RegisterUser(string username, string password)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                string checkSql = "SELECT COUNT(*) FROM Users WHERE Username = @Username";
                using (var cmd = new SqlCommand(checkSql, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", username);
                    int count = (int)cmd.ExecuteScalar();
                    if (count > 0)
                        return -1; // Username already exists
                }
                string hash = ComputeSha256Hash(password);
                string sql = "INSERT INTO Users (Username, PasswordHash) OUTPUT INSERTED.Id VALUES (@Username, @PasswordHash)";
                int userId;
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@PasswordHash", hash);
                    userId = (int)cmd.ExecuteScalar();
                }
                // Create default UserProfile
                string profileSql = "INSERT INTO UserProfile (UserId, Weight) VALUES (@UserId, 70)";
                using (var cmd = new SqlCommand(profileSql, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.ExecuteNonQuery();
                }
                // Create default UserGoals
                string goalsSql = "INSERT INTO UserGoals (UserId, DailyCalories, DailySteps, WeeklyWorkouts) VALUES (@UserId, 2000, 10000, 3)";
                using (var cmd = new SqlCommand(goalsSql, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.ExecuteNonQuery();
                }
                return userId;
            }
        }

        public static bool ValidateUser(string username, string password)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                string sql = "SELECT PasswordHash FROM Users WHERE Username = @Username";
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", username);
                    var result = cmd.ExecuteScalar();
                    if (result == null)
                        return false;
                    string hash = ComputeSha256Hash(password);
                    return result.ToString() == hash;
                }
            }
        }

        private static string ComputeSha256Hash(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public static int GetUserId(string username)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                string sql = "SELECT Id FROM Users WHERE Username = @Username";
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", username);
                    var result = cmd.ExecuteScalar();
                    if (result != null)
                        return (int)result;
                    throw new Exception("User not found.");
                }
            }
        }

        /// <summary>
        /// Logs a workout or activity for a user. All activities and workouts are stored in the UserWorkouts table.
        /// The userId must exist in the Users table to avoid foreign key errors.
        /// </summary>
        public static void LogWorkout(int userId, DateTime date, string workoutType, int durationMinutes, int caloriesBurned)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                string sql = @"INSERT INTO UserWorkouts (UserId, Date, WorkoutType, DurationMinutes, CaloriesBurned)
                               VALUES (@UserId, @Date, @WorkoutType, @DurationMinutes, @CaloriesBurned)";
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@Date", date);
                    cmd.Parameters.AddWithValue("@WorkoutType", workoutType);
                    cmd.Parameters.AddWithValue("@DurationMinutes", durationMinutes);
                    cmd.Parameters.AddWithValue("@CaloriesBurned", caloriesBurned);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void LogMeal(int userId, DateTime date, string mealType, string foodName, int calories, float protein, float carbs, float fat)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                string sql = @"INSERT INTO UserMeals (UserId, Date, MealType, FoodName, Calories, Protein, Carbs, Fat)
                               VALUES (@UserId, @Date, @MealType, @FoodName, @Calories, @Protein, @Carbs, @Fat)";
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@Date", date);
                    cmd.Parameters.AddWithValue("@MealType", mealType);
                    cmd.Parameters.AddWithValue("@FoodName", (object)foodName ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Calories", calories);
                    cmd.Parameters.AddWithValue("@Protein", protein);
                    cmd.Parameters.AddWithValue("@Carbs", carbs);
                    cmd.Parameters.AddWithValue("@Fat", fat);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void AddWaterIntake(int userId, DateTime date, float waterAmount)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                string checkSql = "SELECT COUNT(*) FROM UserActivity WHERE UserId = @UserId AND Date = @Date";
                using (var cmd = new SqlCommand(checkSql, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@Date", date);
                    int count = (int)cmd.ExecuteScalar();
                    if (count > 0)
                    {
                        string updateSql = "UPDATE UserActivity SET WaterIntake = WaterIntake + @WaterAmount WHERE UserId = @UserId AND Date = @Date";
                        using (var updateCmd = new SqlCommand(updateSql, conn))
                        {
                            updateCmd.Parameters.AddWithValue("@WaterAmount", waterAmount);
                            updateCmd.Parameters.AddWithValue("@UserId", userId);
                            updateCmd.Parameters.AddWithValue("@Date", date);
                            updateCmd.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        string insertSql = "INSERT INTO UserActivity (UserId, Date, WaterIntake) VALUES (@UserId, @Date, @WaterAmount)";
                        using (var insertCmd = new SqlCommand(insertSql, conn))
                        {
                            insertCmd.Parameters.AddWithValue("@UserId", userId);
                            insertCmd.Parameters.AddWithValue("@Date", date);
                            insertCmd.Parameters.AddWithValue("@WaterAmount", waterAmount);
                            insertCmd.ExecuteNonQuery();
                        }
                    }
                }
            }
        }

        public static void LogRun(int userId, DateTime date, float distanceKm, int durationMinutes, int caloriesBurned)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                string sql = @"INSERT INTO UserWorkouts (UserId, Date, WorkoutType, DurationMinutes, CaloriesBurned)
                               VALUES (@UserId, @Date, @WorkoutType, @DurationMinutes, @CaloriesBurned)";
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@Date", date);
                    cmd.Parameters.AddWithValue("@WorkoutType", $"Run ({distanceKm} km)");
                    cmd.Parameters.AddWithValue("@DurationMinutes", durationMinutes);
                    cmd.Parameters.AddWithValue("@CaloriesBurned", caloriesBurned);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static int GetCaloriesConsumedToday(int userId)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                string sql = "SELECT ISNULL(SUM(Calories),0) FROM UserMeals WHERE UserId = @UserId AND Date = @Date";
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@Date", DateTime.Now.Date);
                    return (int)cmd.ExecuteScalar();
                }
            }
        }

        public static int GetCaloriesBurnedToday(int userId)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                string sql = "SELECT ISNULL(SUM(CaloriesBurned),0) FROM UserWorkouts WHERE UserId = @UserId AND Date = @Date";
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@Date", DateTime.Now.Date);
                    return (int)cmd.ExecuteScalar();
                }
            }
        }

        public static int GetStepsToday(int userId)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                string sql = "SELECT ISNULL(Steps,0) FROM UserActivity WHERE UserId = @UserId AND Date = @Date";
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@Date", DateTime.Now.Date);
                    var result = cmd.ExecuteScalar();
                    return result != DBNull.Value ? Convert.ToInt32(result) : 0;
                }
            }
        }

        public static float GetWaterToday(int userId)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                string sql = "SELECT ISNULL(WaterIntake,0) FROM UserActivity WHERE UserId = @UserId AND Date = @Date";
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@Date", DateTime.Now.Date);
                    var result = cmd.ExecuteScalar();
                    return result != DBNull.Value ? Convert.ToSingle(result) : 0f;
                }
            }
        }

        public static int GetActiveMinutesToday(int userId)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                string sql = "SELECT ISNULL(ActiveMinutes,0) FROM UserActivity WHERE UserId = @UserId AND Date = @Date";
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@Date", DateTime.Now.Date);
                    var result = cmd.ExecuteScalar();
                    return result != DBNull.Value ? Convert.ToInt32(result) : 0;
                }
            }
        }

        public static (int DailyCalories, int DailySteps, float DailyWater, int ActiveMinutes) GetUserGoals(int userId)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                string sql = "SELECT DailyCalories, DailySteps, DailyCalories/500 AS DailyWater, 60 AS ActiveMinutes FROM UserGoals WHERE UserId = @UserId";
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int dailyCalories = reader.IsDBNull(0) ? 2000 : reader.GetInt32(0);
                            int dailySteps = reader.IsDBNull(1) ? 10000 : reader.GetInt32(1);
                            float dailyWater = reader.IsDBNull(2) ? 2.0f : Convert.ToSingle(reader.GetValue(2));
                            int activeMinutes = reader.IsDBNull(3) ? 60 : reader.GetInt32(3);
                            return (dailyCalories, dailySteps, dailyWater, activeMinutes);
                        }
                        else
                        {
                            return (2000, 10000, 2.0f, 60);
                        }
                    }
                }
            }
        }

        public static float GetProteinConsumedToday(int userId)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                string sql = "SELECT ISNULL(SUM(Protein),0) FROM UserMeals WHERE UserId = @UserId AND Date = @Date";
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@Date", DateTime.Now.Date);
                    var result = cmd.ExecuteScalar();
                    return result != DBNull.Value ? Convert.ToSingle(result) : 0f;
                }
            }
        }

        public static float GetUserWeight(int userId)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                string sql = "SELECT TOP 1 Weight FROM UserProfile WHERE UserId = @UserId";
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    var result = cmd.ExecuteScalar();
                    return result != DBNull.Value ? Convert.ToSingle(result) : 70f; // default 70kg
                }
            }
        }

        public class WorkoutInfo
        {
            public string Type { get; set; }
            public int Duration { get; set; }
            public int Calories { get; set; }
        }

        public static List<WorkoutInfo> GetWorkoutsTodayFull(int userId)
        {
            var workouts = new List<WorkoutInfo>();
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                string sql = "SELECT WorkoutType, DurationMinutes, CaloriesBurned FROM UserWorkouts WHERE UserId = @UserId AND Date = @Date";
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@Date", DateTime.Now.Date);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            workouts.Add(new WorkoutInfo
                            {
                                Type = reader.IsDBNull(0) ? "" : reader.GetString(0),
                                Duration = reader.IsDBNull(1) ? 0 : reader.GetInt32(1),
                                Calories = reader.IsDBNull(2) ? 0 : reader.GetInt32(2)
                            });
                        }
                    }
                }
            }
            return workouts;
        }

        public static int GetActiveMinutesFromWorkoutsToday(int userId)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                string sql = "SELECT ISNULL(SUM(DurationMinutes),0) FROM UserWorkouts WHERE UserId = @UserId AND Date = @Date";
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@Date", DateTime.Now.Date);
                    return (int)cmd.ExecuteScalar();
                }
            }
        }

        // Save the user's daily weight in UserGoals
        public static void SaveDailyWeight(int userId, float dailyWeight)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                // Make sure the UserGoals table has a DailyWeight column (FLOAT or similar)
                string sql = "UPDATE UserGoals SET DailyWeight = @DailyWeight WHERE UserId = @UserId";
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@DailyWeight", dailyWeight);
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void UpdateUserPassword(int userId, string newPassword)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                string hash = ComputeSha256Hash(newPassword);
                string sql = "UPDATE Users SET PasswordHash = @PasswordHash WHERE Id = @UserId";
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@PasswordHash", hash);
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static string GetUsernameById(int userId)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                string sql = "SELECT Username FROM Users WHERE Id = @UserId";
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    var result = cmd.ExecuteScalar();
                    return result != null ? result.ToString() : null;
                }
            }
        }

        public static float GetSleepHoursToday(int userId)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                string sql = "SELECT ISNULL(SUM(SleepMinutes),0) FROM UserSleep WHERE UserId = @UserId AND Date = @Date";
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@Date", DateTime.Now.Date);
                    var result = cmd.ExecuteScalar();
                    return result != DBNull.Value ? Convert.ToSingle(result) / 60f : 0f;
                }
            }
        }
    }
} 