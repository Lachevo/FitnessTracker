CREATE DATABASE FitnessDb;
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Users' AND xtype='U')
CREATE TABLE Users (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(100) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(255) NOT NULL
    -- You can add more fields (email, etc.) if needed
);
select * from Users;
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='UserProfile' AND xtype='U')
BEGIN
    CREATE TABLE UserProfile (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        UserId INT NOT NULL,
        FirstName NVARCHAR(100),
        LastName NVARCHAR(100),
        Email NVARCHAR(100),
        Phone NVARCHAR(50),
        Age INT,
        Gender NVARCHAR(20),
        Height INT,
        Weight INT,
        FOREIGN KEY (UserId) REFERENCES Users(Id)
    )
END
SELECT * FROM UserProfile;
drop table UserProfile;
-- Table for user goals
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='UserGoals' AND xtype='U')
BEGIN
    CREATE TABLE UserGoals (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        UserId INT NOT NULL,
        PrimaryGoal NVARCHAR(100),
        ActivityLevel NVARCHAR(100),
        TargetWeight INT,
        WeeklyWorkouts INT,
        DailySteps INT,
		DailyWeight INT,
		caloriesBurned INT,
        DailyCalories INT,
        FOREIGN KEY (UserId) REFERENCES Users(Id)
    )
END
drop table userGoals;
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='UserWorkouts' AND xtype='U')
BEGIN
    CREATE TABLE UserWorkouts (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        UserId INT NOT NULL,
        Date DATE,
        WorkoutType NVARCHAR(100),
        DurationMinutes INT,
        CaloriesBurned INT,
        FOREIGN KEY (UserId) REFERENCES Users(Id)
    )
END
drop table UserWorkouts;
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='UserMeals' AND xtype='U')
BEGIN
    CREATE TABLE UserMeals (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        UserId INT NOT NULL,
        Date DATE,
        MealType NVARCHAR(50),
		Date1 DATE,
        Calories INT,
        Protein FLOAT,
        Carbs FLOAT,
        Fat FLOAT,
        FOREIGN KEY (UserId) REFERENCES Users(Id)
    )
END
drop table UserMeals;
select * from userMeals;
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='UserActivity' AND xtype='U')
BEGIN
    CREATE TABLE UserActivity (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        UserId INT NOT NULL,
        Date DATE,
        Steps INT,
        CaloriesBurned INT,
        WaterIntake FLOAT,
        ActiveMinutes INT,
        FOREIGN KEY (UserId) REFERENCES Users(Id)
    )
END

drop table UserActivity;
-- Remove Username column if it exists
IF EXISTS (SELECT * FROM sys.columns WHERE Name = N'Username' AND Object_ID = Object_ID(N'UserMeals'))
    ALTER TABLE UserMeals DROP COLUMN Username;

-- Add UserId column if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.columns WHERE Name = N'UserId' AND Object_ID = Object_ID(N'UserMeals'))
    ALTER TABLE UserMeals ADD UserId INT NOT NULL DEFAULT 1;

-- Add foreign key
IF NOT EXISTS (
    SELECT * FROM sys.foreign_keys WHERE parent_object_id = OBJECT_ID(N'UserMeals') AND name = 'FK_UserMeals_Users'
)
    ALTER TABLE UserMeals ADD CONSTRAINT FK_UserMeals_Users FOREIGN KEY (UserId) REFERENCES Users(Id);
	  SELECT * FROM Users;
	  CREATE TABLE UserProgress (
    ProgressId INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL,
    Date DATE NOT NULL,
    Exercise NVARCHAR(100) NOT NULL,
    Reps INT NULL,
    Sets INT NULL,
    Weight FLOAT NULL,
    Distance FLOAT NULL,
    Duration INT NULL,
    FOREIGN KEY (UserId) REFERENCES Users(Id)
);
BEGIN
    CREATE TABLE UserSleep (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        UserId INT NOT NULL,
        Date DATE NOT NULL,
        SleepMinutes INT NOT NULL,
    FOREIGN KEY (UserId) REFERENCES Users(Id)
    )
END";