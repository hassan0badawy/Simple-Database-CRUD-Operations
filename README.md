Video: https://drive.google.com/file/d/1nBp2eA6VsskjWKWJUQZGEX5OVE5TnusI/view?usp=sharing

# Student Management System

A simple C# console application connected to **SQL Server** that performs the four basic CRUD operations (Create, Read, Update, Delete) on a `Student` table, using plain ADO.NET (no ORM).

## Technologies

- C# (.NET 10)
- SQL Server Express + SQL Server Management Studio (SSMS)
- ADO.NET with the `Microsoft.Data.SqlClient` NuGet package

## Student Table

The table is created automatically by the application at startup if it does not already exist:

```sql
IF OBJECT_ID('dbo.Student', 'U') IS NULL
CREATE TABLE Student (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name VARCHAR(100) NOT NULL,
    Age INT,
    GPA DECIMAL(3,2)
);
```

## Features

| Menu option | Operation | SQL used |
|---|---|---|
| 1. Add Student | Create | `INSERT INTO Student (Name, Age, GPA) VALUES (@Name, @Age, @GPA)` |
| 2. View Students | Read | `SELECT * FROM Student` |
| 3. Update Student | Update (by Id) | `UPDATE Student SET ... WHERE Id = @id` |
| 4. Delete Student | Delete (by Id) | `DELETE FROM Student WHERE Id = @id` |
| 0. Exit | Closes the application | |

All queries are **parameterized** (`@name`, `@age`, `@gpa`, `@id`). User input is never concatenated into SQL strings, which prevents SQL injection.

## How It Works

1. The program opens a `SqlConnection` using the connection string.
2. It creates the `Student` table if it does not exist.
3. For each menu option, a `SqlCommand` is built with the SQL text and its parameters.
4. `ExecuteNonQuery()` runs INSERT / UPDATE / DELETE, and `ExecuteReader()` runs SELECT.

## How to Run

1. Make sure SQL Server (or SQL Server Express) is running.
2. In SSMS, create an empty database named `StudentDB`.
3. Open `Program.cs` and change the server name in the connection string to your own SQL Server instance:
   ```csharp
   Server=DESKTOP-P698DH9\SQLEXPRESS;Database=StudentDB;Trusted_Connection=True;TrustServerCertificate=True;
   ```
4. Open `StudentApp.slnx` in Visual Studio and press **Ctrl+F5**, or run from a terminal:
   ```
   cd StudentApp
   dotnet run
   ```

## Screenshots

### Add student (with input validation)
Invalid names, ages, and GPA values are rejected until a valid value is entered.

![Add student](screenshots/02-add.png)

### Delete student
The Id is checked before deleting, and the table afterwards shows the remaining students.

![Delete student](screenshots/05-delete.png)
