using System;
using System.Data;
using System.Data.SqlClient;

class Program
{
    static string connectionString = "Data Source=DESKTOP-DIJ81QJ;Initial Catalog=master;Integrated Security=True;";
    static Random random = new Random();

    static void Main(string[] args)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("----------------- Games Rental System -----------------");
            Console.WriteLine("1. Login as Admin");
            Console.WriteLine("2. Login as User");
            Console.WriteLine("3. SignUp as Admin");
            Console.WriteLine("4. SignUp as User");
            Console.WriteLine("5. Exit");
            Console.Write("Enter your choice: ");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    AdminLogin();
                    break;
                case "2":
                    UserLogin();
                    break;
                case "3":
                    AdminSignUp();
                    break;
                case "4":
                    UserSignUp();
                    break;
                case "5":
                    return;
                default:
                    Console.WriteLine("Invalid choice!");
                    break;
            }
        }
    }

    // Generate random ID for users and admins
    static int GenerateRandomId()
    {
        return random.Next(10, 1000);
    }

    // Check if ID exists in the specified table
    static bool IdExists(int id, string tableName, string idColumnName)
    {
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();
            string query = $"SELECT COUNT(*) FROM {tableName} WHERE {idColumnName} = @id";
            var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", id);
            int count = (int)cmd.ExecuteScalar();
            return count > 0;
        }
    }

    // ADMIN Login and signup 

    static void AdminLogin()
    {
        Console.Clear();
        Console.WriteLine("---- Admin Login ----");
        Console.Write("Enter admin username: ");
        string username = Console.ReadLine();
        Console.Write("Enter password: ");
        string password = Console.ReadLine();

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();
            var cmd = new SqlCommand("SELECT * FROM ADMIN WHERE USERNAME = @u AND PASSWORD = @p", conn);
            cmd.Parameters.AddWithValue("@u", username);
            cmd.Parameters.AddWithValue("@p", password);
            var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                int adminId = (int)reader["ADMINID"];
                reader.Close();
                AdminMenu(adminId);
            }
            else
            {
                Console.WriteLine("Invalid credentials!");
                Console.ReadKey();
            }
        }
    }

    static void AdminSignUp()
    {
        Console.Clear();
        Console.WriteLine("---- Admin SignUp ----");
        Console.Write("Enter admin username: ");
        string username = Console.ReadLine();
        Console.Write("Enter password: ");
        string password = Console.ReadLine();
        Console.Write("Enter email: ");
        string email = Console.ReadLine();
        Console.Write("Enter address: ");
        string address = Console.ReadLine();
        Console.Write("Enter your phone number: ");
        string phone = Console.ReadLine();

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();
            var cmdCheck = new SqlCommand("SELECT * FROM ADMIN WHERE USERNAME = @u", conn);
            cmdCheck.Parameters.AddWithValue("@u", username);
            var reader = cmdCheck.ExecuteReader();

            if (reader.HasRows)
            {
                Console.WriteLine("You already have an account!");
                Console.ReadKey();
                return;
            }
            reader.Close();

            // Generate a unique admin ID
            int adminId;
            do
            {
                adminId = GenerateRandomId();
            } while (IdExists(adminId, "ADMIN", "ADMINID"));

            var cmdInsert = new SqlCommand(
                "INSERT INTO ADMIN (ADMINID, USERNAME, PASSWORD, EMAIL, ADDRESS) VALUES (@id, @u, @p, @e, @a)", conn);
            cmdInsert.Parameters.AddWithValue("@id", adminId);
            cmdInsert.Parameters.AddWithValue("@u", username);
            cmdInsert.Parameters.AddWithValue("@p", password);
            cmdInsert.Parameters.AddWithValue("@e", email);
            cmdInsert.Parameters.AddWithValue("@a", address);
            cmdInsert.ExecuteNonQuery();

            // Add the first phone number
            var cmdInsertPhone = new SqlCommand(
                "INSERT INTO ADMINPHONE (ADMINID, APHONE) VALUES (@id, @pn)", conn);
            cmdInsertPhone.Parameters.AddWithValue("@id", adminId);
            cmdInsertPhone.Parameters.AddWithValue("@pn", phone);
            cmdInsertPhone.ExecuteNonQuery();

            Console.WriteLine("Do you have another phone to add? (y/n)");
            string anotherPhone = Console.ReadLine().ToLower();
            if (anotherPhone == "y")
            {
                Console.Write("Enter another phone number: ");
                string anotherPhoneNumber = Console.ReadLine();
                var cmdInsertPhone2 = new SqlCommand(
                    "INSERT INTO ADMINPHONE (ADMINID, APHONE) VALUES (@id, @pn)", conn);
                cmdInsertPhone2.Parameters.AddWithValue("@id", adminId);
                cmdInsertPhone2.Parameters.AddWithValue("@pn", anotherPhoneNumber);
                cmdInsertPhone2.ExecuteNonQuery();
            }

            Console.WriteLine("Admin account created successfully! Your ID is: " + adminId);
        }

        Console.ReadKey();
    }

    // USER login and signup

    static void UserLogin()
    {
        Console.Clear();
        Console.WriteLine("---- User Login ----");
        Console.Write("Username: ");
        string username = Console.ReadLine();
        Console.Write("Password: ");
        string password = Console.ReadLine();

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();
            var cmd = new SqlCommand("SELECT * FROM \"USER\" WHERE USERNAME = @u AND PASSWORD = @p", conn);
            cmd.Parameters.AddWithValue("@u", username);
            cmd.Parameters.AddWithValue("@p", password);
            var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                int userId = (int)reader["USERID"];
                reader.Close();
                UserMenu(userId);
            }
            else
            {
                Console.WriteLine("Invalid credentials!");
                Console.ReadKey();
            }
        }
    }

    static void UserSignUp()
    {
        Console.Clear();
        Console.WriteLine("---- User SignUp ----");
        Console.Write("Enter username: ");
        string username = Console.ReadLine();
        Console.Write("Enter password: ");
        string password = Console.ReadLine();
        Console.Write("Enter email: ");
        string email = Console.ReadLine();
        Console.Write("Enter address: ");
        string address = Console.ReadLine();
        Console.Write("Enter your phone number: ");
        string phone = Console.ReadLine();

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();
            var cmdCheck = new SqlCommand("SELECT * FROM \"USER\" WHERE USERNAME = @u", conn);
            cmdCheck.Parameters.AddWithValue("@u", username);
            var reader = cmdCheck.ExecuteReader();

            if (reader.HasRows)
            {
                Console.WriteLine("You already have an account!");
                Console.ReadKey();
                return;
            }
            reader.Close();

            // Generate a unique user ID
            int userId;
            do
            {
                userId = GenerateRandomId();
            } while (IdExists(userId, "\"USER\"", "USERID"));

            var cmdInsert = new SqlCommand(
                "INSERT INTO \"USER\" (USERID, USERNAME, PASSWORD, EMAIL, ADDRESS) VALUES (@id, @u, @p, @e, @a)", conn);
            cmdInsert.Parameters.AddWithValue("@id", userId);
            cmdInsert.Parameters.AddWithValue("@u", username);
            cmdInsert.Parameters.AddWithValue("@p", password);
            cmdInsert.Parameters.AddWithValue("@e", email);
            cmdInsert.Parameters.AddWithValue("@a", address);
            cmdInsert.ExecuteNonQuery();

            // Add the first phone number
            var cmdInsertPhone = new SqlCommand(
                "INSERT INTO USERPHONE (USERID, UPHONE) VALUES (@id, @pn)", conn);
            cmdInsertPhone.Parameters.AddWithValue("@id", userId);
            cmdInsertPhone.Parameters.AddWithValue("@pn", phone);
            cmdInsertPhone.ExecuteNonQuery();

            Console.WriteLine("Do you have another phone to add? (y/n)");
            string anotherPhone = Console.ReadLine().ToLower();
            if (anotherPhone == "y")
            {
                Console.Write("Enter another phone number: ");
                string anotherPhoneNumber = Console.ReadLine();
                var cmdInsertPhone2 = new SqlCommand(
                    "INSERT INTO USERPHONE (USERID, UPHONE) VALUES (@id, @pn)", conn);
                cmdInsertPhone2.Parameters.AddWithValue("@id", userId);
                cmdInsertPhone2.Parameters.AddWithValue("@pn", anotherPhoneNumber);
                cmdInsertPhone2.ExecuteNonQuery();
            }

            Console.WriteLine("User account created successfully! Your ID is: " + userId);
        }

        Console.ReadKey();
    }

    static void AdminMenu(int adminId)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("---- Admin Menu ----");
            Console.WriteLine("1. Manage Users");
            Console.WriteLine("2. Manage Games");
            Console.WriteLine("3. Manage Vendors");
            Console.WriteLine("4. View Reports");
            Console.WriteLine("5. Logout");
            Console.Write("Enter choice: ");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    ManageUsers();
                    break;
                case "2":
                    ManageGames(adminId);
                    break;
                case "3":
                    ManageVendors();
                    break;
                case "4":
                    ViewReports();
                    break;
                case "5":
                    return;
                default:
                    Console.WriteLine("Invalid choice!");
                    break;
            }
        }
    }

    static void ManageUsers()
    {
        Console.Clear();
        Console.WriteLine("---- Manage Users ----");
        Console.WriteLine("1. Add User");
        Console.WriteLine("2. Delete User");
        Console.WriteLine("3. View All Users");
        Console.Write("Enter choice: ");
        var choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                Console.Write("Username: ");
                string username = Console.ReadLine();
                Console.Write("Password: ");
                string password = Console.ReadLine();
                Console.Write("Email: ");
                string email = Console.ReadLine();
                Console.Write("Address: ");
                string address = Console.ReadLine();
                Console.Write("Phone number: ");
                string phone = Console.ReadLine();

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    // Generate a unique user ID
                    int userId;
                    do
                    {
                        userId = GenerateRandomId();
                    } while (IdExists(userId, "\"USER\"", "USERID"));

                    var cmd = new SqlCommand(
                    "INSERT INTO \"USER\" (USERID, USERNAME, PASSWORD, EMAIL, ADDRESS) VALUES (@id, @u, @p, @e, @a)", conn);
                    cmd.Parameters.AddWithValue("@id", userId);
                    cmd.Parameters.AddWithValue("@u", username);
                    cmd.Parameters.AddWithValue("@p", password);
                    cmd.Parameters.AddWithValue("@e", email);
                    cmd.Parameters.AddWithValue("@a", address);
                    cmd.ExecuteNonQuery();

                    // Add the first phone number
                    var cmdInsertPhone = new SqlCommand(
                        "INSERT INTO USERPHONE (USERID, UPHONE) VALUES (@id, @pn)", conn);
                    cmdInsertPhone.Parameters.AddWithValue("@id", userId);
                    cmdInsertPhone.Parameters.AddWithValue("@pn", phone);
                    cmdInsertPhone.ExecuteNonQuery();

                    Console.WriteLine("Do you have another phone to add? (y/n)");
                    string anotherPhone = Console.ReadLine().ToLower();
                    if (anotherPhone == "y")
                    {
                        Console.Write("Enter another phone number: ");
                        string anotherPhoneNumber = Console.ReadLine();
                        var cmdInsertPhone2 = new SqlCommand(
                            "INSERT INTO USERPHONE (USERID, UPHONE) VALUES (@id, @pn)", conn);
                        cmdInsertPhone2.Parameters.AddWithValue("@id", userId);
                        cmdInsertPhone2.Parameters.AddWithValue("@pn", anotherPhoneNumber);
                        cmdInsertPhone2.ExecuteNonQuery();
                    }

                    Console.WriteLine("User added successfully! User ID: " + userId);
                }
                break;

            case "2":
                Console.Write("Enter user ID to delete: ");
                int uid = int.Parse(Console.ReadLine());
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    // First delete related phone records due to foreign key constraints
                    var phoneCmd = new SqlCommand("DELETE FROM USERPHONE WHERE USERID = @u", conn);
                    phoneCmd.Parameters.AddWithValue("@u", uid);
                    phoneCmd.ExecuteNonQuery();

                    // Then delete user
                    var cmd = new SqlCommand("DELETE FROM \"USER\" WHERE USERID = @u", conn);
                    cmd.Parameters.AddWithValue("@u", uid);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                        Console.WriteLine("User deleted!");
                    else
                        Console.WriteLine("User not found!");
                }
                break;

            case "3":
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    var cmd = new SqlCommand(@"
                    SELECT U.USERID, U.USERNAME, U.EMAIL, P.UPHONE
                    FROM [USER] U
                    LEFT JOIN USERPHONE P ON U.USERID = P.USERID
                    ORDER BY U.USERID", conn);

                    using (var reader = cmd.ExecuteReader())
                    {
                        int lastUserId = -1;
                        while (reader.Read())
                        {
                            int currentUserId = (int)reader["USERID"];
                            if (currentUserId != lastUserId)
                            {
                                Console.WriteLine($"\nID: {currentUserId}, Username: {reader["USERNAME"]}, Email: {reader["EMAIL"]}");
                                lastUserId = currentUserId;
                            }

                            if (reader["UPHONE"] != DBNull.Value)
                                Console.WriteLine($"  Phone: {reader["UPHONE"]}");
                        }
                    }
                }
                break;
        }

        Console.WriteLine("Press any key to return...");
        Console.ReadKey();
    }


    static void ManageGames(int adminId)
    {
        Console.Clear();
        Console.WriteLine("--- Manage Games ---");
        Console.WriteLine("1. Add Game");
        Console.WriteLine("2. Delete Game");
        Console.WriteLine("3. View Games");
        Console.WriteLine("4. Update Game Details");
        Console.Write("Enter choice: ");
        var choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                AddNewGame(adminId);
                break;
            case "2":
                DeleteGame();
                break;
            case "3":
                ViewGames();
                break;
            case "4":
                UpdateGameDetails();
                break;
            default:
                Console.WriteLine("Invalid choice.");
                break;
        }

        Console.WriteLine("Press any key to return...");
        Console.ReadKey();
    }

    static void UpdateGameDetails()
    {
        Console.Clear();
        Console.WriteLine("--- Update Game Details ---");
        Console.Write("Enter Vendor ID: ");
        int vendorId = int.Parse(Console.ReadLine());
        Console.Write("Enter Game ID: ");
        int gameId = int.Parse(Console.ReadLine());

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();

            // Check if game exists and get current details
            var gameCheck = new SqlCommand(
                "SELECT NAME, DESCRIPTION, QUANTITY, RELEASEYEAR FROM GAME WHERE VENDORID = @vid AND GAMEID = @gid", conn);
            gameCheck.Parameters.AddWithValue("@vid", vendorId);
            gameCheck.Parameters.AddWithValue("@gid", gameId);

            using (SqlDataReader reader = gameCheck.ExecuteReader())
            {
                if (!reader.Read())
                {
                    Console.WriteLine("Error: Game not found!");
                    Console.ReadKey();
                    return;
                }

                string currentName = reader["NAME"].ToString();
                string currentDescription = reader["DESCRIPTION"].ToString();
                int currentQuantity = (int)reader["QUANTITY"];
                int currentReleaseYear = (int)reader["RELEASEYEAR"];
                reader.Close();

                // Display current details
                Console.WriteLine($"\nCurrent Game Details:");
                Console.WriteLine($"Name: {currentName}");
                Console.WriteLine($"Description: {currentDescription}");
                Console.WriteLine($"Quantity: {currentQuantity}");
                Console.WriteLine($"Release Year: {currentReleaseYear}");

                // Display current categories
                Console.WriteLine("\nCurrent Categories:");
                var catCmd = new SqlCommand(
                    "SELECT CATEOGRY FROM GAMECATEGORY WHERE VENDORID = @vid AND GAMEID = @gid", conn);
                catCmd.Parameters.AddWithValue("@vid", vendorId);
                catCmd.Parameters.AddWithValue("@gid", gameId);

                List<string> categories = new List<string>();
                using (SqlDataReader catReader = catCmd.ExecuteReader())
                {
                    while (catReader.Read())
                    {
                        categories.Add(catReader["CATEOGRY"].ToString());
                    }
                }

                if (categories.Count > 0)
                {
                    Console.WriteLine($"    {string.Join(", ", categories)}");
                }
                else
                {
                    Console.WriteLine("    None");
                }

                // Show update options
                while (true)
                {
                    Console.WriteLine("\n--- Update Options ---");
                    Console.WriteLine("1. Update Name");
                    Console.WriteLine("2. Update Description");
                    Console.WriteLine("3. Update Quantity");
                    Console.WriteLine("4. Update Release Year");
                    Console.WriteLine("5. Manage Categories");
                    Console.WriteLine("6. Return to Previous Menu");
                    Console.Write("Choose an option: ");
                    var updateChoice = Console.ReadLine();

                    switch (updateChoice)
                    {
                        case "1":
                            UpdateGameName(vendorId, gameId, currentName, conn);
                            break;
                        case "2":
                            UpdateGameDescription(vendorId, gameId, currentDescription, conn);
                            break;
                        case "3":
                            UpdateGameQuantity(vendorId, gameId, currentQuantity, conn);
                            break;
                        case "4":
                            UpdateGameReleaseYear(vendorId, gameId, currentReleaseYear, conn);
                            break;
                        case "5":
                            ManageGameCategories(vendorId, gameId, conn);
                            break;
                        case "6":
                            return;
                        default:
                            Console.WriteLine("Invalid choice.");
                            break;
                    }

                    // Refresh current details after update
                    gameCheck.Parameters.Clear();
                    gameCheck.Parameters.AddWithValue("@vid", vendorId);
                    gameCheck.Parameters.AddWithValue("@gid", gameId);
                    SqlDataReader refreshReader = gameCheck.ExecuteReader();
                    if (refreshReader.Read())
                    {
                        currentName = refreshReader["NAME"].ToString();
                        currentDescription = refreshReader["DESCRIPTION"].ToString();
                        currentQuantity = (int)refreshReader["QUANTITY"];
                        currentReleaseYear = (int)refreshReader["RELEASEYEAR"];
                    }
                    refreshReader.Close();

                    Console.Clear();
                    Console.WriteLine("--- Update Game Details ---");
                    Console.WriteLine($"\nCurrent Game Details:");
                    Console.WriteLine($"Name: {currentName}");
                    Console.WriteLine($"Description: {currentDescription}");
                    Console.WriteLine($"Quantity: {currentQuantity}");
                    Console.WriteLine($"Release Year: {currentReleaseYear}");
                }
            }
        }
    }

    static void UpdateGameName(int vendorId, int gameId, string currentName, SqlConnection conn)
    {
        Console.WriteLine($"Current Name: {currentName}");
        Console.Write("Enter new name: ");
        string newName = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(newName))
        {
            Console.WriteLine("Name cannot be empty!");
            Console.ReadKey();
            return;
        }

        var updateCmd = new SqlCommand(
            "UPDATE GAME SET NAME = @name WHERE VENDORID = @vid AND GAMEID = @gid", conn);
        updateCmd.Parameters.AddWithValue("@name", newName);
        updateCmd.Parameters.AddWithValue("@vid", vendorId);
        updateCmd.Parameters.AddWithValue("@gid", gameId);
        int rowsAffected = updateCmd.ExecuteNonQuery();

        if (rowsAffected > 0)
            Console.WriteLine("Game name updated successfully!");
        else
            Console.WriteLine("Failed to update game name.");

        Console.ReadKey();
    }

    static void UpdateGameDescription(int vendorId, int gameId, string currentDescription, SqlConnection conn)
    {
        Console.WriteLine($"Current Description: {currentDescription}");
        Console.Write("Enter new description: ");
        string newDescription = Console.ReadLine();

        var updateCmd = new SqlCommand(
            "UPDATE GAME SET DESCRIPTION = @desc WHERE VENDORID = @vid AND GAMEID = @gid", conn);
        updateCmd.Parameters.AddWithValue("@desc", newDescription);
        updateCmd.Parameters.AddWithValue("@vid", vendorId);
        updateCmd.Parameters.AddWithValue("@gid", gameId);
        int rowsAffected = updateCmd.ExecuteNonQuery();

        if (rowsAffected > 0)
            Console.WriteLine("Game description updated successfully!");
        else
            Console.WriteLine("Failed to update game description.");

        Console.ReadKey();
    }

    static void UpdateGameReleaseYear(int vendorId, int gameId, int currentReleaseYear, SqlConnection conn)
    {
        Console.WriteLine($"Current Release Year: {currentReleaseYear}");
        Console.Write("Enter new release year: ");

        if (!int.TryParse(Console.ReadLine(), out int newReleaseYear))
        {
            Console.WriteLine("Invalid year entered!");
            Console.ReadKey();
            return;
        }

        // Basic validation
        if (newReleaseYear < 1950 || newReleaseYear > DateTime.Now.Year + 5)
        {
            Console.WriteLine($"Please enter a reasonable year between 1950 and {DateTime.Now.Year + 5}");
            Console.ReadKey();
            return;
        }

        var updateCmd = new SqlCommand(
            "UPDATE GAME SET RELEASEYEAR = @year WHERE VENDORID = @vid AND GAMEID = @gid", conn);
        updateCmd.Parameters.AddWithValue("@year", newReleaseYear);
        updateCmd.Parameters.AddWithValue("@vid", vendorId);
        updateCmd.Parameters.AddWithValue("@gid", gameId);
        int rowsAffected = updateCmd.ExecuteNonQuery();

        if (rowsAffected > 0)
            Console.WriteLine("Release year updated successfully!");
        else
            Console.WriteLine("Failed to update release year.");

        Console.ReadKey();
    }

    static void UpdateGameQuantity(int vendorId, int gameId, int currentQuantity, SqlConnection conn)
    {
        Console.WriteLine($"Current quantity: {currentQuantity}");
        Console.Write("Enter new quantity: ");

        if (!int.TryParse(Console.ReadLine(), out int newQuantity))
        {
            Console.WriteLine("Invalid quantity entered!");
            Console.ReadKey();
            return;
        }

        if (newQuantity < 0)
        {
            Console.WriteLine("Error: Quantity cannot be negative.");
            Console.ReadKey();
            return;
        }

        // Check if decreasing quantity would conflict with rented games
        if (newQuantity < currentQuantity)
        {
            var rentedCmd = new SqlCommand(
                "SELECT COUNT(*) FROM RENTED_BY WHERE VENDORID = @vid AND GAMEID = @gid", conn);
            rentedCmd.Parameters.AddWithValue("@vid", vendorId);
            rentedCmd.Parameters.AddWithValue("@gid", gameId);
            int rentedCount = (int)rentedCmd.ExecuteScalar();

            int availableCount = currentQuantity - rentedCount;

            if (newQuantity < rentedCount)
            {
                Console.WriteLine($"Error: Cannot reduce quantity below the number of currently rented copies ({rentedCount}).");
                Console.ReadKey();
                return;
            }
        }

        // Update quantity
        var updateCmd = new SqlCommand(
            "UPDATE GAME SET QUANTITY = @qty WHERE VENDORID = @vid AND GAMEID = @gid", conn);
        updateCmd.Parameters.AddWithValue("@qty", newQuantity);
        updateCmd.Parameters.AddWithValue("@vid", vendorId);
        updateCmd.Parameters.AddWithValue("@gid", gameId);
        updateCmd.ExecuteNonQuery();

        Console.WriteLine($"Quantity updated from {currentQuantity} to {newQuantity}.");
        Console.ReadKey();
    }

    static void ManageGameCategories(int vendorId, int gameId, SqlConnection conn)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("--- Manage Game Categories ---");

            // Get game name for better user experience
            var nameCmd = new SqlCommand("SELECT NAME FROM GAME WHERE VENDORID = @vid AND GAMEID = @gid", conn);
            nameCmd.Parameters.AddWithValue("@vid", vendorId);
            nameCmd.Parameters.AddWithValue("@gid", gameId);
            string gameName = nameCmd.ExecuteScalar().ToString();

            Console.WriteLine($"Managing categories for: {gameName}");

            // Show existing categories
            Console.WriteLine("\nCurrent categories:");
            var catCmd = new SqlCommand(
                "SELECT CATEOGRY FROM GAMECATEGORY WHERE VENDORID = @vid AND GAMEID = @gid", conn);
            catCmd.Parameters.AddWithValue("@vid", vendorId);
            catCmd.Parameters.AddWithValue("@gid", gameId);

            List<string> categories = new List<string>();
            using (SqlDataReader catReader = catCmd.ExecuteReader())
            {
                int index = 1;
                while (catReader.Read())
                {
                    string category = catReader["CATEOGRY"].ToString();
                    categories.Add(category);
                    Console.WriteLine($"{index++}. {category}");
                }
            }

            if (categories.Count == 0)
            {
                Console.WriteLine("    No categories found!");
            }

            Console.WriteLine("\nOptions:");
            Console.WriteLine("1. Add Category");
            Console.WriteLine("2. Remove Category");
            Console.WriteLine("3. Return to Game Details");
            Console.Write("Enter choice: ");

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    AddCategoryToGame(vendorId, gameId, conn);
                    break;
                case "2":
                    RemoveCategoryFromGame(vendorId, gameId, categories, conn);
                    break;
                case "3":
                    return;
                default:
                    Console.WriteLine("Invalid choice.");
                    Console.ReadKey();
                    break;
            }
        }
    }

    static void AddCategoryToGame(int vendorId, int gameId, SqlConnection conn)
    {
        Console.Write("Enter category name: ");
        string category = Console.ReadLine().Trim();

        if (string.IsNullOrWhiteSpace(category))
        {
            Console.WriteLine("Category name cannot be empty!");
            Console.ReadKey();
            return;
        }

        // Check if this category is already added to avoid duplicates
        var checkCmd = new SqlCommand(
            "SELECT COUNT(*) FROM GAMECATEGORY WHERE VENDORID = @vid AND GAMEID = @gid AND CATEOGRY = @cat", conn);
        checkCmd.Parameters.AddWithValue("@vid", vendorId);
        checkCmd.Parameters.AddWithValue("@gid", gameId);
        checkCmd.Parameters.AddWithValue("@cat", category);
        int exists = (int)checkCmd.ExecuteScalar();

        if (exists > 0)
        {
            Console.WriteLine($"Category '{category}' is already assigned to this game.");
        }
        else
        {
            var catCmd = new SqlCommand(
                "INSERT INTO GAMECATEGORY (VENDORID, GAMEID, CATEOGRY) VALUES (@vid, @gid, @cat)", conn);
            catCmd.Parameters.AddWithValue("@vid", vendorId);
            catCmd.Parameters.AddWithValue("@gid", gameId);
            catCmd.Parameters.AddWithValue("@cat", category);
            catCmd.ExecuteNonQuery();
            Console.WriteLine($"Category '{category}' added successfully.");
        }

        Console.ReadKey();
    }

    static void RemoveCategoryFromGame(int vendorId, int gameId, List<string> categories, SqlConnection conn)
    {
        if (categories.Count == 0)
        {
            Console.WriteLine("No categories to remove!");
            Console.ReadKey();
            return;
        }

        Console.Write("Enter the number of the category to remove: ");
        if (!int.TryParse(Console.ReadLine(), out int selection) || selection < 1 || selection > categories.Count)
        {
            Console.WriteLine("Invalid selection!");
            Console.ReadKey();
            return;
        }

        string categoryToRemove = categories[selection - 1];

        var deleteCmd = new SqlCommand(
            "DELETE FROM GAMECATEGORY WHERE VENDORID = @vid AND GAMEID = @gid AND CATEOGRY = @cat", conn);
        deleteCmd.Parameters.AddWithValue("@vid", vendorId);
        deleteCmd.Parameters.AddWithValue("@gid", gameId);
        deleteCmd.Parameters.AddWithValue("@cat", categoryToRemove);

        int rowsAffected = deleteCmd.ExecuteNonQuery();
        if (rowsAffected > 0)
        {
            Console.WriteLine($"Category '{categoryToRemove}' removed successfully!");
        }
        else
        {
            Console.WriteLine("Failed to remove category.");
        }

        Console.ReadKey();
    }

    // The following methods remain unchanged from original code
    static void AddNewGame(int adminId)
    {
        Console.Write("Game Name: ");
        string name = Console.ReadLine();
        Console.Write("Description: ");
        string description = Console.ReadLine();
        Console.Write("Quantity: ");
        int quantity = int.Parse(Console.ReadLine());
        Console.Write("Release Year: ");
        int releaseYear = int.Parse(Console.ReadLine());
        Console.Write("Vendor ID: ");
        int vendorId = int.Parse(Console.ReadLine());
        Console.Write("Game ID: ");
        int gameId = int.Parse(Console.ReadLine());

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();

            // Check if vendor exists
            var vendorCheck = new SqlCommand("SELECT COUNT(*) FROM VENDOR WHERE VENDORID = @vid", conn);
            vendorCheck.Parameters.AddWithValue("@vid", vendorId);
            int vendorExists = (int)vendorCheck.ExecuteScalar();

            if (vendorExists == 0)
            {
                Console.WriteLine("Error: Vendor ID does not exist!");
                return;
            }

            // Check if game already exists
            var gameCheck = new SqlCommand("SELECT COUNT(*) FROM GAME WHERE VENDORID = @vid AND GAMEID = @gid", conn);
            gameCheck.Parameters.AddWithValue("@vid", vendorId);
            gameCheck.Parameters.AddWithValue("@gid", gameId);
            int gameExists = (int)gameCheck.ExecuteScalar();

            if (gameExists > 0)
            {
                Console.WriteLine("Error: Game ID already exists for this vendor!");
                return;
            }

            // Add the game
            var cmd = new SqlCommand(
                "INSERT INTO GAME (VENDORID, GAMEID, ADMINID, NAME, DESCRIPTION, QUANTITY, RELEASEYEAR) " +
                "VALUES (@vid, @gid, @aid, @name, @desc, @qty, @year)", conn);
            cmd.Parameters.AddWithValue("@vid", vendorId);
            cmd.Parameters.AddWithValue("@gid", gameId);
            cmd.Parameters.AddWithValue("@aid", adminId);
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@desc", description);
            cmd.Parameters.AddWithValue("@qty", quantity);
            cmd.Parameters.AddWithValue("@year", releaseYear);
            cmd.ExecuteNonQuery();

            Console.WriteLine("Game added successfully!");

            // Handle multiple categories
            AddCategoriesToGame(vendorId, gameId, conn);
        }
    }

    static void AddCategoriesToGame(int vendorId, int gameId, SqlConnection conn = null)
    {
        bool closeConnection = false;
        if (conn == null)
        {
            conn = new SqlConnection(connectionString);
            conn.Open();
            closeConnection = true;
        }

        bool addingCategories = true;
        Console.WriteLine("Enter categories for the game (type 'done' when finished):");

        int categoryCount = 0;
        while (addingCategories)
        {
            Console.Write($"Category {categoryCount + 1}: ");
            string category = Console.ReadLine();

            if (category.ToLower() == "done")
            {
                addingCategories = false;
                if (categoryCount == 0)
                {
                    Console.WriteLine("Warning: No categories were added to the game.");
                }
            }
            else if (!string.IsNullOrWhiteSpace(category))
            {
                // Check if this category is already added to avoid duplicates
                var checkCmd = new SqlCommand(
                    "SELECT COUNT(*) FROM GAMECATEGORY WHERE VENDORID = @vid AND GAMEID = @gid AND CATEOGRY = @cat", conn);
                checkCmd.Parameters.AddWithValue("@vid", vendorId);
                checkCmd.Parameters.AddWithValue("@gid", gameId);
                checkCmd.Parameters.AddWithValue("@cat", category.Trim());
                int exists = (int)checkCmd.ExecuteScalar();

                if (exists > 0)
                {
                    Console.WriteLine($"Category '{category}' is already assigned to this game.");
                }
                else
                {
                    var catCmd = new SqlCommand(
                        "INSERT INTO GAMECATEGORY (VENDORID, GAMEID, CATEOGRY) VALUES (@vid, @gid, @cat)", conn);
                    catCmd.Parameters.AddWithValue("@vid", vendorId);
                    catCmd.Parameters.AddWithValue("@gid", gameId);
                    catCmd.Parameters.AddWithValue("@cat", category.Trim());
                    catCmd.ExecuteNonQuery();
                    categoryCount++;
                    Console.WriteLine($"Category '{category}' added successfully.");
                }
            }
        }

        Console.WriteLine($"Added {categoryCount} categories to the game.");

        if (closeConnection)
        {
            conn.Close();
        }
    }

    static void DeleteGame()
    {
        Console.Write("Enter Vendor ID: ");
        int vId = int.Parse(Console.ReadLine());
        Console.Write("Enter Game ID: ");
        int gId = int.Parse(Console.ReadLine());

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();

            // Delete game categories first due to foreign key constraints
            var catCmd = new SqlCommand("DELETE FROM GAMECATEGORY WHERE VENDORID = @vid AND GAMEID = @gid", conn);
            catCmd.Parameters.AddWithValue("@vid", vId);
            catCmd.Parameters.AddWithValue("@gid", gId);
            catCmd.ExecuteNonQuery();

            // Delete game rentals due to foreign key constraints
            var rentCmd = new SqlCommand("DELETE FROM RENTED_BY WHERE VENDORID = @vid AND GAMEID = @gid", conn);
            rentCmd.Parameters.AddWithValue("@vid", vId);
            rentCmd.Parameters.AddWithValue("@gid", gId);
            rentCmd.ExecuteNonQuery();

            // Delete the game
            var cmd = new SqlCommand("DELETE FROM GAME WHERE VENDORID = @vid AND GAMEID = @gid", conn);
            cmd.Parameters.AddWithValue("@vid", vId);
            cmd.Parameters.AddWithValue("@gid", gId);
            int rowsAffected = cmd.ExecuteNonQuery();

            if (rowsAffected > 0)
                Console.WriteLine("Game deleted successfully!");
            else
                Console.WriteLine("Game not found!");
        }
    }

    // Helper class to store game information
    class GameInfo
    {
        public int VendorId { get; set; }
        public int GameId { get; set; }
        public string Name { get; set; }
        public string VendorName { get; set; }
        public int ReleaseYear { get; set; }
        public int Quantity { get; set; }
    }

    static void ManageVendors()
        {
            Console.Clear();
            Console.WriteLine("---- Manage Vendors ----");
            Console.WriteLine("1. Add Vendor");
            Console.WriteLine("2. Delete Vendor");
            Console.WriteLine("3. View All Vendors");
            Console.Write("Enter choice: ");
            var choice = Console.ReadLine();

            switch (choice)
            {
            case "1":
                Console.Write("Vendor Name: ");
                string name = Console.ReadLine();
                Console.Write("Specialization: ");
                string specialization = Console.ReadLine();
                Console.Write("Email: ");
                string email = Console.ReadLine();
                Console.Write("Phone Number: ");
                string phone = Console.ReadLine();

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Generate a unique vendor ID
                    int vendor_id;
                    do
                    {
                        vendor_id = GenerateRandomId();
                    } while (IdExists(vendor_id, "VENDOR", "VENDORID"));

                    var cmd = new SqlCommand(
                        "INSERT INTO VENDOR (VENDORID, NAME, SPECIALIZATION, EMAIL) VALUES (@id, @name, @spec, @email)", conn);
                    cmd.Parameters.AddWithValue("@id", vendor_id);
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@spec", specialization);
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.ExecuteNonQuery();

                    // Add vendor phone
                    var phoneCmd = new SqlCommand(
                        "INSERT INTO VENDORPHONE (VENDORID, VPHONE) VALUES (@id, @phone)", conn);
                    phoneCmd.Parameters.AddWithValue("@id", vendor_id);
                    phoneCmd.Parameters.AddWithValue("@phone", phone);
                    phoneCmd.ExecuteNonQuery();

                    Console.WriteLine("Do you have another phone to add? (y/n)");
                    string anotherPhone = Console.ReadLine().ToLower();
                    if (anotherPhone == "y")
                    {
                        Console.Write("Enter another phone number: ");
                        string anotherPhoneNumber = Console.ReadLine();
                        var phoneCmd2 = new SqlCommand(
                            "INSERT INTO VENDORPHONE (VENDORID, VPHONE) VALUES (@id, @phone)", conn);
                        phoneCmd2.Parameters.AddWithValue("@id", vendor_id);
                        phoneCmd2.Parameters.AddWithValue("@phone", anotherPhoneNumber);
                        phoneCmd2.ExecuteNonQuery();
                    }

                    Console.WriteLine("Vendor added successfully! Vendor ID: " + vendor_id);
                }
                break;

            case "2":
                Console.Write("Enter Vendor ID to delete: ");
                int vendorId = int.Parse(Console.ReadLine());
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    try
                    {
                        // Check if vendor has games
                        var checkCmd = new SqlCommand("SELECT COUNT(*) FROM GAME WHERE VENDORID = @vid", conn);
                        checkCmd.Parameters.AddWithValue("@vid", vendorId);
                        int gameCount = (int)checkCmd.ExecuteScalar();

                        if (gameCount > 0)
                        {
                            Console.WriteLine("Cannot delete vendor: Vendor has associated games. Delete games first.");
                            Console.ReadKey();
                            return;
                        }

                        // Delete vendor phones first due to foreign key constraints
                        var phoneCmd = new SqlCommand("DELETE FROM VENDORPHONE WHERE VENDORID = @vid", conn);
                        phoneCmd.Parameters.AddWithValue("@vid", vendorId);
                        phoneCmd.ExecuteNonQuery();

                        // Delete the vendor
                        var cmd = new SqlCommand("DELETE FROM VENDOR WHERE VENDORID = @vid", conn);
                        cmd.Parameters.AddWithValue("@vid", vendorId);
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                            Console.WriteLine("Vendor deleted successfully!");
                        else
                            Console.WriteLine("Vendor not found!");
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine($"Error deleting vendor: {ex.Message}");
                    }
                }
                break;

            case "3":
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    var cmd = new SqlCommand(@"
                        SELECT v.VENDORID, v.NAME, v.SPECIALIZATION, v.EMAIL, vp.VPHONE
                        FROM VENDOR v
                        LEFT JOIN VENDORPHONE vp ON v.VENDORID = vp.VENDORID
                        ORDER BY v.VENDORID", conn);

                    using (var reader = cmd.ExecuteReader())
                    {
                        int lastVendorId = -1;
                        if (!reader.HasRows)
                        {
                            Console.WriteLine("No vendors.");
                        }
                        else {
                            while (reader.Read())
                            {
                                int currentVendorId = (int)reader["VENDORID"];
                                if (currentVendorId != lastVendorId)
                                {
                                    Console.WriteLine(
                                        $"\nVendor ID: {currentVendorId}, Name: {reader["NAME"]}, " +
                                        $"Specialization: {reader["SPECIALIZATION"]}, Email: {reader["EMAIL"]}");
                                    lastVendorId = currentVendorId;
                                }

                                if (reader["VPHONE"] != DBNull.Value)
                                    Console.WriteLine($"  Phone: {reader["VPHONE"]}");
                            }
                        }
                    }
                }
                break;
        }

            Console.WriteLine("Press any key to return...");
            Console.ReadKey();
        }

    static void ViewReports()
    {
        Console.Clear();
        Console.WriteLine("--- Rental Reports ---");
        Console.WriteLine("1. Games Rental Report (with Detailed Statistics)");
        Console.WriteLine("2. User Rental History (with Spending Analysis)");
        Console.WriteLine("3. Show Ranked Games");
        Console.WriteLine("4. Revenue Summary by Month");
        Console.Write("Enter choice: ");
        var choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                GamesRentalReport();
                break;
            case "2":
                UserRentalHistory();
                break;
            case "3":
                RankedGames();
                break;
            case "4":
                RevenueSummaryByMonth();
                break;
            default:
                Console.WriteLine("Invalid choice");
                break;
        }

        Console.WriteLine("\nPress any key to return...");
        Console.ReadKey();
    }

    static void GamesRentalReport()
    {
        Console.Clear();
        Console.WriteLine("--- Games Rental Report ---");

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();

            // More comprehensive game rental statistics
            var cmd = new SqlCommand(
                "SELECT g.NAME, v.NAME AS Vendor, " +
                "COUNT(*) AS RentalCount, " +
                "SUM(r.FEES) AS TotalRevenue, " +
                "AVG(r.FEES) AS AverageRentalFee, " +
                "MAX(r.DATE) AS LatestRental, " +
                "g.QUANTITY AS CurrentAvailability " +
                "FROM RENTED_BY r " +
                "JOIN GAME g ON r.VENDORID = g.VENDORID AND r.GAMEID = g.GAMEID " +
                "JOIN VENDOR v ON g.VENDORID = v.VENDORID " +
                "GROUP BY g.NAME, v.NAME, g.QUANTITY " +
                "ORDER BY COUNT(*) DESC", conn);

            var reader = cmd.ExecuteReader();

            if (!reader.HasRows)
            {
                Console.WriteLine("No rental data available.");
            }
            else
            {
                Console.WriteLine(new string('-', 100));
                Console.WriteLine($"{"Game",-30} {"Vendor",-20} {"Rentals",-8} {"Revenue",-10} {"Avg Fee",-10} {"Latest Rental",-15} {"Available",-8}");
                Console.WriteLine(new string('-', 100));

                while (reader.Read())
                {
                    string name = reader["NAME"].ToString();
                    string vendor = reader["Vendor"].ToString();
                    int rentalCount = (int)reader["RentalCount"];
                    decimal totalRevenue = Convert.ToDecimal(reader["TotalRevenue"]);
                    decimal avgFee = Convert.ToDecimal(reader["AverageRentalFee"]);
                    DateTime latestRental = (DateTime)reader["LatestRental"];
                    int availability = (int)reader["CurrentAvailability"];

                    Console.WriteLine(
                        $"{name,-30} {vendor,-20} {rentalCount,-8} ${totalRevenue,-9:F2} ${avgFee,-9:F2} {latestRental.ToShortDateString(),-15} {availability,-8}");
                }
                Console.WriteLine(new string('-', 100));
            }
            reader.Close();
        }
    }

    static void UserRentalHistory()
    {
        Console.Clear();
        Console.WriteLine("--- User Rental History ---");
        Console.Write("Enter User ID (leave blank for all users): ");
        string userIdInput = Console.ReadLine();

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();
            SqlCommand cmd;

            if (string.IsNullOrWhiteSpace(userIdInput))
            {
                // Summary for all users
                cmd = new SqlCommand(
                    "SELECT u.USERID, u.USERNAME, " +
                    "COUNT(*) AS TotalRentals, " +
                    "SUM(r.FEES) AS TotalSpent, " +
                    "MAX(r.DATE) AS LastRental " +
                    "FROM RENTED_BY r " +
                    "JOIN \"USER\" u ON r.USERID = u.USERID " +
                    "GROUP BY u.USERID, u.USERNAME " +
                    "ORDER BY COUNT(*) DESC", conn);

                var reader = cmd.ExecuteReader();

                if (!reader.HasRows)
                {
                    Console.WriteLine("No rental history found.");
                }
                else
                {
                    Console.WriteLine(new string('-', 75));
                    Console.WriteLine($"{"User ID",-8} {"Username",-20} {"Total Rentals",-15} {"Total Spent",-15} {"Last Rental",-15}");
                    Console.WriteLine(new string('-', 75));

                    while (reader.Read())
                    {
                        int userId = (int)reader["USERID"];
                        string username = reader["USERNAME"].ToString();
                        int totalRentals = (int)reader["TotalRentals"];
                        decimal totalSpent = Convert.ToDecimal(reader["TotalSpent"]);
                        DateTime lastRental = (DateTime)reader["LastRental"];

                        Console.WriteLine(
                            $"{userId,-8} {username,-20} {totalRentals,-15} ${totalSpent,-14:F2} {lastRental.ToShortDateString(),-15}");
                    }
                    Console.WriteLine(new string('-', 75));
                }
                reader.Close();
            }
            else
            {
                int userId = int.Parse(userIdInput);

                // User summary first
                cmd = new SqlCommand(
                    "SELECT u.USERNAME, COUNT(*) AS TotalRentals, SUM(r.FEES) AS TotalSpent " +
                    "FROM RENTED_BY r " +
                    "JOIN \"USER\" u ON r.USERID = u.USERID " +
                    "WHERE u.USERID = @uid " +
                    "GROUP BY u.USERNAME", conn);
                cmd.Parameters.AddWithValue("@uid", userId);

                var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    string username = reader["USERNAME"].ToString();
                    int totalRentals = (int)reader["TotalRentals"];
                    decimal totalSpent = Convert.ToDecimal(reader["TotalSpent"]);

                    Console.WriteLine($"User: {username}");
                    Console.WriteLine($"Total Games Rented: {totalRentals}");
                    Console.WriteLine($"Total Spent: ${totalSpent:F2}");
                    Console.WriteLine();
                }
                reader.Close();

                // Then detailed rental history
                cmd = new SqlCommand(
                    "SELECT g.NAME, v.NAME AS Vendor, r.DATE, r.FEES " +
                    "FROM RENTED_BY r " +
                    "JOIN \"USER\" u ON r.USERID = u.USERID " +
                    "JOIN GAME g ON r.VENDORID = g.VENDORID AND r.GAMEID = g.GAMEID " +
                    "JOIN VENDOR v ON g.VENDORID = v.VENDORID " +
                    "WHERE u.USERID = @uid " +
                    "ORDER BY r.DATE DESC", conn);
                cmd.Parameters.AddWithValue("@uid", userId);

                reader = cmd.ExecuteReader();

                if (!reader.HasRows)
                {
                    Console.WriteLine("No rental history found.");
                }
                else
                {
                    Console.WriteLine("Rental History:");
                    Console.WriteLine(new string('-', 80));
                    Console.WriteLine($"{"Game",-30} {"Vendor",-20} {"Date",-15} {"Fee",-10}");
                    Console.WriteLine(new string('-', 80));

                    while (reader.Read())
                    {
                        string game = reader["NAME"].ToString();
                        string vendor = reader["Vendor"].ToString();
                        DateTime date = (DateTime)reader["DATE"];
                        decimal fee = Convert.ToDecimal(reader["FEES"]);

                        Console.WriteLine(
                            $"{game,-30} {vendor,-20} {date.ToShortDateString(),-15} ${fee,-10:F2}");
                    }
                    Console.WriteLine(new string('-', 80));
                }
            }
        }
    }

    static void RankedGames()
    {
        Console.Clear();
        Console.WriteLine("--- Ranked Games ---");

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();

            var cmd = new SqlCommand(
                "SELECT TOP 10 g.NAME, v.NAME AS Vendor, " +
                "COUNT(*) AS RentalCount " +
                "FROM RENTED_BY r " +
                "JOIN GAME g ON r.VENDORID = g.VENDORID AND r.GAMEID = g.GAMEID " +
                "JOIN VENDOR v ON g.VENDORID = v.VENDORID " +
                "GROUP BY g.NAME, v.NAME " +
                "ORDER BY COUNT(*) DESC", conn);

            var reader = cmd.ExecuteReader();

            if (!reader.HasRows)
            {
                Console.WriteLine("No rental data available.");
            }
            else
            {
                Console.WriteLine(new string('-', 75));
                Console.WriteLine($"{"Rank",-5} {"Game",-30} {"Vendor",-15} {"Rentals",-8}");
                Console.WriteLine(new string('-', 75));

                int rank = 1;
                while (reader.Read())
                {
                    string name = reader["NAME"].ToString();
                    string vendor = reader["Vendor"].ToString();
                    int rentalCount = (int)reader["RentalCount"];

                    Console.WriteLine(
                        $"{rank,-5} {name,-30} {vendor,-15} {rentalCount,-8}");
                    rank++;
                }
                Console.WriteLine(new string('-', 75));
            }
        }
    }

    static void RevenueSummaryByMonth()
    {
        Console.Clear();
        Console.WriteLine("--- Monthly Revenue Summary ---");

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();

            var cmd = new SqlCommand(
                "SELECT " +
                "YEAR(r.DATE) AS Year, " +
                "MONTH(r.DATE) AS Month, " +
                "COUNT(*) AS RentalCount, " +
                "SUM(r.FEES) AS MonthlyRevenue, " +
                "AVG(r.FEES) AS AvgRentalPrice " +
                "FROM RENTED_BY r " +
                "GROUP BY YEAR(r.DATE), MONTH(r.DATE) " +
                "ORDER BY YEAR(r.DATE) DESC, MONTH(r.DATE) DESC", conn);

            var reader = cmd.ExecuteReader();

            if (!reader.HasRows)
            {
                Console.WriteLine("No revenue data available.");
            }
            else
            {
                Console.WriteLine(new string('-', 65));
                Console.WriteLine($"{"Period",-10} {"Games Rented",-15} {"Total Revenue",-15} {"Avg Price",-10} {"vs. Prev Month"}");
                Console.WriteLine(new string('-', 65));

                decimal? previousRevenue = null;

                while (reader.Read())
                {
                    int year = (int)reader["Year"];
                    int month = (int)reader["Month"];
                    int rentalCount = (int)reader["RentalCount"];
                    decimal revenue = Convert.ToDecimal(reader["MonthlyRevenue"]);
                    decimal avgPrice = Convert.ToDecimal(reader["AvgRentalPrice"]);

                    string period = $"{month:D2}/{year}";
                    string trend = "";

                    if (previousRevenue.HasValue)
                    {
                        decimal change = revenue - previousRevenue.Value;
                        decimal percentChange = previousRevenue.Value != 0 ?
                            (change / previousRevenue.Value) * 100 : 0;

                        trend = percentChange > 0 ?
                            $"+{percentChange:F1}% ↑" :
                            $"{percentChange:F1}% ↓";
                    }

                    Console.WriteLine(
                        $"{period,-10} {rentalCount,-15} ${revenue,-14:F2} ${avgPrice,-9:F2} {trend}");

                    previousRevenue = revenue;
                }
                Console.WriteLine(new string('-', 65));
            }
        }
    }
    static void UserMenu(int userId)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("---- User Menu ----");
            Console.WriteLine("1. View Games");
            Console.WriteLine("2. Rent a Game");
            Console.WriteLine("3. Return a Game");
            Console.WriteLine("4. My Rentals");
            Console.WriteLine("5. Manage Payment Cards");
            Console.WriteLine("6. Update Profile");
            Console.WriteLine("7. Logout");
            Console.Write("Enter your choice: ");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    ViewGames();
                    break;
                case "2":
                    RentGame(userId);
                    break;
                case "3":
                    ReturnGame(userId);
                    break;
                case "4":
                    MyRentals(userId);
                    break;
                case "5":
                    ManagePaymentCards(userId);
                    break;
                case "6":
                    UpdateUserProfile(userId);
                    break;
                case "7":
                    return;
                default:
                    Console.WriteLine("Invalid choice!");
                    break;
            }
        }
    }

    static void UpdateUserProfile(int userId)
    {
        Console.Clear();
        Console.WriteLine("---- Update Profile ----");

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();

            // First, retrieve the current user information
            var getUserCmd = new SqlCommand("SELECT USERNAME, EMAIL, ADDRESS FROM \"USER\" WHERE USERID = @id", conn);
            getUserCmd.Parameters.AddWithValue("@id", userId);

            SqlDataReader reader = getUserCmd.ExecuteReader();
            if (!reader.Read())
            {
                reader.Close();
                Console.WriteLine("Error retrieving user profile!");
                Console.ReadKey();
                return;
            }

            // Display current information
            string currentUsername = reader["USERNAME"].ToString();
            string currentEmail = reader["EMAIL"].ToString();
            string currentAddress = reader["ADDRESS"].ToString();
            reader.Close();

            Console.WriteLine($"Current Username: {currentUsername}");
            Console.WriteLine($"Current Email: {currentEmail}");
            Console.WriteLine($"Current Address: {currentAddress}");
            Console.WriteLine();

            // Menu for updating profile
            Console.WriteLine("What would you like to update?");
            Console.WriteLine("1. Username");
            Console.WriteLine("2. Password");
            Console.WriteLine("3. Email");
            Console.WriteLine("4. Address");
            Console.WriteLine("5. Phone Numbers");
            Console.WriteLine("6. Return to Main Menu");
            Console.Write("Enter your choice: ");

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    UpdateUsername(userId, currentUsername, conn);
                    break;
                case "2":
                    UpdatePassword(userId, conn);
                    break;
                case "3":
                    UpdateEmail(userId, currentEmail, conn);
                    break;
                case "4":
                    UpdateAddress(userId, currentAddress, conn);
                    break;
                case "5":
                    ManagePhoneNumbers(userId, conn);
                    break;
                case "6":
                    return;
                default:
                    Console.WriteLine("Invalid choice!");
                    Console.ReadKey();
                    break;
            }
        }
    }

    // Implementation of each update function

    static void UpdateUsername(int userId, string currentUsername, SqlConnection conn)
    {
        Console.WriteLine($"Current Username: {currentUsername}");
        Console.Write("Enter new username: ");
        string newUsername = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(newUsername))
        {
            Console.WriteLine("Username cannot be empty!");
            Console.ReadKey();
            return;
        }

        // Check if the new username already exists
        var checkCmd = new SqlCommand("SELECT COUNT(*) FROM \"USER\" WHERE USERNAME = @username AND USERID != @id", conn);
        checkCmd.Parameters.AddWithValue("@username", newUsername);
        checkCmd.Parameters.AddWithValue("@id", userId);

        int existingCount = (int)checkCmd.ExecuteScalar();
        if (existingCount > 0)
        {
            Console.WriteLine("Username already exists! Please choose a different one.");
            Console.ReadKey();
            return;
        }

        // Update the username
        var updateCmd = new SqlCommand("UPDATE \"USER\" SET USERNAME = @username WHERE USERID = @id", conn);
        updateCmd.Parameters.AddWithValue("@username", newUsername);
        updateCmd.Parameters.AddWithValue("@id", userId);

        int rowsAffected = updateCmd.ExecuteNonQuery();
        if (rowsAffected > 0)
        {
            Console.WriteLine("Username updated successfully!");
        }
        else
        {
            Console.WriteLine("Failed to update username!");
        }

        Console.ReadKey();
    }

    static void UpdatePassword(int userId, SqlConnection conn)
    {
        Console.Write("Enter current password: ");
        string currentPassword = Console.ReadLine();

        // Verify current password
        var verifyCmd = new SqlCommand("SELECT COUNT(*) FROM \"USER\" WHERE USERID = @id AND PASSWORD = @password", conn);
        verifyCmd.Parameters.AddWithValue("@id", userId);
        verifyCmd.Parameters.AddWithValue("@password", currentPassword);

        int matchCount = (int)verifyCmd.ExecuteScalar();
        if (matchCount == 0)
        {
            Console.WriteLine("Current password is incorrect!");
            Console.ReadKey();
            return;
        }

        Console.Write("Enter new password: ");
        string newPassword = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(newPassword))
        {
            Console.WriteLine("Password cannot be empty!");
            Console.ReadKey();
            return;
        }

        Console.Write("Confirm new password: ");
        string confirmPassword = Console.ReadLine();

        if (newPassword != confirmPassword)
        {
            Console.WriteLine("Passwords do not match!");
            Console.ReadKey();
            return;
        }

        // Update the password
        var updateCmd = new SqlCommand("UPDATE \"USER\" SET PASSWORD = @password WHERE USERID = @id", conn);
        updateCmd.Parameters.AddWithValue("@password", newPassword);
        updateCmd.Parameters.AddWithValue("@id", userId);

        int rowsAffected = updateCmd.ExecuteNonQuery();
        if (rowsAffected > 0)
        {
            Console.WriteLine("Password updated successfully!");
        }
        else
        {
            Console.WriteLine("Failed to update password!");
        }

        Console.ReadKey();
    }

    static void UpdateEmail(int userId, string currentEmail, SqlConnection conn)
    {
        Console.WriteLine($"Current Email: {currentEmail}");
        Console.Write("Enter new email: ");
        string newEmail = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(newEmail))
        {
            Console.WriteLine("Email cannot be empty!");
            Console.ReadKey();
            return;
        }

        // Update the email
        var updateCmd = new SqlCommand("UPDATE \"USER\" SET EMAIL = @email WHERE USERID = @id", conn);
        updateCmd.Parameters.AddWithValue("@email", newEmail);
        updateCmd.Parameters.AddWithValue("@id", userId);

        int rowsAffected = updateCmd.ExecuteNonQuery();
        if (rowsAffected > 0)
        {
            Console.WriteLine("Email updated successfully!");
        }
        else
        {
            Console.WriteLine("Failed to update email!");
        }

        Console.ReadKey();
    }

    static void UpdateAddress(int userId, string currentAddress, SqlConnection conn)
    {
        Console.WriteLine($"Current Address: {currentAddress}");
        Console.Write("Enter new address: ");
        string newAddress = Console.ReadLine();

        // Update the address
        var updateCmd = new SqlCommand("UPDATE \"USER\" SET ADDRESS = @address WHERE USERID = @id", conn);
        updateCmd.Parameters.AddWithValue("@address", newAddress);
        updateCmd.Parameters.AddWithValue("@id", userId);

        int rowsAffected = updateCmd.ExecuteNonQuery();
        if (rowsAffected > 0)
        {
            Console.WriteLine("Address updated successfully!");
        }
        else
        {
            Console.WriteLine("Failed to update address!");
        }

        Console.ReadKey();
    }

    static void ManagePhoneNumbers(int userId, SqlConnection conn)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("---- Manage Phone Numbers ----");

            // Get current phone numbers
            var getPhoneCmd = new SqlCommand("SELECT UPHONE FROM USERPHONE WHERE USERID = @id", conn);
            getPhoneCmd.Parameters.AddWithValue("@id", userId);

            SqlDataReader reader = getPhoneCmd.ExecuteReader();

            List<string> phoneNumbers = new List<string>();
            int index = 1;

            Console.WriteLine("Current Phone Numbers:");
            if (!reader.HasRows)
            {
                Console.WriteLine("No phone numbers found!");
            }
            else
            {
                while (reader.Read())
                {
                    string phone = reader["UPHONE"].ToString();
                    phoneNumbers.Add(phone);
                    Console.WriteLine($"{index++}. {phone}");
                }
            }
            reader.Close();

            Console.WriteLine();
            Console.WriteLine("1. Add a phone number");
            Console.WriteLine("2. Remove a phone number");
            Console.WriteLine("3. Return to profile update menu");
            Console.Write("Enter your choice: ");

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    AddPhoneNumber(userId, conn);
                    break;
                case "2":
                    if (phoneNumbers.Count > 0)
                    {
                        RemovePhoneNumber(userId, phoneNumbers, conn);
                    }
                    else
                    {
                        Console.WriteLine("No phone numbers to remove!");
                        Console.ReadKey();
                    }
                    break;
                case "3":
                    return;
                default:
                    Console.WriteLine("Invalid choice!");
                    Console.ReadKey();
                    break;
            }
        }
    }

    static void AddPhoneNumber(int userId, SqlConnection conn)
    {
        Console.Write("Enter new phone number: ");
        string newPhone = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(newPhone))
        {
            Console.WriteLine("Phone number cannot be empty!");
            Console.ReadKey();
            return;
        }

        // Check if this phone number already exists for this user
        var checkCmd = new SqlCommand("SELECT COUNT(*) FROM USERPHONE WHERE USERID = @id AND UPHONE = @phone", conn);
        checkCmd.Parameters.AddWithValue("@id", userId);
        checkCmd.Parameters.AddWithValue("@phone", newPhone);

        int existingCount = (int)checkCmd.ExecuteScalar();
        if (existingCount > 0)
        {
            Console.WriteLine("This phone number is already registered with your account!");
            Console.ReadKey();
            return;
        }

        // Insert the new phone number
        var insertCmd = new SqlCommand("INSERT INTO USERPHONE (USERID, UPHONE) VALUES (@id, @phone)", conn);
        insertCmd.Parameters.AddWithValue("@id", userId);
        insertCmd.Parameters.AddWithValue("@phone", newPhone);

        int rowsAffected = insertCmd.ExecuteNonQuery();
        if (rowsAffected > 0)
        {
            Console.WriteLine("Phone number added successfully!");
        }
        else
        {
            Console.WriteLine("Failed to add phone number!");
        }

        Console.ReadKey();
    }

    static void RemovePhoneNumber(int userId, List<string> phoneNumbers, SqlConnection conn)
    {
        Console.Write("Enter the number of the phone to remove: ");
        if (!int.TryParse(Console.ReadLine(), out int selection) || selection < 1 || selection > phoneNumbers.Count)
        {
            Console.WriteLine("Invalid selection!");
            Console.ReadKey();
            return;
        }

        string phoneToRemove = phoneNumbers[selection - 1];

        // Count how many phone numbers the user has
        var countCmd = new SqlCommand("SELECT COUNT(*) FROM USERPHONE WHERE USERID = @id", conn);
        countCmd.Parameters.AddWithValue("@id", userId);

        int totalPhones = (int)countCmd.ExecuteScalar();
        if (totalPhones <= 1)
        {
            Console.WriteLine("Cannot remove your only phone number! You must have at least one phone number.");
            Console.ReadKey();
            return;
        }

        // Delete the phone number
        var deleteCmd = new SqlCommand("DELETE FROM USERPHONE WHERE USERID = @id AND UPHONE = @phone", conn);
        deleteCmd.Parameters.AddWithValue("@id", userId);
        deleteCmd.Parameters.AddWithValue("@phone", phoneToRemove);

        int rowsAffected = deleteCmd.ExecuteNonQuery();
        if (rowsAffected > 0)
        {
            Console.WriteLine("Phone number removed successfully!");
        }
        else
        {
            Console.WriteLine("Failed to remove phone number!");
        }

        Console.ReadKey();
    }

    static void RentGame(int userId)
    {
        Console.Clear();
        Console.WriteLine("--- Rent a Game ---");

        Console.Write("Enter Vendor ID: ");
        int vendorId = int.Parse(Console.ReadLine());
        Console.Write("Enter Game ID: ");
        int gameId = int.Parse(Console.ReadLine());

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();

            // Check if game exists and is available
            var checkCmd = new SqlCommand(
                "SELECT NAME, QUANTITY FROM GAME WHERE VENDORID = @vid AND GAMEID = @gid", conn);
            checkCmd.Parameters.AddWithValue("@vid", vendorId);
            checkCmd.Parameters.AddWithValue("@gid", gameId);

            SqlDataReader reader = checkCmd.ExecuteReader();
            if (!reader.Read())
            {
                reader.Close();
                Console.WriteLine("Game not found!");
                Console.ReadKey();
                return;
            }

            string gameName = reader["NAME"].ToString();
            int availableQuantity = (int)reader["QUANTITY"];
            reader.Close();

            if (availableQuantity <= 0)
            {
                Console.WriteLine($"Game '{gameName}' is not available for rent!");
                Console.ReadKey();
                return;
            }

            // Display available quantity
            Console.WriteLine($"Game: {gameName}");
            Console.WriteLine($"Available copies: {availableQuantity}");

            // Set default rental quantity to 1
            int rentQuantity = 1;

            if (rentQuantity > availableQuantity)
            {
                Console.WriteLine($"Error: Only {availableQuantity} copies available.");
                Console.ReadKey();
                return;
            }

            // Ask for rental fee
            Console.Write("Enter rental fee per copy: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal feePerCopy) || feePerCopy < 0)
            {
                Console.WriteLine("Invalid fee amount.");
                Console.ReadKey();
                return;
            }

            decimal totalFee = feePerCopy * rentQuantity;

            // Check if user already has this game rented
            var rentCheckCmd = new SqlCommand(
                "SELECT COUNT(*) FROM RENTED_BY " +
                "WHERE VENDORID = @vid AND GAMEID = @gid AND USERID = @uid", conn);
            rentCheckCmd.Parameters.AddWithValue("@vid", vendorId);
            rentCheckCmd.Parameters.AddWithValue("@gid", gameId);
            rentCheckCmd.Parameters.AddWithValue("@uid", userId);
            int alreadyRented = (int)rentCheckCmd.ExecuteScalar();

            if (alreadyRented > 0)
            {
                Console.WriteLine("You have already rented this game! Please return it first if you want more copies.");
                Console.ReadKey();
                return;
            }

            // Add rental record (without quantity since it doesn't exist in RENTED_BY table)
            var rentCmd = new SqlCommand(
                "INSERT INTO RENTED_BY (VENDORID, GAMEID, USERID, DATE, FEES) " +
                "VALUES (@vid, @gid, @uid, @date, @fee)", conn);
            rentCmd.Parameters.AddWithValue("@vid", vendorId);
            rentCmd.Parameters.AddWithValue("@gid", gameId);
            rentCmd.Parameters.AddWithValue("@uid", userId);
            rentCmd.Parameters.AddWithValue("@date", DateTime.Now);
            rentCmd.Parameters.AddWithValue("@fee", totalFee);
            rentCmd.ExecuteNonQuery();

            // Update game quantity
            var updateQuantityCmd = new SqlCommand(
                "UPDATE GAME SET QUANTITY = QUANTITY - @qty " +
                "WHERE VENDORID = @vid AND GAMEID = @gid", conn);
            updateQuantityCmd.Parameters.AddWithValue("@qty", rentQuantity);
            updateQuantityCmd.Parameters.AddWithValue("@vid", vendorId);
            updateQuantityCmd.Parameters.AddWithValue("@gid", gameId);
            updateQuantityCmd.ExecuteNonQuery();

            Console.WriteLine($"Successfully rented '{gameName}' for ${totalFee}");
        }

        Console.WriteLine("Press any key to return...");
        Console.ReadKey();
    }

    static void ReturnGame(int userId)
    {
        Console.Clear();
        Console.WriteLine("--- Return a Game ---");

        // Display user's currently rented games
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();
            var rentedCmd = new SqlCommand(
                "SELECT r.VENDORID, r.GAMEID, g.NAME, r.DATE, r.FEES " +
                "FROM RENTED_BY r " +
                "JOIN GAME g ON r.VENDORID = g.VENDORID AND r.GAMEID = g.GAMEID " +
                "WHERE r.USERID = @uid", conn);
            rentedCmd.Parameters.AddWithValue("@uid", userId);
            var reader = rentedCmd.ExecuteReader();

            if (!reader.HasRows)
            {
                Console.WriteLine("You don't have any rented games.");
                reader.Close();
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Your currently rented games:");
            while (reader.Read())
            {
                int vendor_id = (int)reader["VENDORID"];
                int game_id = (int)reader["GAMEID"];
                string name = reader["NAME"].ToString();
                DateTime dateRented = (DateTime)reader["DATE"];

                // Handle potential type conversion issues
                decimal fees;
                if (reader["FEES"] is int)
                {
                    fees = Convert.ToDecimal((int)reader["FEES"]);
                }
                else
                {
                    fees = (decimal)reader["FEES"];
                }

                Console.WriteLine(
                    $"Vendor ID: {vendor_id}, Game ID: {game_id}, " +
                    $"Name: {name}, Rented on: {dateRented.ToShortDateString()}, " +
                    $"Fee: ${fees}");
            }
            reader.Close();

            Console.Write("\nEnter Vendor ID of game to return: ");
            int vendorId = int.Parse(Console.ReadLine());
            Console.Write("Enter Game ID of game to return: ");
            int gameId = int.Parse(Console.ReadLine());

            // Check if user has this game rented
            var checkCmd = new SqlCommand(
                "SELECT COUNT(*) FROM RENTED_BY " +
                "WHERE VENDORID = @vid AND GAMEID = @gid AND USERID = @uid", conn);
            checkCmd.Parameters.AddWithValue("@vid", vendorId);
            checkCmd.Parameters.AddWithValue("@gid", gameId);
            checkCmd.Parameters.AddWithValue("@uid", userId);

            object result = checkCmd.ExecuteScalar();
            if (result == null || (int)result == 0)
            {
                Console.WriteLine("You haven't rented this game!");
                Console.ReadKey();
                return;
            }

            // Set return quantity to 1 by default
            int returnQuantity = 1;

            // Remove the rental record
            var deleteCmd = new SqlCommand(
                "DELETE FROM RENTED_BY " +
                "WHERE VENDORID = @vid AND GAMEID = @gid AND USERID = @uid", conn);
            deleteCmd.Parameters.AddWithValue("@vid", vendorId);
            deleteCmd.Parameters.AddWithValue("@gid", gameId);
            deleteCmd.Parameters.AddWithValue("@uid", userId);
            deleteCmd.ExecuteNonQuery();

            // Update game quantity
            var updateQuantityCmd = new SqlCommand(
                "UPDATE GAME SET QUANTITY = QUANTITY + @qty " +
                "WHERE VENDORID = @vid AND GAMEID = @gid", conn);
            updateQuantityCmd.Parameters.AddWithValue("@qty", returnQuantity);
            updateQuantityCmd.Parameters.AddWithValue("@vid", vendorId);
            updateQuantityCmd.Parameters.AddWithValue("@gid", gameId);
            updateQuantityCmd.ExecuteNonQuery();

            Console.WriteLine($"Successfully returned the game.");
        }

        Console.WriteLine("Press any key to return...");
        Console.ReadKey();
    }

    static void MyRentals(int userId)
    {
        Console.Clear();
        Console.WriteLine("--- My Rentals ---");

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();
            var cmd = new SqlCommand(
                "SELECT r.VENDORID, r.GAMEID, g.NAME, r.DATE, r.FEES " +
                "FROM RENTED_BY r " +
                "JOIN GAME g ON r.VENDORID = g.VENDORID AND r.GAMEID = g.GAMEID " +
                "WHERE r.USERID = @uid " +
                "ORDER BY r.DATE DESC", conn);
            cmd.Parameters.AddWithValue("@uid", userId);
            var reader = cmd.ExecuteReader();

            if (!reader.HasRows)
            {
                Console.WriteLine("You don't have any rented games.");
            }
            else
            {
                while (reader.Read())
                {
                    Console.WriteLine(
                        $"Game: {reader["NAME"]}, " +
                        $"Rented on: {((DateTime)reader["DATE"]).ToShortDateString()}, " +
                        $"Fee paid: ${reader["FEES"]}");
                }
            }
        }

        Console.WriteLine("Press any key to return...");
        Console.ReadKey();
    }

    static void ManagePaymentCards(int userId)
        {
            Console.Clear();
            Console.WriteLine("---- Manage Payment Cards ----");
            Console.WriteLine("1. Add Payment Card");
            Console.WriteLine("2. Remove Payment Card");
            Console.WriteLine("3. View My Cards");
            Console.Write("Enter choice: ");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.Write("Enter card number (last 4 digits only): ");
                    string cardNumber = Console.ReadLine();
                    Console.Write("Enter card type (e.g., Visa, MasterCard): ");
                    string cardType = Console.ReadLine();

                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        // Check if card already exists
                        var checkCmd = new SqlCommand(
                            "SELECT COUNT(*) FROM CARD " +
                            "WHERE USERID = @uid AND CARDID = @cardno", conn);
                        checkCmd.Parameters.AddWithValue("@uid", userId);
                        checkCmd.Parameters.AddWithValue("@cardno", cardNumber);
                        int cardExists = (int)checkCmd.ExecuteScalar();

                        if (cardExists > 0)
                        {
                            Console.WriteLine("This card is already registered!");
                            Console.ReadKey();
                            return;
                        }

                        // Add card
                        var cmd = new SqlCommand(
                            "INSERT INTO CARD (USERID, CARDID, CARDTYPE) " +
                            "VALUES (@uid, @cardno, @type)", conn);
                        cmd.Parameters.AddWithValue("@uid", userId);
                        cmd.Parameters.AddWithValue("@cardno", cardNumber);
                        cmd.Parameters.AddWithValue("@type", cardType);
                        cmd.ExecuteNonQuery();
                        Console.WriteLine("Payment card added successfully!");
                    }
                    break;

                case "2":
                    Console.Write("Enter card number to remove: ");
                    string cardNum = Console.ReadLine();

                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        var cmd = new SqlCommand(
                            "DELETE FROM CARD " +
                            "WHERE USERID = @uid AND CARDID = @cardno", conn);
                        cmd.Parameters.AddWithValue("@uid", userId);
                        cmd.Parameters.AddWithValue("@cardno", cardNum);
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                            Console.WriteLine("Payment card removed successfully!");
                        else
                            Console.WriteLine("Card not found!");
                    }
                    break;

                case "3":
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        var cmd = new SqlCommand(
                            "SELECT CARDID, CARDTYPE " +
                            "FROM CARD " +
                            "WHERE USERID = @uid", conn);
                        cmd.Parameters.AddWithValue("@uid", userId);
                        var reader = cmd.ExecuteReader();

                        if (!reader.HasRows)
                        {
                            Console.WriteLine("You don't have any registered payment cards.");
                        }
                        else
                        {
                            while (reader.Read())
                            {
                                Console.WriteLine(
                                    $"Card Number: XXXX-XXXX-XXXX-{reader["CARDID"]}, " +
                                    $"Type: {reader["CARDTYPE"]}");
                            }
                        }
                    }
                    break;
            }

            Console.WriteLine("Press any key to return...");
            Console.ReadKey();
        }
    
    static void ViewGames()
    {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("===== GAME INVENTORY MANAGEMENT SYSTEM =====");
                Console.WriteLine("1. View All Games");
                Console.WriteLine("2. View Games by Vendor");
                Console.WriteLine("3. View Games by Category");
                Console.WriteLine("4. View Games by Release Year");
                Console.WriteLine("5. View Games by Quantity (Low to High)");
                Console.WriteLine("6. Search Games by Name");
                Console.WriteLine("0. Exit");
                Console.Write("\nEnter your choice: ");

                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    switch (choice)
                    {
                        case 0:
                            return;
                        case 1:
                            ViewAllGames();
                            break;
                        case 2:
                            ViewGamesByVendor();
                            break;
                        case 3:
                            ViewGamesByCategory();
                            break;
                        case 4:
                            ViewGamesByReleaseYear();
                            break;
                        case 5:
                            ViewGamesByQuantity();
                            break;
                        case 6:
                            SearchGamesByName();
                            break;
                        default:
                            Console.WriteLine("Invalid choice. Press any key to continue...");
                            Console.ReadKey();
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Press any key to continue...");
                    Console.ReadKey();
                }
            }
        }

        static void ViewAllGames()
        {
            Console.Clear();
            Console.WriteLine("===== ALL GAMES =====\n");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                DisplayGames(conn, "SELECT g.VENDORID, g.GAMEID, g.NAME, g.DESCRIPTION, g.QUANTITY, g.RELEASEYEAR, " +
                    "v.NAME as VENDOR_NAME " +
                    "FROM GAME g JOIN VENDOR v ON g.VENDORID = v.VENDORID " +
                    "ORDER BY g.NAME");
            }

            WaitForKeyPress();
        }

        static void ViewGamesByVendor()
        {
            Console.Clear();
            Console.WriteLine("===== VIEW GAMES BY VENDOR =====\n");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // Get list of vendors
                var vendorCmd = new SqlCommand("SELECT VENDORID, NAME FROM VENDOR ORDER BY NAME", conn);
                var vendors = new List<(int id, string name)>();

                using (SqlDataReader reader = vendorCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        vendors.Add(((int)reader["VENDORID"], reader["NAME"].ToString()));
                    }
                }

                // Display vendors
                for (int i = 0; i < vendors.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {vendors[i].name}");
                }

                Console.Write("\nSelect vendor (enter number): ");
                if (int.TryParse(Console.ReadLine(), out int selection) && selection > 0 && selection <= vendors.Count)
                {
                    int vendorId = vendors[selection - 1].id;
                    string vendorName = vendors[selection - 1].name;

                    Console.Clear();
                    Console.WriteLine($"===== GAMES BY VENDOR: {vendorName} =====\n");

                    string query = "SELECT g.VENDORID, g.GAMEID, g.NAME, g.DESCRIPTION, g.QUANTITY, g.RELEASEYEAR, " +
                                   "v.NAME as VENDOR_NAME " +
                                   "FROM GAME g JOIN VENDOR v ON g.VENDORID = v.VENDORID " +
                                   "WHERE g.VENDORID = @vendorId " +
                                   "ORDER BY g.NAME";

                    var cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@vendorId", vendorId);

                    DisplayGamesWithCommand(conn, cmd);
                }
                else
                {
                    Console.WriteLine("Invalid selection.");
                }
            }

            WaitForKeyPress();
        }

        static void ViewGamesByCategory()
        {
            Console.Clear();
            Console.WriteLine("===== VIEW GAMES BY CATEGORY =====\n");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // Get list of unique categories
                var categoryCmd = new SqlCommand("SELECT DISTINCT CATEOGRY FROM GAMECATEGORY ORDER BY CATEOGRY", conn);
                var categories = new List<string>();

                using (SqlDataReader reader = categoryCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        categories.Add(reader["CATEOGRY"].ToString());
                    }
                }

                // Display categories
                for (int i = 0; i < categories.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {categories[i]}");
                }

                Console.Write("\nSelect category (enter number): ");
                if (int.TryParse(Console.ReadLine(), out int selection) && selection > 0 && selection <= categories.Count)
                {
                    string category = categories[selection - 1];

                    Console.Clear();
                    Console.WriteLine($"===== GAMES BY CATEGORY: {category} =====\n");

                    string query = "SELECT g.VENDORID, g.GAMEID, g.NAME, g.DESCRIPTION, g.QUANTITY, g.RELEASEYEAR, " +
                                   "v.NAME as VENDOR_NAME " +
                                   "FROM GAME g " +
                                   "JOIN VENDOR v ON g.VENDORID = v.VENDORID " +
                                   "JOIN GAMECATEGORY gc ON g.VENDORID = gc.VENDORID AND g.GAMEID = gc.GAMEID " +
                                   "WHERE gc.CATEOGRY = @category " +
                                   "ORDER BY g.NAME";

                    var cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@category", category);

                    DisplayGamesWithCommand(conn, cmd);
                }
                else
                {
                    Console.WriteLine("Invalid selection.");
                }
            }

            WaitForKeyPress();
        }

        static void ViewGamesByReleaseYear()
        {
            Console.Clear();
            Console.WriteLine("===== VIEW GAMES BY RELEASE YEAR =====\n");

            Console.Write("Enter release year: ");
            if (int.TryParse(Console.ReadLine(), out int year))
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    Console.Clear();
                    Console.WriteLine($"===== GAMES RELEASED IN {year} =====\n");

                    string query = "SELECT g.VENDORID, g.GAMEID, g.NAME, g.DESCRIPTION, g.QUANTITY, g.RELEASEYEAR, " +
                                   "v.NAME as VENDOR_NAME " +
                                   "FROM GAME g JOIN VENDOR v ON g.VENDORID = v.VENDORID " +
                                   "WHERE g.RELEASEYEAR = @year " +
                                   "ORDER BY g.NAME";

                    var cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@year", year);

                    DisplayGamesWithCommand(conn, cmd);
                }
            }
            else
            {
                Console.WriteLine("Invalid year format.");
            }

            WaitForKeyPress();
        }

        static void ViewGamesByQuantity()
        {
            Console.Clear();
            Console.WriteLine("===== VIEW GAMES BY QUANTITY (LOW TO HIGH) =====\n");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                DisplayGames(conn, "SELECT g.VENDORID, g.GAMEID, g.NAME, g.DESCRIPTION, g.QUANTITY, g.RELEASEYEAR, " +
                                  "v.NAME as VENDOR_NAME " +
                                  "FROM GAME g JOIN VENDOR v ON g.VENDORID = v.VENDORID " +
                                  "ORDER BY g.QUANTITY ASC, g.NAME ASC");
            }

            WaitForKeyPress();
        }

        static void SearchGamesByName()
        {
            Console.Clear();
            Console.WriteLine("===== SEARCH GAMES BY NAME =====\n");

            Console.Write("Enter game name (or part of name): ");
            string searchTerm = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    Console.Clear();
                    Console.WriteLine($"===== SEARCH RESULTS FOR: '{searchTerm}' =====\n");

                    string query = "SELECT g.VENDORID, g.GAMEID, g.NAME, g.DESCRIPTION, g.QUANTITY, g.RELEASEYEAR, " +
                                   "v.NAME as VENDOR_NAME " +
                                   "FROM GAME g JOIN VENDOR v ON g.VENDORID = v.VENDORID " +
                                   "WHERE g.NAME LIKE @searchTerm " +
                                   "ORDER BY g.NAME";

                    var cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@searchTerm", "%" + searchTerm + "%");

                    DisplayGamesWithCommand(conn, cmd);
                }
            }
            else
            {
                Console.WriteLine("Search term cannot be empty.");
            }

            WaitForKeyPress();
        }

        static void DisplayGames(SqlConnection conn, string query)
        {
            var cmd = new SqlCommand(query, conn);
            DisplayGamesWithCommand(conn, cmd);
        }

        static void DisplayGamesWithCommand(SqlConnection conn, SqlCommand cmd)
        {
            List<GameInfo> games = new List<GameInfo>();

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (!reader.HasRows)
                {
                    Console.WriteLine("No Games Available.");
                    return;
                }

                while (reader.Read())
                {
                    games.Add(new GameInfo
                    {
                        VendorId = (int)reader["VENDORID"],
                        GameId = (int)reader["GAMEID"],
                        Name = reader["NAME"].ToString(),
                        VendorName = reader["VENDOR_NAME"].ToString(),
                        ReleaseYear = (int)reader["RELEASEYEAR"],
                        Quantity = (int)reader["QUANTITY"]
                    });
                }
            }

            // Now process each game and get its categories
            foreach (var game in games)
            {
                Console.WriteLine(
                    $"Vendor ID: {game.VendorId}, Game ID: {game.GameId}, " +
                    $"Name: {game.Name}, Vendor: {game.VendorName}, " +
                    $"Release Year: {game.ReleaseYear}, Quantity: {game.Quantity}");

                // Get categories for this game
                var catCmd = new SqlCommand(
                    "SELECT CATEOGRY FROM GAMECATEGORY WHERE VENDORID = @vid AND GAMEID = @gid", conn);
                catCmd.Parameters.AddWithValue("@vid", game.VendorId);
                catCmd.Parameters.AddWithValue("@gid", game.GameId);

                List<string> categories = new List<string>();
                using (SqlDataReader catReader = catCmd.ExecuteReader())
                {
                    while (catReader.Read())
                    {
                        categories.Add(catReader["CATEOGRY"].ToString());
                    }
                }

                if (categories.Count > 0)
                {
                    Console.WriteLine($"    Categories: {string.Join(", ", categories)}");
                }
                else
                {
                    Console.WriteLine("    Categories: None");
                }

                Console.WriteLine();
            }
        }

        static void WaitForKeyPress()
        {
            Console.WriteLine("\nPress any key to return to the main menu...");
            Console.ReadKey();
        }
    }
