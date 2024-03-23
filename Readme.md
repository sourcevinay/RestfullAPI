DataBase - CustomerDb

CREATE TABLE Customers (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    FirstName NVARCHAR(100) NOT NULL,
    LastName NVARCHAR(100) NOT NULL,
    Password NVARCHAR(255) NOT NULL,
    LoginUser NVARCHAR(100) NOT NULL,
    Email NVARCHAR(255) NOT NULL,
    PhoneNumber NVARCHAR(20) NOT NULL
);

Connection String : 
"DefaultConnection": "Data Source=BIG-BOSS;Initial Catalog=CustomerDb;User Id=sa;password=star;Trusted_Connection=False;MultipleActiveResultSets=true;"

====

GET
https://localhost:44385/api/Customers
-----------------
GET 
https://localhost:44385/api/Customers/1
-----------------
POST
https://localhost:44385/api/Customers 
{
  "firstName": "Vinay",
  "lastName": "Vish",
  "password": "123",
  "loginUser": "VinayVish",
  "email": "Vish@gmail.com",
  "phoneNumber": "1234567890"
}
-----------------------
PUT
https://localhost:44385/api/Customers/1
{
    "firstName": "Vinay Update",
    "lastName": "Vish update",
    "password": "123",
    "loginUser": "VinayVish",
    "email": "Vish@gmail.com",
    "phoneNumber": "1234567890"
}
----------------
DELETE
https://localhost:44385/api/Customers/1
