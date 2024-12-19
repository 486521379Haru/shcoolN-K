using System.Data.SQLite;

namespace school
{
    public class DatabaseInitializer
    {
        private const string ConnectionString = "Data Source=db.db;Version=3;";

        public void InitializeDatabase()
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string createTableQuery = @"
                    CREATE TABLE IF NOT EXISTS Word (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Russian TEXT NOT NULL,
                        English TEXT NOT NULL
                    )";

                using (var command = new SQLiteCommand(createTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
