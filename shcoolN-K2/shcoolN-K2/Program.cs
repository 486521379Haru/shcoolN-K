using System;
using System.Windows.Forms;

namespace school
{
    internal static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            DatabaseInitializer dbInitializer = new DatabaseInitializer();
            dbInitializer.InitializeDatabase();
            Console.WriteLine("База данных инициализирована.");
            Application.Run(new Form1());
        }
    }
}
