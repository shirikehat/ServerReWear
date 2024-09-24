﻿Use master
Go
IF EXISTS (SELECT * FROM sys.databases WHERE name = N'ReWear_DB')
BEGIN
    DROP DATABASE ReWear_DB;
END
Go
Create Database ReWear_DB
Go
Use ReWear_DB
Go

CREATE TABLE Users (
    UserId INT PRIMARY KEY Identity,    -- מפתח ראשי
    UserName NVARCHAR(100),    -- שם משתמש
    Password NVARCHAR(100),    -- סיסמה
    Phone NVARCHAR(10),        --טלפון
    Email NVARCHAR(100)        --אימייל
);

SELECT * FROM Products


CREATE TABLE Products (
    ProductCode INT PRIMARY KEY Identity,      -- מפתח ראשי
    Price INT,                                 -- מחיר המוצר
    UserId INT,                                    -- מפתח זר לטבלת משתמשים
    FOREIGN KEY (UserId) REFERENCES Users(UserId), -- קישור לטבלת המשתמשים

    Size NVARCHAR(15),
    StatusId INT,                                         -- מפתח זר לטבלת סטטוס
    FOREIGN KEY (StatusId) REFERENCES Status(StatusCode), -- קישור לטבלת הסטטוס

    TypeId INT,                                      -- מפתח זר לטבלת סוגים
    FOREIGN KEY (TypeId) REFERENCES Types(TypeCode), -- קישור לטבלת הסוגים

    Picture VARBINARY(MAX)
);


CREATE TABLE Status(
    StatusCode INT PRIMARY KEY,
    Name NVARCHAR(15)
);


CREATE TABLE Types(
    TypeCode INT PRIMARY KEY,
    Name NVARCHAR(15)
);


CREATE TABLE Cart(
UserId INT,
FOREIGN KEY (UserId) REFERENCES Users(Userid),

ProductCode INT,
FOREIGN KEY (ProductCode) REFERENCES Products(ProductCode)
);


CREATE TABLE WishList(
UserId INT,
FOREIGN KEY (UserId) REFERENCES Users(Userid),

ProductCode INT,
FOREIGN KEY (ProductCode) REFERENCES Products(ProductCode)
);


CREATE TABLE OrdersFrom(
UserId INT,
FOREIGN KEY (UserId) REFERENCES Users(Userid),

ProductCode INT,
FOREIGN KEY (ProductCode) REFERENCES Products(ProductCode),

Adress NVARCHAR(100)
);

-- Create a login for the admin user
CREATE LOGIN [AdminLogin] WITH PASSWORD = 'admin123';
Go

-- Create a user in the TamiDB database for the login
CREATE USER [AdminUser] FOR LOGIN [AdminLogin];
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



