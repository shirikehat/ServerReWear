use master
Go
IF EXISTS (SELECT * FROM sys.databases WHERE name = N'ReWear_DB')
BEGIN
    DROP DATABASE ReWear_DB;
END
Go
Create Database ReWear_DB
Go
use ReWear_DB
Go


--טבלת משתמשים
CREATE TABLE Users (
    UserId INT PRIMARY KEY Identity,    -- מפתח ראשי
    UserName NVARCHAR(100) unique not null,    -- שם משתמש
    Password NVARCHAR(100) not null,    -- סיסמה
    Phone NVARCHAR(10) not null,        --טלפון
    Email NVARCHAR(100) not null,       --אימייל
    IsManager bit default(0) not null, --האם המשתמש הוא מנהל המערכת
    IsBlocked bit Not Null Default 0
);


--טבלת סטטוס
CREATE TABLE Status(
    StatusCode INT PRIMARY KEY,  --קוד סטטוס
    Name NVARCHAR(15)            --שם
);


--טבלת סוג מוצר
CREATE TABLE Types(
    TypeCode INT PRIMARY KEY,   --קוד סוג מוצר
    Name NVARCHAR(15)           --שם
);

--טבלת מוצרים
CREATE TABLE Products (
    ProductCode INT PRIMARY KEY Identity,      -- מפתח ראשי
    Price INT not null,                                 -- מחיר המוצר
    UserId INT not null,                                    -- מפתח זר לטבלת משתמשים
    FOREIGN KEY (UserId) REFERENCES Users(UserId), -- קישור לטבלת המשתמשים

    Size NVARCHAR(15),
    StatusId INT not null,                                         -- מפתח זר לטבלת סטטוס
    FOREIGN KEY (StatusId) REFERENCES Status(StatusCode), -- קישור לטבלת הסטטוס

    TypeId INT not null,                                      -- מפתח זר לטבלת סוגים
    FOREIGN KEY (TypeId) REFERENCES Types(TypeCode), -- קישור לטבלת הסוגים

    Store NVARCHAR(25),
    Description NVARCHAR(100)
);




--עגלת קניות
CREATE TABLE Cart(
CartId INT PRIMARY KEY Identity, --מפתח ראשי

UserId INT,                                    --מפתח זר לטבלת משתמשים
FOREIGN KEY (UserId) REFERENCES Users(Userid), --קישור לטבלת משתמשים

ProductCode INT,                                             --מפתח זר לטבלת מוצרים
FOREIGN KEY (ProductCode) REFERENCES Products(ProductCode)   --קישור לטבלת מוצרים
);

--טבלת לייקים
CREATE TABLE WishList(
WishlistId INT PRIMARY KEY Identity,  --מפתח ראשי

UserId INT,                                    --מפתח זר לטבלת משתמשים
FOREIGN KEY (UserId) REFERENCES Users(Userid), --קישור לטבלת משתמשים

ProductCode INT,                                           --מפתח זר לטבלת מוצרים
FOREIGN KEY (ProductCode) REFERENCES Products(ProductCode) --קישור לטבלת מוצרים
);


--טבלת מוצרים שהזמינו ממני
CREATE TABLE OrdersFrom(
UserId INT,                                    --מפתח זר לטבלת משתמשים
FOREIGN KEY (UserId) REFERENCES Users(Userid), --קישור לטבלת משתמשים

ProductCode INT,                                            --מפתח זר לטבלת מוצרים
FOREIGN KEY (ProductCode) REFERENCES Products(ProductCode), --קישור לטבלת מוצרים

Adress NVARCHAR(100)  --כתובת למשלוח
);



-- Create a login for the admin user
CREATE LOGIN [AdminUser] WITH PASSWORD = 'admin123';
Go

-- Create a user in the TamiDB database for the login
CREATE USER [AdminUser] FOR LOGIN [AdminUser];
Go

-- Add the user to the db_owner role to grant admin privileges
ALTER ROLE db_owner ADD MEMBER [AdminUser];
Go



insert into Status values (1, N'לא נקנה')
insert into Status values (2, N'נקנה')

insert into Types values (1, N'חולצה')
insert into Types values (2, N'מכנסיים')
insert into Types values (3, N'חצאית')
insert into Types values (4, N'שמלה')
insert into Types values (5, N'נעליים')
insert into Types values (6, N'תיק')
insert into Types values (7, N'כובע')
insert into Types values (8, N'תכשיטים')
insert into Types values (9, N'בגד ים')

insert into Users(Username, Password, Phone, Email) values ('Shira', 'Shira123', '0505555500', 'shira@gmail.com')
insert into Users(Username, Password, Phone, Email, IsManager) values ('shiri','admin123','0009998887','a@gmail.com', 1)
insert into Products  values(30, 1, 'xs', 1, 1, 'h&m', 'good')
insert into Products  values(35, 1, 's', 1, 2, 'bershka', 'great')
insert into Products  values(100, 1, '39', 1, 5, 'h&m', 'perf')
insert into Products  values(40, 1, 's', 1, 4, 'bershka', 'great')
GO
Select * From Users
Select * From Status
Select * From Types
Select * From Products
select * from cart

--EF Code
/*
scaffold-DbContext "Server = (localdb)\MSSQLLocalDB;Initial Catalog=ReWear_DB;User ID=AdminUser;Password=admin123;" Microsoft.EntityFrameworkCore.SqlServer -OutPutDir Models -Context ShiriDBContext -DataAnnotations -force
*/