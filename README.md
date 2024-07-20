# Clubzone-User-Management-System
using c# console based app to create a login and registration system.

## Description
This is a console-based App User Management System implemented in C#. It provides basic functionality for user registration and login, with a focus on security and data validation. The system uses SQLite for data storage and BCrypt for password hashing.

## Features
- User Registration with validation for:
  - Username
  - Email
  - Password strength
  - Nigerian phone number format
  - Nigerian cities
  - Minimum age requirement (13 years)
- User Login with account lockout after multiple failed attempts
- Secure password hashing using BCrypt
- Persistent data storage using SQLite

## Requirements
- .NET Core SDK (version 3.1 or later)
- SQLite

## NuGet Packages
- System.Data.SQLite.Core Version="4.0.3"
- BCrypt.Net-Next Version="1.0.118"

## Project Structure
- `User.cs`: Defines the User model
- `DatabaseManager.cs`: Handles SQLite database operations
- `UserManager.cs`: Manages user registration, login, and related operations
- `Program.cs`: Contains the main program loop and user interface

## Setup and Running
1. Clone the repository
2. Navigate to the project directory
3. Restore the NuGet packages:
  <ItemGroup>
    <PackageReference Include="BCrypt.Net-Next" Version="4.0.3" />
    <PackageReference Include="System.Data.SQLite.Core" Version="1.0.118" />
  </ItemGroup>
4. Build the project: dotnet build
5. Run the project: dotnet run
   
## Usage
Upon running the application, you'll be presented with a menu:
1. Register
2. Login
3. Exit

Choose an option by entering the corresponding number.

### Registration
You'll be prompted to enter:
- First Name
- Last Name
- Email Address
- Phone Number (Nigerian format)
- State in Nigeria
- Username
- Password
- Date of Birth (YYYY-MM-DD)

### Login
Enter your username and password when prompted.

## Security Features
- Passwords are hashed using BCrypt before storage
- Account lockout after 5 failed login attempts
- Minimum password strength requirements
- Input validation for all fields

## Limitations and Future Improvements
- This is a basic implementation and would need further enhancements for a production environment
- Error handling and logging could be improved
- Unit tests should be added for robustness

## Contributing
Contributions, issues, and feature requests are welcome. Feel free to check issues page if you want to contribute.

## License
[MIT](https://choosealicense.com/licenses/mit/)
