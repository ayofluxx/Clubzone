using System.Data.SQLite;

public class DbManager
{
    private string _connectionString;

    public DbManager(string dbpath)
    {
        _connectionString = $"Data Source = {dbpath}; Version=3";
        InitializeDatabase();
    }

    public void InitializeDatabase()
    {
        if (!File.Exists("Clubzone.db"))
        {
            SQLiteConnection.CreateFile("Clubzone.db");
        }

        using (var connection = new SQLiteConnection(_connectionString))
        {
            connection.Open();
            string sql = @"
              CREATE TABLE IF NOT EXISTS Users(
              Id INTEGER PRIMARY KEY AUTOINCREMENT,
              FirstName TEXT NOT NULL,
              LastName TEXT NOT NULL,
              Email TEXT NOT NULL UNIQUE,
              PhoneNumber TEXT NOT NULL UNIQUE,
              State TEXT NOT NULL UNIQUE,
              UserName TEXT NOT NULL UNIQUE,
              PasswordHash TEXT NOT NULL,
              DateOfBirth DATE NOT NULL,
              LoginAttempts INTEGER DEFAULT 0,
              LockOutEnd TEXT 
              );";
            
            using (var command = new SQLiteCommand(sql, connection))
            {
                command.ExecuteNonQuery();
            }
        }
    }

    public SQLiteConnection GetConnection()
    {
        return new SQLiteConnection(_connectionString);
    }
}
