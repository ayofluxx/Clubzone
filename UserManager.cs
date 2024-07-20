using System;
using System.Data.SQLite;
using BCrypt.Net;
using System.Text.RegularExpressions;
using System.Net.Mail;
using System.Data;

public class UserManager
{
    private DbManager _dbManager;
    private const int MaxLoginAttempts = 5;
    private const int LockoutDurationMinutes = 15;

    public UserManager(DbManager dbManager)
    {
        _dbManager = dbManager;
    }

    public bool RegisterUser(string username, string email, string password, string firstName, string lastName, string phoneNumber, string state, DateTime dob)
    {
        if (!IsValidUsername(username))
        {
            Console.WriteLine("Invalid username format, Username should be 3-20 characters long and contain only letters, numbers, and underscores. ");
            return false;
        }

        if (!IsValidEmail(email))
        {
            Console.WriteLine("Invalid email format");
            return false;
        }

        if (!IsStrongPassword(password))
        {
            Console.WriteLine("Password is not strong enough, Try something harder");
            return false;
        }

        if (!IsValidNigerianPhoneNumber(phoneNumber))
        {
            Console.WriteLine("Invalid Nigerian Phone number format.");
            return false;
        }

        if (!IsValidAge(dob))
        {
            Console.WriteLine("You must be at least 13 years old");
            return false;
        }

        string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

        using (var connection = _dbManager.GetConnection())
        {
            connection.Open();
            string sql = @"INSERT INTO Users (UserName, PasswordHash, Email, FirstName, LastName, PhoneNumber, State, DateOfBirth) 
                           VALUES (@UserName, @PasswordHash, @Email, @FirstName, @LastName, @PhoneNumber, @State, @DateOfBirth)";
            
            using (var command = new SQLiteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@UserName", username);
                command.Parameters.AddWithValue("@PasswordHash", passwordHash);
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@FirstName", firstName);
                command.Parameters.AddWithValue("@LastName", lastName);
                command.Parameters.AddWithValue("@PhoneNumber", phoneNumber);
                command.Parameters.AddWithValue("@State", state);
                command.Parameters.AddWithValue("@DateOfBirth", dob.ToString("yyyy-MM-dd"));

                try
                {
                    command.ExecuteNonQuery();
                    Console.WriteLine("Registration successful");
                    return true;
                }
                catch (SQLiteException)
                {
                    Console.WriteLine("UserName, email, or phone number already exists");
                    return false;
                }
            }
        }
    }

    public bool LoginUser(string username, string password)
    {
        using (var connection = _dbManager.GetConnection())
        {
            connection.Open();
            string sql = "SELECT * FROM Users WHERE UserName = @UserName";
            using (var command = new SQLiteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@UserName", username);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var user = new User
                        {
                            Id = reader.GetInt32(0),
                            UserName = reader.GetString(1),
                            PasswordHash = reader.GetString(2),
                            Email = reader.GetString(3),
                            FirstName = reader.GetString(4),
                            LastName = reader.GetString(5),
                            PhoneNumber = reader.GetString(6),
                            State = reader.GetString(7),
                            DateOfBirth = DateTime.Parse(reader.GetString(8)),
                            LoginAttempts = reader.GetInt32(9),
                            LockOutEnd = reader.IsDBNull(10) ? (DateTime?)null : DateTime.Parse(reader.GetString(10))
                        };

                        if (user.LockOutEnd.HasValue && user.LockOutEnd > DateTime.Now)
                        {
                            Console.WriteLine($"Account is locked. Try again after {user.LockOutEnd.Value}");
                            return false;
                        }

                        if (BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                        {
                            ResetLoginAttempts(user.Id);
                            Console.WriteLine("Login successful");
                            return true;
                        }
                        else
                        {
                            IncrementLoginAttempts(user);
                            return false;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid username or password");
                        return false;
                    }
                }
            }
        }
    }

    private void IncrementLoginAttempts(User user)
    {
        user.LoginAttempts++;
        if (user.LoginAttempts >= MaxLoginAttempts)
        {
            user.LockOutEnd = DateTime.Now.AddMinutes(LockoutDurationMinutes);
            Console.WriteLine($"Account locked. Try again after {user.LockOutEnd}");
        }
        else
        {
            Console.WriteLine($"Invalid password. Attempts remaining: {MaxLoginAttempts - user.LoginAttempts}");
        }

        using (var connection = _dbManager.GetConnection())
        {
            connection.Open();
            string sql = "UPDATE Users SET LoginAttempts = @LoginAttempts, LockoutEnd = @LockOutEnd WHERE Id = @Id";
            using (var command = new SQLiteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@LoginAttempts", user.LoginAttempts);
                command.Parameters.AddWithValue("@LockOutEnd", user.LockOutEnd?.ToString("o") ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Id", user.Id);
                command.ExecuteNonQuery();
            }
        }
    }

    private void ResetLoginAttempts(int userId)
    {
        using (var connection = _dbManager.GetConnection())
        {
            connection.Open();
            string sql = "UPDATE Users SET LoginAttempts = 0, LockoutEnd = NULL WHERE Id = @Id";
            using (var command = new SQLiteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Id", userId);
                command.ExecuteNonQuery();
            }
        }
    }

    private bool IsValidUsername(string username)
    {
        return Regex.IsMatch(username, @"^[a-zA-Z0-9_]{3,20}$");
    }

    private bool IsValidEmail(string email)
{
    try {
    email = new MailAddress(email).Address;
} catch(FormatException) {
    Console.WriteLine("Wrong format");
}
return false;
}


    private bool IsStrongPassword(string password)
    {
        return password.Length >= 8 &&
               Regex.IsMatch(password, @"[A-Z]") &&
               Regex.IsMatch(password, @"[a-z]") &&
               Regex.IsMatch(password, @"[0-9]") &&
               Regex.IsMatch(password, @"[^A-Za-z0-9]");
    }

    private bool IsValidNigerianPhoneNumber(string phoneNumber)
    {
        return Regex.IsMatch(phoneNumber, @"^(\+234|0)[789]\d{9}$");
    }

    private bool IsValidAge(DateTime dob)
    {
        int age = DateTime.Now.Year - dob.Year;
        if (DateTime.Now.DayOfYear < dob.DayOfYear)
            age--;
        return age >= 13;
    }
}
