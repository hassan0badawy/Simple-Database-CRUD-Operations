
using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

string connectionString =
    @"Server=DESKTOP-P698DH9\SQLEXPRESS;Database=StudentDB;Trusted_Connection=True;TrustServerCertificate=True;";

using var connection = new SqlConnection(connectionString);
connection.Open();


string createtable= @"
if object_id ('dbo.Student', 'U') is null
create table student(
Id int primary key identity(1,1),
Name varchar(50) not null,
Age integer,
GPA decimal(3,2)
);";

using var create = new SqlCommand(createtable, connection);
create.ExecuteNonQuery();

while (true)
{
    Console.WriteLine("Choose a number \n1. Add student \n2. View students \n3. Update student \n4. Delete student \n0. Exit");
    int choice;
    while(!int.TryParse(Console.ReadLine(), out choice))
    {
        Console.Write("Invalid input. Please enter a valid choice (0-4): ");
    }

    switch (choice)
    {
        case 1:
            {
                Console.Write("Enter student name: ");
                string name = Console.ReadLine();

                while (!IsValidName(name))
                {
                    Console.Write("Invalid input. Please enter a valid name: ");
                    name = Console.ReadLine();
                }
                Console.Write("Enter student age: ");
                int age;
                while (!int.TryParse(Console.ReadLine(), out age) || age <= 0)
                {
                    Console.Write("Invalid input. Please enter a valid age: ");
                }
                Console.Write("Enter student GPA: ");
                decimal GPA;
                while (!decimal.TryParse(Console.ReadLine(), out GPA) || GPA < 0 || GPA > 4.0m)
                {
                    Console.Write("Invalid input. Please enter a valid GPA (0.00-4.00): ");
                }

                string add_Student = @"insert into student (Name, Age, GPA) values(@Name, @Age, @GPA)";
                using var add = new SqlCommand(add_Student, connection);
                add.Parameters.AddWithValue("@Name", name);
                add.Parameters.AddWithValue("@Age", age);
                add.Parameters.AddWithValue("@GPA", GPA);
                add.ExecuteNonQuery();
                Console.WriteLine("Student added successfully.");
                break;
            }
        case 2:
            {
                string view_Student = @"select * from student";
                using var view = new SqlCommand(view_Student, connection);
                using var reader = view.ExecuteReader();

                Console.WriteLine("ID|Name|Age|GPA \n");
                while (reader.Read())
                {
                    Console.WriteLine($"{reader["ID"]}|{reader["Name"]}|{reader["Age"]}|{reader["GPA"]}");
                }
                break;
            }
        case 3:
            { 
            Console.WriteLine("Enter the ID of the student to update: ");
            int updateId;
            while(!int.TryParse(Console.ReadLine(), out updateId) || !StudentExists(connection, updateId) )
            {
                Console.Write("Invalid input or not exist. Please enter a valid ID: ");
            }   
            Console.WriteLine("Choose the field to update: \n1. Name \n2. Age \n3. GPA");
            int updateChoice;
            while(!int.TryParse(Console.ReadLine(), out updateChoice) || updateChoice < 1 || updateChoice > 3)
            {
                Console.Write("Invalid input. Please enter a valid choice (1-3): ");
            }
            switch (updateChoice)
            {
                case 1:
                    {
                        Console.WriteLine("Enter the new name: ");
                        String newname = Console.ReadLine();
                            while (!IsValidName(newname))
                            {
                                Console.Write("Invalid input. Please enter a valid name: ");
                                newname = Console.ReadLine();
                            }
                            string Update_NAME = @"Update student set name= @name where id=@id";
                        using var update_name = new SqlCommand(Update_NAME, connection);
                        update_name.Parameters.AddWithValue("@name", newname);
                        update_name.Parameters.AddWithValue("@id", updateId);
                        update_name.ExecuteNonQuery();
                        break;
                    }
                case 2:
                    {
                        Console.WriteLine("Enter the new age: ");
                        int newage;
                        while (!int.TryParse(Console.ReadLine(), out newage) || newage <= 0)
                        {
                            Console.Write("Invalid input. Please enter a valid age: ");
                        }
                        string Update_AGE = @"Update student set age= @age where id=@id";
                        using var update_age = new SqlCommand(Update_AGE, connection);
                        update_age.Parameters.AddWithValue("@age", newage);
                        update_age.Parameters.AddWithValue("@id", updateId);
                        update_age.ExecuteNonQuery();
                        break;
                    }
                case 3:
                    {
                        Console.WriteLine("Enter the New GPA: ");
                        Decimal newGPA;
                        while (!Decimal.TryParse(Console.ReadLine(), out newGPA) || newGPA < 0 || newGPA > 4.0m)
                        {
                            Console.Write("Invalid input. Please enter a valid GPA: ");
                        }
                        string Update_GPA = @"Update student set GPA=@GPA where id=@id";
                        using var update_GPA = new SqlCommand(Update_GPA, connection);
                        update_GPA.Parameters.AddWithValue("@GPA", newGPA);
                        update_GPA.Parameters.AddWithValue("@id", updateId);
                        update_GPA.ExecuteNonQuery();
                        break;
                    }
                 default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
             }
                break;
            }
        case 4:
            {
                Console.WriteLine("Enter the ID of the student to delete: ");
                int deleted_Id;
                while (!int.TryParse(Console.ReadLine(), out deleted_Id) || !StudentExists(connection, deleted_Id))
                {
                    Console.Write("Invalid input or not exist. Please enter a valid ID: ");
                }
                string delete_Student = @"delete from student where id=@id";
                using var delete= new SqlCommand(delete_Student,connection);
                delete.Parameters.AddWithValue("@id", deleted_Id);
                delete.ExecuteNonQuery();
                Console.WriteLine("Student deleted successfully.");
                break;
            }
        case 0:
            {
                Console.WriteLine("Exiting the application.");
                return;
            }
        default:
            Console.WriteLine("Invalid choice. Please try again.");
            break;
    }
}


static bool StudentExists(SqlConnection connection, int id)
{
    string checkStudent = @"select count(*) from student where id=@id";
    using var check = new SqlCommand(checkStudent, connection);
    check.Parameters.AddWithValue("@id", id);
    int count = (int)check.ExecuteScalar();
    return count > 0;
}

static Boolean IsValidName( string name)
{
    return !string.IsNullOrWhiteSpace(name) && name.Length <= 50 && name.All (c => char .IsLetter(c) || char.IsWhiteSpace(c));
}