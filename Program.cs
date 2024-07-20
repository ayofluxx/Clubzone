
class Program{

    static UserManager userManager;
    static void Main(string[]args){
        var dbManager = new DbManager("Clubzone.db");
        userManager = new UserManager(dbManager);

        while(true){
            Console.WriteLine("\n1. Register");
            Console.WriteLine("2. Login");
            Console.WriteLine("3. Exit");
            Console.Write("Choose an option: ");

            string choice = Console.ReadLine();

            switch(choice){
                case "1":
                    Register();
                    break;

               case "2":
                    Login();
                    break;

                case "3":
                    Environment.Exit(0);
                    break;

                default:
                Console.WriteLine("Invalid Option, Please Try again");
                break;



               
            }
        }

    }

    static void Register(){
        Console.WriteLine("First Name: ");
        string firstName= Console.ReadLine();

        Console.WriteLine("Last Name: ");
        string lastName = Console.ReadLine();

        Console.WriteLine("Email Address: ");
        string email = Console.ReadLine();

        Console.WriteLine("Phone Number(Nigeria Format): ");
        string phoneNumber = Console.ReadLine();

        Console.WriteLine("State in Nigeria: ");
        string state = Console.ReadLine();

        
        Console.WriteLine("Username: ");
        string userName = Console.ReadLine();

        Console.WriteLine("Password:");
        string password = Console.ReadLine();

        Console.WriteLine("Date of Birth (YYYY-MM-DD): ");
       if(DateTime.TryParse(Console.ReadLine(), out DateTime dob)){
        userManager.RegisterUser(userName,password,email,firstName,lastName,phoneNumber, state, dob);
       }
        else
            {
                Console.WriteLine("Invalid date format. Registration failed.");
            }
        }

        static void Login()
        {
            Console.Write("Enter username: ");
            string username = Console.ReadLine();

            Console.Write("Enter password: ");

            string password = Console.ReadLine();

            userManager.LoginUser(username, password);
        }
    }



    

