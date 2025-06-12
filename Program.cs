using System;
using System.Windows.Forms;

namespace FitTrackerPro
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            DatabaseHelper.InitializeDatabase();
            Application.Run(new LoginForm());
        }
    }
} 